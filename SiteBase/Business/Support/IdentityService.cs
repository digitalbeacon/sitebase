// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Security;
using Common.Logging;
using DigitalBeacon.Business;
using DigitalBeacon.Business.Support;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Data;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.Util;

namespace DigitalBeacon.SiteBase.Business.Support
{
	public class IdentityService : BaseService, IIdentityService
	{
		#region Private Members

		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private const string ChangedPassword = "Changed Password";
		private const string ChangedSecurityChallege = "Changed Security Question/Answer";
		private const string ChangedIsApproved = "IsApproved = {0}";

		private static readonly VersionedEntityHelper VersionHelper = new VersionedEntityHelper();
		private static readonly IPersonDao PersonDao = ServiceFactory.Instance.GetService<IPersonDao>();
		private static readonly IUserDao UserDao = ServiceFactory.Instance.GetService<IUserDao>();
		private static readonly IRoleDao RoleDao = ServiceFactory.Instance.GetService<IRoleDao>();
		private static readonly IRoleGroupDao RoleGroupDao = ServiceFactory.Instance.GetService<IRoleGroupDao>();
		private static readonly IRoleHomeDao RoleHomeDao = ServiceFactory.Instance.GetService<IRoleHomeDao>();
		private static readonly IMailService MailService = ServiceFactory.Instance.GetService<IMailService>();
		private static readonly IModuleService ModuleService = ServiceFactory.Instance.GetService<IModuleService>();

		#endregion

		#region IIdentityService Members

		public AuthenticationStatus AuthenticateUser(string username, string password)
		{
			var retVal = AuthenticationStatus.Failed;

			if (MembershipValidateUser(username, password) || ValidateDemoUser(username, password))
			{
				var user = UserDao.FetchByUsername(username);
				if (user != null)
				{
					var associationId = GetCurrentAssociationId();
					var isAssociated = user.SuperUser;
					if (!isAssociated)
					{
						foreach (var ae in user.Associations)
						{
							if (ae.Id == associationId)
							{
								isAssociated = true;
								break;
							}
						}
					}
					if (isAssociated)
					{
						GetMembershipUserInfo(user);
						if (Membership.Provider.Name == BusinessConstants.SqlMembershipProvider && String.IsNullOrEmpty(user.SecurityQuestion))
						{
							retVal = AuthenticationStatus.PasswordUpdateRequired;
						}
						else
						{
							AuditingService.CreateAuditLogEntry(AuditAction.LogIn, associationId, user.Id, String.Empty);
							retVal = AuthenticationStatus.Success;
						}
					}
					else
					{
						Log.Error(String.Format("User with username [{0}] authenticated but was not associated with association Id [{1}].", username, associationId));
					}
				}
				else
				{
					Log.Error(String.Format("Authenticated ASP.NET user with username [{0}] but could not locate UserEntity with that username.", username));
				}
			}
			if (retVal == AuthenticationStatus.Failed)
			{
				var aspNetUser = MembershipGetUser(username);
				if (aspNetUser != null && aspNetUser.IsLockedOut)
				{
					retVal = AuthenticationStatus.AccountLocked;
				}
				AuditingService.CreateAuditLogEntry(AuditAction.LogInFailed, GetCurrentAssociationId(), username);
			}
			return retVal;
		}

		public void SignOutUser()
		{
			var user = GetCurrentUser();
			if (user != null)
			{
				AuditingService.CreateAuditLogEntry(AuditAction.LogOut, GetCurrentAssociationId(), user.Id, String.Empty);
			}
		}

		public bool IsUsernameAvailable(string username)
		{
			return GetUser(username) == null && Membership.Providers[BusinessConstants.SqlMembershipProvider].GetUser(username, false) == null;
		}

