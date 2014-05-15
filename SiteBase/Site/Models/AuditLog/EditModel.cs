// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.ComponentModel;
using DigitalBeacon.SiteBase.Web.Models;
using DigitalBeacon.Web;

namespace DigitalBeacon.SiteBase.Models.AuditLog
{
	public class EditModel : EntityModel
	{
		[ReadOnly(true)]
		[LocalizedDisplayName("Common.Date.Label")]
		public DateTime Created { get; set; }

		[ReadOnly(true)]
		[LocalizedDisplayName("AuditLog.Action.Label")]
		public string Action { get; set; }

		[ReadOnly(true)]
		[LocalizedDisplayName("Common.Username.Label")]
		public string Username { get; set; }

		[ReadOnly(true)]
		[LocalizedDisplayName("AuditLog.EntityType.Label")]
		public string EntityType { get; set; }

		[ReadOnly(true)]
		[LocalizedDisplayName("AuditLog.RefId.Label")]
		public string RefId { get; set; }

		[ReadOnly(true)]
		[LocalizedDisplayName("AuditLog.Details.Label")]
		public string Details { get; set; }
	}
}