// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Collections.Generic;
using DigitalBeacon.Data;
using DigitalBeacon.SiteBase.Model;

namespace DigitalBeacon.SiteBase.Data
{
	/// <summary>
	/// The interface for accessing data associated with secure message folders
	/// </summary>
	public interface INavigationItemDao : IDao<NavigationItemEntity>
	{
		/// <summary>
		/// Fetches the children.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns></returns>
		IList<NavigationItemEntity> FetchChildren(long id);

		/// <summary>
		/// Fetches the items that are parent candidates for the given item.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="associationId">The association id.</param>
		/// <param name="navigationId">The navigation id.</param>
		/// <returns></returns>
		IList<NavigationItemEntity> FetchParentCandidates(long id, long associationId, long navigationId);

		/// <summary>
		/// Fetches navigation items
		/// </summary>
		/// <param name="associationId">The association id.</param>
		/// <param name="nav">The nav.</param>
		/// <param name="userId">The user id.</param>
		/// <returns></returns>
		IList<NavigationItemEntity> FetchItems(long associationId, Navigation nav, long? userId);
	}
}
