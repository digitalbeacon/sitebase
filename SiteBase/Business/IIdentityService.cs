// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Collections.Generic;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Model;

namespace DigitalBeacon.SiteBase.Business
{
	/// <summary>
	/// interface for business logic pertaining to identity operations
	/// </summary>
	public interface IIdentityService
	{
		#region User Maintenance

		/// <summary>
		/// Set the user's enabled status
		/// </summary>
		/// <param name="username"></param>
		/// <param name="enabled"></param>
		void SetUserEnabled(string username, bool enabled);

		/// <summary>
		/// Update an existing user
		/// </summary>
		/// <param name="user">The user.</param>
		void UpdateUser(UserEntity user);

		/// <summary>
		/// Generates the new password.
		/// </summary>
		/// <param name="username">The username.</param>
		/// <returns></returns>
		string GenerateNewPassword(string username);

		/// <summary>
		/// Change a user password
		/// </summary>
		/// <param name="username"></param>
		/// <param name="currentPassword"></param>
		/// <param name="newPassword"></param>
		/// <returns></returns>
		bool ChangePassword(string username, string currentPassword, string newPassword);

		/// <summary>
		/// Change a user password with the security answer
		/// </summary>
		/// <param name="username"></param>
		/// <param name="securityAnswer"></param>
		/// <param name="newPassword"></param>
		/// <returns></returns>
		bool ChangePasswordWithSecurityAnswer(string username, string securityAnswer, string newPassword);

		/// <summary>
		/// Change the security question and security answer for a user
		/// </summary>
		/// <param name="username"></param>
		/// <param name="currentPassword"></param>
		/// <param name="securityQuestion"></param>
		/// <param name="securityAnswer"></param>
		/// <returns></returns>
		bool ChangeSecurityQuestionAndAnswer(string username, string currentPassword, string securityQuestion, string securityAnswer);

		/// <summary>
		/// Send username request email
		/// </summary>
		/// <param name="user"></param>
		void SendUsernameRequestEmail(UserEntity user);

		/// <summary>
		/// Delete a user
		/// </summary>
		/// <param name="user"></param>
		void DeleteUser(UserEntity user);

		#endregion

		#region Registration Methods

		/// <summary>
		/// Determines whether username is available.
		/// </summary>
		/// <param name="username">The username.</param>
		/// <returns>
		/// 	<c>true</c> if username is available; otherwise, <c>false</c>.
		/// </returns>
		bool IsUsernameAvailable(string username);

		/// <summary>
		/// Register a new user
		/// </summary>
		/// <param name="user">The user.</param>
		/// <param name="selfRegistration">if set to <c>true</c> [self registration].</param>
		/// <returns></returns>
		RegistrationResponse RegisterUser(UserEntity user, bool selfRegistration, bool sendEmail = true);

		/// <summary>
		/// Gets the username for the given registration token
		/// </summary>
		/// <param name="token"></param>
		/// <returns>
		/// Username of associated user
		/// </returns>
		string GetUsernameByRegistrationToken(string token);

		/// <summary>
		/// Confirm a registration
		/// </summary>
		/// <param name="token"></param>
		void ConfirmRegistration(string token);

		#endregion

		#region Users

		/// <summary>
		/// Get a user by Id
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		UserEntity GetUser(long userId);

		/// <summary>
		/// Get a user by username
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		UserEntity GetUser(string username);

		/// <summary>
		/// Get a user by email
		/// </summary>
		/// <param name="email"></param>
		/// <returns></returns>
		UserEntity GetUserByEmail(string email);

		/// <summary>
		/// Get list of users by specified role
		/// </summary>
		/// <param name="associationId"></param>
		/// <param name="role"></param>
		/// <returns></returns>
		IList<UserEntity> GetUsersByRole(long associationId, Role role);

		/// <summary>
		/// Get currently authenticated user
		/// </summary>
		/// <returns></returns>
		UserEntity GetCurrentUser();

		/// <summary>
		/// Get list of all users
		/// </summary>
		/// <returns></returns>
		IList<UserEntity> GetAllUsers();

		/// <summary>
		/// Get users
		/// </summary>
		/// <param name="searchInfo">The search info.</param>
		/// <returns></returns>
		IList<UserEntity> GetUsers(UserSearchInfo searchInfo);

		/// <summary>
		/// Get the user count.
		/// </summary>
		/// <param name="searchInfo">The search info.</param>
		/// <returns></returns>
		int GetUserCount(UserSearchInfo searchInfo);

		#endregion

		#region Roles

		/// <summary>
		/// Get role groups by association
		/// </summary>
		/// <param name="associationId"></param>
		/// <returns></returns>
		IList<RoleGroupEntity> GetRoleGroupsInUse(long associationId);

		/// <summary>
		/// Get roles by association and role group
		/// </summary>
		/// <param name="associationId"></param>
		/// <param name="roleGroupId"></param>
		/// <returns></returns>
		IList<RoleEntity> GetRoles(long associationId, long? roleGroupId);

		/// <summary>
		/// Gets the role home.
		/// </summary>
		/// <param name="associationId">The association id.</param>
		/// <param name="userId">The user id.</param>
		/// <returns></returns>
		RoleHomeEntity GetRoleHome(long associationId, long? userId);

		#endregion

		#region Other

		/// <summary>
		/// Get person by Id
		/// </summary>
		/// <param name="personId"></param>
		/// <returns></returns>
		PersonEntity GetPerson(long personId);

		/// <summary>
		/// Searches for people who match the specified criteria.
		/// </summary>
		/// <param name="criteria">The criteria.</param>
		/// <returns></returns>
		IList<PersonEntity> FindPeople(PredicateGroupEntity criteria);

		/// <summary>
		/// Get the current association
		/// </summary>
		/// <returns></returns>
		long GetCurrentAssociationId();

		/// <summary>
		/// Get list of security questions
		/// </summary>
		/// <returns></returns>
		IList<SecurityQuestionEntity> GetSecurityQuestions();

		/// <summary>
		/// Authenticates a user by username and password
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		AuthenticationStatus AuthenticateUser(string username, string password);

		/// <summary>
		/// Signs out logged in user
		/// </summary>
		void SignOutUser();

		/// <summary>
		/// Detaches the specified entity from the active NHibernate session.
		/// </summary>
		/// <param name="entity">The entity.</param>
		void Detach(IBaseEntity entity);

		#endregion
	}
}
