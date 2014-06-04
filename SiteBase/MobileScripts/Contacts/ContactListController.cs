// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Html;
using ng;

namespace DigitalBeacon.SiteBase.Mobile.Contacts
{
	public class ContactListController : BaseListController
	{
		private ContactService _contactService;

		public string CommentTypeId = "";
		public string BirthMonth = "";
		public string Inactive = "";

		public dynamic[] contacts;

		public ContactListController(Scope scope, dynamic state, ILocation location, ContactService contactService)
		{
			Scope = scope;
			RouterState = state;
			Location = location;
			_contactService = contactService;
			//Scope.on("$destroy", new Action(() => console.log("here")));
		}

		public override void init()
		{
			base.init();
			list.sortTextOptions = new[] { new Option("Last Name", "LastName") };
			list.sortText = list.sortTextOptions[0].value;
			list.sortDirection = list.sortDirectionOptions[1].value;
			//SearchFields = new Option[0];
			//SearchFields.push(new Option("Last Name", "LastName"));
			if (isListState())
			{
				search();
			}
		}

		public override void showList(ApiResponse response = null)
		{
			base.showList(response);
			if (list.pageCount < 0)
			{
				search();
			}
		}

		public override void search(bool requestMore = false)
		{
			if (!requestMore)
			{
				list.page = 1;
			}
			list.isFiltered = list.searchText && list.searchText.hasText();
			_contactService.search(
				new 
				{
					PageSize = list.pageSize,
					Page = list.page,
					SearchText = list.searchText, 
					SortValue = getSortValue(),
					CommentTypeId = CommentTypeId,
					BirthMonth = BirthMonth,
					Inactive = Inactive
				}, 
				(Action<dynamic>)(x => handleResponse(x, requestMore)));
		}

		private void handleResponse(dynamic response, bool isRequestForMore)
		{
			if (isRequestForMore)
			{
				foreach (object c in response.Data)
				{
					contacts.push(c);
				}
			}
			else
			{
				contacts = response.Data;
				list.pageCount = Math.ceil(response.Total / list.pageSize);
				enableLoadMoreOnScroll();
			}
		}
	}
}