		public RegistrationResponse RegisterUser(UserEntity user, bool selfRegistration, bool sendEmail)
		{
			var retVal = new RegistrationResponse { Status = RegistrationStatus.NonSpecifiedError };
			try
			{
				MembershipCreateStatus createStatus;
				MembershipProvider provider;
				if (selfRegistration)
				{
					provider = Membership.Providers[BusinessConstants.SqlMembershipProvider];
				}
				else
				{
					provider = Membership.Providers[BusinessConstants.AdminMembershipProvider];
					if (provider == null)
					{
						throw new ServiceException("Could not find admin membership provider [{0}].", BusinessConstants.AdminMembershipProvider);
					}
					if (user.SecurityQuestion.IsNullOrBlank())
					{
						user.SecurityQuestion = null;
					}
					if (user.SecurityAnswer.IsNullOrBlank())
					{
						user.SecurityAnswer = null;
					}
					if (String.IsNullOrEmpty(user.Password))
					{
						user.Password = Membership.GeneratePassword(10, 1);
					}
				}
				provider.CreateUser(user.Username, user.Password, user.Email, user.SecurityQuestion, user.SecurityAnswer, user.Approved, null, out createStatus);
				if (createStatus == MembershipCreateStatus.Success)
				{
					var password = user.Password;
					user.Person = SaveWithAudit(user.Person);
					user.DisplayName = GetDisplayName(user);
					user = DataAdapter.Save(user);
					var msDef = selfRegistration ? ModuleSettingDefinition.RegistrationEmailSubmitted : ModuleSettingDefinition.RegistrationEmailUserCreatedByAdmin;
					var ms = ModuleService.GetGlobalModuleSetting(msDef, user.Language);
					var msg = new MessageInfo { UserId = user.Id, Subject = ms.Subject };
					var substitutions = new Dictionary<SubstitutionDefinition, string>();
					substitutions[SubstitutionDefinition.UserDisplayName] = user.DisplayName;
					if (selfRegistration)
					{
						var ms2 = ModuleService.GetGlobalModuleSetting(ModuleSettingDefinition.GlobalSiteUrl);
						var ms3 = ModuleService.GetGlobalModuleSetting(ModuleSettingDefinition.RegistrationUrlConfirmBaseUrl);
						var confirmUrl = "{0}{1}?token={2}".FormatWith(ms2.Value, ms3.Value, MembershipGetUser(user.Username).ProviderUserKey);
						if (user.Language != null)
						{
							confirmUrl = "{0}&{1}={2}".FormatWith(confirmUrl, BusinessConstants.PersistentLanguageKey, (long)user.Language.Value);
						}
						substitutions[SubstitutionDefinition.ConfirmRegistrationUrl] = confirmUrl;
					}
					else
					{
						substitutions[SubstitutionDefinition.Username] = user.Username;
						substitutions[SubstitutionDefinition.Password] = password;
					}
					msg.Body = ModuleService.GetModuleSettingValueWithSubstitutions(ms, substitutions);
					var queuedEmailId = MailService.QueueEmail(user.Associations[0].Id, msg);
					if (!sendEmail)
					{
						var qe = MailService.GetQueuedEmail(queuedEmailId);
						qe.DateProcessed = DateTime.Now;
						qe.ErrorMessage = "Email notification was suppressed.";
					}
					retVal.UserId = user.Id;
					retVal.Status = RegistrationStatus.Success;
					user.Password = String.Empty;
					user.SecurityAnswer = String.Empty;
					AuditingService.CreateAuditLogEntry(AuditAction.CreateEntity, user.Associations[0].Id, selfRegistration ? user.Id : GetCurrentUser().Id, user, user.ToString());
				}
				else if (createStatus == MembershipCreateStatus.DuplicateUserName)
				{
					retVal.Status = RegistrationStatus.DuplicateUsername;
				}
				else if (createStatus == MembershipCreateStatus.DuplicateEmail)
				{
					retVal.Status = RegistrationStatus.DuplicateEmail;
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex);
				throw;
			}
			return retVal;
		}

		public string GetUsernameByRegistrationToken(string token)
		{
			string retVal = null;
			var userId = token.ToGuid();
			if (userId == null)
			{
				throw new ServiceException("The token [{0}] is not a valid Guid.", token);
			}
			var aspNetUser = Membership.GetUser(userId.Value);
			if (aspNetUser != null && !aspNetUser.IsApproved)
			{
				retVal = aspNetUser.UserName;
			}
			return retVal;
		}

		public void SetUserEnabled(string username, bool enabled)
		{
			if (Membership.Provider.Name != BusinessConstants.SqlMembershipProvider)
			{
				return;
			}
			var aspNetUser = MembershipGetUser(username);
			if (aspNetUser == null)
			{
				throw new ServiceException("Could not find ASP.NET user with username [{0}].", username);
			}
			var user = GetUser(username);
			if (user == null)
			{
				throw new ServiceException("Could not find user entity with username [{0}].", username);
			}
			aspNetUser.IsApproved = enabled;
			Membership.UpdateUser(aspNetUser);
			var currentUser = GetCurrentUser();
			AuditingService.CreateAuditLogEntry(AuditAction.UpdateEntity, GetCurrentAssociationId(), currentUser != null ? currentUser.Id : user.Id, user, String.Format(ChangedIsApproved, enabled));
		}

