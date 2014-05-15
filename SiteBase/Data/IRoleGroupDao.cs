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
	public interface IRoleGroupDao : IDao<RoleGroupEntity>
	{
		/// <summary>
		/// Get role groups that are referenced by association
		/// </summary>
		/// <param name="associationId"></param>
		/// <returns></returns>
		IList<RoleGroupEntity> FetchInUse(long associationId);
	}
}
