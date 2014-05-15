// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using DigitalBeacon.Web;

namespace DigitalBeacon.SiteBase.Models.Localization
{
	public class ListItem
	{
		public long Id { get; set; }

		[LocalizedDisplayName("Common.Language.Label")]
		public string Language { get; set; }

		[LocalizedDisplayName("Common.Type.Label")]
		public string Type { get; set; }

		[LocalizedDisplayName("Common.Key.Label")]
		public string Key { get; set; }

		[LocalizedDisplayName("Common.Property.Label")]
		public string Property { get; set; }

		[LocalizedDisplayName("Common.Default.Label")]
		public string Default { get; set; }

		[LocalizedDisplayName("Common.Value.Label")]
		public string Value { get; set; }
	}
}