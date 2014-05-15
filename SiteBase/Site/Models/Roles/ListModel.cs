// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.ComponentModel;
using DigitalBeacon.SiteBase.Web.Models;

namespace DigitalBeacon.SiteBase.Models.Roles
{
	public class ListModel : ListModel<ListItem>
	{
		[ReadOnly(true)]
		public bool ShowRoleGroupsLink { get; set; }
	}
}