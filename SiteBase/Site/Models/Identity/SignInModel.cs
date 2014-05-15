// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using DigitalBeacon.SiteBase.Web.Models;
using DigitalBeacon.Web;
using DigitalBeacon.Web.Validation;

namespace DigitalBeacon.SiteBase.Models.Identity
{
	public class SignInModel : BaseViewModel
	{
		[Required]
		[LocalizedDisplayName("Common.Username.Label")]
		public string Username { get; set; }

		[Required]
		[LocalizedDisplayName("Common.Password.Label")]
		public string Password { get; set; }
	}
}
