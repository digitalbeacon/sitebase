// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using System.Transactions;
using System.Web.Mvc;
using DigitalBeacon.Business;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Business;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Models;
using DigitalBeacon.SiteBase.Models.Users;
using DigitalBeacon.SiteBase.Web;
using DigitalBeacon.SiteBase.Web.Models;
using DigitalBeacon.Util;
using DigitalBeacon.Web;

namespace DigitalBeacon.SiteBase.Controllers
{
	[Authorization(Role.Administrator)]
	public class UsersController : EntityController<UserEntity, EditModel>
	{
		private static readonly IMessageService MsgService = ServiceFactory.Instance.GetService<IMessageService>();

		/// <summary>
		/// Initializes a new instance of the <see cref="UsersController"/> class.
		/// </summary>
		public UsersController()
		{
			DefaultSort = new[] { new SortItem { Member = UserEntity.UsernameProperty } };
			CheckCustomResources = true;
		}

		#region Common

		/// <summary>
		/// Validates the email.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="email">The email.</param>
		/// <returns></returns>
		public ActionResult ValidateEmail(long id, string email)
		{
			var user = IdentityService.GetUserByEmail(email);
			return Json((user == null || user.Id == id), JsonRequestBehavior.AllowGet);
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

		#endregion

		#region Change Password

		/// <summary>
		/// Displays the change password form
		/// </summary>
		/// <returns></returns>
		public ActionResult ChangePassword(long id)
		{
			return View(new ChangePasswordModel { Username = IdentityService.GetUser(id).Username });
		}

		/// <summary>
		/// Handles the change password action
		/// </summary>
		/// <param name="model">The model.</param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult ChangePassword(ChangePasswordModel model)
		{
			ActionResult retVal = null;
			if (GetParamAsString(WebConstants.CancelKey).HasText())
			{
				retVal = RedirectToAction(EditActionName);
			}
			if (ModelState.IsValid)
			{
				IdentityService.ChangePassword(model.Username, IdentityService.GenerateNewPassword(model.Username), model.NewPassword);
				if (RenderPartial)
				{
					retVal = RedirectToMessageAction(model.Username, "Users.ChangePassword.Confirmation");
				}
				else
				{
					AddTransientMessage("Users.ChangePassword.Confirmation");
					retVal = RedirectToAction(EditActionName);
				}
			}
			return retVal ?? View(model);
		}

		#endregion

		#region EntityController

		public override ActionResult Edit(long id)
		{
			if (!IsAdmin)
			{
				var user = GetEntity(id);
				if (user.SuperUser || user.HasRole(CurrentAssociationId, Role.Administrator))
				{
					return new HttpForbiddenResult();
				}
			}
			return base.Edit(id);
		}

		public override ActionResult Update(FormCollection form)
		{
			var result = base.Update(form);
			if (Model != null)
			{
				Model.ShowMiddleName =
					ModuleService.GetGlobalModuleSetting(ModuleSettingDefinition.IdentityShowMiddleName).ValueAsBoolean ?? false;
			}
			return result;
		}

		public override ActionResult Create(FormCollection form)
		{
			var result = base.Create(form);
			if (Model != null)
			{
				Model.ShowMiddleName =
					ModuleService.GetGlobalModuleSetting(ModuleSettingDefinition.IdentityShowMiddleName).ValueAsBoolean ?? false;
			}
			return result;
		}

		public override ActionResult Delete(long id)
		{
			if (!IsAdmin)
			{
				var user = GetEntity(id);
				if (user.SuperUser || user.HasRole(CurrentAssociationId, Role.Administrator))
				{
					return new HttpForbiddenResult();
				}
			}
			return base.Delete(id);
		}

		//public override ActionResult Delete(EditModel model)
		//{
		//	if (model.Cancel.HasText())
		//	{
		//		return RedirectToAction(EditActionName);
		//	}
		//	if (model.Id == CurrentUserId)
		//	{
		//		var retVal = RedirectToErrorAction(GetDeleteHeading(model), "Users.Error.Delete.CurrentUser");
		//		MessageModel.ReturnUrl = Url.Action(EditActionName);
		//		MessageModel.ReturnText = ReturnTextSingular;
		//		return retVal;
		//	}
		//	return base.Delete(model);
		//}

		protected override string GetDescription(EditModel model)
		{
			return model.Username;
		}

		protected override string NewView
		{
			get { return NewViewName; }
		}

		protected override SearchInfo<UserEntity> ConstructSearchInfo()
		{
			var retVal = new UserSearchInfo { AssociationId = CurrentAssociationId };
			if (!IsAdmin)
			{
				retVal.IsAdmin = false;
			}
			return retVal;
		}

		protected override ListModelBase ConstructListModel()
		{
			return new ListModel { ShowRolesLink = IsAdmin || PermissionService.HasPermissionToSitePath(CurrentUser, "/roles") };
		}

		protected override IEnumerable ConstructGridItems(IEnumerable<UserEntity> source, ListModelBase model)
		{
			return source.Select(x => new User(x));
		}

		protected override EditModel PopulateSelectLists(EditModel model)
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
				AddSelectList(model, EditModel.NotificationPreferenceProperty,
							  MsgService.GetNotificationPreferences());
				var roles = IdentityService.GetRoles(CurrentAssociationId, null);
				if (!IsAdmin)
				{
					roles.Where(x => x.Id == (long)Role.Administrator).SingleOrDefault().IfNotNull(x => { roles.Remove(x); return x; });
				}
				model.RoleGroups[GetLocalizedText("Users.NoRoleGroup.Label")] =
					roles.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name, Selected = model.Roles.Contains(x.Id) });
				var currentAssociationId = CurrentAssociationId;
				foreach (var roleGroup in IdentityService.GetRoleGroupsInUse(currentAssociationId))
				{
					if (!IsAdmin && roleGroup.Id == (long)RoleGroup.Admin)
					{
						continue;
					}
					model.RoleGroups[roleGroup.Name] = IdentityService.GetRoles(currentAssociationId, roleGroup.Id)
						.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name, Selected = model.Roles.Contains(x.Id) });
				}
			}
			return model;
		}

		protected override EditModel ConstructCreateModel()
		{
			var model = new CreateModel { 
				Approved = true,
				SendEmail = true,
				ShowMiddleName = ModuleService.GetGlobalModuleSetting(ModuleSettingDefinition.IdentityShowMiddleName).ValueAsBoolean ?? false,
				RequireAddress = ModuleService.GetGlobalModuleSetting(ModuleSettingDefinition.IdentityRequireAddress).ValueAsBoolean ?? false
			};
			model.Roles.Add((long)Role.User);
			return model;
		}

		protected override EditModel ConstructCreateModel(FormCollection form)
		{
			var model = (CreateModel)PopulateModelForValidation(new CreateModel(), form);
			TryUpdateModel(model);
			if (WebConstants.UseEmailForUsername)
			{
				model.Username = model.Email;
			}
			return model;
		}

		protected override EditModel PopulateModelForValidation(EditModel model, FormCollection form)
		{
			model.RequireAddress = ModuleService.GetGlobalModuleSetting(ModuleSettingDefinition.IdentityRequireAddress).ValueAsBoolean ?? false;
			return model;
		}

		protected override IEnumerable<UserEntity> GetEntities(SearchInfo<UserEntity> searchInfo, ListModelBase model)
		{
			return IdentityService.GetUsers((UserSearchInfo)searchInfo);
		}

		protected override long GetEntityCount(SearchInfo<UserEntity> searchInfo, ListModelBase model)
		{
			return IdentityService.GetUserCount((UserSearchInfo)searchInfo);
		}

		protected override UserEntity GetEntity(long id)
		{
			return IdentityService.GetUser(id);
		}

		protected override EditModel ConstructModel(UserEntity entity)
		{
			var model = new EditModel
			{
				Id = entity.Id,
				Username = entity.Username,
				Email = entity.Email,
				Language = entity.Language.HasValue ? (long)entity.Language.Value : (long?)null,
				Approved = entity.Approved,
				LockedOut = entity.LockedOut,
				FirstName = entity.Person.FirstName,
				MiddleName = entity.Person.MiddleName,
				LastName = entity.Person.LastName,
				ShowMiddleName = ModuleService.GetGlobalModuleSetting(ModuleSettingDefinition.IdentityShowMiddleName).ValueAsBoolean ?? false,
				RequireAddress = ModuleService.GetGlobalModuleSetting(ModuleSettingDefinition.IdentityRequireAddress).ValueAsBoolean ?? false
			};
			if (entity.Person.Address != null)
			{
				if (entity.Person.Address.Country.HasValue)
				{
					model.Country = (long)entity.Person.Address.Country.Value;
				}
				model.Line1 = entity.Person.Address.Line1;
				model.Line2 = entity.Person.Address.Line2;
				model.City = entity.Person.Address.City;
				model.PostalCode = entity.Person.Address.PostalCode;
				if (entity.Person.Address.State.HasValue)
				{
					model.State = (long)entity.Person.Address.State.Value;
				}
			}
			model.Roles = entity.Roles.Select(x => (long)x.Role).ToList();
			var pref = PreferenceService.GetUserPreference(
				CurrentAssociationId, model.Id, MessagingConstants.NotificationPreferenceKey);
			if (pref != null)
			{
				model.NotificationPreference = pref.Value;
			}
			return model;
		}

		protected override UserEntity ConstructEntity(EditModel model)
		{
			var user = model.Id == 0 ? new UserEntity() : GetEntity(model.Id);
			user.Language = model.Language.HasValue ? (Language)model.Language.Value : (Language?)null;
			user.Approved = model.Approved;
			user.LockedOut = model.LockedOut;
			if (user.Person == null)
			{
				user.Person = new PersonEntity();
			}
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
			var currentAssociationId = CurrentAssociationId;
			PopulateSelectLists(model);
			foreach (var list in model.RoleGroups.Values)
			{
				foreach (var roleId in list.Select(x => Convert.ToInt64(x.Value)))
				{
					if (model.Roles.Contains(roleId))
					{
						user.AddRole(currentAssociationId, (Role)roleId);
					}
					else
					{
						user.RemoveRole(currentAssociationId, (Role)roleId);
					}
				}
			}
			return user;
		}

		protected override void Validate(UserEntity entity, EditModel model)
		{
			if ((entity.SuperUser || entity.HasRole(CurrentAssociationId, Role.Administrator)) && !IsAdmin)
			{
				ModelState.AddModelError(String.Empty, entity.IsNew ? "Users.Error.Create.Admin" : "Users.Error.Update.Admin");
				ValidationRedirect = true;
				return;
			}
			var user = IdentityService.GetUserByEmail(model.Email);
			if (user != null && user.Id != model.Id)
			{
				AddPropertyValidationError(UserEntity.EmailProperty, "Users.Error.DuplicateEmail");
			}
			if (model.Id == 0 && !IdentityService.IsUsernameAvailable(model.Username))
			{
				AddPropertyValidationError(UserEntity.UsernameProperty, "Identity.Error.DuplicateUsername");
			}
		}

		protected override void ValidateForDelete(UserEntity entity, EditModel model)
		{
			if (model.Id == CurrentUserId)
			{
				ModelState.AddModelError(String.Empty, "Users.Error.Delete.CurrentUser");
				ValidationRedirect = true;
				return;
			}
			if ((entity.SuperUser || entity.HasRole(CurrentAssociationId, Role.Administrator)) && !IsAdmin)
			{
				ModelState.AddModelError(String.Empty, "Users.Error.Delete.Admin");
				ValidationRedirect = true;
				return;
			}
		}

		protected override UserEntity SaveEntity(UserEntity entity, EditModel model)
		{
			//using (var tx = new TransactionScope())
			//{
				if (model.Id == 0)
				{
					var createModel = (CreateModel)model;
					entity.Username = createModel.Username;
					entity.Password = createModel.Password;
					entity.Email = createModel.Email;
					entity.Person.Address.Email = createModel.Email;
					entity.Associations = new List<AssociationEntity> { LookupService.Get<AssociationEntity>(CurrentAssociationId) };
					var response = IdentityService.RegisterUser(entity, false, createModel.SendEmail ?? false);
					if (response.Status != RegistrationStatus.Success)
					{
						throw new Exception(response.Status.ToString());
					}
					entity.Id = response.UserId;
				}
				else
				{
					IdentityService.UpdateUser(entity);
				}
				if (model.NotificationPreference.HasText())
				{
					PreferenceService.SetUserPreference(CurrentAssociationId, entity.Id,
														MessagingConstants.NotificationPreferenceKey, model.NotificationPreference);
				}
				else if (model.Id > 0)
				{
					PreferenceService.DeleteUserPreference(CurrentAssociationId, model.Id,
														   MessagingConstants.NotificationPreferenceKey);
				}
			//	tx.Complete();
			//}
			return entity;
		}

		protected override void DeleteEntity(long id)
		{
			IdentityService.DeleteUser(GetEntity(id));
		}

		#endregion
	}
}
