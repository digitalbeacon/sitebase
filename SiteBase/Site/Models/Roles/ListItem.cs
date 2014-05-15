// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using DigitalBeacon.Web;

namespace DigitalBeacon.SiteBase.Models.Roles
{
	public class ListItem
	{
		public long Id { get; set; }

		[LocalizedDisplayName("Common.Name.Label")]
		public string Name { get; set; }
		
		[LocalizedDisplayName("RoleGroups.Singular.Label")]
		public string RoleGroup { get; set; }
	}
}