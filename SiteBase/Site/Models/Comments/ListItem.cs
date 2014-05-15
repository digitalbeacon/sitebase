// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using DigitalBeacon.Web;

namespace DigitalBeacon.SiteBase.Models.Comments
{
	public class ListItem
	{
		public long Id { get; set; }

		[LocalizedDisplayName("Common.Type.Label")]
		public string CommentTypeName { get; set; }

		[LocalizedDisplayName("Common.Date.Label")]
		public DateTime Date { get; set; }

		[LocalizedDisplayName("Comments.Flagged.Label")]
		public bool Flagged { get; set; }

		[LocalizedDisplayName("Comments.Text.Label")]
		public string Text { get; set; }
	}
}