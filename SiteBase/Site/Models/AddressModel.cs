// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.ComponentModel;
using System.Web.Script.Serialization;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Web.Models;
using DigitalBeacon.Web;
using DigitalBeacon.Web.Validation;

namespace DigitalBeacon.SiteBase.Models
{
	public class AddressModel : EntityModel
	{
		[LocalizedDisplayName("Common.Country.Label")]
		public virtual long? CountryId { get; set; }

		[StringLength(AddressEntity.Line1MaxLength)]
		[LocalizedDisplayName("Common.Line1.Label")]
		public virtual string Line1 { get; set; }

		[StringLength(AddressEntity.Line2MaxLength)]
		[LocalizedDisplayName("Common.Line2.Label")]
		public virtual string Line2 { get; set; }

		[StringLength(AddressEntity.CityMaxLength)]
		[LocalizedDisplayName("Common.City.Label")]
		public virtual string City { get; set; }

		[LocalizedDisplayName("Common.State.Label")]
		public virtual long? StateId { get; set; }

		[StringLength(AddressEntity.PostalCodeMaxLength)]
		[LocalizedDisplayName("Common.PostalCode.Label")]
		public virtual string PostalCode { get; set; }

		[StringLength(AddressEntity.CountyMaxLength)]
		[LocalizedDisplayName("Common.County.Label")]
		public virtual string County { get; set; }

		[Email]
		[StringLength(AddressEntity.EmailMaxLength)]
		[LocalizedDisplayName("Common.Email.Label")]
		public virtual string Email { get; set; }

		[LocalizedDisplayName("Common.DefaultPhone.Label")]
		public virtual long? DefaultPhoneId { get; set; }

		[StringLength(AddressEntity.HomePhoneMaxLength)]
		[LocalizedDisplayName("Common.HomePhone.Label")]
		public virtual string HomePhone { get; set; }

		[StringLength(AddressEntity.WorkPhoneMaxLength)]
		[LocalizedDisplayName("Common.WorkPhone.Label")]
		public virtual string WorkPhone { get; set; }

		[StringLength(AddressEntity.MobilePhoneMaxLength)]
		[LocalizedDisplayName("Common.MobilePhone.Label")]
		public virtual string MobilePhone { get; set; }

		[StringLength(AddressEntity.FaxMaxLength)]
		[LocalizedDisplayName("Common.Fax.Label")]
		public virtual string Fax { get; set; }

		[ReadOnly(true)]
		[ScriptIgnore]
		public virtual Country? Country
		{
			get { return CountryId.HasValue? (Country)CountryId.Value : (Country?)null; }
			set { CountryId = value.HasValue ? (long)value.Value : (long?)null; }
		}

		[ReadOnly(true)]
		[ScriptIgnore]
		public virtual State? State
		{
			get { return StateId.HasValue ? (State)StateId.Value : (State?)null; }
			set { StateId = value.HasValue ? (long)value.Value : (long?)null; }
		}

		public virtual PhoneType? DefaultPhone
		{
			get { return DefaultPhoneId.HasValue ? (PhoneType)DefaultPhoneId.Value : (PhoneType?)null; }
			set { DefaultPhoneId = value.HasValue ? (long)value.Value : (long?)null; }
		}
	}
}