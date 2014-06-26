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
	[ScriptObjectLiteral]
	public class ListScopeData : BaseScopeData
	{
		public bool isFiltered;
		public string searchText;
		public string sortText;
		public string sortDirection;
		public int page;
		public int pageSize;
		public int pageCount;
		public int footerHeight;
		public bool listVisible;
		public bool isCollapsedAdvancedSearch;
		public dynamic[] items;
		public bool transitionPrevious;
		public bool transitionNext;
	}
}