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
using System.Web.Routing;
using AutoMapper;
using DigitalBeacon.Business;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Business;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Model.Content;
using DigitalBeacon.SiteBase.Models.Content;
using DigitalBeacon.SiteBase.Web;
using DigitalBeacon.SiteBase.Web.Models;
using DigitalBeacon.Util;
using Spark;

namespace DigitalBeacon.SiteBase.Controllers
{
	[Precompile("Content/EntryList")]
	[Precompile("Content/EntryEdit")]
	[Authorization(Role.Administrator)]
	public class ContentEntriesController : EntityController<ContentEntryEntity, ContentEntryModel>
	{
		private static readonly IContentService ContentService = ServiceFactory.Instance.GetService<IContentService>();

		static ContentEntriesController()
		{
			Mapper.CreateMap<ContentEntryEntity, ContentEntryListItem>();
			//Mapper.CreateMap<ContentEntryEntity, ContentEntryModel>();
		}

		public ContentEntriesController()
		{
			DefaultSort = new[] { new SortItem { Member = BaseEntity.DisplayOrderProperty } };
			RequireParentId = true;
		}

		#region EntityController implementation

		protected override string ListView
		{
			get { return "Content/EntryList"; }
		}

		protected override string EditView
		{
			get { return "Content/EntryEdit"; }
		}

		protected override string DisplayView
		{
			get { return "Content/EntryEdit"; }
		}

		protected override string GetIndexUrl(ContentEntryModel model)
		{
			return Url.Action("entries", new
			{
				controller = "contentGroups",
				id = model.ContentGroup.HasText() ? Convert.ToInt64(model.ContentGroup) : 0
			});
		}

		protected override string GetListHeading()
		{
			return "{0} - {1}".FormatWith(PluralLabel, ContentService.GetContentGroup(ParentId.Value).Name);
		}

		protected override string GetDescription(ContentEntryModel model)
		{
			return model.Title;
		}

		protected override IEnumerable<ContentEntryEntity> GetEntities(SearchInfo<ContentEntryEntity> searchInfo, ListModelBase model)
		{
			return ContentService.GetEntries(ParentId.Value, false, searchInfo);
		}

		protected override long GetEntityCount(SearchInfo<ContentEntryEntity> searchInfo, ListModelBase model)
		{
			return ContentService.GetEntryCount(ParentId.Value, false, searchInfo);
		}

		protected override ContentEntryEntity GetEntity(long id)
		{
			return ContentService.GetEntry(id);
		}

		protected override ContentEntryEntity SaveEntity(ContentEntryEntity entity, ContentEntryModel model)
		{
			entity.LastModificationDate = DateTime.Now;
			return ContentService.SaveEntry(entity);
		}

		protected override void DeleteEntity(long id)
		{
			ContentService.DeleteEntry(id);
		}

		protected override ContentEntryModel ConstructCreateModel()
		{
			var model = new ContentEntryModel();
			if (ParentId.HasValue)
			{
				model.ContentGroup = ParentId.Value.ToString(); 
			}
			return model;
		}

		protected override RouteValueDictionary ConstructRouteValuesForBulkCreate()
		{
			ParentId = Entity.ContentGroup.Id;
			return base.ConstructRouteValuesForBulkCreate();
		}

		protected override ContentEntryModel ConstructModel(ContentEntryEntity entity)
		{
			var model = new ContentEntryModel
			{
				Id = entity.Id,
				ContentGroup = entity.ContentGroup.Id.ToString(),
				ContentDate = entity.ContentDate,
				Title = entity.Title,
				Body = entity.Body,
				DisplayOrder = entity.DisplayOrder
			};
			return model;
		}

		protected override ContentEntryEntity ConstructEntity(ContentEntryModel model)
		{
			var entity = model.Id == 0 ? new ContentEntryEntity() : ContentService.GetEntry(model.Id);
			entity.ContentDate = model.ContentDate;
			entity.Title = model.Title;
			entity.Body = Server.HtmlDecode(model.Body);
			entity.ContentGroup = ContentService.GetContentGroup(Convert.ToInt64(model.ContentGroup));
			if (model.DisplayOrder == null)
			{
				entity.DisplayOrder = 0;
				entity.CalculateDisplayOrder = true;
			}
			else
			{
				entity.DisplayOrder = model.DisplayOrder.Value;
			}
			return entity;
		}

		protected override ListModelBase ConstructListModel()
		{
			return new ListModel<ContentEntryListItem>();
		}

		protected override IEnumerable ConstructGridItems(IEnumerable<ContentEntryEntity> source, ListModelBase model)
		{
			return source.Select(x => Mapper.Map<ContentEntryListItem>(x));
		}

		protected override ContentEntryModel PopulateSelectLists(ContentEntryModel model)
		{
			if (model.ListItems.Count == 0)
			{
				AddSelectList(model, ContentEntryEntity.ContentGroupProperty,
					ContentService.GetContentGroups(CurrentAssociationId, false, null));
			}
			return model;
		}

		#endregion
	}
}
