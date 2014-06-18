// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using DigitalBeacon.Business;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Business;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Models;
using DigitalBeacon.SiteBase.Models.Account;
using DigitalBeacon.SiteBase.Web;
using DigitalBeacon.Util;

namespace DigitalBeacon.SiteBase.Controllers
{
	[Authorization(false)]
	public class AccountController : SiteBaseController
	{
		//private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private static readonly IMessageService MsgService = ServiceFactory.Instance.GetService<IMessageService>();

		public AccountController()
		{
			MobileModuleName = "account";
		}

		#region Index

		/// <summary>
		/// Indexes this instance.
		/// </summary>
		/// <returns></returns>
		public ActionResult Index()
		{
			return View("PartialNav", AddTransientMessages(new PartialNavModel { Url = "~/account" }));
		}

		#endregion

		#region Change Password

		/// <summary>
		/// Displays the change password form
		/// </summary>
		/// <returns></returns>
		public ActionResult ChangePassword()
		{
			return View(new ChangePasswordModel { Username = CurrentUsername });
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

			var model = new ChangePasswordModel { Username = CurrentUsername };
			TryUpdateModel(model);
			if (ModelState.IsValid)
			{
				if (IdentityService.ChangePassword(model.Username, model.CurrentPassword, model.NewPassword))
				{
					AddTransientMessage("Identity.Message.PasswordChanged");
					retVal = RedirectToAction(DefaultActionName);
				}
				else
				{
					AddPropertyValidationError(ChangePasswordModel.CurrentPasswordProperty, "Identity.Error.CurrentPassword.Incorrect");
				}
			}
			return retVal ?? View(model);
		}

		#endregion

		#region Change Security Question

		/// <summary>
		/// Displays the change security question form
		/// </summary>
		/// <returns></returns>
		public ActionResult ChangeSecurityQuestion()
		{
			return View(PopulateListItems(new ChangeSecurityQuestionModel()));
		}

		/// <summary>
		/// Handles the change security question action
		/// </summary>
		/// <param name="model">The model.</param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult ChangeSecurityQuestion(ChangeSecurityQuestionModel model)
		{
			ActionResult retVal = null;

			if (ModelState.IsValid)
			{
				if (IdentityService.ChangeSecurityQuestionAndAnswer(
					CurrentUsername, model.CurrentPassword, model.SecurityQuestion, model.SecurityAnswer))
				{
					AddTransientMessage("Identity.Message.SecurityQuestionChanged");
					retVal = RedirectToAction(DefaultActionName);
				}
				else
				{
					AddPropertyValidationError(ChangePasswordModel.CurrentPasswordProperty, "Identity.Error.CurrentPassword.Incorrect");
				}
			}
			return retVal ?? View(PopulateListItems(model));
		}

		private ChangeSecurityQuestionModel PopulateListItems(ChangeSecurityQuestionModel model)
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

		#region Update Profile

		/// <summary>
		/// Displays the update profile form
		/// </summary>
		/// <returns></returns>
		public ActionResult UpdateProfile()
		{
			AllowJsonGet = true;
			var model = ConstructUpdateProfileModel(IsMobile && !RenderJson);
			return View(AddTransientMessages(RenderJson ? model : PopulateListItems(model)));
		}

