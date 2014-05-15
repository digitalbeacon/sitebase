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
	public class ResetPasswordModel
	{
		public const string StepProperty = "Step";

		public class StepOne : BaseViewModel
		{
			public int Step { get; set; }
			public bool GoBack { get; set; }

			[Required]
			[StringLength(UserEntity.UsernameMaxLength)]
			[LocalizedDisplayName("Common.Username.Label")]
			public string Username { get; set; }

			/// <summary>
			/// Initializes a new instance of the <see cref="StepOne"/> class.
			/// </summary>
			public StepOne()
			{
				Step = 1;
			}
		}

		[Validator(typeof(StepTwoValidator))]
		public class StepTwo : StepOne
		{
			[ReadOnly(true)]
			[StringLength(100)]
			[LocalizedDisplayName("Common.SecurityQuestion.Label")]
			public string SecurityQuestion { get; set; }

			[Required]
			[StringLength(100)]
			[LocalizedDisplayName("Common.SecurityAnswer.Label")]
			public string SecurityAnswer { get; set; }

			[Required]
			[StringLength(20)]
			[ManagedRegularExpression(WebConstants.PasswordRegexKey, "Identity.Error.Password.Invalid")]
			[LocalizedDisplayName("Common.NewPassword.Label")]
			public string Password { get; set; }

			[Required]
			[StringLength(20)]
			[LocalizedDisplayName("Common.NewPasswordConfirm.Label")]
			public string PasswordConfirm { get; set; }

			/// <summary>
			/// Initializes a new instance of the <see cref="StepTwo"/> class.
			/// </summary>
			public StepTwo()
			{
				Step = 2;
			}
		}

		public class StepTwoValidator : BaseValidator<StepTwo>
		{
			public StepTwoValidator()
			{
				RuleFor(x => x.PasswordConfirm)
					.Equal(x => x.Password)
					.WithLocalizedMessage("Identity.Error.PasswordConfirm.NotMatched");
				RuleFor(x => x.Password)
					.NotEqual(x => x.Username)
					.WithLocalizedMessage("Identity.Error.Password.Invalid");
			}
		}
	}
}
