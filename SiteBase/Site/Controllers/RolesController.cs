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
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Models.Roles;
using DigitalBeacon.SiteBase.Web;
using DigitalBeacon.SiteBase.Web.Models;

namespace DigitalBeacon.SiteBase.Controllers
{
	[Authorization(Role.Administrator)]
	public class RolesController : NamedEntityController<RoleEntity, EditModel>
	{
		public RolesController()
		{
			DefaultSort = new[] 
			{ 
				new SortItem { Member = GetPropertyName(RoleEntity.RoleGroupProperty, BaseEntity.NameProperty) }, 
				new SortItem { Member = BaseEntity.NameProperty } 
			};
		}

		#region EntityController

		protected override string GetSortMember(string member)
		{
			if (member == RoleEntity.RoleGroupProperty)
			{
				return GetPropertyName(RoleEntity.RoleGroupProperty, BaseEntity.NameProperty);
			}
			return member;
		}

		protected override EditModel PopulateSelectLists(EditModel model)
		{
			if (model.ListItems.Count == 0)
			{
				var excludedRoleGroups = new[] { RoleGroup.Everyone, RoleGroup.Authenticated, RoleGroup.Unauthenticated };
				AddSelectList(model, RoleEntity.RoleGroupProperty,
					LookupService.GetEntityList(CurrentAssociationId, new SearchInfo<RoleGroupEntity> { MatchNullAssociations = true })
						.Where(x => !excludedRoleGroups.Contains((RoleGroup)x.Id)));
			}
			return model;
		}

		protected override ListModelBase ConstructListModel()
		{
			return new ListModel { ShowRoleGroupsLink = IsAdmin || PermissionService.HasPermissionToSitePath(CurrentUser, "/roleGroups") };
		}

		protected override EditModel ConstructModel(RoleEntity entity)
		{
			return new EditModel
			{
				Id = entity.Id,
				Name = entity.Name,
				RoleGroup = entity.RoleGroup != null ? entity.RoleGroup.Id : (long?)null
			};
		}
		protected override RoleEntity ConstructEntity(EditModel model)
		{
			var entity = base.ConstructEntity(model);
			entity.Name = model.Name;
			entity.RoleGroup = model.RoleGroup.HasValue ? LookupService.Get<RoleGroupEntity>(model.RoleGroup.Value) : null;
			return entity;
		}

		protected override IEnumerable ConstructGridItems(IEnumerable<RoleEntity> source, ListModelBase listModel)
		{
			return source.Select(entity => new ListItem
			{ 
				Id = entity.Id, 
				Name = entity.Name, 
				RoleGroup = entity.RoleGroup != null ? entity.RoleGroup.Name : String.Empty
			});
		}

		#endregion
	}
}
