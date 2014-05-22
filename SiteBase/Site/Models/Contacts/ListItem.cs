// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Globalization;
using DigitalBeacon.Util;
using DigitalBeacon.Web;

namespace DigitalBeacon.SiteBase.Models.Contacts
{
	public class ListItem
	{
		public long Id { get; set; }
		
		[LocalizedDisplayName("Common.FirstName.Label")]
		public string FirstName { get; set; }

		[LocalizedDisplayName("Common.LastName.Label")]
		public string LastName { get; set; }

		[LocalizedDisplayName("Common.Email.Label")]
		public string Email { get; set; }

		[LocalizedDisplayName("Common.MobilePhone.Label")]
		public string MobilePhone { get; set; }

		[LocalizedDisplayName("Common.DateOfBirth.Label")]
		public DateTime? DateOfBirth { get; set; }

		[LocalizedDisplayName("Common.Age.Label")]
		public int? Age
		{
			get { return DateOfBirth.HasValue ? DateOfBirth.Value.Age() : (int?)null; }
		}

		[LocalizedDisplayName("Common.Inactive.Label")]
		public bool Inactive { get; set; }

		public bool Enabled
		{
			get { return !Inactive; }
			set { Inactive = !value; }
		}

		public bool HasFlaggedComment { get; set; }
	}
}