// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using DigitalBeacon.Model;

namespace DigitalBeacon.Data
{
	/// <summary>
	/// Data access interface for archived entities
	/// </summary>
	public interface IArchiveDao<T> where T:IArchivedEntity, new()
	{
		/// <summary>
		/// Retrieve a list of all entities of the specified type by the
		/// reference Id.
		/// </summary>
		/// <returns></returns>
		IList<T> FetchAllByRefId(long refId);
	}
}
