// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Web;
using DigitalBeacon.Web;
using DigitalBeacon.Web.Validation;
using FluentValidation;
using FluentValidation.Attributes;

namespace DigitalBeacon.SiteBase.Models.Users
{
	[Validator(typeof(CreateModelValidator))]
	public class CreateModel : EditModel
	{
		[Required]
		public override string Email { get; set; }

		[StringLength(UserEntity.UsernameMaxLength)]
		[ManagedRegularExpression(WebConstants.UsernameRegexKey, "Identity.Error.Username.Invalid")]
		[LocalizedDisplayName("Common.Username.Label")]
		public override string Username { get; set; }

		[StringLength(20)]
		[ManagedRegularExpression(WebConstants.PasswordRegexKey, "Identity.Error.Password.Invalid")]
		[LocalizedDisplayName("Common.Password.Label")]
		public string Password { get; set; }

		[StringLength(20)]
		[LocalizedDisplayName("Common.PasswordConfirm.Label")]
		public string PasswordConfirm { get; set; }

		[LocalizedDisplayName("Users.SendEmail.Label")]
		public bool? SendEmail { get; set; }
	}

	public class CreateModelValidator : BaseValidator<CreateModel>
	{
		public CreateModelValidator()
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
