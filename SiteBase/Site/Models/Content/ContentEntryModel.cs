// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using DigitalBeacon.SiteBase.Model.Content;
using DigitalBeacon.SiteBase.Web.Models;
using DigitalBeacon.Web.Validation;
using DigitalBeacon.Web;
using System;

namespace DigitalBeacon.SiteBase.Models.Content
{
	public class ContentEntryModel : EntityModel
	{
		[Required]
		[LocalizedDisplayName("ContentEntries.ContentGroups.Label")]
		public string ContentGroup { get; set; }

		[LocalizedDisplayName("ContentEntries.ContentDate.Label")]
		public DateTime? ContentDate { get; set; }

		[StringLength(ContentEntryEntity.TitleMaxLength)]
		[LocalizedDisplayName("ContentEntries.Title.Label")]
		public string Title { get; set; }

		[Required]
		[StringLength(ContentEntryEntity.BodyMaxLength)]
		[LocalizedDisplayName("ContentEntries.Body.Label")]
		public string Body { get; set; }

		[LocalizedDisplayName("Common.DisplayOrder.Label")]
		public int? DisplayOrder { get; set; }
	}
}
