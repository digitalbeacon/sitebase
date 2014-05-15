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
	/// The interface for accessing data associated with content entries
	/// </summary>
	public interface IContentEntryDao : IDao<ContentEntryEntity>
	{
		/// <summary>
		/// Retrieve a list of entries to display for content group
		/// </summary>
		/// <param name="contentGroupId"></param>
		/// <returns></returns>
		IList<ContentEntryEntity> FetchForDisplay(long contentGroupId);

		/// <summary>
		/// Fetch the max used display order for entries associated with the given content group.
		/// </summary>
		/// <param name="contentGroupId"></param>
		/// <returns></returns>
		int FetchMaxDisplayOrder(long contentGroupId);
	}
}
