// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using DigitalBeacon.Business;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Business;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Models;
using DigitalBeacon.SiteBase.Models.Identity;
using DigitalBeacon.SiteBase.Web;
using DigitalBeacon.Util;
using DigitalBeacon.Web;
using DigitalBeacon.Web.Validation;

namespace DigitalBeacon.SiteBase.Controllers
{
	[Authorization(RequireUnauthenticated=true,Exclude="SignOut")]
	public class IdentityController : SiteBaseController
	{
		public const string PasswordUpdateRequiredUsernameKey = "Identity.PasswordUpdateRequiredUsername";
		public const string PasswordUpdateRequiredPasswordKey = "Identity.PasswordUpdateRequiredPassword";

		public IdentityController()
		{
			MobileModuleName = "identity";
		}

		#region SignIn

		/// <summary>
		/// Displays the sign-in form
		/// </summary>
		/// <param name="returnUrl">The return URL.</param>
		/// <returns></returns>
		public ActionResult SignIn(string returnUrl)
		{
			//if (IsMobile)
			//{
			//	return View(new SignInModel { ReturnUrl = returnUrl, Heading = GetLocalizedText("Identity.SignIn.Heading") });
			//}
			var setting = ModuleService.GetGlobalModuleSetting(ModuleDefinition.Login, "Login.Content");
			var loginHeading = setting != null ? setting.Subject : null;
			var loginContent = setting != null ? setting.Value : null;
			return View(AddTransientMessages(new SignInModel { ReturnUrl = returnUrl, Heading = loginHeading, Content = loginContent }));
		}

		/// <summary>
		/// Handles the sign-in submission
		/// </summary>
		/// <param name="model">The model.</param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult SignIn(SignInModel model)
		{
			ActionResult retVal = null;

			if (ModelState.IsValid)
			{
				switch (IdentityService.AuthenticateUser(model.Username, model.Password))
				{
					case AuthenticationStatus.Success:
						FormsAuthentication.SetAuthCookie(model.Username, false);
						retVal = Redirect(FormsAuthentication.GetRedirectUrl(model.Username, false));
						break;
					case AuthenticationStatus.AccountLocked:
						AddError(model, "Identity.Error.AccountLocked");
						break;
					case AuthenticationStatus.PasswordUpdateRequired:
						Session[PasswordUpdateRequiredUsernameKey] = model.Username;
						Session[PasswordUpdateRequiredPasswordKey] = model.Password;
						retVal = Redirect(Url.Action("ChangePassword", "Identity"));
						break;
					default:
					//case AuthenticationStatus.Failed:
						AddError(model, "Identity.Error.InvalidCredentials");
						break;
				}
			}
			var setting = ModuleService.GetGlobalModuleSetting(ModuleDefinition.Login, "Login.Content");
			model.Heading = setting != null ? setting.Subject : null;
			model.Content = setting != null ? setting.Value : null;
			return retVal ?? View(model);
		}

		#endregion

		#region SignOut

		/// <summary>
		/// Handles the sign-out operation
		/// </summary>
		/// <returns></returns>
		public ActionResult SignOut()
		{
			IdentityService.SignOutUser();
			Session.Abandon();
			FormsAuthentication.SignOut();
			return Redirect(FormsAuthentication.DefaultUrl);
		}

		#endregion

		#region Register

		/// <summary>
		/// Displays the registration form
		/// </summary>
		/// <returns></returns>
		public ActionResult Register()
		{
			if (!(ModuleService.GetGlobalModuleSetting(ModuleSettingDefinition.RegistrationEnabled).ValueAsBoolean ?? false))
			{
				return new HttpForbiddenResult();
			}
			return View(PopulateListItems(new RegisterModel {
				ShowMiddleName = ModuleService.GetGlobalModuleSetting(ModuleSettingDefinition.IdentityShowMiddleName).ValueAsBoolean ?? false,
				RequireAddress = ModuleService.GetGlobalModuleSetting(ModuleSettingDefinition.IdentityRequireAddress).ValueAsBoolean ?? false
			}));
		}

