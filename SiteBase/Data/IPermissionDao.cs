// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using DigitalBeacon.Data;
using DigitalBeacon.SiteBase.Model;

namespace DigitalBeacon.SiteBase.Data
{
	public interface IPermissionDao : IDao<PermissionEntity>
	{
		/// <summary>
		/// Fetches the permissions based on key.
		/// </summary>
		/// <param name="key1">The key1.</param>
		/// <param name="key2">The key2.</param>
		/// <param name="key3">The key3.</param>
		/// <param name="mask">The mask.</param>
		/// <returns></returns>
		IList<PermissionEntity> FetchList(string key1, long? key2, string key3, Permission? mask);

		/// <summary>
		/// Fetches the permissions based on key.
		/// </summary>
		/// <param name="key1">The key1.</param>
		/// <param name="key2">The key2.</param>
		/// <param name="key3">The key3.</param>
		/// <param name="mask">The mask.</param>
		/// <param name="matchNulls">if set to <c>true</c> match nulls.</param>
		/// <param name="partialMatchForKey3">if set to <c>true</c> perform partial match on key3.</param>
		/// <returns></returns>
		IList<PermissionEntity> FetchList(string key1, long? key2, string key3, Permission? mask, bool matchNulls, bool partialMatchForKey3);

		/// <summary>
		/// Fetches a specific permission.
		/// </summary>
		/// <param name="key1">The key1.</param>
		/// <param name="key2">The key2.</param>
		/// <param name="key3">The key3.</param>
		/// <param name="entityType">Type of the entity.</param>
		/// <param name="entityId">The entity id.</param>
		/// <returns></returns>
		PermissionEntity Fetch(string key1, long? key2, string key3, EntityType entityType, long entityId);
	}
}
