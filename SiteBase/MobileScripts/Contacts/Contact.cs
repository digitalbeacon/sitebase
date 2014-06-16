// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;

namespace DigitalBeacon.SiteBase.Mobile.Contacts
{
	[ScriptObjectLiteral]
	public class Contact
	{
		public long PhotoId;
		public bool Inactive;
		public string FirstName;
		public string LastName;
		public string MiddleName;
		public long GenderId;
		public Date DateOfBirth;
		public long RaceId;
		public int Age;
		public string GenderDisplayValue;
		public bool Enabled;
		public string DisplayName;
		public string CommentDate;
		public string CommentType;
		public string CommentText;

		public int CountryId;
		public string Line1;
		public string Line2;
		public string City;
		public int StateId;
		public string PostalCode;
		public string County;
		public string Email;
		public int DefaultPhoneId;
		public string HomePhone;
		public string WorkPhone;
		public string MobilePhone;
		public string Fax;
		public string CountryDisplayValue;
		public string StateDisplayValue;
		public string PhoneDisplayValue;

		public long Id;

		public bool Success;
	}
}