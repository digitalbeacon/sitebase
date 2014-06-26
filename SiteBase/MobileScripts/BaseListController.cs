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
using ng;

namespace DigitalBeacon.SiteBase.Mobile
{
	public abstract class BaseListController : BaseController
	{
		protected new ListScopeData ScopeData
		{
			get { return (ListScopeData)data; }
		}

		protected override void init()
		{
			base.init();
			jQuery.extend(data,
				new ListScopeData
				{
					page = 1,
					pageSize = 20,
					pageCount = -1,
					sortDirection = "",
					isCollapsedAdvancedSearch = true,
					footerHeight = 140,
					listVisible = true
				});
			ScopeData.listVisible = isListState();
			Scope.on("hideDetails", new Action<object, ApiResponse>((evt, args) => { showList(args); }));
			Scope.on("showDetails", new Action(() => { hideList(); }));
			Scope.on("showPrevious", new Action<object, int>((evt, currentId) => { showPrevious(currentId); }));
			Scope.on("showNext", new Action<object, int>((evt, currentId) => { showNext(currentId); }));
			Scope.on("detailsChanged", new Action(() => { getStateData("list").refresh = true; }));
			Scope.on("alerts", new Action<object, Alert[]>((evt, args) => { ScopeData.alerts = args; }));
			Scope.watch(new Func<string>(() => Location.url()), new Action<string>(url =>
			{
				if (!ScopeData.listVisible && url && isListState())
				{
					showList();
				}
				if (!RouterState.@is("list.display"))
				{
					ScopeData.transitionPrevious = false;
					ScopeData.transitionNext = false;
				}
				window.scrollTo(0, 0);
			}));
		}

		public abstract void search(bool requestMore = false);

		public virtual void queueSearch()
		{
			window.setTimeout(new Action(() => 
			{
				search();
				Scope.apply();
			}), 100);
		}

		public virtual void loadMore()
		{
			if (ScopeData.page < ScopeData.pageCount)
			{
				ScopeData.page++;
				search(true);
			}
		}

		public virtual void addNew()
		{
			clearAlerts();
			RouterState.go("list.new");
			ScopeData.model = new { };
		}

		public virtual void showDetails(int id)
		{
			clearAlerts();
			RouterState.go("list.display", new { id = id });
			for (int i = 0; i < ScopeData.items.length; i++)
			{
				if (ScopeData.items[i].Id == id)
				{
					if (i >= ScopeData.items.length - 2)
					{
						loadMore();
					}
					break;
				}
			}
		}

		public virtual void showPrevious(int currentId)
		{
			if (!ScopeData.items || ScopeData.items.length < 2)
			{
				return;
			}
			ScopeData.transitionPrevious = true;
			ScopeData.transitionNext = false;
			for (int i = 1; i < ScopeData.items.length; i++)
			{
				if (ScopeData.items[i].Id == currentId)
				{
					showDetails(ScopeData.items[i - 1].Id);
					break;
				}
			}
		}

		public virtual void showNext(int currentId)
		{
			if (!ScopeData.items || ScopeData.items.length < 2)
			{
				return;
			}
			ScopeData.transitionPrevious = false;
			ScopeData.transitionNext = true;
			for (int i = 0; i < ScopeData.items.length - 1; i++)
			{
				if (ScopeData.items[i].Id == currentId)
				{
					showDetails(ScopeData.items[i + 1].Id);
					break;
				}
			}
		}

		public virtual void hideList()
		{
			ScopeData.listVisible = false;
			disableLoadMoreOnScroll();
		}

		public virtual void showList(ApiResponse response = null)
		{
			ScopeData.listVisible = true;
			RouterState.go("list");
			if (ScopeData.pageCount < 0 || getStateData("list").refresh)
			{
				getStateData("list").refresh = false;
				search();
			}
			enableLoadMoreOnScroll();
			if (response)
			{
				ControllerHelper.handleResponse(response, Scope);
			}
		}

		protected bool isListState()
		{
			return RouterState.current.name == "list";
		}

		protected virtual void clearSearch()
		{
			ScopeData.searchText = "";
			ScopeData.sortDirection = "";
			//if (ScopeData.isFiltered)
			//{
				search();
			//}
			ScopeData.isFiltered = false;
			ScopeData.page = 1;
		}

		protected virtual string getSortValue()
		{
			return ScopeData.sortText + ScopeData.sortDirection;
		}

		protected virtual void enableLoadMoreOnScroll()
		{
			if (ScopeData.pageCount <= 1 || !ScopeData.listVisible)
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
					if (d.height() > w.height() && ((w.scrollTop() >= d.height() - w.height() - ScopeData.footerHeight) || (w.scrollTop() >= d.height() / 2)))
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
