// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using DigitalBeacon.Web;
using DigitalBeacon.Web.Validation;

namespace DigitalBeacon.SiteBase.Web.Models
{
	public class NamedEntityModel : EntityModel
	{
		[Required]
		[StringLength(100)]
		[LocalizedDisplayName("Common.Name.Label")]
		public virtual string Name { get; set; }
	}
}