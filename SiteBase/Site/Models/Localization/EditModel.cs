// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using DigitalBeacon.SiteBase.Web.Models;
using DigitalBeacon.Web;
using DigitalBeacon.Web.Validation;

namespace DigitalBeacon.SiteBase.Models.Localization
{
	public class EditModel : EntityModel
	{
		[Required]
		[LocalizedDisplayName("Common.Language.Label")]
		public long? Language { get; set; }

		[LocalizedDisplayName("Common.Type.Label")]
		public string Type { get; set; }

		[LocalizedDisplayName("Common.Key.Label")]
		public string Key { get; set; }

		[LocalizedDisplayName("Common.Property.Label")]
		public string Property { get; set; }

		[LocalizedDisplayName("Common.Value.Label")]
		public string Value { get; set; }
	}
}