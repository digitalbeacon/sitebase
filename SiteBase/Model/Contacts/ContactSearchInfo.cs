// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using DigitalBeacon.Model;

namespace DigitalBeacon.SiteBase.Model.Contacts
{
	/// <summary>
	/// A struct used to aggregate search parameters
	/// </summary>
	public class ContactSearchInfo : SearchInfo<ContactEntity>
	{
		public long? ContactId { get; set; }
		public ContactType? ContactType { get; set; }
		public bool? Inactive { get; set; }
		public long? CommentTypeId { get; set; }
		public bool? HasFlaggedComment { get; set; }
	}
}