		/// <summary>
		/// Handles the update profile submission
		/// </summary>
		/// <param name="form">The form.</param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult UpdateProfile(FormCollection form)
		{
			ActionResult retVal = null;
			var model = new UpdateProfileModel
			{
				RequireAddress = ModuleService.GetGlobalModuleSetting(ModuleSettingDefinition.IdentityRequireAddress).ValueAsBoolean ?? false
			};
			TryUpdateModel(model);
			if (ModelState.IsValid)
			{
				var user = IdentityService.GetUserByEmail(model.Email);
				if (user != null && user.Id != CurrentUserId)
				{
					AddPropertyValidationError(UserEntity.EmailProperty, "Account.Error.DuplicateEmail");
				}
				else
				{
					IdentityService.UpdateUser(ConstructUserEntity(model));
					if (model.NotificationPreference.HasText())
					{
						PreferenceService.SetUserPreference(CurrentAssociationId, CurrentUserId, 
							MessagingConstants.NotificationPreferenceKey, model.NotificationPreference);
					}
					else
					{
						PreferenceService.DeleteUserPreference(CurrentAssociationId, CurrentUserId, 
							MessagingConstants.NotificationPreferenceKey);
					}
					AddTransientMessage("Account.UpdateProfile.Confirmation");
					var routeValues = new RouteValueDictionary();
					if (model.Language != null)
					{
						routeValues[BusinessConstants.PersistentLanguageKey] = model.Language.Value;
					}
					retVal = RedirectToAction(DefaultActionName, routeValues);
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
		/// Validates the email.
		/// </summary>
		/// <param name="email">The email.</param>
		/// <returns></returns>
		public ActionResult ValidateEmail(string email)
		{
			var user = IdentityService.GetUserByEmail(email);
			return Json((user == null || user.Id == CurrentUserId), JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// Extends the session.
		/// </summary>
		/// <returns></returns>
		public ActionResult ExtendSession()
		{
			return Json(true, JsonRequestBehavior.AllowGet);
		}

		private UpdateProfileModel ConstructUpdateProfileModel(bool forTemplate)
		{
			var model = new UpdateProfileModel
			{
				ShowMiddleName = ModuleService.GetGlobalModuleSetting(ModuleSettingDefinition.IdentityShowMiddleName).ValueAsBoolean ?? false,
				RequireAddress = ModuleService.GetGlobalModuleSetting(ModuleSettingDefinition.IdentityRequireAddress).ValueAsBoolean ?? false
			};
			if (forTemplate)
			{
				return model;
			}
			var user = CurrentUser;
			model.FirstName = user.Person.FirstName;
			model.MiddleName = user.Person.MiddleName;
			model.LastName = user.Person.LastName;
			model.Email = user.Email;
			model.Language = user.Language.HasValue ? (long)user.Language.Value : (long?)null;

			if (user.Person.Address != null)
			{
				if (user.Person.Address.Country.HasValue)
				{
					model.Country = (long)user.Person.Address.Country.Value;
				}
				model.Line1 = user.Person.Address.Line1;
				model.Line2 = user.Person.Address.Line2;
				model.City = user.Person.Address.City;
				model.PostalCode = user.Person.Address.PostalCode;
				if (user.Person.Address.State.HasValue)
				{
					model.State = (long)user.Person.Address.State.Value;
				}
			}
			var pref = PreferenceService.GetUserPreference(
				CurrentAssociationId, CurrentUserId, MessagingConstants.NotificationPreferenceKey);
			if (pref != null)
			{
				model.NotificationPreference = pref.Value;
			}
			return model;
		}

		private UpdateProfileModel PopulateListItems(UpdateProfileModel model)
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
				var languages = LookupService.GetNameList<LanguageEntity>();
				if (languages.Count > 1)
				{
					AddSelectList(model, UserEntity.LanguageProperty, languages);
				}
				else if (languages.Count == 1 && !model.Language.HasValue)
				{
					model.Language = languages[0].Id;
				}
				AddSelectList(model, UpdateProfileModel.NotificationPreferenceProperty,
							  MsgService.GetNotificationPreferences());
			}
			return model;
		}

		private UserEntity ConstructUserEntity(UpdateProfileModel model)
		{
			var user = CurrentUser;
			user.Language = model.Language.HasValue ? (Language)model.Language.Value : (Language?)null;
			user.Person.FirstName = model.FirstName;
			user.Person.MiddleName = model.MiddleName;
			user.Person.LastName = model.LastName;
			if (user.Person.Address == null)
			{
				user.Person.Address = new AddressEntity();
			}
			user.Person.Address.Country = model.Country.HasValue ? (Country)model.Country.Value : (Country?)null;
			user.Person.Address.Line1 = model.Line1;
			user.Person.Address.Line2 = model.Line2;
			user.Person.Address.City = model.City;
			user.Person.Address.State = model.State.HasValue ? (State)model.State.Value : (State?)null;
			user.Person.Address.PostalCode = model.PostalCode;
			if (!WebConstants.UseEmailForUsername)
			{
				user.Email = model.Email;
				user.Person.Address.Email = model.Email;
			}
			return user;
		}

		#endregion
	}
}
