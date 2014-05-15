// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.ComponentModel.DataAnnotations;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Web.Models;
using DigitalBeacon.Web;

namespace DigitalBeacon.SiteBase.Models.PostalCodes
{
	public class EditModel : EntityModel
	{
		[Required]
		[StringLength(PostalCodeEntity.CodeMaxLength)]
		[LocalizedDisplayName("Common.PostalCode.Label")]
		public string Code { get; set; }

		[Required]
		[StringLength(PostalCodeEntity.CityMaxLength)]
		[LocalizedDisplayName("Common.City.Label")]
		public string City { get; set; }

		[Required]
		[LocalizedDisplayName("Common.State.Label")]
		public long? StateId { get; set; }

		[Required]
		[StringLength(PostalCodeEntity.CountyMaxLength)]
		[LocalizedDisplayName("Common.County.Label")]
		public string County { get; set; }
	}
}