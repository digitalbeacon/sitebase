// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using DigitalBeacon.SiteBase.Web.Models;
using DigitalBeacon.Web;

namespace DigitalBeacon.SiteBase.Models.Roles
{
	public class EditModel : NamedEntityModel
	{
		[LocalizedDisplayName("RoleGroups.Singular.Label")]
		public long? RoleGroup { get; set; }
	}
}