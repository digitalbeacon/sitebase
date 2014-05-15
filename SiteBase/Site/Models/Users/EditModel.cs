// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Web;
using DigitalBeacon.SiteBase.Web.Models;
using DigitalBeacon.Web;
using DigitalBeacon.Web.Validation;
using FluentValidation;
using FluentValidation.Attributes;

namespace DigitalBeacon.SiteBase.Models.Users
{
	[Validator(typeof(EditModelValidator))]
	public class EditModel : EntityModel
	{
		public const string NotificationPreferenceProperty = "NotificationPreference";

		private IList<long> _roles;
		private IDictionary<string, IEnumerable<SelectListItem>> _roleGroups;

		[ReadOnly(true)]
		public bool ShowMiddleName { get; set; }

		[ReadOnly(true)]
		public bool RequireAddress { get; set; }

		public bool RequirePostalCode
		{
			get { return RequireAddress && Country.HasValue && Country.Value == (long)DigitalBeacon.SiteBase.Model.Country.UnitedStates; }
		}

		public bool RequireState
		{
			get { return RequirePostalCode; }
		}

		public virtual string Username { get; set; }

		[Required]
		[StringLength(PersonEntity.FirstNameMaxLength)]
		[LocalizedDisplayName("Common.FirstName.Label")]
		public string FirstName { get; set; }

		[Required]
		[StringLength(PersonEntity.LastNameMaxLength)]
		[LocalizedDisplayName("Common.LastName.Label")]
		public string LastName { get; set; }

		[StringLength(PersonEntity.MiddleNameMaxLength)]
		[LocalizedDisplayName("Common.MiddleName.Label")]
		public string MiddleName { get; set; }

		[StringLength(AddressEntity.EmailMaxLength)]
		[Email]
		[LocalizedDisplayName("Common.Email.Label")]
		public virtual string Email { get; set; }

		[LocalizedDisplayName("Common.Country.Label")]
		public long? Country { get; set; }

		[StringLength(AddressEntity.Line1MaxLength)]
		[LocalizedDisplayName("Common.Line1.Label")]
		public string Line1 { get; set; }

		[StringLength(AddressEntity.Line2MaxLength)]
		[LocalizedDisplayName("Common.Line2.Label")]
		public string Line2 { get; set; }

		[StringLength(AddressEntity.CityMaxLength)]
		[LocalizedDisplayName("Common.City.Label")]
		public string City { get; set; }

		[LocalizedDisplayName("Common.State.Label")]
		public long? State { get; set; }

		[StringLength(AddressEntity.PostalCodeMaxLength)]
		[LocalizedDisplayName("Common.PostalCode.Label")]
		public string PostalCode { get; set; }

		[LocalizedDisplayName("Account.NotificationPreference.Label")]
		public string NotificationPreference { get; set; }

		[LocalizedDisplayName("Common.Approved.Label")]
		public bool Approved { get; set; }

		[LocalizedDisplayName("Common.IsLockedOut.Label")]
		public bool LockedOut { get; set; }

		[LocalizedDisplayName("Common.Language.Label")]
		public long? Language { get; set; }

		public IList<long> Roles 
		{
			get
			{
				if (_roles == null)
				{
					_roles = new List<long>();
				}
				return _roles;
			}
			set { _roles = value; }
		}

		public IDictionary<string, IEnumerable<SelectListItem>> RoleGroups
		{
			get
			{
				if (_roleGroups == null)
				{
					_roleGroups = new Dictionary<string, IEnumerable<SelectListItem>>();
				}
				return _roleGroups;
			}
		}
	}

	public class EditModelValidator : BaseValidator<EditModel>
	{
		public EditModelValidator()
		{
			RuleFor(x => x.Email)
				.NotNullOrBlank()
				.When(x => !WebConstants.UseEmailForUsername)
				.WithLocalizedMessage("Validation.Error.Required", "Common.Email.Label");
			RuleFor(x => x.Country)
				.NotNull()
				.When(x => x.RequireAddress)
				.WithLocalizedMessage("Validation.Error.Required", "Common.Country.Label");
			RuleFor(x => x.Line1)
				.NotNullOrBlank()
				.When(x => x.RequireAddress)
				.WithLocalizedMessage("Validation.Error.Required", "Common.Line1.Label");
			RuleFor(x => x.City)
				.NotNullOrBlank()
				.When(x => x.RequireAddress)
				.WithLocalizedMessage("Validation.Error.Required", "Common.City.Label");
			RuleFor(x => x.State)
				.NotNull()
				.When(x => x.RequireState)
				.WithLocalizedMessage("Validation.Error.Required", "Common.State.Label");
			RuleFor(x => x.PostalCode)
				.NotNullOrBlank()
				.When(x => x.RequirePostalCode)
				.WithLocalizedMessage("Validation.Error.Required", "Common.PostalCode.Label");
		}
	}
}
