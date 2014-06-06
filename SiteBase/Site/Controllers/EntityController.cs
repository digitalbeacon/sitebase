// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;
using DigitalBeacon.Business;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Models;
using DigitalBeacon.SiteBase.Web;
using DigitalBeacon.SiteBase.Web.Models;
using DigitalBeacon.Util;
using Telerik.Web.Mvc;

namespace DigitalBeacon.SiteBase.Controllers
{
	public abstract class EntityController<TEntity, TModel> : SiteBaseController
		where TEntity : class, IBaseEntity, new()
		where TModel : EntityModel, new()
	{
		#region Protected Members

		protected const string PageSizeKey = "/{0}/list/pageSize";

		private string _singularLabel;
		private string _pluralLabel;

		protected string BaseName { get; set; }
		protected bool CheckCustomResources { get; set; }
		protected bool RequireParentId { get; set; }
		protected bool EnableListAction { get; set; }
		protected bool EnableShowAction { get; set; }
		protected bool EnableEditAction { get; set; }
		protected bool EnableNewAction { get; set; }
		protected bool EnableDeleteAction { get; set; }
		protected bool ValidationRedirect { get; set; }
		protected IEnumerable<SortItem> DefaultSort { get; set; }
		protected long? ParentId { get; set; }
		protected TEntity Entity { get; set; }
		protected TModel Model { get; set; }

		protected EntityController()
			: this(String.Empty)
		{
		}

		protected EntityController(string baseName)
		{
			BaseName = baseName.DefaultTo(GetType().Name.Replace("Controller", String.Empty));
			EnableListAction = true;
			EnableShowAction = true;
			EnableEditAction = true;
			EnableNewAction = true;
			EnableDeleteAction = true;
		}

		protected string SingularLabel
		{
			get
			{
				if (_singularLabel == null)
				{
					_singularLabel = GetLocalizedText("{0}.Singular.Label".FormatWith(BaseName));
				}
				return _singularLabel;
			}
			set { _singularLabel = value; }
		}

		protected string PluralLabel
		{
			get
			{
				if (_pluralLabel == null)
				{
					_pluralLabel = GetLocalizedText("{0}.Plural.Label".FormatWith(BaseName));
				}
				return _pluralLabel;
			}
			set { _pluralLabel = value; }
		}

		//protected string ConstructSharedViewName(string viewName)
		//{
		//	return "{0}/{1}".FormatWith(BaseName, viewName);
		//}

		/// <summary>
		/// Add property validation error
		/// </summary>
		/// <typeparam name="TProperty">The type of the property.</typeparam>
		/// <param name="propertyExpression">The property expression.</param>
		/// <param name="message">The message.</param>
		/// <param name="args">The args.</param>
		protected void AddPropertyValidationError<TProperty>(Expression<Func<TModel, TProperty>> propertyExpression, string message, params object[] args)
		{
			propertyExpression.Guard("propertyExpression");
			var property = propertyExpression.GetMember();
			if (property == null)
			{
				throw new ArgumentException("Could not get property for expression {0}.".FormatWith(propertyExpression));
			}
			AddPropertyValidationError(property.Name, message, args);
		}

		#endregion

		#region Abstract Members

		protected abstract string GetDescription(TModel model);

		protected abstract IEnumerable<TEntity> GetEntities(SearchInfo<TEntity> searchInfo, ListModelBase model);

		protected abstract long GetEntityCount(SearchInfo<TEntity> searchInfo, ListModelBase model);

		protected abstract TEntity GetEntity(long id);

		protected abstract TModel ConstructModel(TEntity entity);

		protected abstract TEntity SaveEntity(TEntity entity, TModel model);

		protected abstract void DeleteEntity(long id);

		#endregion

		#region Virtual Members

		protected virtual string ListView
		{
			get { return ListViewName; }
		}

		protected virtual string EditView
		{
			get { return EditViewName; }
		}

		protected virtual string DisplayView
		{
			get { return EditViewName; }
		}

		protected virtual string NewView
		{
			get { return EditView; }
		}

		protected virtual string DeleteView
		{
			get { return DeleteViewName; }
		}

		protected virtual string ReturnTextSingular
		{
			get { return GetLocalizedText("Common.ReturnToEntity.Label").FormatWith(SingularLabel); }
		}

		protected virtual string ReturnTextPlural
		{
			get { return GetLocalizedText("Common.ReturnToEntity.Label").FormatWith(PluralLabel); }
		}

		protected virtual string GetListHeading()
		{
			return GetResource("{0}.List.Heading", PluralLabel);
		}

		protected virtual string GetEditHeading(TModel model)
		{
			var description = GetDescription(model);
			return GetResource("{0}.Edit.Heading",
				description.HasText() ? "Common.Edit.Heading" : "Common.Edit.Heading.NoDescription",
				SingularLabel, description);
		}

		protected virtual string GetNewHeading()
		{
			return GetResource("{0}.New.Heading", "Common.New.Heading", SingularLabel);
		}

		protected virtual string GetDeleteHeading(TModel model)
		{
			return GetResource("{0}.Delete.Heading", "Common.Delete.Heading", SingularLabel, GetDescription(model));
		}

		protected virtual ListModelBase ConstructListModel()
		{
			return new ListModel<TEntity>();
		}

		protected virtual SearchInfo<TEntity> ConstructSearchInfo()
		{
			return new SearchInfo<TEntity>();
		}

		protected virtual SearchInfo<TEntity> PrepareSearchInfo(SearchInfo<TEntity> searchInfo, ListModelBase model)
		{
			searchInfo.Page = model.Page;
			searchInfo.PageSize = model.PageSize;
			searchInfo.SearchText = model.SearchText;
			searchInfo.AssociationId = CurrentAssociationId;
			searchInfo.ParentId = model.ParentId;
			searchInfo.ApplyDefaultSorting = false;
			return searchInfo;
		}

		protected virtual IEnumerable ConstructGridItems(IEnumerable<TEntity> source, ListModelBase model)
		{
			return source;
		}

		protected virtual string GetSortMember(string member)
		{
			return member;
		}

		protected virtual ListSortDirection GetSortDirection(string member, ListSortDirection sortDirection)
		{
			return sortDirection;
		}

		protected virtual TModel ConstructUpdateModel(long id)
		{
			var entity = GetEntity(id);
			return ConstructModel(entity);
		}

		protected virtual TModel ConstructUpdateModel(FormCollection form)
		{
			var model = PopulateModelForValidation(new TModel(), form);
			TryUpdateModel(model);
			return model;
		}

		protected virtual TModel ConstructCreateModel()
		{
			return new TModel();
		}

		protected virtual TModel ConstructCreateModel(FormCollection form)
		{
			return ConstructUpdateModel(form);
		}

		protected virtual RouteValueDictionary ConstructRouteValuesForBulkCreate()
		{
			var routeValues = new RouteValueDictionary();
			routeValues[EntityModel.BulkCreateProperty] = true;
			if (ParentId.HasValue)
			{
				routeValues[WebConstants.ParentIdRouteValueKey] = ParentId.Value;
			}
			return routeValues;
		}

		protected virtual TEntity ConstructEntity(TModel model)
		{
			return model.IsNew ? new TEntity() : GetEntity(model.Id);
		}

		protected virtual TModel PopulateModelForValidation(TModel model, FormCollection form)
		{
			return model;
		}

		protected virtual TModel PopulateSelectLists(TModel model)
		{
			return model;
		}

		protected virtual void Validate(TEntity entity, TModel model)
		{
			// default does nothing
		}

		protected virtual void ValidateForDelete(TEntity entity, TModel model)
		{
			// default does nothing
		}

		protected virtual void DeleteEntity(TEntity entity, TModel model)
		{
			DeleteEntity(entity.Id);
		}

		protected ActionResult GetIndexAction(TModel model)
		{
			return new RedirectResult(GetIndexUrl(model));
		}

		protected virtual string GetIndexUrl(TModel model)
		{
			return Url.Action(ListActionName, new { id = String.Empty });
		}

		#endregion

		#region List

		/// <summary>
		/// Displays the list
		/// </summary>
		/// <returns></returns>
		[GridAction]
		public virtual ActionResult Index(GridCommand command, string searchText, long? parentId)
		{
			if (!EnableListAction)
			{
				throw new NotImplementedException("List action is not implemented.");
			}
			if (IsMobile && !RenderTemplate && !RenderJson && MobileModuleName.HasText())
			{
				return View("Index");
			}
			if (RequireParentId)
			{
				parentId.Guard("parentId");
			}
			ParentId = parentId;
			var model = ConstructListModel();
			model.Page = command.Page > 0 ? command.Page : 1;
			model.PageSize = command.PageSize > 0 ? command.PageSize : PreferenceService.GetListPageSize(PageSizeKey.FormatWith(BaseName), CurrentAssociationId, CurrentUser);
			model.SearchText = searchText;
			model.ParentId = parentId;
			if (model.Heading.IsNullOrBlank())
			{
				model.Heading = GetListHeading();
			}
			if (model.SingularLabel.IsNullOrBlank())
			{
				model.SingularLabel = SingularLabel;
			}
			if (model.PluralLabel.IsNullOrBlank())
			{
				model.PluralLabel = PluralLabel;
			}

			if (RenderTemplate)
			{
				return View(ListView, AddTransientMessages(model));
			}

			var searchInfo = ConstructSearchInfo();
			PrepareSearchInfo(searchInfo, model);
			if (command.SortDescriptors.Count > 0)
			{
				foreach (var sort in command.SortDescriptors)
				{
					searchInfo.AddSort(GetSortMember(sort.Member), GetSortDirection(sort.Member, sort.SortDirection));
				}
			}
			else if (DefaultSort != null)
			{
				foreach (var sort in DefaultSort)
				{
					searchInfo.AddSort(GetSortMember(sort.Member), GetSortDirection(sort.Member, sort.SortDirection));
				}
			}
			else
			{
				searchInfo.ApplyDefaultSorting = true;
			}
			model.UntypedItems = ConstructGridItems(GetEntities(searchInfo, model), model);
			model.TotalCount = model.UntypedItems.Cast<object>().LongCount();
			if (model.Page > 1 || model.TotalCount == model.PageSize)
			{
				model.TotalCount = GetEntityCount(searchInfo, model);
			}
			return View(ListView, AddTransientMessages(model));
		}

		/// <summary>
		/// Ajax method for retrieving items
		/// </summary>
		/// <returns></returns>
		[GridAction(EnableCustomBinding = true)]
		public virtual ActionResult Search(GridCommand command, string searchText, long? parentId, int? pageSize, int? page, string sortValue)
		{
			if (!EnableListAction)
			{
				throw new NotImplementedException("Search action is not implemented.");
			}
			if (RequireParentId)
			{
				parentId.Guard("parentId");
			}
			ParentId = parentId;
			var model = ConstructListModel();
			model.Page = page ?? command.Page;
			model.PageSize = pageSize ?? command.PageSize;
			model.SearchText = searchText;
			model.ParentId = parentId;
			var searchInfo = ConstructSearchInfo();
			PrepareSearchInfo(searchInfo, model);

			var sortItems = ParseSortValue(sortValue);
			if (sortItems == null && command.SortDescriptors.Count > 0)
			{
				foreach (var sort in command.SortDescriptors)
				{
					searchInfo.AddSort(GetSortMember(sort.Member), GetSortDirection(sort.Member, sort.SortDirection));
				}
			}
			else
			{
				sortItems = sortItems ?? DefaultSort;
				if (sortItems != null)
				{
					foreach (var sort in sortItems)
					{
						searchInfo.AddSort(GetSortMember(sort.Member), GetSortDirection(sort.Member, sort.SortDirection));
					}
				}
			}
			var entities = GetEntities(searchInfo, model);
			var count = entities.Cast<object>().LongCount();
			var gridModel = new GridModel
			{
				Data = ConstructGridItems(entities, model),
				Total = (int)((model.PageSize > 1 || count == model.PageSize) ? GetEntityCount(searchInfo, model) : count)
			};
			return Json(gridModel, JsonRequestBehavior.AllowGet);
		}

		protected virtual IEnumerable<SortItem> ParseSortValue(string sortValue)
		{
			if (sortValue.IsNullOrBlank())
			{
				return null;
			}
			var list = new List<SortItem>();
			var tokens = sortValue.Split(';');
			foreach (var token in tokens)
			{
				var sortDirection = ListSortDirection.Ascending;
				var member = token;
				if (token.EndsWith("-asc", StringComparison.InvariantCultureIgnoreCase))
				{
					member = token.Substring(0, token.Length - 4);
				}
				else if (token.EndsWith("-desc", StringComparison.InvariantCultureIgnoreCase))
				{
					member = token.Substring(0, token.Length - 5);
					sortDirection = ListSortDirection.Descending;
				}
				list.Add(new SortItem { Member = member, SortDirection = sortDirection });
			}
			return list;
		}

		#endregion

		#region Show

		/// <summary>
		/// The show action.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns></returns>
		public virtual ActionResult Show(long id)
		{
			if (!EnableShowAction)
			{
				throw new NotImplementedException("Show action is not implemented.");
			}
			if (IsMobile && !RenderTemplate && !RenderJson && MobileModuleName.HasText())
			{
				return View("Index");
			}
			var model = RenderTemplate ? ConstructModel(new TEntity()) : ConstructUpdateModel(id);
			model.Heading = GetEditHeading(model);
			model.Sequencer = GetParamAsString(EntityModel.SequencerProperty);
			if (RenderJson)
			{
				return Json(model, JsonRequestBehavior.AllowGet);
			}
			return View(DisplayView, AddTransientMessages(PopulateSelectLists(model)));
		}

		#endregion

		#region Edit/Update

		/// <summary>
		/// Displays the edit action
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns></returns>
		public virtual ActionResult Edit(long id)
		{
			if (!EnableEditAction)
			{
				throw new NotImplementedException("Edit action is not implemented.");
			}
			if (IsMobile && !RenderTemplate && !RenderJson && MobileModuleName.HasText())
			{
				return View("Index");
			}
			var model = RenderTemplate ? ConstructModel(new TEntity()) : ConstructUpdateModel(id);
			model.Heading = GetEditHeading(model);
			model.Sequencer = GetParamAsString(EntityModel.SequencerProperty);
			if (RenderJson)
			{
				return Json(model, JsonRequestBehavior.AllowGet);
			}
			return View(EditView, AddTransientMessages(PopulateSelectLists(model)));
		}

		/// <summary>
		/// Handles the update action
		/// </summary>
		/// <param name="form">The form.</param>
		/// <returns></returns>
		[HttpPut]
		public virtual ActionResult Update(FormCollection form)
		{
			if (form[WebConstants.CancelKey].HasText())
			{
				return GetIndexAction(ConstructUpdateModel(form));
			}
			if (!EnableEditAction)
			{
				throw new NotImplementedException("Update action is not implemented.");
			}
			if (form[WebConstants.DeleteKey].HasText())
			{
				return RedirectToAction(DeleteActionName);
			}
			ActionResult retVal = null;
			var model = ConstructUpdateModel(form);
			if (ModelState.IsValid)
			{
				var entity = ConstructEntity(model);
				if (entity != null && !entity.IsNew)
				{
					IdentityService.Detach(entity);
				}
				Validate(entity, model);
				if (ModelState.IsValid)
				{
					entity = SaveEntity(entity, model);
					if (ModelState.IsValid)
					{
						Model = model;
						Entity = entity;
						var confirmationText = GetResource("{0}.Update.Confirmation", "Common.Update.Confirmation", GetDescription(model), SingularLabel);
						if (RenderJson)
						{
							retVal = Json(new ApiResponse { Success = true, Message = GetSafeFormattedText(confirmationText).ToHtmlString() });
						}
						else if (RenderPartial)
						{
							retVal = RedirectToMessageAction(GetEditHeading(model), confirmationText);
							var routeValues = new RouteValueDictionary();
							routeValues[WebConstants.RenderTypeKey] = WebConstants.RenderTypePartial;
							if (model.Sequencer.HasText())
							{
								routeValues[EntityModel.SequencerProperty] = model.Sequencer;
							}
							if (entity != null)
							{
								MessageModel.ReturnUrl = Url.Action(EditActionName, routeValues);
								MessageModel.ReturnText = ReturnTextSingular;
							}
						}
						else
						{
							AddTransientMessage(confirmationText);
							retVal = GetIndexAction(model);
						}
					}
				}
			}
			if (retVal == null)
			{
				if (RenderJson)
				{
					var response = new ApiResponse();
					foreach (var key in ModelState.Keys)
					{
						var errors = ModelState[key].Errors.Select(x => x.ErrorMessage).ToArray();
						if (errors.Length > 0)
						{
							response.ValidationErrors[key] = errors;
						}
					}
					retVal = Json(response);
				}
				else if (ValidationRedirect)
				{
					retVal = GetValidationRedirect(GetEditHeading(model));
					if (!RenderPartial)
					{
						MessageModel.ReturnUrl = Url.Action(EditActionName);
						MessageModel.ReturnText = ReturnTextSingular;
					}
				}
				else
				{
					model.Heading = GetEditHeading(model);
					Model = model;
					retVal = View(EditView, PopulateSelectLists(model));
				}
			}
			return retVal;
		}

		#endregion

		#region New/Create

		/// <summary>
		/// Displays the create form
		/// </summary>
		/// <returns></returns>
		public virtual ActionResult New(long? parentId)
		{
			if (!EnableNewAction)
			{
				throw new NotImplementedException("New action is not implemented.");
			}
			if (RequireParentId)
			{
				parentId.Guard("parentId");
			}
			if (IsMobile && !RenderTemplate && !RenderJson && MobileModuleName.HasText())
			{
				return View("Index");
			}
			ParentId = parentId;
			var model = ConstructCreateModel();
			model.BulkCreate = GetParamAsString(EntityModel.BulkCreateProperty).ToBoolean() ?? false;
			if (model.BulkCreate)
			{
				TryUpdateModel(model);
				ModelState.Clear();
			}
			model.Heading = GetNewHeading();
			model.SingularLabel = SingularLabel;
			model.PluralLabel = PluralLabel;
			return View(NewView, AddTransientMessages(PopulateSelectLists(model)));
		}

		/// <summary>
		/// Handles the create submission
		/// </summary>
		/// <param name="form">The form.</param>
		/// <returns></returns>
		[HttpPost]
		public virtual ActionResult Create(FormCollection form)
		{
			if (!EnableNewAction)
			{
				throw new NotImplementedException("Create action is not implemented.");
			}
			var model = ConstructCreateModel(form);
			if (model.Cancel.HasText())
			{
				return GetIndexAction(model);
			}
			ActionResult retVal = null;
			if (ModelState.IsValid)
			{
				var entity = ConstructEntity(model);
				Validate(entity, model);
				if (ModelState.IsValid)
				{
					entity = SaveEntity(entity, model);
					if (ModelState.IsValid)
					{
						Model = model;
						Entity = entity;
						var confirmationText = GetResource("{0}.Create.Confirmation", "Common.Create.Confirmation", GetDescription(model), SingularLabel);
						if (model.BulkCreate)
						{
							AddTransientMessage(confirmationText);
							retVal = RedirectToAction(NewActionName, ConstructRouteValuesForBulkCreate());
						}
						else
						{
							if (RenderJson)
							{
								retVal = Json(new ApiResponse { Success = true, Message = GetSafeFormattedText(confirmationText).ToHtmlString(), Id = Entity.Id });
							}
							else if (RenderPartial)
							{
								retVal = RedirectToMessageAction(GetNewHeading(), confirmationText);
								if (entity != null)
								{
									MessageModel.ReturnUrl = Url.Action(EditActionName, new { id = entity.Id, RenderType = WebConstants.RenderTypePartial });
									MessageModel.ReturnText = ReturnTextSingular;
								}
							}
							else
							{
								AddTransientMessage(confirmationText);
								retVal = GetIndexAction(model);
							}
						}
					}
				}
			}
			if (retVal == null)
			{
				if (RenderJson)
				{
					var response = new ApiResponse();
					foreach (var key in ModelState.Keys)
					{
						var errors = ModelState[key].Errors.Select(x => x.ErrorMessage).ToArray();
						if (errors.Length > 0)
						{
							response.ValidationErrors[key] = errors;
						}
					}
					retVal = Json(response);
				}
				else if (ValidationRedirect)
				{
					retVal = GetValidationRedirect(GetNewHeading());
					if (!RenderPartial)
					{
						MessageModel.ReturnUrl = Url.Action(ListActionName, new { id = String.Empty });
						MessageModel.ReturnText = ReturnTextPlural;
					}
				}
				else
				{
					model.Heading = GetNewHeading();
					model.SingularLabel = SingularLabel;
					model.PluralLabel = PluralLabel;
					Model = model;
					retVal = View(NewView, PopulateSelectLists(model));
				}
			}
			return retVal;
		}

		#endregion

		#region Delete

		public virtual ActionResult Delete(long id)
		{
			if (!EnableDeleteAction)
			{
				throw new NotImplementedException("Delete action is not implemented.");
			}
			var model = new TModel { Id = id };
			var entity = GetEntity(id);
			if (entity == null)
			{
				throw new ValidationException("Could not retrieve {0} with id {1}.", typeof(TEntity).Name, model.Id);
			}
			model.Heading = GetDeleteHeading(model);
			model.Description = GetDescription(ConstructModel(entity));
			return View(DeleteView, model);
		}

		[HttpDelete]
		public virtual ActionResult Delete(TModel model)
		{
			if (!EnableDeleteAction)
			{
				throw new NotImplementedException("Delete action is not implemented.");
			}
			if (model.Cancel.HasText())
			{
				return EnableEditAction ? RedirectToAction(EditActionName) : RedirectToAction(ListActionName, new { id = String.Empty });
			}
			ActionResult retVal = null;
			var entity = GetEntity(model.Id);
			if (entity == null)
			{
				throw new ValidationException("Could not retrieve {0} with id {1}.", typeof(TEntity).Name, model.Id);
			}
			model = ConstructModel(entity);
			ModelState.Clear();
			ValidateForDelete(entity, model);
			if (ModelState.IsValid)
			{
				try
				{
					DeleteEntity(entity, model);
					var confirmationText = GetResource("{0}.Delete.Confirmation", "Common.Delete.Confirmation", GetDescription(model), SingularLabel);
					if (RenderJson)
					{
						retVal = Json(new ApiResponse { Success = true, Message = GetSafeFormattedText(confirmationText).ToHtmlString() });
					}
					else if (RenderPartial)
					{
						retVal = RedirectToMessageAction(GetDeleteHeading(model), confirmationText);
					}
					else
					{
						AddTransientMessage(confirmationText);
						retVal = GetIndexAction(model);
					}
				}
				catch (EntityDependencyException)
				{
					var errorMessage = GetResource("{0}.Error.DeleteEntity.Dependency", "Error.DeleteEntity.Dependency");
					if (RenderJson)
					{
						retVal = Json(new ApiResponse { Success = false, ErrorMessage = GetSafeFormattedText(errorMessage).ToHtmlString() });
					}
					else
					{
						retVal = RedirectToErrorAction(GetDeleteHeading(model), errorMessage);
						MessageModel.ReturnUrl = EnableEditAction ? Url.Action(EditActionName) : Url.Action(ListActionName, new { id = String.Empty });
						MessageModel.ReturnText = EnableEditAction ? ReturnTextSingular : ReturnTextPlural;
					}
				}
			}
			if (retVal == null)
			{
				if (RenderJson)
				{
					var response = new ApiResponse();
					foreach (var key in ModelState.Keys)
					{
						var errors = ModelState[key].Errors.Select(x => x.ErrorMessage).ToArray();
						if (errors.Length > 0)
						{
							response.ValidationErrors[key] = errors;
						}
					}
					retVal = Json(response);
				}
				else if (ValidationRedirect)
				{
					retVal = GetValidationRedirect(GetDeleteHeading(model));
					if (!RenderPartial)
					{
						MessageModel.ReturnUrl = EnableEditAction ? Url.Action(EditActionName) : Url.Action(ListActionName, new { id = String.Empty });
						MessageModel.ReturnText = EnableEditAction ? ReturnTextSingular : ReturnTextPlural;
					}
				}
				else
				{
					model.Heading = GetDeleteHeading(model);
					model.Description = GetDescription(model);
					retVal = View(DeleteView, model);
				}
			}
			return retVal;
		}

		#endregion

		#region Private Methods

		private ActionResult GetValidationRedirect(string heading)
		{
			ActionResult retVal = null;
			if (ModelState.ContainsKey(String.Empty))
			{
				var errors = ModelState[String.Empty].Errors;
				if (errors.Count > 0)
				{
					retVal = RedirectToErrorAction(heading, errors[0].ErrorMessage);
				}
				for (var i = 1; i < errors.Count; i++)
				{
					AddError(MessageModel, errors[i].ErrorMessage);
				}
			}
			return retVal ?? RedirectToDefaultErrorAction();
		}

		private string GetResource(string customKey, string defaultKey, params object[] args)
		{
			string text = null;
			if (CheckCustomResources)
			{
				var key = customKey.FormatWith(BaseName);
				var customText = GetLocalizedText(key);
				if (customText != key)
				{
					text = customText;
				}
			}
			if (text == null)
			{
				text = GetLocalizedText(defaultKey);
			}
			return args != null ? text.FormatWith(args) : text;
		}

		#endregion
	}
}
