// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Dom;
using System.Html;
using DigitalBeacon;
using DigitalBeacon.SiteBase;
using DigitalBeacon.SiteBase.Mobile;
using jQueryLib;
using ng;

namespace DigitalBeacon.SiteBase.Mobile.Contacts
{
	[ScriptIgnoreNamespace]
	public class ContactsController : BaseListController
	{
		private ContactService _contactService;

		public string CommentTypeId = "";
		public string BirthMonth = "";
		public string Inactive = "";

		public dynamic[] contacts;
		public dynamic contact;

		//static ClientsController()
		//{
		//	((Action<dynamic, dynamic>)instance)["$inject"] = new[] { "$scope", "contactService" };
		//}

		//public static void instance(dynamic scope, dynamic contactService)
		//{
		//	contactService.search(null, (Action<dynamic>)(response => contacts = response.Data));

		//	showDetails = (Action<int>)
		//		(id => window.location.href = "#/" + id);
		//}

		public ContactsController(dynamic scope, dynamic routeParams, ILocation location, ContactService contactService)
		{
			Scope = scope;
			RouteParams = routeParams;
			Location = location;
			_contactService = contactService;
			((Scope)scope).on("$destroy", new Action(() => console.log("here")));
		}

		public override void init()
		{
			if (Location.path() == "/")
			{
				SortTextOptions = new[] { new Option("Last Name", "LastName") };
				SortText = SortTextOptions[0].value;
				SortDirection = SortDirectionOptions[1].value;
				//SearchFields = new Option[0];
				//SearchFields.push(new Option("Last Name", "LastName"));
				search();
			}
			else if (RouteParams.id)
			{
				contact = _contactService.get(new { id = RouteParams.id });
			}
		}

		public override void search(bool requestMore = false)
		{
			if (!requestMore)
			{
				Page = 1;
			}
			IsFiltered = SearchText && SearchText.hasText();
			_contactService.search(
				new 
				{
					PageSize = PageSize, 
					Page = Page, 
					SearchText = SearchText, 
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
				PageCount = Math.ceil(response.Total / PageSize);
				if (PageCount > 1)
				{
					enableLoadMoreOnScroll();
				}
			}
		}

		public void submit()
		{
			_contactService.save(formData,
				(Action<dynamic>)(response =>
				{
					if (response.Success)
					{
						Location.path("/");
					}
					ApiResponseHelper.handleResponse(response, Scope);
				}));
		}

		public void cancel()
		{
			Location.path("/");
		}
	}
}
