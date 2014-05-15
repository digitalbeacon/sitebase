// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using DigitalBeacon.SiteBase.Web.Models;
using DigitalBeacon.Web.Validation;

namespace DigitalBeacon.SiteBase.Models
{
	public class PartialNavModel : BaseViewModel
	{
		[Required]
		public string Url { get; set; }
	}
}
