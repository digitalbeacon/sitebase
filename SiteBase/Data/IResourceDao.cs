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
	public interface IResourceDao : IDao<ResourceEntity>
	{
		/// <summary>
		/// Fetches the unique types.
		/// </summary>
		/// <returns></returns>
		IList<string> FetchUniqueTypes();
	}
}
