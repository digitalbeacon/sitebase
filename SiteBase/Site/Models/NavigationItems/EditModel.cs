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

namespace DigitalBeacon.SiteBase.Models.NavigationItems
{
	public class EditModel : EntityModel
	{
		[LocalizedDisplayName("Common.Enabled.Label")]
		public bool? Enabled { get; set; }

		[LocalizedDisplayName("Common.DisplayOrder.Label")]
		public int? DisplayOrder { get; set; }

		[LocalizedDisplayName("NavigationItems.Parent.Label")]
		public long? Parent { get; set; }

		[Required]
		[LocalizedDisplayName("NavigationItems.Navigation.Label")]
		public long? Navigation { get; set; }

		[Required]
		[StringLength(NavigationItemEntity.TextMaxLength)]
		[LocalizedDisplayName("Common.Text.Label")]
		public string Text { get; set; }

		[Required]
		[StringLength(NavigationItemEntity.UrlMaxLength)]
		[LocalizedDisplayName("NavigationItems.Url.Label")]
		public string Url { get; set; }

		[StringLength(NavigationItemEntity.ImageUrlMaxLength)]
		[LocalizedDisplayName("NavigationItems.ImageUrl.Label")]
		public string ImageUrl { get; set; }
	}
}