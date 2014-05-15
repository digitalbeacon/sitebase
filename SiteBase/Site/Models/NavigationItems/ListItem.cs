// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using DigitalBeacon.Web;

namespace DigitalBeacon.SiteBase.Models.NavigationItems
{
	public class ListItem
	{
		public long Id { get; set; }

		[LocalizedDisplayName("Common.DisplayOrder.Label")]
		public int DisplayOrder { get; set; }

		[LocalizedDisplayName("Common.Enabled.Label")]
		public bool Enabled { get; set; }

		[LocalizedDisplayName("NavigationItems.Parent.Label")]
		public string Parent { get; set; }

		[LocalizedDisplayName("NavigationItems.Navigation.Label")]
		public string Navigation { get; set; }

		[LocalizedDisplayName("Common.Text.Label")]
		public string Text { get; set; }

		[LocalizedDisplayName("NavigationItems.Url.Label")]
		public string Url { get; set; }
	}
}