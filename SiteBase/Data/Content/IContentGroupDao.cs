// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Collections.Generic;
using DigitalBeacon.Data;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Model.Content;

namespace DigitalBeacon.SiteBase.Data.Content
{
	/// <summary>
	/// The interface for accessing data associated with content groups
	/// </summary>
	public interface IContentGroupDao : IDao<ContentGroupEntity>, INameDao<ContentGroupEntity>
	{
		/// <summary>
		/// Fetch group by association and name
		/// </summary>
		/// <param name="associationId"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		ContentGroupEntity Fetch(long associationId, string name);

		/// <summary>
		/// Fetches the max display order.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns></returns>
		int FetchMaxDisplayOrder(long id);
	}
}
