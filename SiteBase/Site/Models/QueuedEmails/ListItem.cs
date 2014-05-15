// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;

namespace DigitalBeacon.SiteBase.Models.QueuedEmails
{
	public class ListItem
	{
		public long Id { get; set; }
		public string Subject { get; set; }
		public string To { get; set; }
		public DateTime? SendDate { get; set; }
		public DateTime? DateProcessed { get; set; }
		public DateTime? DateSent { get; set; }
		public string Error { get; set; }
	}
}