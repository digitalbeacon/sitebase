// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Models.Comments;
using DigitalBeacon.SiteBase.Web;
using DigitalBeacon.SiteBase.Web.Models;
using DigitalBeacon.Util;
using AutoMapper;
using System.Collections;
using System.Web.Routing;

namespace DigitalBeacon.SiteBase.Controllers
{
	[Authorization(Role.Administrator)]
	public abstract class CommentsController<T> : EntityController<T, EditModel> where T : class, IBaseEntity, new()
	{
		private const string DefaultCommentTypePropertyName = "CommentType";
		private const string DefaultFlaggedPropertyName = "Flagged";

		private string _description = String.Empty;

		protected CommentsController() : base("Comments")
		{
			RequireParentId = true;
			DefaultSort = new[] { new SortItem { Member = Model.PropertyName(x => x.Date), SortDirection = ListSortDirection.Descending } };
		}

		protected abstract string ParentIdProperty { get; }

		protected abstract string GetParentName(long parentId);

		protected virtual string PanelPrefix 
		{
			get { return typeof(T).Name.Replace("CommentEntity", String.Empty).ToCamelCase(); }
		}

		protected virtual bool CanAdd
		{
			get { return true; }
		}

		protected virtual bool CanUpdate
		{
			get { return true; }
		}

		protected virtual bool CanDelete
		{
			get { return true; }
		}

		protected virtual bool ShowTextInline
		{
			get { return true; }
		}

		protected virtual string CommentTypePropertyName
		{
			get { return DefaultCommentTypePropertyName; }
		}

		protected virtual bool CommentTypeRequired
		{
			get { return true; }
		}

		protected virtual string FlaggedPropertyName
		{
			get { return DefaultFlaggedPropertyName; }
		}

		//public ActionResult Text(long id)
		//{
		//	var entity = GetEntity(id);
		//	return Json(entity != null ? entity.Text : String.Empty, JsonRequestBehavior.AllowGet);
		//}
		
		#region EntityController

		protected override string ListView
		{
			get { return "CommentsList"; }
		}

		protected override string EditView
		{
			get { return "CommentsEdit"; }
		}

		protected override string DisplayView
		{
			get { return "CommentsEdit"; }
		}

		protected override string GetIndexUrl(EditModel model)
		{
			return Url.Action(EditActionName, GetType().Name.Replace("Controller", String.Empty).ToCamelCase(), new { id = model.ParentId });
		}

		protected override ListModelBase ConstructListModel()
		{
			return new DigitalBeacon.SiteBase.Models.Comments.ListModel<T>(CommentTypePropertyName, FlaggedPropertyName, ShowTextInline)
			{ 
				PanelPrefix = PanelPrefix.ToCamelCase(),
				CanAdd = this.CanAdd,
				CanDelete = this.CanDelete,
				CanUpdate = this.CanUpdate
			};
		}

		protected override EditModel ConstructUpdateModel(long id)
		{
			var model = base.ConstructUpdateModel(id);
			model.PanelPrefix = PanelPrefix.ToCamelCase();
			return model;
		}

		protected override EditModel ConstructUpdateModel(FormCollection form)
		{
			var model = base.ConstructUpdateModel(form);
			model.PanelPrefix = PanelPrefix.ToCamelCase();
			model.CanDelete = CanDelete;
			return model;
		}

		protected override EditModel ConstructCreateModel()
		{
			return new EditModel<T>(CommentTypePropertyName) { Date = DateTime.Today, ParentId = ParentId.Value, PanelPrefix = PanelPrefix.ToCamelCase(), CommentTypeRequired = this.CommentTypeRequired };
		}

		protected override string GetSortMember(string member)
		{
			if (member == "CommentTypeName")
			{
				return "{0}.Name".FormatWith(CommentTypePropertyName);
			}
			return member;
		}

		protected override string GetDescription(EditModel model)
		{
			if (_description.IsNullOrBlank() && model.ParentId > 0)
			{
				_description = GetLocalizedText("Comments.Description.Format").FormatWith(GetParentName(model.ParentId));
			}
			return _description;
		}

		protected override RouteValueDictionary ConstructRouteValuesForBulkCreate()
		{
			ParentId = Model.ParentId;
			return base.ConstructRouteValuesForBulkCreate();
		}

		protected override SearchInfo<T> PrepareSearchInfo(SearchInfo<T> searchInfo, ListModelBase model)
		{
			searchInfo.AddFilter(ParentIdProperty, model.ParentId);
			return base.PrepareSearchInfo(searchInfo, model);
		}

		protected override IEnumerable<T> GetEntities(SearchInfo<T> searchInfo, ListModelBase model)
		{
			return LookupService.GetEntityList(CurrentAssociationId, searchInfo);
		}

		protected override long GetEntityCount(SearchInfo<T> searchInfo, ListModelBase model)
		{
			return LookupService.GetEntityCount(CurrentAssociationId, searchInfo);
		}

		protected override T GetEntity(long id)
		{
			return LookupService.Get<T>(id);
		}

		protected override T SaveEntity(T entity, EditModel model)
		{
			return LookupService.SaveEntity(CurrentAssociationId, entity);
		}

		protected override void DeleteEntity(long id)
		{
			LookupService.DeleteEntity<T>(id);
		}

		protected override EditModel ConstructModel(T entity)
		{
			var model = new EditModel<T>(CommentTypePropertyName) { CommentTypeRequired = this.CommentTypeRequired };
			Mapper.Map(entity, (EditModel)model);
			model.ParentId = entity.GetPropertyValue<long>(ParentIdProperty);
			return model;
		}

		protected override T ConstructEntity(EditModel model)
		{
			var entity = model.Id == 0 ? new T() : GetEntity(model.Id);
			if (model.IsNew)
			{
				entity.SetPropertyValue(ParentIdProperty, model.ParentId);
			}
			Mapper.Map(model, entity);
			return entity;
		}

		protected override IEnumerable ConstructGridItems(IEnumerable<T> source, ListModelBase model)
		{
			return source.Select(entity => Mapper.Map<ListItem>(entity));
		}

		#endregion
	}
}