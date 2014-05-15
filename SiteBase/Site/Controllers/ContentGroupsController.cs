// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using DigitalBeacon.Business;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Business;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Model.Content;
using DigitalBeacon.SiteBase.Models.Content;
using DigitalBeacon.SiteBase.Web;
using DigitalBeacon.SiteBase.Web.Models;
using DigitalBeacon.Util;
using DigitalBeacon.Web;
using Spark;

namespace DigitalBeacon.SiteBase.Controllers
{
	[Precompile("Content/GroupList")]
	[Precompile("Content/GroupEdit")]
	[Authorization(Role.Administrator)]
	public class ContentGroupsController : NamedEntityController<ContentGroupEntity, ContentGroupModel>
	{
		//private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public const string EntriesPageSizeKey = "/contentGroups/{0}/entries/pageSize";

		private static readonly IContentService ContentService = ServiceFactory.Instance.GetService<IContentService>();

		public ContentGroupsController()
		{
			DefaultSort = new[] { new SortItem { Member = BaseEntity.DisplayOrderProperty } };
		}

		public ActionResult Entries(long id, int? page)
		{
			var routeValues = new RouteValueDictionary
			{
				{ WebConstants.ParentControllerRouteValueKey, "contentGroups" },
				{ WebConstants.ParentIdRouteValueKey, id }
			};
			if (page.HasValue)
			{
				routeValues[WebConstants.PageRouteValueKey] = page.Value;
			}
			return new TransferResult(Url.Action(ListActionName, "contentEntries", routeValues));
		}

		#region EntityController

		protected override string ListView
		{
			get { return "Content/GroupList"; }
		}

		protected override string EditView
		{
			get { return "Content/GroupEdit"; }
		}

		protected override string DisplayView		{
			get { return "Content/GroupEdit"; }
		}

		protected override IEnumerable<ContentGroupEntity> GetEntities(SearchInfo<ContentGroupEntity> searchInfo, ListModelBase model)
		{
			return ContentService.GetContentGroups(CurrentAssociationId, false, searchInfo);
		}

		protected override long GetEntityCount(SearchInfo<ContentGroupEntity> searchInfo, ListModelBase model)
		{
			return ContentService.GetContentGroupCount(CurrentAssociationId, false, searchInfo);
		}

		protected override ContentGroupEntity GetEntity(long id)
		{
			return ContentService.GetContentGroup(id);
		}

		protected override ContentGroupEntity SaveEntity(ContentGroupEntity entity, ContentGroupModel model)
		{
			var retVal = ContentService.SaveContentGroup(entity);
			var defaultPageSize = (int)ModuleService.GetGlobalModuleSetting(ModuleSettingDefinition.GlobalListPageSize).ValueAsInt64;
			if (model.PageSize.HasValue && model.PageSize.Value != defaultPageSize)
			{
				PreferenceService.SetPreference(CurrentAssociationId, EntriesPageSizeKey.FormatWith(retVal.Id), model.PageSize.Value.ToString());
			}
			else
			{
				PreferenceService.DeletePreference(CurrentAssociationId, EntriesPageSizeKey.FormatWith(retVal.Id));
			}
			return retVal;
		}

		protected override void DeleteEntity(long id)
		{
			ContentService.DeleteContentGroup(id);
		}

		protected override ListModelBase ConstructListModel()
		{
			return new ListModel<ContentGroupListItem>();
		}

		protected override string GetSortMember(string member)
		{
			if (member == ContentGroupEntity.ContentGroupTypeProperty)
			{
				return GetPropertyName(ContentGroupEntity.ContentGroupTypeProperty, BaseEntity.NameProperty);
			}
			return member;
		}

		protected override ContentGroupModel ConstructModel(ContentGroupEntity entity)
		{
			var pageSizePref = PreferenceService.GetPreference(CurrentAssociationId, EntriesPageSizeKey.FormatWith(entity.Id));
			return new ContentGroupModel
			{
				Id = entity.Id,
				Name = entity.Name,
				Title = entity.Title,
				ContentGroupType = entity.ContentGroupType.Id,
				DisplayOrder = entity.DisplayOrder,
				PageSize = pageSizePref != null ? (int)pageSizePref.ValueAsInt64 :
					(int)ModuleService.GetGlobalModuleSetting(ModuleSettingDefinition.GlobalListPageSize).ValueAsInt64
			};
		}

		protected override ContentGroupEntity ConstructEntity(ContentGroupModel model)
		{
			var entity = model.Id == 0 ? new ContentGroupEntity { AssociationId = CurrentAssociationId } : ContentService.GetContentGroup(model.Id);
			entity.Name = model.Name;
			entity.Title = model.Title;
			entity.ContentGroupType = ContentService.GetContentGroupType((ContentGroupType)model.ContentGroupType.Value);
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

		protected override ContentGroupModel PopulateSelectLists(ContentGroupModel model)
		{
			if (model.ListItems.Count == 0)
			{
				AddSelectList(model, ContentGroupEntity.ContentGroupTypeProperty,
					LookupService.GetNameList<ContentGroupTypeEntity>());
			}
			return model;
		}

		protected override IEnumerable ConstructGridItems(IEnumerable<ContentGroupEntity> source, ListModelBase model)
		{
			return source.Select(entity => new ContentGroupListItem { 
				Id = entity.Id, 
				Name = entity.Name, 
				Title = entity.Title, 
				ContentGroupType = entity.ContentGroupType.Name
			});
		}

		#endregion

		#region NamedEntityController

		protected override ContentGroupEntity GetEntityByName(string name)
		{
			return ContentService.GetContentGroup(CurrentAssociationId, name);
		}

		#endregion
	}
}
