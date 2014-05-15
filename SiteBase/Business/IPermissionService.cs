// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Model;
using System.Collections.Generic;

namespace DigitalBeacon.SiteBase.Business
{
	public interface IPermissionService
	{
		/// <summary>
		/// Gets the permission.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns></returns>
		PermissionEntity GetPermission(long id);

		/// <summary>
		/// Gets the permissions.
		/// </summary>
		/// <param name="searchInfo">The search info.</param>
		/// <returns></returns>
		IList<PermissionEntity> GetPermissions(SearchInfo<PermissionEntity> searchInfo);

		/// <summary>
		/// Gets the permission count.
		/// </summary>
		/// <param name="searchInfo">The search info.</param>
		/// <returns></returns>
		long GetPermissionCount(SearchInfo<PermissionEntity> searchInfo);

		/// <summary>
		/// Saves the permission.
		/// </summary>
		/// <param name="permission">The permission.</param>
		/// <returns></returns>
		PermissionEntity SavePermission(PermissionEntity permission);

		/// <summary>
		/// Saves the permissions.
		/// </summary>
		/// <param name="key1">The key1.</param>
		/// <param name="key2">The key2.</param>
		/// <param name="key3">The key3.</param>
		/// <param name="permissions">The permissions.</param>
		void SavePermissions(string key1, long? key2, string key3, IEnumerable<PermissionEntity> permissions);

		/// <summary>
		/// Deletes the permission.
		/// </summary>
		/// <param name="id">The id.</param>
		void DeletePermission(long id);

		/// <summary>
		/// Gets the permissions.
		/// </summary>
		/// <param name="key1">The key1.</param>
		/// <param name="key2">The key2.</param>
		/// <param name="key3">The key3.</param>
		/// <param name="mask">The mask.</param>
		/// <param name="matchNulls">if set to <c>true</c> match nulls.</param>
		/// <returns></returns>
		IList<PermissionEntity> GetPermissions(string key1, long? key2, string key3, Permission? mask, bool matchNulls);

		/// <summary>
		/// Checks the permission.
		/// </summary>
		/// <param name="key1">The key1.</param>
		/// <param name="key2">The key2.</param>
		/// <param name="key3">The key3.</param>
		/// <param name="entityType">Type of the entity.</param>
		/// <param name="entityId">The entity id.</param>
		/// <param name="mask">The mask.</param>
		/// <returns></returns>
		bool? CheckPermission(string key1, long? key2, string key3, EntityType entityType, long entityId, Permission mask);

		/// <summary>
		/// Determines whether the specified entity has a given permission.
		/// </summary>
		/// <param name="key1">The key1.</param>
		/// <param name="key2">The key2.</param>
		/// <param name="key3">The key3.</param>
		/// <param name="entityType">Type of the entity.</param>
		/// <param name="entityId">The entity id.</param>
		/// <param name="mask">The mask.</param>
		/// <returns>
		/// 	<c>true</c> if the specified entity has a given permission; otherwise, <c>false</c>.
		/// </returns>
		bool HasPermission(string key1, long? key2, string key3, EntityType entityType, long entityId, Permission mask);

		/// <summary>
		/// Checks the permission.
		/// </summary>
		/// <param name="user">The user.</param>
		/// <param name="key1">The key1.</param>
		/// <param name="key2">The key2.</param>
		/// <param name="key3">The key3.</param>
		/// <param name="mask">The mask.</param>
		/// <returns></returns>
		bool? CheckPermission(UserEntity user, string key1, long? key2, string key3, Permission mask);

		/// <summary>
		/// Determines whether the specified user has permission.
		/// </summary>
		/// <param name="user">The user.</param>
		/// <param name="key1">The key1.</param>
		/// <param name="key2">The key2.</param>
		/// <param name="key3">The key3.</param>
		/// <param name="mask">The mask.</param>
		/// <returns>
		/// 	<c>true</c> if the specified user has permission; otherwise, <c>false</c>.
		/// </returns>
		bool HasPermission(UserEntity user, string key1, long? key2, string key3, Permission mask);

		/// <summary>
		/// Determines whether the specified user has permission.
		/// </summary>
		/// <param name="user">The user.</param>
		/// <param name="key">The key.</param>
		/// <returns>
		/// 	<c>true</c> if the specified user has permission; otherwise, <c>false</c>.
		/// </returns>
		bool HasPermission(UserEntity user, string key);

		/// <summary>
		/// Determines whether the specified user has permission to the given path.
		/// </summary>
		/// <param name="user">The user.</param>
		/// <param name="path">The path.</param>
		/// <returns>
		/// 	<c>true</c> if the specified user has permission; otherwise, <c>false</c>.
		/// </returns>
		bool HasPermissionToSitePath(UserEntity user, string path);
	}
}