		public void ConfirmRegistration(string token)
		{
			var username = GetUsernameByRegistrationToken(token);
			if (String.IsNullOrEmpty(username))
			{
				throw new ServiceException("Could not find username for the registration token [{0}].", token);
			}
			var user = UserDao.FetchByUsername(GetCurrentAssociationId(), username);
			if (user == null)
			{
				throw new ServiceException("Could not find user with username [{0}].", username);
			}
			user.AddRole(GetCurrentAssociationId(), Role.User);
			UserDao.Save(user);
			SetUserEnabled(username, true);
			var ms = ModuleService.GetGlobalModuleSetting(ModuleSettingDefinition.RegistrationEmailComplete, user.Language);
			var msg = new MessageInfo { UserId = user.Id, Subject = ms.Subject };
			var substitutions = new Dictionary<SubstitutionDefinition, string>();
			substitutions[SubstitutionDefinition.UserDisplayName] = user.DisplayName;
			msg.Body = ModuleService.GetModuleSettingValueWithSubstitutions(ms, substitutions);
			MailService.QueueEmail(GetCurrentAssociationId(), msg);
		}

		public UserEntity GetUser(long userId)
		{
			var user = UserDao.Fetch(userId);
			if (user != null)
			{
				GetMembershipUserInfo(user);
			}
			return user;
		}

		public UserEntity GetUser(string username)
		{
			var user = UserDao.FetchByUsername(username);
			if (user != null)
			{
				GetMembershipUserInfo(user);
			}
			return user;
		}

		public UserEntity GetUserByEmail(string email)
		{
			UserEntity retVal = null;
			var username = Membership.GetUserNameByEmail(email);
			if (username.HasText())
			{
				retVal = GetUser(username);
			}
			return retVal;
		}

		public IList<UserEntity> GetUsersByRole(long associationId, Role role)
		{
			var users = UserDao.FetchByRole(associationId, role);
			foreach (var user in users)
			{
				GetMembershipUserInfo(user);
			}
			return users;
		}

		public UserEntity GetCurrentUser()
		{
			return GetUser(HttpContext.Current.User.Identity.Name);
		}

		public IList<UserEntity> GetAllUsers()
		{
			var users = UserDao.FetchActive(GetCurrentAssociationId());
			foreach (var user in users)
			{
				GetMembershipUserInfo(user);
			}
			return users;
		}

		public IList<UserEntity> GetUsers(UserSearchInfo searchInfo)
		{
			var users = UserDao.FetchUsers(searchInfo);
			foreach (var user in users)
			{
				GetMembershipUserInfo(user);
			}
			return users;
		}

		public int GetUserCount(UserSearchInfo searchInfo)
		{
			return UserDao.FetchUserCount(searchInfo);
		}

		public IList<SecurityQuestionEntity> GetSecurityQuestions()
		{
			return LocalizeRegistered(DataAdapter.FetchAll<SecurityQuestionEntity>(BaseEntity.DisplayOrderProperty));
		}

		public void UpdateUser(UserEntity user)
		{
			var provider = Membership.Providers[BusinessConstants.AdminMembershipProvider];
			if (provider == null)
			{
				throw new ServiceException("Could not find admin membership provider [{0}].", BusinessConstants.AdminMembershipProvider);
			}
			var aspNetUser = provider.GetUser(user.Username, false);
			if (aspNetUser == null)
			{
				throw new ServiceException("Could not find ASP.NET user with username [{0}].", user.Username);
			}
			if (user.Password.HasText())
			{
				aspNetUser.ChangePassword(aspNetUser.ResetPassword(), user.Password);
				if (user.SecurityQuestion.HasText() && user.SecurityAnswer.HasText())
				{
					aspNetUser.ChangePasswordQuestionAndAnswer(user.Password, user.SecurityQuestion, user.SecurityAnswer);
				}
			}
			if (aspNetUser.IsLockedOut && !user.LockedOut)
			{
				aspNetUser.UnlockUser();
			}
			if (aspNetUser.Email != user.Email || aspNetUser.IsApproved != user.Approved)
			{
				aspNetUser.IsApproved = user.Approved;
				aspNetUser.Email = user.Email;
				provider.UpdateUser(aspNetUser);
			}
			user.DisplayName = GetDisplayName(user);
			user = SaveWithAudit(user);
			user.Person = SaveWithAudit(user.Person);

			user.Password = String.Empty;
			user.SecurityAnswer = String.Empty;
			AuditingService.CreateAuditLogEntry(AuditAction.UpdateEntity, GetCurrentAssociationId(), GetCurrentUser().Id, user, user.ToString());
		}

