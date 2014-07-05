// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Dom;
using System.Html;
using System.Linq;
using ng;
using jQueryLib;
using DigitalBeacon.SiteBase;
using DigitalBeacon.SiteBase.Mobile;
using ng.ui.router;

namespace DigitalBeacon.SiteBase.Mobile.Contacts
{
	public static class ContactsModule
	{
		static ContactsModule()
		{
			Angular.module("contacts", new[] { "sitebase", "ui.router", "siteBaseService", "contactService" })
				.config(new object[] 
				{ 
					"$stateProvider",
					(Action<dynamic>)
					((stateProvider) =>
					{
						stateProvider
							.state("list", new
							{
								url = digitalbeacon.resolveUrl("~/contacts"),
								templateUrl = digitalbeacon.resolveUrl("~/contacts/template"),
								controller = "contactListController"
							})
							.state("list.new", new
							{
								url = "/new",
								templateUrl = digitalbeacon.resolveUrl("~/contacts/new/template"),
								controller = "contactDetailsController"
							})
							.state("list.display", new
							{
								url = "/{id:[0-9]{1,9}}",
								templateUrl = digitalbeacon.resolveUrl("~/contacts/0/template"),
								controller = "contactDetailsController",
							})
							.state("list.edit", new
							{
								url = "/{id:[0-9]{1,9}}/edit",
								templateUrl = digitalbeacon.resolveUrl("~/contacts/0/edit/template"),
								controller = "contactDetailsController",
							});
					})
				})
				.controller("contactListController",
					new object[] { "$scope", "$state", "$location", "contactService", 
						(Action<Scope, State, ILocation, ContactService>)
						((scope, state, location, contactService) => 
							BaseController.initScope(scope, new ContactListController(scope, state, location, contactService))) })
				.controller("contactDetailsController",
					new object[] { "$scope", "$state", "$location", "siteBaseService", "contactService", 
						(Action<Scope, State, ILocation, SiteBaseService, ContactService>)
						((scope, state, location, siteBaseService, contactService) => 
							BaseController.initScope(scope, new ContactDetailsController(scope, state, location, siteBaseService, contactService))) })
				.run(new object[]
				{
					"$state",
					new Action<dynamic>(state => 
					{
						digitalbeacon.loadCssFile("~/resources/base/contacts/styles.css");
						state.transitionTo("list");
					})
				});
		}
	}
}
