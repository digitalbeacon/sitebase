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
using System.Web.Mvc;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Models.NavigationItems;
using DigitalBeacon.SiteBase.Web;
using DigitalBeacon.SiteBase.Web.Models;
using DigitalBeacon.Util;

namespace DigitalBeacon.SiteBase.Controllers
{
	[Authorization(Role.Administrator)]
	public class NavigationItemsController : EntityController<NavigationItemEntity, EditModel>
	{
		public NavigationItemsController()
		{
			DefaultSort = new[] 
			{ 
				//new SortItem { Member = GetPropertyName(NavigationItemEntity.NavigationProperty, BaseEntity.NameProperty) }, 
				new SortItem { Member = GetPropertyName(NavigationItemEntity.ParentProperty, NavigationItemEntity.TextProperty) }, 
				new SortItem { Member = NavigationItemEntity.DisplayOrderProperty } 
			};
		}

		public ActionResult GetParentCandidates(long id, long navigation)
		{
			return Json(ModuleService.GetParentCandidatesForNavigationItem(id, CurrentAssociationId, navigation)
						.Select(x => new { x.Id, x.Text }), JsonRequestBehavior.AllowGet);
		}

		#region EntityController

		protected override string GetDescription(EditModel model)
		{
			return model.Text;
		}

		protected override string GetSortMember(string member)
		{
			if (member == NavigationItemEntity.NavigationProperty)
			{
				return GetPropertyName(NavigationItemEntity.NavigationProperty, BaseEntity.NameProperty);
			}
			if (member == NavigationItemEntity.ParentProperty)
			{
				return GetPropertyName(NavigationItemEntity.ParentProperty, NavigationItemEntity.TextProperty);
			}
			return member;
		}

		protected override EditModel PopulateSelectLists(EditModel model)
		{
			if (model.ListItems.Count == 0)
			{
				AddSelectList(model, NavigationItemEntity.NavigationProperty,
					LookupService.GetEntityList<NavigationEntity>(CurrentAssociationId));
				if (model.Navigation.HasValue)
				{
					model.ListItems[NavigationItemEntity.ParentProperty] =
						ModuleService.GetParentCandidatesForNavigationItem(model.Id, CurrentAssociationId, model.Navigation.Value)
							.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Text });
				}
				else
				{
					model.ListItems[NavigationItemEntity.ParentProperty] = new SelectListItem[0];
				}
			}
			return model;
		}

		protected override ListModelBase ConstructListModel()
		{
			var model = new ListModel
			{
				Navigation = GetParamAsString(NavigationItemEntity.NavigationProperty).ToInt64() ?? (long)Navigation.TopLeft
			};
			AddSelectList(model, NavigationItemEntity.NavigationProperty,
						  LookupService.GetNameList<NavigationEntity>());
			return model;
		}

		protected override EditModel ConstructCreateModel()
		{
			var retVal = base.ConstructCreateModel();
			retVal.Navigation = GetParamAsString(NavigationItemEntity.NavigationProperty).ToInt64() ?? (long)Navigation.TopLeft;
			retVal.Enabled = true;
			return retVal;
		}

		protected override EditModel ConstructModel(NavigationItemEntity entity)
		{
			return new EditModel
			{
				Id = entity.Id,
				Enabled = entity.Enabled,
				Parent = entity.Parent != null ? entity.Parent.Id : (long?)null,
				Navigation = entity.Navigation != null ? entity.Navigation.Id : (long?)null,
				Text = entity.Text,
				Url = entity.Url,
				ImageUrl = entity.ImageUrl,
				DisplayOrder = entity.DisplayOrder
			};
		}

		protected override NavigationItemEntity ConstructEntity(EditModel model)
		{
			var entity = base.ConstructEntity(model);
			entity.Enabled = model.Enabled ?? false;
			if (model.Parent != null)
			{
				entity.Parent = GetEntity(model.Parent.Value);
				entity.Navigation = entity.Parent.Navigation;
			}
			else
			{
				entity.Parent = null;
				entity.Navigation = LookupService.Get<NavigationEntity>(model.Navigation ?? (long)Navigation.TopLeft);
			}
			entity.Text = model.Text;
			entity.Url = model.Url;
			entity.ImageUrl = model.ImageUrl.DefaultTo((string)null);
			entity.DisplayOrder = model.DisplayOrder ?? 1;
			return entity;
		}

		protected override IEnumerable ConstructGridItems(IEnumerable<NavigationItemEntity> source, ListModelBase listModel)
		{
			return source.Select(entity => new ListItem
			{
				Id = entity.Id,
				Enabled = entity.Enabled,
				Parent = entity.Parent != null ? entity.Parent.Text : String.Empty,
				Navigation = entity.Navigation != null ? entity.Navigation.Name : String.Empty,
				Text = entity.Text,
				Url = entity.Url,
				DisplayOrder = entity.DisplayOrder
			});
		}

		protected override SearchInfo<NavigationItemEntity> PrepareSearchInfo(SearchInfo<NavigationItemEntity> searchInfo, ListModelBase model)
		{
			searchInfo.MatchNullAssociations = true;
			searchInfo.AddFilter(x => x.Navigation.Id, ((ListModel)model).Navigation);
			return base.PrepareSearchInfo(searchInfo, model);
		}

		protected override IEnumerable<NavigationItemEntity> GetEntities(SearchInfo<NavigationItemEntity> searchInfo, ListModelBase model)
		{
			return ModuleService.GetNavigationItems(searchInfo);
		}

		protected override long GetEntityCount(SearchInfo<NavigationItemEntity> searchInfo, ListModelBase model)
		{
			return ModuleService.GetNavigationItemCount(searchInfo);
		}

		protected override NavigationItemEntity GetEntity(long id)
		{
			return ModuleService.GetNavigationItem(id);
		}

		protected override void DeleteEntity(long id)
		{
			ModuleService.DeleteNavigationItem(id, true);
		}

		protected override NavigationItemEntity SaveEntity(NavigationItemEntity entity, EditModel model)
		{
			return ModuleService.SaveNavigationItem(entity);
		}

		#endregion
	}
}
