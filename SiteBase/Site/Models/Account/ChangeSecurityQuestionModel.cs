// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using DigitalBeacon.SiteBase.Web.Models;
using DigitalBeacon.Web;
using DigitalBeacon.Web.Validation;

namespace DigitalBeacon.SiteBase.Models.Account
{
	public class ChangeSecurityQuestionModel : BaseViewModel
	{
		public const string CurrentPasswordProperty = "CurrentPassword";

		[Required]
		[StringLength(50)]
		[LocalizedDisplayName("Common.CurrentPassword.Label")]
		public string CurrentPassword { get; set; }

		[Required]
		[StringLength(100)]
		[LocalizedDisplayName("Common.SecurityQuestion.Label")]
		public string SecurityQuestion { get; set; }

		[Required]
		[StringLength(100)]
		[LocalizedDisplayName("Common.SecurityAnswer.Label")]
		public string SecurityAnswer { get; set; }
	}
}
