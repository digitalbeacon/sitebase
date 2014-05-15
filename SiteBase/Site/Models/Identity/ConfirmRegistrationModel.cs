// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Web.Models;
using DigitalBeacon.Web;
using DigitalBeacon.Web.Validation;

namespace DigitalBeacon.SiteBase.Models.Identity
{
	public class ConfirmRegistrationModel : BaseViewModel
	{
		public const string TokenProperty = "Token";

		[Required]
		[StringLength(UserEntity.UsernameMaxLength)]
		[LocalizedDisplayName("Common.Username.Label")]
		public string Username { get; set; }

		[Required]
		[Guid]
		[StringLength(36)]
		[LocalizedDisplayName("Identity.Token.Label")]
		public string Token { get; set; }
	}
}