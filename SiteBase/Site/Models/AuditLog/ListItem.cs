// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using DigitalBeacon.Web;

namespace DigitalBeacon.SiteBase.Models.AuditLog
{
	public class ListItem
	{
		public long Id { get; set; }

		[LocalizedDisplayName("Common.Date.Label")]
		public DateTime Created { get; set; }

		[LocalizedDisplayName("AuditLog.Action.Label")]
		public string Action { get; set; }

		[LocalizedDisplayName("Common.Username.Label")]
		public string Username { get; set; }

		[LocalizedDisplayName("AuditLog.EntityType.Label")]
		public string EntityType { get; set; }

		[LocalizedDisplayName("AuditLog.RefId.Label")]
		public string RefId { get; set; }
	}
}