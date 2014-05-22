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
	public class CommentTypeListItem : LookupInfo
	{
		[LocalizedDisplayName("Common.Flagged.Label")]
		public bool Flagged { get; set; }
	}
}