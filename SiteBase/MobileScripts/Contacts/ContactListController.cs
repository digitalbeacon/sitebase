// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Html;
using jQueryLib;
using ng;

namespace DigitalBeacon.SiteBase.Mobile.Contacts
{
	public class ContactListController : BaseListController
	{
		private ContactService _contactService;
		private const string DefaultSortText = "LastName";

		protected new ContactListScopeData ScopeData
		{
			get { return (ContactListScopeData)data; }
		}

		public ContactListController(Scope scope, dynamic state, ILocation location, ContactService contactService)
		{
			Scope = scope;
			RouterState = state;
			Location = location;
			_contactService = contactService;
			//Scope.on("$destroy", new Action(() => console.log("here")));
		}

		protected override void init()
		{
			base.init();
			ScopeData.sortText = DefaultSortText;
			ScopeData.CommentTypeId = "";
			ScopeData.Inactive = "";
			//if (isListState())
			//{
				search();
			//}
		}

		public override void search(bool requestMore = false)
		{
			if (!requestMore)
			{
				ScopeData.page = 1;
			}
			ScopeData.isFiltered = ScopeData.searchText && ScopeData.searchText.hasText();
			_contactService.search(
				new 
				{
					PageSize = ScopeData.pageSize,
					Page = ScopeData.page,
					SearchText = ScopeData.searchText,
					SortValue = getSortValue(),
					CommentTypeId = ScopeData.CommentTypeId,
					Inactive = ScopeData.Inactive
				}, 
				(Action<dynamic>)(x => handleResponse(x, requestMore)));
		}

		protected override void clearSearch()
		{
			if (!ScopeData.isCollapsedAdvancedSearch)
			{
				ScopeData.sortText = DefaultSortText;
				ScopeData.CommentTypeId = "";
				ScopeData.Inactive = "";
			}
			base.clearSearch();
		}

		private void handleResponse(dynamic response, bool isRequestForMore)
		{
			foreach (dynamic c in response.Data)
			{
				if (c.PhotoId)
				{
					c.photoUrl = digitalbeacon.resolveUrl("~/contacts/{0}/thumbnail?x={1}".formatWith((int)c.Id, (int)c.PhotoId));
				}
			}
			if (isRequestForMore)
			{
				foreach (dynamic c in response.Data)
				{
					ScopeData.items.push((object)c);
				}
			}
			else
			{
				ScopeData.items = response.Data;
				ScopeData.pageCount = Math.ceil(response.Total / ScopeData.pageSize);
				enableLoadMoreOnScroll();
			}
		}

		[ScriptObjectLiteral]
		public class ContactListScopeData : ListScopeData
		{
			public string CommentTypeId;
			public string Inactive;
		}
	}
}
