// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.Web;

namespace DigitalBeacon.SiteBase.Models.Permissions
{
	public class PermissionItem
	{
		public long Id { get; set; }
		public long EntityType { get; set; }
		public long EntityId { get; set; }

		[LocalizedDisplayName("Permissions.Mask.Label")]
		public int Mask { get; set; }

		[LocalizedDisplayName("Permissions.Key1.Label")]
		public string Key1 { get; set; }
		[LocalizedDisplayName("Permissions.Key2.Label")]
		public long? Key2 { get; set; }
		[LocalizedDisplayName("Permissions.Key3.Label")]
		public string Key3 { get; set; }

		[LocalizedDisplayName("Permissions.EntityType.Label")]
		public string EntityTypeName { get; set; }

		[LocalizedDisplayName("Permissions.EntityName.Label")]
		public string EntityName { get; set; }

		[LocalizedDisplayName("Permissions.Access.Label")]
		public bool AccessPermission
		{
			get { return (Mask & (int)Permission.Access) > 0; }
		}

		[LocalizedDisplayName("Permissions.Create.Label")]
		public bool CreatePermission
		{
			get { return (Mask & (int)Permission.Create) > 0; }
		}

		[LocalizedDisplayName("Permissions.Update.Label")]
		public bool UpdatePermission
		{
			get { return (Mask & (int)Permission.Update) > 0; }
		}

		[LocalizedDisplayName("Permissions.Delete.Label")]
		public bool DeletePermission
		{
			get { return (Mask & (int)Permission.Delete) > 0; }
		}

		[LocalizedDisplayName("Permissions.Admin.Label")]
		public bool AdminPermission
		{
			get { return (Mask & (int)Permission.Admin) > 0; }
		}
	}
}