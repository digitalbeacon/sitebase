// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using DigitalBeacon.Web;

namespace DigitalBeacon.SiteBase.Models.PostalCodes
{
	public class ListItem
	{
		public long Id { get; set; }

		[LocalizedDisplayName("Common.PostalCode.Label")]
		public string Code { get; set; }

		[LocalizedDisplayName("Common.City.Label")]
		public string City { get; set; }

		[LocalizedDisplayName("Common.State.Label")]
		public string StateCode { get; set; }

		public long? StateId { get; set; }

		[LocalizedDisplayName("Common.County.Label")]
		public string County { get; set; }
	}
}