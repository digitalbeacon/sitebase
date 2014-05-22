// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using DigitalBeacon.SiteBase.Web.Models;
using DigitalBeacon.Web;

namespace DigitalBeacon.SiteBase.Models.Contacts
{
	public class ListModel : ListModel<ListItem>
	{
		public bool? Inactive { get; set; }
		public long? CommentTypeId { get; set; }
		public bool CanDelete { get; set; }
	}
}