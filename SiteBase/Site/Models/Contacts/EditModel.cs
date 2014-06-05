// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.ComponentModel;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Model.Contacts;
using DigitalBeacon.Web;
using DigitalBeacon.Web.Validation;

namespace DigitalBeacon.SiteBase.Models.Contacts
{
	public class EditModel : AddressModel
	{
		#region Address Overrides

		//[Required]
		//public override string Line1 { get; set; }

		//[Required]
		//public override string PostalCode { get; set; }

		//[Required]
		//public override string City { get; set; }

		//[Required]
		//public override long? StateId { get; set; }

		[LocalizedDisplayName("Common.Phone.Label")]
		public override string MobilePhone { get; set; }

		#endregion

		[ReadOnly(true)]
		public bool CanDelete { get; set; }

		[ReadOnly(true)]
		public long PhotoId { get; set; }

		[LocalizedDisplayName("Common.Photo.Label")]
		public string Photo { get; set; }

		[LocalizedDisplayName("Common.Inactive.Label")]
		public bool? Inactive { get; set; }

		[Required]
		[StringLength(ContactEntity.FirstNameMaxLength)]
		[LocalizedDisplayName("Common.FirstName.Label")]
		public virtual string FirstName { get; set; }

		[Required]
		[StringLength(ContactEntity.LastNameMaxLength)]
		[LocalizedDisplayName("Common.LastName.Label")]
		public virtual string LastName { get; set; }

		[StringLength(ContactEntity.MiddleNameMaxLength)]
		[LocalizedDisplayName("Common.MiddleName.Label")]
		public virtual string MiddleName { get; set; }

		[LocalizedDisplayName("Common.Gender.Label")]
		public virtual long? GenderId { get; set; }

		[LocalizedDisplayName("Common.DateOfBirth.Label")]
		public virtual DateTime? DateOfBirth { get; set; }

		[LocalizedDisplayName("Races.Singular.Label")]
		public virtual long? RaceId { get; set; }

		[ReadOnly(true)]
		[LocalizedDisplayName("Common.Age.Label")]
		public virtual int? Age { get; set; }

		[ReadOnly(true)]
		public virtual Gender? Gender
		{
			get { return GenderId.HasValue ? (Gender)GenderId.Value : (Gender?)null; }
			set { GenderId = value.HasValue ? (long)value.Value : (long?)null; }
		}

		[ReadOnly(true)]
		public virtual bool Enabled
		{
			get { return Inactive.HasValue ? !Inactive.Value : true; }
			set { Inactive = !value; }
		}
	}
}