		public string GenerateNewPassword(string username)
		{
			var provider = Membership.Providers[BusinessConstants.AdminMembershipProvider];
			if (provider == null)
			{
				throw new ServiceException("Could not find admin membership provider [{0}].", BusinessConstants.AdminMembershipProvider);
			}
			var aspNetUser = provider.GetUser(username, false);
			if (aspNetUser == null)
			{
				throw new ServiceException("Could not find ASP.NET user with username [{0}].", username);
			}
			return aspNetUser.ResetPassword();
		}

		public bool ChangePassword(string username, string currentPassword, string newPassword)
		{
			var retVal = false;
			var aspNetUser = MembershipGetUser(username);
			if (aspNetUser == null)
			{
				throw new ServiceException("Could not find ASP.NET user with username [{0}].", username);
			}
			var user = GetUser(username);
			if (user == null)
			{
				throw new ServiceException("Could not find user entity with username [{0}].", username);
			}
			if (currentPassword.HasText() && newPassword.HasText())
			{
				retVal = aspNetUser.ChangePassword(currentPassword, newPassword);
				var currentUser = GetCurrentUser();
				if (currentUser != null && currentUser.Username.EqualsIgnoreCase(user.Username))
				{
					SendPasswordChangedEmail(user);
				}
				AuditingService.CreateAuditLogEntry(AuditAction.UpdateEntity, GetCurrentAssociationId(), currentUser != null ? currentUser.Id : user.Id, user, ChangedPassword);
			}
			return retVal;
		}

		public bool ChangePasswordWithSecurityAnswer(string username, string securityAnswer, string newPassword)
		{
			var retVal = false;
			var aspNetUser = MembershipGetUser(username);
			if (aspNetUser == null)
			{
				throw new ServiceException("Could not find ASP.NET user with username [{0}].", username);
			}
			var user = GetUser(username);
			if (user == null)
			{
				throw new ServiceException("Could not find user entity with username [{0}].", username);
			}
			try
			{
				retVal = aspNetUser.ChangePassword(aspNetUser.ResetPassword(securityAnswer), newPassword);
				SendPasswordChangedEmail(user);
				var currentUser = GetCurrentUser();
				AuditingService.CreateAuditLogEntry(AuditAction.UpdateEntity, GetCurrentAssociationId(), currentUser != null ? currentUser.Id : user.Id, user, ChangedPassword);
			}
			catch (MembershipPasswordException)
			{
				// ignore, return false
			}
			return retVal;
		}

		public bool ChangeSecurityQuestionAndAnswer(string username, string currentPassword, string securityQuestion, string securityAnswer)
		{
			var retVal = false;
			var aspNetUser = MembershipGetUser(username);
			if (aspNetUser == null)
			{
				throw new ServiceException("Could not find ASP.NET user with username [{0}].", username);
			}
			var user = GetUser(username);
			if (user == null)
			{
				throw new ServiceException("Could not find user entity with username [{0}].", username);
			}
			if (currentPassword.HasText())
			{
				if (securityQuestion.HasText() && securityAnswer.HasText())
				{
					retVal = aspNetUser.ChangePasswordQuestionAndAnswer(currentPassword, securityQuestion, securityAnswer);
					var currentUser = GetCurrentUser();
					AuditingService.CreateAuditLogEntry(AuditAction.UpdateEntity, GetCurrentAssociationId(), currentUser != null ? currentUser.Id : user.Id, user, ChangedSecurityChallege);
				}
			}
			return retVal;
		}

		public PersonEntity GetPerson(long personId)
		{
			return DataAdapter.Fetch<PersonEntity>(personId);
		}

		public IList<PersonEntity> FindPeople(PredicateGroupEntity criteria)
		{
			return PersonDao.Find(criteria);
		}

		public void SendUsernameRequestEmail(UserEntity user)
		{
			user.Guard("user");
			var ms = ModuleService.GetGlobalModuleSetting(ModuleSettingDefinition.LoginEmailUsernameRequested, user.Language);
			var msg = new MessageInfo { UserId = user.Id, Subject = ms.Subject };
			var substitutions = new Dictionary<SubstitutionDefinition, string>();
			substitutions[SubstitutionDefinition.UserDisplayName] = user.DisplayName;
			substitutions[SubstitutionDefinition.Username] = user.Username;
			msg.Body = ModuleService.GetModuleSettingValueWithSubstitutions(ms, substitutions);
			MailService.QueueEmail(GetCurrentAssociationId(), msg);
		}

