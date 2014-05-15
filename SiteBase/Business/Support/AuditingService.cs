// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using System.Web;
using DigitalBeacon.Data;
using DigitalBeacon.Model;
using DigitalBeacon.Business;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Data;
using DigitalBeacon.Util;

namespace DigitalBeacon.SiteBase.Business.Support
{
	public class AuditingService : IAuditingService
	{
		#region Private Members

		private static readonly IDataAdapter DataAdapter = ServiceFactory.Instance.GetService<IDataAdapter>();
		private static readonly IUserDao UserDao = ServiceFactory.Instance.GetService<IUserDao>();

		#endregion

		#region IAuditingService Members

		public void CreateAuditLogEntry(AuditAction action, long? associationId, long? userId, IBaseEntity entity, string details)
		{
			var entry = new AuditLogEntity 
			{ 
				AssociationId = associationId == 0 ? GetCurrentAssociationId() : associationId,
				Action = DataAdapter.Fetch<AuditActionEntity>((long)action), 
				User = userId.HasValue ? UserDao.Fetch(userId.Value) : null 
			};
			if (entity != null)
			{
				entry.RefId = entity.Id;
				var type = entity.GetType();
				entry.EntityType = type.Namespace.HasText() ? type.FullName : type.BaseType.FullName;
			}
			if (!String.IsNullOrEmpty(details))
			{
				entry.Details = details;
			}
			CreateAuditLogEntry(entry);
		}

		public void CreateAuditLogEntry(AuditAction action, long? associationId, long? userId, string details)
		{
			CreateAuditLogEntry(action, associationId, userId, null, details);
		}

		public void CreateAuditLogEntry(AuditAction action, long? associationId, string details)
		{
			var currentUser = GetCurrentUser();
			CreateAuditLogEntry(action, associationId, currentUser != null ? (long?)currentUser.Id : null, null, details);
		}

		public void CreateAuditLogEntry(AuditAction action, long? associationId, IBaseEntity entity, string details)
		{
			var currentUser = GetCurrentUser();
			CreateAuditLogEntry(action, associationId, currentUser != null ? (long?)currentUser.Id : null, entity, details);
		}

		public void CreateAuditLogEntry(AuditAction action, long? associationId, IBaseEntity entity)
		{
			var currentUser = GetCurrentUser();
			CreateAuditLogEntry(action, associationId, currentUser != null ? (long?)currentUser.Id : null, entity, entity.ToString());
		}

		public void CreateSaveOrUpdateAuditLogEntry(long? associationId, IBaseEntity entity)
		{
			var currentUser = GetCurrentUser();
			CreateAuditLogEntry(entity.IsNew ? AuditAction.CreateEntity : AuditAction.UpdateEntity, associationId, currentUser != null ? (long?)currentUser.Id : null, entity, entity.ToString());
		}

		public void CreateAuditLogEntry(AuditLogEntity entry)
		{
			if (!entry.IsNew)
			{
				throw new ServiceException("Saving existing audit log entries is not allowed. User attempted to save audit log entry with Id [{0}].", entry.Id);
			}
			DataAdapter.Save(entry);
		}

		public AuditLogEntity GetAuditLogEntry(long id)
		{
			return DataAdapter.Fetch<AuditLogEntity>(id);
		}

		public IList<AuditLogEntity> GetAuditLogEntries(SearchInfo<AuditLogEntity> searchInfo)
		{
			return DataAdapter.FetchList(PrepareSearch(searchInfo));
		}

		public long GetAuditLogCount(SearchInfo<AuditLogEntity> searchInfo)
		{
			return DataAdapter.FetchCount(PrepareSearch(searchInfo));
		}

		#endregion

		#region Private Methods

		private static SearchInfo<AuditLogEntity> PrepareSearch(SearchInfo<AuditLogEntity> searchInfo)
		{
			if (searchInfo.ApplyDefaultFilters)
			{
				searchInfo.AddFilter(x => x.AssociationId, searchInfo.AssociationId).Grouping = -1;
				if (searchInfo.MatchNullAssociations)
				{
					searchInfo.AddFilter(x => x.AssociationId, ComparisonOperator.Null).Grouping = -1;
				}
				if (searchInfo.SearchText.HasText())
				{
					searchInfo.AddFilter(x => x.User.DisplayName, ComparisonOperator.Contains, searchInfo.SearchText).Grouping = 1;
					searchInfo.AddFilter(x => x.User.Username, ComparisonOperator.Contains, searchInfo.SearchText).Grouping = 2;
					searchInfo.AddFilter(x => x.EntityType, ComparisonOperator.Contains, searchInfo.SearchText).Grouping = 3;
					searchInfo.AddFilter(x => x.Details, ComparisonOperator.Contains, searchInfo.SearchText).Grouping = 4;
					var searchDate = searchInfo.SearchText.ToDate();
					if (searchDate.HasValue)
					{
						searchInfo.AddFilter(x => x.Created, ComparisonOperator.GreaterThanOrEqual, searchInfo.SearchText.ToDate().Value).Grouping = 5;
						searchInfo.AddFilter(x => x.Created, ComparisonOperator.LessThanOrEqual, searchInfo.MaxDateForSearchText.Value).Grouping = 5;
					}
				}
				searchInfo.ApplyDefaultFilters = false;
			}
			return searchInfo;
		}

		private static long GetCurrentAssociationId()
		{
			return 1;
			//var user = GetCurrentUser();
			//if (user.Associations != null && user.Associations.Count > 0)
			//{
			//	return user.Associations[0].Id;
			//}
			//var associations = DataAdapter.FetchNameList<AssociationEntity>();
			//if (associations.Count > 0)
			//{
			//	return associations[0].Id;
			//}
			//return null;
		}

		private static UserEntity GetCurrentUser()
		{
			if (HttpContext.Current == null)
			{
				return null;
			}
			return UserDao.FetchByUsername(HttpContext.Current.User.Identity.Name);
		}

		#endregion
	}
}
