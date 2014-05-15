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

namespace DigitalBeacon.SiteBase.Models.Content
{
	public class ContentGroupModel : NamedEntityModel
	{
		[Required]
		[StringLength(ContentGroupEntity.NameMaxLength)]
		[LocalizedDisplayName("Common.Name.Label")]
		public override string Name { get; set; }

		[Required]
		[StringLength(ContentGroupEntity.NameMaxLength)]
		[LocalizedDisplayName("ContentGroups.Title.Label")]
		public string Title { get; set; }

		[Required]
		[LocalizedDisplayName("ContentGroups.ContentGroupType.Label")]
		public long? ContentGroupType { get; set; }
		
		[LocalizedDisplayName("Common.DisplayOrder.Label")]
		public int? DisplayOrder { get; set; }

		[LocalizedDisplayName("Common.PageSize.Label")]
		public int? PageSize { get; set; }
	}
}
