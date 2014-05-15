// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using DigitalBeacon.SiteBase.Web.Models;
using DigitalBeacon.Web;
using DigitalBeacon.Web.Validation;
using FluentValidation;
using FluentValidation.Attributes;
using DigitalBeacon.SiteBase.Web;

namespace DigitalBeacon.SiteBase.Models.Users
{
	[Validator(typeof(ChangePasswordModelValidator))]
	public class ChangePasswordModel : BaseViewModel
	{
		public string Username { get; set; }

		[Required]
		[StringLength(20)]
		[ManagedRegularExpression(WebConstants.PasswordRegexKey, "Identity.Error.Password.Invalid")]
		[LocalizedDisplayName("Common.NewPassword.Label")]
		public string NewPassword { get; set; }

		[Required]
		[StringLength(50)]
		[LocalizedDisplayName("Common.NewPasswordConfirm.Label")]
		public string NewPasswordConfirm { get; set; }
	}

	public class ChangePasswordModelValidator : BaseValidator<ChangePasswordModel>
	{
		public ChangePasswordModelValidator()
		{
			RuleFor(x => x.NewPasswordConfirm)
				.Equal(x => x.NewPassword)
				.WithMessage(GetLocalizedString("Identity.Error.PasswordConfirm.NotMatched"));
			RuleFor(x => x.NewPassword)
				.NotEqual(x => x.Username)
				.WithMessage(GetLocalizedString("Identity.Error.Password.Invalid"));
		}
	}
}
