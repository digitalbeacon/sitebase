// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Collections.Generic;

namespace DigitalBeacon.SiteBase.Models
{
	public class NavigationItem
	{
		private IList<NavigationItem> _items;

		public long Id { get; set; }
		public string Text { get; set; }
		public string Url { get; set; }
		public string ImageUrl { get; set; }

		public IList<NavigationItem> Items
		{
			get
			{
				if (_items == null)
				{
					_items = new List<NavigationItem>();
				}
				return _items;
			}
			set { _items = value; }
		}
	}
}
