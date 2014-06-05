﻿// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.ComponentModel;
using DigitalBeacon.Web;
using DigitalBeacon.Web.Validation;

namespace DigitalBeacon.SiteBase.Web.Models
{
	public class LookupInfo
	{
		[ReadOnly(true)]
		public virtual long Id { get; set; }

		[ReadOnly(true)]
		[LocalizedDisplayName("Common.Name.Label")]
		public virtual string Name { get; set; }

		[ReadOnly(true)]
		[LocalizedDisplayName("Common.Code.Label")]
		public virtual string Code { get; set; }

		[ReadOnly(true)]
		[LocalizedDisplayName("Common.DisplayOrder.Label")]
		public virtual int? DisplayOrder { get; set; }

		[ReadOnly(true)]
		[LocalizedDisplayName("Common.Comment.Label")]
		public virtual string Comment { get; set; }

		[ReadOnly(true)]
		[LocalizedDisplayName("Common.Inactive.Label")]
		public virtual bool Inactive
		{
			get { return DisplayOrder.HasValue && DisplayOrder == 0; }
			set { DisplayOrder = 0; }
		}
	}
}