		/// <summary>
		/// Handles the registration submission
		/// </summary>
		/// <param name="model">The model.</param>
		/// <returns></returns>
		[CaptchaValidator]
		[HttpPost]
		public ActionResult Register(FormCollection form)
		{
			if (!(ModuleService.GetGlobalModuleSetting(ModuleSettingDefinition.RegistrationEnabled).ValueAsBoolean ?? false))
			{
				return new HttpForbiddenResult();
			}
			ActionResult retVal = null;
			var model = new RegisterModel
			{
				RequireAddress = ModuleService.GetGlobalModuleSetting(ModuleSettingDefinition.IdentityRequireAddress).ValueAsBoolean ?? false
			};
			TryUpdateModel(model);
			if (ModelState.IsValid && ValidateCaptcha())
			{
				if (WebConstants.UseEmailForUsername)
				{
					model.Username = model.Email;
				}
				var response = IdentityService.RegisterUser(ConstructUserEntity(model), true);
				if (response.Status == RegistrationStatus.Success)
				{
					AddTransientMessage("Identity.Register.Confirmation");
					retVal = Redirect(FormsAuthentication.DefaultUrl);
				}
				else if (response.Status == RegistrationStatus.DuplicateUsername)
				{
					AddPropertyValidationError(UserEntity.UsernameProperty, "Identity.Error.DuplicateUsername");
				}
				else if (response.Status == RegistrationStatus.DuplicateEmail)
				{
					AddPropertyValidationError(UserEntity.EmailProperty, "Users.Error.DuplicateEmail");
				}
				else
				{
					throw new Exception(response.ToString());
				}
			}
			if (retVal == null)
			{
				model.ShowMiddleName =
					ModuleService.GetGlobalModuleSetting(ModuleSettingDefinition.IdentityShowMiddleName).ValueAsBoolean ?? false;
			}
			return retVal ?? View(PopulateListItems(model));
		}