		public void DeleteUser(UserEntity user)
		{
			user.Guard("user");
			if (!Membership.DeleteUser(user.Username, true))
			{
				//throw new ServiceException("Unable to delete ASP.NET user with username [{0}].", user.Username);
				Log.Error("Unable to delete ASP.NET user with username [{0}].".FormatWith(user.Username));
			}
			user.Deleted = DateTime.Now;
			user.Username = String.Format("{0}-{1}", user.Username, user.Deleted.Value.Ticks);
			UserDao.Save(user);
			AuditingService.CreateAuditLogEntry(AuditAction.DeleteEntity, GetCurrentAssociationId(), user);
			VersionHelper.Delete<PersonEntity, PersonArchiveEntity>(user.Person.Id);
		}

		public long GetCurrentAssociationId()
		{
			return 1;
		}

		public IList<RoleGroupEntity> GetRoleGroupsInUse(long associationId)
		{
			return RoleGroupDao.FetchInUse(associationId);
		}

		public IList<RoleEntity> GetRoles(long associationId, long? roleGroupId)
		{
			return RoleDao.FetchByGroup(associationId, roleGroupId);
		}

		public RoleHomeEntity GetRoleHome(long associationId, long? userId)
		{
			return RoleHomeDao.GetRoleHome(associationId, userId);
		}

		public void Detach(IBaseEntity entity)
		{
			DataAdapter.Evict<UserEntity>(entity);
		}
		
		#endregion

		#region Private Helpers

		private static void GetMembershipUserInfo(UserEntity user)
		{
			var aspNetUser = MembershipGetUser(user.Username);
			if (aspNetUser == null)
			{
				//throw new ServiceException("Could not find ASP.NET user with username [{0}].", user.Username);
				Log.Error("Could not find ASP.NET user with username [{0}].".FormatWith(user.Username));
				return;
			}
			if (aspNetUser.Email.HasText())
			{
				user.Email = aspNetUser.Email;
			}
			if (aspNetUser.PasswordQuestion.HasText())
			{
				user.SecurityQuestion = aspNetUser.PasswordQuestion;
			}
			user.Approved = aspNetUser.IsApproved;
			user.LockedOut = aspNetUser.IsLockedOut;
		}

		private static bool MembershipValidateUser(string username, string password)
		{
			bool retVal = Membership.ValidateUser(username, password);
			if (!retVal && Membership.Provider.Name != BusinessConstants.SqlMembershipProvider)
			{
				retVal = Membership.Providers[BusinessConstants.SqlMembershipProvider].ValidateUser(username, password);
			}
			return retVal;
		}

		private static bool ValidateDemoUser(string username, string password)
		{
			var setting = ModuleService.GetGlobalModuleSetting(ModuleDefinition.Login, "Demo.Usernames");
			if (setting != null)
			{
				return setting.Value.Split(',').Any(x => x.Trim().EqualsIgnoreCase(username));
			}
			return false;
		}

		private static MembershipUser MembershipGetUser(string username)
		{
			var user = Membership.GetUser(username);
			if (user == null && Membership.Provider.Name != BusinessConstants.SqlMembershipProvider)
			{
				user = Membership.Providers[BusinessConstants.SqlMembershipProvider].GetUser(username, false);
			}
			return user;
		}

		private string GetDisplayName(UserEntity user)
		{
			if (user.Language.HasValue)
			{
				var language = DataAdapter.Fetch<LanguageEntity>((long)user.Language);
				if (language.Code != ResourceManager.SystemCulture.Name)
				{
					return ResourceManager.Instance.GetString(CultureInfo.GetCultureInfo(language.Code), PersonEntity.DisplayNameFormatKey,
															  user.Person.FirstName, user.Person.LastName, user.Person.MiddleName).Replace("  ", " ");
				}
			}
			return user.Person.DisplayName;
		}

		private void SendPasswordChangedEmail(UserEntity user)
		{
			user.Guard("user");
			var ms = ModuleService.GetGlobalModuleSetting(ModuleSettingDefinition.LoginEmailPasswordChanged, user.Language);
			var msg = new MessageInfo { UserId = user.Id, Subject = ms.Subject };
			var substitutions = new Dictionary<SubstitutionDefinition, string>();
			substitutions[SubstitutionDefinition.UserDisplayName] = user.DisplayName;
			msg.Body = ModuleService.GetModuleSettingValueWithSubstitutions(ms, substitutions);
			MailService.QueueEmail(GetCurrentAssociationId(), msg);
		}

		#endregion
	}
}
