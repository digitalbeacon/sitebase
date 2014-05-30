// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using System.Dom;
using System.Html;
using DigitalBeacon.SiteBase;
using DigitalBeacon.SiteBase.Mobile;
using jQueryLib;
using ng;

namespace DigitalBeacon.SiteBase.Mobile
{
	public abstract class BaseListController : BaseController
	{
		public BaseListState list = new BaseListState();

		public override void init()
		{
			base.init();
			Scope.on("showList", new Action<object, object>((evt, args) => { showList(args); }));
			Scope.on("hideList", new Action(() => { hideList(); }));
		}

		public abstract void search(bool requestMore = false);

		public virtual void loadMore()
		{
			if (list.page < list.pageCount)
			{
				list.page++;
				search(true);
			}
		}

		public virtual void addNew()
		{
			clearAlerts();
			RouterState.go("list.new");
			model = new Dictionary<object>();
			//Location.path("/new");
			//window.location.href = "#/new";
		}

		public virtual void showDetails(int id)
		{
			clearAlerts();
			RouterState.go("list.edit", new { id = id });
			//Location.path("/" + id);
			//window.location.href = "#/" + id;
		}

		public virtual void hideList()
		{
			list.visible = false;
			disableLoadMoreOnScroll();
		}

		public virtual void showList(dynamic response = null)
		{
			list.visible = true;
			RouterState.go("list");
			enableLoadMoreOnScroll();
			if (response)
			{
				if (response.Success)
				{
					search();
				}
				ApiResponseHelper.handleResponse(response, Scope);
			}
		}

		protected virtual void clearSearchText()
		{
			list.searchText = "";
			if (list.isFiltered)
			{
				search();
			}
			list.isFiltered = false;
			list.page = 1;
		}

		protected virtual string getSortValue()
		{
			return list.sortText + list.sortDirection;
		}

		protected virtual void enableLoadMoreOnScroll()
		{
			if (list.pageCount <= 1)
			{
				return;
			}

			jQuery.Select(window.self).on("scroll.sbClientListPanel", null, null,
				(Action<jQueryLib.Event>)(e =>
				{
					var w = jQuery.Select(window.self);
					var d = jQuery.Select(window.document);
					//console.log("{0}, {1}, {2}".formatWith(w.scrollTop(), d.height(), w.height()));
					//console.log("{0}, {1}".formatWith(w.scrollTop(), jQuery.Select(window.document).height() - w.height() - FooterHeight));
					if ((w.scrollTop() >= d.height() - w.height() - list.footerHeight) || (w.scrollTop() >= d.height() / 2))
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
