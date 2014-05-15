// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;

namespace DigitalBeacon.SiteBase.Models.Content
{
	public class ContentEntryListItem
	{
		public long Id { get; set; }
		public string Title { get; set; }
		public DateTime LastModificationDate { get; set; }
		public DateTime? ContentDate { get; set; }
	}
}
