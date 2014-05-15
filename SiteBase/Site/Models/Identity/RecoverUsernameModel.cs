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
	public class RecoverUsernameModel : BaseViewModel
	{
		[Required]
		[StringLength(AddressEntity.EmailMaxLength)]
		[Email]
		[LocalizedDisplayName("Common.Email.Label")]
		public string Email { get; set; }
	}
}
