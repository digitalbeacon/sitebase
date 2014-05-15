// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Web.Models;
using DigitalBeacon.Util;
using DigitalBeacon.Web;
using DigitalBeacon.Web.Validation;

namespace DigitalBeacon.SiteBase.Models.Permissions
{
	public class EditModel : EntityModel
	{
		public EditModel() { Permissions = new List<PermissionItem>(); }

		[Required]
		[StringLength(PermissionEntity.Key1MaxLength)]
		[LocalizedDisplayName("Permissions.Key1.Label")]
		public string Key1 { get; set; }

		[LocalizedDisplayName("Permissions.Key2.Label")]
		public long? Key2 { get; set; }

		[StringLength(PermissionEntity.Key3MaxLength)]
		[LocalizedDisplayName("Permissions.Key3.Label")]
		public string Key3 { get; set; }

		public IList<PermissionItem> Permissions { get; set; }

		public ListModel<PermissionItem> PermissionsListModel
		{
			get
			{
				return new ListModel<PermissionItem>
				{
					Items = Permissions,
					TotalCount = Permissions.Count
				};
			} 
		}

		public string PermissionsJson
		{
			get { return Permissions.Select(x => new { x.EntityType, x.EntityId, x.Mask }).ToJson(); }
		}
	}
}