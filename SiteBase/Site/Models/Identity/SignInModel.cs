// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Web;
using DigitalBeacon.SiteBase.Web.Models;
using DigitalBeacon.Web;
using DigitalBeacon.Web.Validation;

namespace DigitalBeacon.SiteBase.Models.Identity
{
	public class SignInModel : BaseViewModel
	{
		[Required]
		[StringLength(UserEntity.UsernameMaxLength)]
		[LocalizedDisplayName("Common.Username.Label")]
		public string Username { get; set; }

		[Required]
		//[StringLength(100)]
		[LocalizedDisplayName("Common.Password.Label")]
		public string Password { get; set; }
	}
}
