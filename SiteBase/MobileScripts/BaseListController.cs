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
			list.visible = isListState();
			Scope.on("hideDetails", new Action<object, ApiResponse>((evt, args) => { showList(args); }));
			Scope.on("showDetails", new Action(() => { hideList(); }));
			Scope.on("alerts", new Action<object, Alert[]>((evt, args) => { alerts = args; }));
			Scope.watch(new Func<string>(() => Location.url()), new Action<string>(url =>
			{
				if (!list.visible && url && isListState())
				{
					showList();
				}
			}));
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
		}

		public virtual void showDetails(int id)
		{
			clearAlerts();
			RouterState.go("list.edit", new { id = id });
		}

		public virtual void hideList()
		{
			list.visible = false;
			disableLoadMoreOnScroll();
		}

		public virtual void showList(ApiResponse response = null)
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
				ControllerHelper.handleResponse(response, Scope);
			}
		}

		protected bool isListState()
		{
			return RouterState.current.name == "list";
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
			if (list.pageCount <= 1 || !list.visible)
			{
				return;
			}
			window.scrollTo(0, 0);
			jQuery.Select(window.self).on("scroll.sbClientListPanel", null, null,
				(Action<jQueryLib.Event>)(e =>
				{
					var w = jQuery.Select(window.self);
					var d = jQuery.Select(window.document);
					//console.log("{0}, {1}, {2}".formatWith(w.scrollTop(), d.height(), w.height()));
					//console.log("{0}, {1}".formatWith(w.scrollTop(), jQuery.Select(window.document).height() - w.height() - list.footerHeight));
					if (d.height() > w.height() && ((w.scrollTop() >= d.height() - w.height() - list.footerHeight) || (w.scrollTop() >= d.height() / 2)))
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
