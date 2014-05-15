// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.ComponentModel;
using System.Web.Mvc;
using DigitalBeacon.SiteBase.Web.Models;
using DigitalBeacon.Web;
using DigitalBeacon.Web.Validation;
using FluentValidation;
using FluentValidation.Attributes;
using DigitalBeacon.SiteBase.Web;

namespace DigitalBeacon.SiteBase.Models.Identity
{
	[Validator(typeof(ChangePasswordModelValidator))]
	[Bind(Exclude = "Username")]
	public class ChangePasswordModel : BaseViewModel
	{
		public const string CurrentPasswordProperty = "CurrentPassword";

		[ReadOnly(true)]
		public string Username { get; set; }

		[Required]
		[StringLength(20)]
		[ManagedRegularExpression(WebConstants.PasswordRegexKey, "Identity.Error.Password.Invalid")]
		[LocalizedDisplayName("Common.NewPassword.Label")]
		public string Password { get; set; }

		[Required]
		[StringLength(20)]
		[LocalizedDisplayName("Common.NewPasswordConfirm.Label")]
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

	public class ChangePasswordModelValidator : BaseValidator<ChangePasswordModel>
	{
		public ChangePasswordModelValidator()
		{
			RuleFor(x => x.PasswordConfirm)
				.Equal(x => x.Password)
				.WithMessage(GetLocalizedString("Identity.Error.PasswordConfirm.NotMatched"));
			RuleFor(x => x.Password)
				.NotEqual(x => x.Username)
				.WithMessage(GetLocalizedString("Identity.Error.Password.Invalid"));
		}
	}
}
