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
	public interface IPredicateGroupDao : IDao<PredicateGroupEntity>
	{
		/// <summary>
		/// Fetches predicate group for specified parameters.
		/// </summary>
		/// <param name="entityType">Type of the entity.</param>
		/// <param name="associationId">The association id.</param>
		/// <param name="userId">The user id.</param>
		/// <returns></returns>
		IList<PredicateGroupEntity> Fetch(EntityType entityType, long associationId, long? userId);
	}
}
