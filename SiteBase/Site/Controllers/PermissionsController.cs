// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Models.Permissions;
using DigitalBeacon.SiteBase.Web;
using DigitalBeacon.SiteBase.Web.Models;
using DigitalBeacon.Util;

namespace DigitalBeacon.SiteBase.Controllers
{
	[Authorization(Role.Administrator)]
	public class PermissionsController : EntityController<PermissionEntity, EditModel>
	{
		#region EntityController

		public ActionResult Manage(string keyType, string keyVal)
		{
			var model = ConstructModel(new PermissionEntity { Key1 = keyType, Key2 = keyVal.ToInt64(), Key3 = keyVal.IsInt64() ? null : keyVal.DefaultTo((string)null) });
			model.Heading = GetEditHeading(model);
			model.Sequencer = GetParamAsString(EntityModel.SequencerProperty);
			return View(EditView, AddTransientMessages(PopulateSelectLists(model)));
		}

		protected override string GetDescription(EditModel model)
		{
			return String.Empty;
		}

		protected override ListModelBase ConstructListModel()
		{
			return new ListModel<PermissionItem>();
		}

		protected override IEnumerable ConstructGridItems(IEnumerable<PermissionEntity> source, ListModelBase model)
		{
			return source.Select(x => new PermissionItem
			{
				Id = x.Id,
				Key1 = x.Key1,
				Key2 = x.Key2,
				Key3 = x.Key3,
				EntityType = (long)x.EntityType,
				EntityTypeName = LookupService.GetName<EntityTypeEntity>((long)x.EntityType),
				EntityId = x.EntityId,
				EntityName = GetEntityName(x.EntityType, x.EntityId),
				Mask = x.Mask
			});
		}

		protected override EditModel PopulateSelectLists(EditModel model)
		{
			if (model.ListItems.Count == 0)
			{
				var entityTypes = new[] { EntityType.Role, EntityType.RoleGroup }; //, EntityType.User };
				var roleGroups = new[] { RoleGroup.Everyone, RoleGroup.Authenticated, RoleGroup.Unauthenticated };
				AddSelectList(model, PermissionEntity.EntityTypeProperty,
					LookupService.GetNameList<EntityTypeEntity>().Where(x => entityTypes.Contains((EntityType)x.Id)));
				AddSelectList(model, EntityType.Role.ToString(),
					LookupService.GetEntityList(CurrentAssociationId, new SearchInfo<RoleEntity> { MatchNullAssociations = true }));
				AddSelectList(model, EntityType.RoleGroup.ToString(),
					LookupService.GetEntityList(CurrentAssociationId, new SearchInfo<RoleGroupEntity> { MatchNullAssociations = true })
						.Where(x => roleGroups.Contains((RoleGroup)x.Id)));
			}
			return model;
		}

		protected override IEnumerable<PermissionEntity> GetEntities(SearchInfo<PermissionEntity> searchInfo, ListModelBase model)
		{
			return PermissionService.GetPermissions(searchInfo);
		}

		protected override long GetEntityCount(SearchInfo<PermissionEntity> searchInfo, ListModelBase model)
		{
			return PermissionService.GetPermissionCount(searchInfo);
		}

		protected override PermissionEntity GetEntity(long id)
		{
			return PermissionService.GetPermission(id);
		}

		protected override PermissionEntity SaveEntity(PermissionEntity entity, EditModel model)
		{
			var permissions = Enumerable.Empty<PermissionEntity>();
			var key3 = model.Key3.DefaultTo((string)null);
			if (model.Permissions != null && model.Permissions.Count > 0)
			{
				permissions = model.Permissions.Select(x => new PermissionEntity
				{
					Key1 = model.Key1,
					Key2 = model.Key2,
					Key3 = key3,
					EntityType = (EntityType)x.EntityType,
					EntityId = x.EntityId,
					Mask = x.Mask
				});
			}
			PermissionService.SavePermissions(model.Key1, model.Key2, key3, permissions);
			return null;
		}

		protected override void DeleteEntity(long id)
		{
			PermissionService.DeletePermission(id);
		}

		protected override EditModel ConstructModel(PermissionEntity entity)
		{
			return new EditModel
			{
				Id = entity.Id,
				Key1 = entity.Key1,
				Key2 = entity.Key2,
				Key3 = entity.Key3,
				Permissions = PermissionService.GetPermissions(entity.Key1, entity.Key2, entity.Key3, null, true)
					.Select(x => new PermissionItem {
						EntityType = (long)x.EntityType,
						EntityTypeName = LookupService.GetName<EntityTypeEntity>((long)x.EntityType),
						EntityId = x.EntityId,
						EntityName = GetEntityName(x.EntityType, x.EntityId),
						Mask = x.Mask
					}).ToList()

			};
		}

		private string GetEntityName(EntityType type, long id)
		{
			if (type == EntityType.Role)
			{
				return LookupService.GetName<RoleEntity>(id);
			}
			if (type == EntityType.RoleGroup)
			{
				return LookupService.GetName<RoleGroupEntity>(id);
			}
			if (type == EntityType.User)
			{
				return IdentityService.GetUser(id).DisplayName;
			}
			return id.ToString();
		}

		#endregion
	}
}
