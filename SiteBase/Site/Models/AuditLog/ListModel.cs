// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using DigitalBeacon.SiteBase.Web.Models;

namespace DigitalBeacon.SiteBase.Models.AuditLog
{
	public class ListModel : ListModel<ListItem>
	{
		//public long? AssociationId { get; set; }
		public long? Action { get; set; }
	}
}