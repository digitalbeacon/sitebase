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

namespace DigitalBeacon.SiteBase.Models.States
{
	public class EditModel : NamedEntityModel
	{
		[Required]
		[LocalizedDisplayName("Common.Country.Label")]
		public long? Country { get; set; }

		[Required]
		[StringLength(StateEntity.CodeMaxLength)]
		[LocalizedDisplayName("Common.Code.Label")]
		public string Code { get; set; }
	}
}