// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.ComponentModel;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Web;
using DigitalBeacon.SiteBase.Web.Models;
using DigitalBeacon.Web;
using DigitalBeacon.Web.Validation;
using FluentValidation;
using FluentValidation.Attributes;

namespace DigitalBeacon.SiteBase.Models.Identity
{
	[Validator(typeof(RegisterModelValidator))]
	public class RegisterModel : BaseViewModel
	{
		[ReadOnly(true)]
		public bool ShowMiddleName { get; set; }

		[ReadOnly(true)]
		public bool ShowAddress { get; set; }

		[ReadOnly(true)]
		public bool RequireAddress { get; set; }

		public bool RequirePostalCode
		{
			get { return ShowAddress && RequireAddress && Country.HasValue && Country.Value == (long)DigitalBeacon.SiteBase.Model.Country.UnitedStates; }
		}

		public bool RequireState
		{
			get { return RequirePostalCode; }
		}

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

		[Required]
		[StringLength(AddressEntity.EmailMaxLength)]
		[Email]
		[LocalizedDisplayName("Common.Email.Label")]
		public string Email { get; set; }

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

		[StringLength(UserEntity.UsernameMaxLength)]
		[ManagedRegularExpression(WebConstants.UsernameRegexKey, "Identity.Error.Username.Invalid")]
		[LocalizedDisplayName("Common.Username.Label")]
		public string Username { get; set; }

		[Required]
		[StringLength(20)]
		[ManagedRegularExpression(WebConstants.PasswordRegexKey, "Identity.Error.Password.Invalid")]
		[LocalizedDisplayName("Common.Password.Label")]
		public string Password { get; set; }

		[Required]
		[StringLength(20)]
		[LocalizedDisplayName("Common.PasswordConfirm.Label")]
		public string PasswordConfirm { get; set; }

		[Required]
		[StringLength(100)]
		[LocalizedDisplayName("Common.SecurityQuestion.Label")]
		public string SecurityQuestion { get; set; }

		[Required]
		[StringLength(100)]
		[LocalizedDisplayName("Common.SecurityAnswer.Label")]
		public string SecurityAnswer { get; set; }
	}

	public class RegisterModelValidator : BaseValidator<RegisterModel>
	{
		public RegisterModelValidator()
		{
			RuleFor(x => x.Username)
				.NotNullOrBlank()
				.When(x => !WebConstants.UseEmailForUsername)
				.WithLocalizedMessage("Validation.Error.Required", "Common.Username.Label");
			RuleFor(x => x.PasswordConfirm)
				.Equal(x => x.Password)
				.WithLocalizedMessage("Identity.Error.PasswordConfirm.NotMatched");
			RuleFor(x => x.Password)
				.NotEqual(x => x.Username)
				.WithLocalizedMessage("Identity.Error.Password.Invalid");
			RuleFor(x => x.Country)
				.NotNull()
				.When(x => x.ShowAddress && x.RequireAddress)
				.WithLocalizedMessage("Validation.Error.Required", "Common.Country.Label");
			RuleFor(x => x.Line1)
				.NotNullOrBlank()
				.When(x => x.ShowAddress && x.RequireAddress)
				.WithLocalizedMessage("Validation.Error.Required", "Common.Line1.Label");
			RuleFor(x => x.City)
				.NotNullOrBlank()
				.When(x => x.ShowAddress && x.RequireAddress)
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
