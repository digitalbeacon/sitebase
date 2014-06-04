// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Html;

namespace DigitalBeacon.SiteBase.Mobile
{
	public class BaseListState
	{
		public bool isFiltered;
		public string searchText;
		public string sortText;
		public string sortDirection;
		public Option[] sortTextOptions;
		public Option[] sortDirectionOptions = new[] { new Option("Ascending", ""), new Option("Descending", "-DESC") };
		public int page = 1;
		public int pageSize = 4;
		public int pageCount = -1;
		public int footerHeight = 140;
		public bool visible = true;
	}
}