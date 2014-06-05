// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DigitalBeacon.Web;
using DigitalBeacon.Web.Validation;

namespace DigitalBeacon.SiteBase.Web.Models
{
	public class ListModel<T> : ListModelBase
	{
		[ReadOnly(true)]
		public IEnumerable<T> Items
		{
			get { return (IEnumerable<T>)UntypedItems; }
			set { UntypedItems = value; }
		}
	}

	public class ListModelBase : BaseViewModel
	{
		[ReadOnly(true)]
		public int PageSize { get; set; }

		[ReadOnly(true)]
		public int Page { get; set; }

		[ReadOnly(true)]
		public long TotalCount { get; set; }

		[StringLength(100)]
		[LocalizedDisplayName("Common.SearchText.Label")]
		public string SearchText { get; set; }

		[ReadOnly(true)]
		public string SingularLabel { get; set; }

		[ReadOnly(true)]
		public string PluralLabel { get; set; }

		[ReadOnly(true)]
		public IEnumerable UntypedItems { get; set; }

		[ReadOnly(true)]
		public long? ParentId { get; set; }

		[ReadOnly(true)]
		public int Pages
		{
			get { return PageSize == 0 ? 1 : (int)Math.Ceiling(TotalCount / (double)PageSize); }
		}
	}
}
