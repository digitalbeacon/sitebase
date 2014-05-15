// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using DigitalBeacon.SiteBase.Web.Models;

namespace DigitalBeacon.SiteBase.Models.QueuedEmails
{
	public class ListModel : ListModel<ListItem>
	{
		public const string SentProperty = "Sent";
		public const string HasErrorProperty = "HasError";

		public string Sent { get; set; }
		public string HasError { get; set; }
	}
}