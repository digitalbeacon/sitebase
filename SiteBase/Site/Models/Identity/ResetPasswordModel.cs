// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.ComponentModel;
using DigitalBeacon.Business;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Web.Models;
using DigitalBeacon.Web;
using DigitalBeacon.Web.Validation;
using FluentValidation;
using FluentValidation.Attributes;
using DigitalBeacon.SiteBase.Web;

namespace DigitalBeacon.SiteBase.Models.Identity
{
	[Validator(typeof(ResetPasswordValidator))]
	public class ResetPasswordModel : BaseViewModel
	{
		public const string StepProperty = "Step";

		public int Step { get; set; }
		public bool GoBack { get; set; }

		public ResetPasswordModel()
		{
			Step = 1;
		}

		[Required]
		[StringLength(UserEntity.UsernameMaxLength)]
		[LocalizedDisplayName("Common.Username.Label")]
		public string Username { get; set; }

		[ReadOnly(true)]
		[StringLength(100)]
		[LocalizedDisplayName("Common.SecurityQuestion.Label")]
		public string SecurityQuestion { get; set; }

		[StringLength(100)]
		[LocalizedDisplayName("Common.SecurityAnswer.Label")]
		public string SecurityAnswer { get; set; }

		[StringLength(20)]
		[ManagedRegularExpression(WebConstants.PasswordRegexKey, "Identity.Error.Password.Invalid")]
		[LocalizedDisplayName("Common.NewPassword.Label")]
		public string Password { get; set; }

		[StringLength(20)]
		[LocalizedDisplayName("Common.NewPasswordConfirm.Label")]
		public string PasswordConfirm { get; set; }
	}

	public class ResetPasswordValidator : BaseValidator<ResetPasswordModel>
	{
		public ResetPasswordValidator()
		{
			RuleFor(x => x.SecurityAnswer)
				.NotNullOrBlank()
				.When(x => x.Step > 1)
				.WithLocalizedMessage("Validation.Error.Required", "Common.SecurityAnswer.Label");
			RuleFor(x => x.Password)
				.NotNullOrBlank()
				.When(x => x.Step > 1)
				.WithLocalizedMessage("Validation.Error.Required", "Common.NewPassword.Label");
			RuleFor(x => x.Password)
				.NotEqual(x => x.Username)
				.When(x => x.Step > 1)
				.WithLocalizedMessage("Identity.Error.Password.Invalid");
			RuleFor(x => x.PasswordConfirm)
				.NotNullOrBlank()
				.When(x => x.Step > 1)
				.WithLocalizedMessage("Validation.Error.Required", "Common.NewPasswordConfirm.Label");
			RuleFor(x => x.PasswordConfirm)
				.Equal(x => x.Password)
				.When(x => x.Step > 1)
				.WithLocalizedMessage("Identity.Error.PasswordConfirm.NotMatched");
		}
	}
}
