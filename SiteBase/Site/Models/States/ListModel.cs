// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Web.Models;

namespace DigitalBeacon.SiteBase.Models.States
{
	public class ListModel : ListModel<StateEntity>
	{
		public long Country { get; set; }
	}
}