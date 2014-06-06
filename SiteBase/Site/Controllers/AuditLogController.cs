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
using DigitalBeacon.Business;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Business;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Models.AuditLog;
using DigitalBeacon.SiteBase.Web;
using DigitalBeacon.SiteBase.Web.Models;
using DigitalBeacon.Util;

namespace DigitalBeacon.SiteBase.Controllers
{
	[Authorization(Role.Administrator)]
	public class AuditLogController : EntityController<AuditLogEntity, EditModel>
	{
		private static readonly IAuditingService AuditingService = ServiceFactory.Instance.GetService<IAuditingService>();

		public AuditLogController()
		{
			EnableNewAction = false;
			EnableEditAction = false;
			EnableDeleteAction = false;
			DefaultSort = new[] 
			{ 
				new SortItem { Member = AuditLogEntity.CreatedProperty, SortDirection = ListSortDirection.Descending } 
			};
		}

		#region EntityController

		protected override string GetDescription(EditModel model)
		{
			return model.Action;
		}

		protected override string GetSortMember(string member)
		{
			if (member == AuditLogEntity.ActionProperty)
			{
				return GetPropertyName(AuditLogEntity.ActionProperty, BaseEntity.NameProperty);
			}
			if (member == UserEntity.UsernameProperty)
			{
				return GetPropertyName(AuditLogEntity.UserProperty, UserEntity.DisplayNameProperty);
			}
			return member;
		}

		protected override ListModelBase ConstructListModel()
		{
			var model = new ListModel
			{
				Action = GetParamAsString(AuditLogEntity.ActionProperty).ToInt64()
			};
			AddSelectList(model, AuditLogEntity.ActionProperty,
						  LookupService.GetNameList<AuditActionEntity>());
			return model;
		}

		protected override EditModel ConstructModel(AuditLogEntity entity)
		{
			return new EditModel
			{
				Id = entity.Id,
				Created = entity.Created,
				Action = entity.Action.Name,
				Username = entity.User != null ? entity.User.Username : String.Empty,
				EntityType = entity.EntityType,
				RefId = entity.RefId.ToStringSafe(),
				Details = entity.Details
			};
		}

		protected override AuditLogEntity ConstructEntity(EditModel model)
		{
			throw new NotImplementedException();
		}

		protected override IEnumerable ConstructGridItems(IEnumerable<AuditLogEntity> source, ListModelBase listModel)
		{
			return source.Select(entity => new ListItem
			{
				Id = entity.Id,
				Created = entity.Created,
				Action = entity.Action.Name,
				Username = entity.User != null ? entity.User.Username : String.Empty,
				EntityType = entity.EntityType,
				RefId = entity.RefId.ToStringSafe()
			});
		}

		protected override SearchInfo<AuditLogEntity> PrepareSearchInfo(SearchInfo<AuditLogEntity> searchInfo, ListModelBase model)
		{
			var listModel = (ListModel)model;
			if (listModel.Action.HasValue)
			searchInfo.AddFilter(x => x.Action.Id, listModel.Action.Value);
			return base.PrepareSearchInfo(searchInfo, model);
		}

		protected override IEnumerable<AuditLogEntity> GetEntities(SearchInfo<AuditLogEntity> searchInfo, ListModelBase model)
		{
			return AuditingService.GetAuditLogEntries(searchInfo);
		}

		protected override long GetEntityCount(SearchInfo<AuditLogEntity> searchInfo, ListModelBase model)
		{
			return AuditingService.GetAuditLogCount(searchInfo);
		}

		protected override AuditLogEntity GetEntity(long id)
		{
			return AuditingService.GetAuditLogEntry(id);
		}

		protected override void DeleteEntity(long id)
		{
			throw new NotImplementedException();
		}

		protected override AuditLogEntity SaveEntity(AuditLogEntity entity, EditModel model)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
