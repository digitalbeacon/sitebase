// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using DigitalBeacon.SiteBase.Web.Models;

namespace DigitalBeacon.SiteBase.Models.NavigationItems
{
	public class ListModel : ListModel<ListItem>
	{
		public long Navigation { get; set; }
	}
}