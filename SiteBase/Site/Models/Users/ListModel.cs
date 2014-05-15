// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.ComponentModel;
using DigitalBeacon.SiteBase.Web.Models;

namespace DigitalBeacon.SiteBase.Models.Users
{
	public class ListModel : ListModel<User>
	{
		[ReadOnly(true)]
		public bool ShowRolesLink { get; set; }
	}
}