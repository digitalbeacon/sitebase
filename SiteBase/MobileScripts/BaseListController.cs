// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Dom;
using System.Html;
using DigitalBeacon.SiteBase;
using DigitalBeacon.SiteBase.Mobile;
using jQueryLib;

namespace DigitalBeacon.SiteBase.Mobile
{
	public abstract class BaseListController : BaseController
	{
		public bool IsFiltered;
		public string SearchText;
		public string SortText;
		public string SortDirection;
		public Option[] SortTextOptions;
		public Option[] SortDirectionOptions = new[] { new Option("Ascending", ""), new Option("Descending", "-DESC") };
		public int Page = 1;
		public int PageSize = 4;
		public int PageCount = 1;
		public int FooterHeight = 140;

		public abstract void search(bool requestMore = false);

		public virtual void loadMore()
		{
			if (Page < PageCount)
			{
				Page++;
				search(true);
			}
		}

		public virtual void addNew()
		{
			disableLoadMoreOnScroll();
			Location.path("/new");
			//window.location.href = "#/new";
		}

		public virtual void showDetails(int id)
		{
			disableLoadMoreOnScroll();
			Location.path("/" + id);
			//window.location.href = "#/" + id;
		}

		protected virtual void clearSearchText()
		{
			SearchText = "";
			if (IsFiltered)
			{
				search();
			}
			IsFiltered = false;
			Page = 1;
		}

		protected virtual string getSortValue()
		{
			return SortText + SortDirection;
		}

		protected virtual void enableLoadMoreOnScroll()
		{
			jQuery.Select(window.self).on("scroll.sbClientListPanel", null, null,
				(Action<jQueryLib.Event>)(e =>
				{
					var w = jQuery.Select(window.self);
					var d = jQuery.Select(window.document);
					//console.log("{0}, {1}, {2}".formatWith(w.scrollTop(), d.height(), w.height()));
					//console.log("{0}, {1}".formatWith(w.scrollTop(), jQuery.Select(window.document).height() - w.height() - FooterHeight));
					if ((w.scrollTop() >= d.height() - w.height() - FooterHeight) || (w.scrollTop() >= d.height() / 2))
					{
						loadMore();
					}
				})
			);
		}

		protected virtual void disableLoadMoreOnScroll()
		{
			jQuery.Select(window.self).off("scroll.sbClientListPanel");
		}
	}
}
