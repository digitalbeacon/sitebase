// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DigitalBeacon.Business;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Data;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.Util;

namespace DigitalBeacon.SiteBase.Business.Support
{
	public class PermissionService : BaseService, IPermissionService
	{
		#region Private Members

		private readonly IPermissionDao _permissionDao = ServiceFactory.Instance.GetService<IPermissionDao>();

		#endregion

		#region IPermissionService Members

		public PermissionEntity GetPermission(long id)
		{
			return _permissionDao.Fetch(id);
		}

		public IList<PermissionEntity> GetPermissions(SearchInfo<PermissionEntity> searchInfo)
		{
			return _permissionDao.FetchList(PrepareSearch(searchInfo));
		}

		public long GetPermissionCount(SearchInfo<PermissionEntity> searchInfo)
		{
			return _permissionDao.FetchCount(PrepareSearch(searchInfo));
		}

		public PermissionEntity SavePermission(PermissionEntity permission)
		{
			if (permission.IsNew)
			{
				var p = _permissionDao.Fetch(permission.Key1, permission.Key2, permission.Key3, permission.EntityType, permission.EntityId);
				if (p != null)
				{
					permission.Id = p.Id;
					permission.ModificationCounter = p.ModificationCounter;
				}
			}
			return SaveWithAudit(permission);
		}

		public void SavePermissions(string key1, long? key2, string key3, IEnumerable<PermissionEntity> permissions)
		{
			var orphaned = GetPermissions(key1, key2, key3, null, true);
			foreach (var p in permissions)
			{
				var savedPermission = SavePermission(p);
				orphaned.Where(x => x.Id == savedPermission.Id)
					.SingleOrDefault()
					.IfNotNull(x => { orphaned.Remove(x); });
			}
			foreach (var p in orphaned)
			{
				DeleteWithAudit(p);
			}
		}

		public void DeletePermission(long id)
		{
			DeleteWithAudit<PermissionEntity>(id);
		}

		public IList<PermissionEntity> GetPermissions(string key1, long? key2, string key3, Permission? mask, bool matchNulls)
		{
			return _permissionDao.FetchList(key1, key2, key3, mask, matchNulls, false);
		}

		public bool? CheckPermission(string key1, long? key2, string key3, EntityType entityType, long entityId, Permission mask)
		{
			bool? retVal = null;
			var p = _permissionDao.Fetch(key1, key2, key3, entityType, entityId);
			if (p != null)
			{
				retVal = (p.Mask & (int)mask) > 0;
			}
			return retVal;
		}

		public bool HasPermission(string key1, long? key2, string key3, EntityType entityType, long entityId, Permission mask)
		{
			return CheckPermission(key1, key2, key3, entityType, entityId, mask) ?? false;
		}

		public bool? CheckPermission(UserEntity user, string key1, long? key2, string key3, Permission mask)
		{
			var permissions = _permissionDao.FetchList(key1, key2, key3, null);
			return permissions.Count == 0 ? (bool?)null : permissions.Any(x => IsMatch(x, user, mask));
		}

		public bool HasPermission(UserEntity user, string key1, long? key2, string key3, Permission mask)
		{
			var parts = (key3.IsNullOrBlank() || key3[0] != '/') ? null
				: key3.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
			if (parts == null || parts.Length < 2) 
			{
				return CheckPermission(user, key1, key2, key3, mask) ?? ((user != null && user.SuperUser) ? true : false);
			}
			var retVal = false;
			var permissions = _permissionDao.FetchList(key1, key2, "/" + parts[0], null, true, true);
			if (permissions.Count > 0)
			{
				var paths = new string[parts.Length];
				for (var i = 0; i < parts.Length; i++)
				{
					var sb = new StringBuilder();
					for (var j = 0; j <= i; j++)
					{
						sb.Append("/");
						sb.Append(parts[j]);
					}
					paths[parts.Length - i - 1] = sb.ToString();
				}
				foreach (var path in paths)
				{
					var pathPermissions = permissions.Where(x => x.Key3.EqualsIgnoreCase(path)).ToList();
					if (pathPermissions.Count > 0)
					{
						retVal = pathPermissions.Any(x => IsMatch(x, user, mask));
						break;
					}
				}
			}
			return retVal;
		}

		public bool HasPermission(UserEntity user, string key)
		{
			return HasPermission(user, key, null, null, Permission.Access);
		}

		public bool HasPermissionToSitePath(UserEntity user, string path)
		{
			return HasPermission(user, SiteBaseConstants.SitePathKey, null, path, Permission.Access);
		}

		#endregion

		#region Private Helpers

		private static bool IsMatch(PermissionEntity x, UserEntity user, Permission mask)
		{
			if (user != null && user.SuperUser)
			{
				return true;
			}

			var retVal = false;
			if ((x.Mask & (int)mask) > 0)
			{
				if (x.EntityType == EntityType.RoleGroup && x.EntityId == (long)RoleGroup.Everyone)
				{
					retVal = true;
				}
				else if (user == null)
				{
					retVal = x.EntityType == EntityType.RoleGroup && x.EntityId == (long)RoleGroup.Unauthenticated;
				}
				else
				{
					retVal = (x.EntityType == EntityType.User && x.EntityId == user.Id)
						  || (x.EntityType == EntityType.RoleGroup && x.EntityId == (long)RoleGroup.Authenticated)
						  || (x.EntityType == EntityType.Person && x.EntityId == user.Person.Id)
						  || (x.EntityType == EntityType.Role && user.Roles.Select(r => (long)r.Role).Contains(x.EntityId));
				}
			}
			return retVal;
		}

		private static SearchInfo<PermissionEntity> PrepareSearch(SearchInfo<PermissionEntity> searchInfo)
		{
			if (searchInfo.ApplyDefaultFilters)
			{
				if (searchInfo.SearchText.HasText())
				{
					searchInfo.AddFilter(x => x.Key1, ComparisonOperator.Contains, searchInfo.SearchText).Grouping = 1;
					if (searchInfo.SearchText.ToInt64().HasValue)
					{
						searchInfo.AddFilter(x => x.Key2, searchInfo.SearchText.ToInt64().Value).Grouping = 2;
					}
					searchInfo.AddFilter(x => x.Key3, ComparisonOperator.Contains, searchInfo.SearchText).Grouping = 3;
				}
				searchInfo.ApplyDefaultFilters = false;
			}
			return searchInfo;
		}

		#endregion
	}
}
