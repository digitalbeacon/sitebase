// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.ComponentModel;
using DigitalBeacon.SiteBase.Web;
using DigitalBeacon.Util;
using DigitalBeacon.Web;
using DigitalBeacon.Web.Validation;

namespace DigitalBeacon.SiteBase.Models.Contacts
{
	public class CreateModel : EditModel
	{
		[ReadOnly(true)]
		public DateTime? Date { get; set; }

		[LocalizedDisplayName("Common.Date.Label")]
		public string CommentDate
		{
			get { return Date.HasValue ? Date.Value.ToString(WebConstants.DefaultDateTimeFormat) : string.Empty; }
			set { Date = value.ToDate(); }
		}

		[LocalizedDisplayName("ContactCommentTypes.Singular.Label.Short")]
		public long? CommentTypeId { get; set; }

		[LocalizedDisplayName("Comments.Plural.Label")]
		public string Comments { get; set; }
	}
}