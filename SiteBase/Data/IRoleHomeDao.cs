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
	public interface IRoleHomeDao : IDao<RoleHomeEntity>
	{
		/// <summary>
		/// Gets the role home entity for given user.
		/// </summary>
		/// <param name="associationId">The association id.</param>
		/// <param name="userId">The user id.</param>
		/// <returns></returns>
		RoleHomeEntity GetRoleHome(long associationId, long? userId);
	}
}