		/// <summary>
		/// Validates the username.
		/// </summary>
		/// <param name="username">The username.</param>
		/// <returns></returns>
		public ActionResult ValidateUsername(string username)
		{
			return Json(IdentityService.IsUsernameAvailable(username) && IdentityService.GetUserByEmail(username) == null, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// Validates the email.
		/// </summary>
		/// <param name="email">The email.</param>
		/// <returns></returns>
		public ActionResult ValidateEmail(string email)
		{
			return Json(IdentityService.GetUserByEmail(email) == null, JsonRequestBehavior.AllowGet);
		}

		private RegisterModel PopulateListItems(RegisterModel model)
		{
			if (model.ListItems.Count == 0)
			{
				var countries = LookupService.GetNameList<CountryEntity>();
				if (countries.Count > 1)
				{
					AddSelectList(model, AddressEntity.CountryProperty, countries);
				}
				else if (countries.Count == 1 && !model.Country.HasValue)
				{
					model.Country = countries[0].Id;
				}
				if (model.Country.HasValue)
				{
					var stateSearch = new SearchInfo<StateEntity> { ApplyDefaultFilters = false };
					stateSearch.AddFilter(x => x.Country.Id, model.Country.Value);
					AddSelectList(model, AddressEntity.StateProperty, LookupService.GetEntityList(CurrentAssociationId, stateSearch));
				}
				else
				{
					AddSelectList(model, AddressEntity.StateProperty, Enumerable.Empty<StateEntity>());
				}
				model.ListItems[UserEntity.SecurityQuestionProperty] = new SelectList(
					IdentityService.GetSecurityQuestions(),
					SecurityQuestionEntity.TextProperty, SecurityQuestionEntity.TextProperty);
			}
			return model;
		}

		private UserEntity ConstructUserEntity(RegisterModel model)
		{
			var language = LookupService.GetByCode<LanguageEntity>(ResourceManager.ClientCulture.Name);
			return new UserEntity
			{
				Username = model.Username,
				Password = model.Password,
				Email = model.Email,
				Language = language != null ? (Language)language.Id : (Language?)null,
				SecurityQuestion = model.SecurityQuestion,
				SecurityAnswer = model.SecurityAnswer,
				Associations = new List<AssociationEntity> { LookupService.Get<AssociationEntity>(CurrentAssociationId) },
				Person = new PersonEntity
				{
					FirstName = model.FirstName,
					MiddleName = model.MiddleName,
					LastName = model.LastName,
					Address = new AddressEntity
					{
						Country = model.Country.HasValue ? (Country)model.Country.Value : (Country?)null,
						Line1 = model.Line1,
						Line2 = model.Line2,
						City = model.City,
						State = model.State.HasValue ? (State)model.State.Value : (State?)null,
						PostalCode = model.PostalCode,
						Email = model.Email
					}
				}
			};
		}

		#endregion

		#region Confirm Registration

		/// <summary>
		/// Displays the confirm registration form
		/// </summary>
		/// <returns></returns>
		public ActionResult ConfirmRegistration(string username, string token)
		{
			var model = new ConfirmRegistrationModel { Username = username, Token = token };
			if (model.Token.HasText())
			{
				return ConfirmRegistration(model);
			}
			return View(model);
		}

		/// <summary>
		/// Handles the confirm registration submission
		/// </summary>
		/// <param name="model">The model.</param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult ConfirmRegistration(ConfirmRegistrationModel model)
		{
			ActionResult retVal = null;

			if (ModelState.IsValid)
			{
				var username = IdentityService.GetUsernameByRegistrationToken(model.Token);
				if (username.HasText() && (model.Username.IsNullOrBlank() || username.EqualsIgnoreCase(model.Username)))
				{
					IdentityService.ConfirmRegistration(model.Token);
					AddTransientMessage("Identity.ConfirmRegistration.Confirmation");
					retVal = Redirect(FormsAuthentication.LoginUrl);
				}
				else
				{
					AddError(model, "Identity.ConfirmRegistration.Failed");
				}
			}
			return retVal ?? View(model);
		}

		#endregion

		#region Change Password

		/// <summary>
		/// Displays the change password form
		/// </summary>
		/// <returns></returns>
		public ActionResult ChangePassword()
		{
			var model = PopulateListItems(new ChangePasswordModel());
			var username = Session[PasswordUpdateRequiredUsernameKey] as string;
			if (username.HasText()
				&& Session[PasswordUpdateRequiredPasswordKey] != null
				&& IdentityService.GetUser(username) != null)
			{
				model.Username = username;
				AddMessage(model, "Identity.Message.PasswordUpdateRequired");
			}
			else
			{
				throw new Exception("Data for action not available.");
			}
			return View(model);
		}

		/// <summary>
		/// Handles the change password action
		/// </summary>
		/// <param name="form">The form.</param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult ChangePassword(FormCollection form)
		{
			ActionResult retVal = null;
			var model = new ChangePasswordModel();
			var username = Session[PasswordUpdateRequiredUsernameKey] as string;
			if (username.HasText()
				&& Session[PasswordUpdateRequiredPasswordKey] != null
				&& IdentityService.GetUser(username) != null)
			{
				model.Username = username;
				AddMessage(model, "Identity.Message.PasswordUpdateRequired");
			}
			else
			{
				throw new Exception("Data for action not available.");
			}

			TryUpdateModel(model);

			if (ModelState.IsValid)
			{
				var currentPassword = Session[PasswordUpdateRequiredPasswordKey] as string;
				if (IdentityService.ChangeSecurityQuestionAndAnswer(model.Username, currentPassword, model.SecurityQuestion, model.SecurityAnswer))
				{
					if (IdentityService.ChangePassword(model.Username, currentPassword, model.Password))
					{
						AddTransientMessage("Identity.Message.PasswordAndSecurityQuestionChanged");
						FormsAuthentication.SetAuthCookie(model.Username, false);
						retVal = Redirect(FormsAuthentication.GetRedirectUrl(model.Username, false));
					}
					else
					{
						throw new Exception("Error updating password.");
					}
				}
				else
				{
					throw new Exception("Error updating challenge question and response.");
				}
				if (retVal != null)
				{
					Session.Remove(PasswordUpdateRequiredUsernameKey);
					Session.Remove(PasswordUpdateRequiredPasswordKey);
					if (IdentityService.AuthenticateUser(model.Username, model.Password) != AuthenticationStatus.Success)
					{
						throw new Exception("Could not authenticate user after required password update.");
					}
				}
			}
			return retVal ?? View(PopulateListItems(model));
		}

		private ChangePasswordModel PopulateListItems(ChangePasswordModel model)
		{
			if (model.ListItems.Count == 0)
			{
				model.ListItems[UserEntity.SecurityQuestionProperty] = new SelectList(
					IdentityService.GetSecurityQuestions(),
					SecurityQuestionEntity.TextProperty, SecurityQuestionEntity.TextProperty);
			}
			return model;
		}

		#endregion

		#region Recover Username

		/// <summary>
		/// Displays the recover username form
		/// </summary>
		/// <returns></returns>
		public ActionResult RecoverUsername()
		{
			if (WebConstants.UseEmailForUsername)
			{
				return new HttpForbiddenResult();
			}
			return View(new RecoverUsernameModel());
		}

		/// <summary>
		/// Handles the recover username submission
		/// </summary>
		/// <param name="model">The model.</param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult RecoverUsername(RecoverUsernameModel model)
		{
			if (WebConstants.UseEmailForUsername)
			{
				return new HttpForbiddenResult();
			}

			ActionResult retVal = null;

			if (ModelState.IsValid)
			{
				var user = IdentityService.GetUserByEmail(model.Email);
				if (user == null || !user.Approved)
				{
					AddPropertyValidationError(UserEntity.EmailProperty, "Identity.Error.EmailNotFound");
				}
				else
				{
					IdentityService.SendUsernameRequestEmail(user);
					AddTransientMessage("Identity.RecoverUsername.Confirmation");
					retVal = Redirect(FormsAuthentication.LoginUrl);
				}
			}
			return retVal ?? View(model);
		}

		#endregion

		#region Reset Password

		/// <summary>
		/// Displays the reset password form
		/// </summary>
		/// <returns></returns>
		public ActionResult ResetPassword()
		{
			return View(new ResetPasswordModel.StepOne());
		}

		/// <summary>
		/// Handles the reset password submission
		/// </summary>
		/// <param name="form">The form.</param>
		/// <returns></returns>
		[CaptchaValidator]
		[HttpPost]
		public ActionResult ResetPassword(FormCollection form)
		{
			ActionResult retVal;

			switch (Convert.ToInt32(form[ResetPasswordModel.StepProperty]))
			{
				default:
					var stepOneModel = new ResetPasswordModel.StepOne();
					TryUpdateModel(stepOneModel);
					retVal = ResetPassword(stepOneModel);
					break;
				case 2:
					var stepTwoModel = new ResetPasswordModel.StepTwo();
					TryUpdateModel(stepTwoModel);
					retVal = ResetPassword(stepTwoModel);
					break;
			}

			return retVal;
		}

		private ActionResult ResetPassword(ResetPasswordModel.StepOne model)
		{
			ActionResult retVal = null;

			if (ModelState.IsValid)
			{
				var user = IdentityService.GetUser(model.Username);
				if (user != null && user.Approved)
				{
					var stepTwoModel = new ResetPasswordModel.StepTwo
					{
						Username = user.Username, 
						SecurityQuestion = user.SecurityQuestion
					};
					if (stepTwoModel.SecurityQuestion.IsNullOrBlank())
					{
						AddPropertyValidationError(String.Empty, "Identity.Error.ResetPassword.NoSecurityQuestion");
					}
					else
					{
						retVal = View("ResetPasswordStepTwo", stepTwoModel);
					}
				}
				else
				{
					AddPropertyValidationError(UserEntity.UsernameProperty, "Identity.Error.UsernameNotFound");
				}
			}
			return retVal ?? View(model);
		}

		private ActionResult ResetPassword(ResetPasswordModel.StepTwo model)
		{
			ActionResult retVal = null;

			if (model.GoBack)
			{
				ModelState.Clear();
				retVal = View(model);
			}
			else if (ModelState.IsValid && ValidateCaptcha())
			{
				if (IdentityService.ChangePasswordWithSecurityAnswer(model.Username, model.SecurityAnswer, model.Password))
				{
					AddTransientMessage("Identity.ResetPassword.Confirmation");
					retVal = Redirect(FormsAuthentication.LoginUrl);
				}
				else
				{
					AddPropertyValidationError(UserEntity.SecurityAnswerProperty, "Identity.Error.SecurityAnswer.Incorrect");
				}
			}
			if (retVal == null)
			{
				// repopulate the security question
				var user = IdentityService.GetUser(model.Username);
				if (user != null)
				{
					model.SecurityQuestion = user.SecurityQuestion;
				}
				else
				{
					throw new Exception("Could not get user entity to re-populate the model.");
				}
			}
			return retVal ?? View("ResetPasswordStepTwo", model);
		}

		#endregion
	}
}
