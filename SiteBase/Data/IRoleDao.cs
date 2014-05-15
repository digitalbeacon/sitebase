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
	public interface IRoleDao : IDao<RoleEntity>
	{
		/// <summary>
		/// Get roles by group
		/// </summary>
		/// <param name="associationId"></param>
		/// <param name="roleGroupId"></param>
		/// <returns></returns>
		IList<RoleEntity> FetchByGroup(long associationId, long? roleGroupId);
	}
}
