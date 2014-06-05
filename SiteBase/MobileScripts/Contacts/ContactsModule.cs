﻿// ---------------------------------------------------------------------- //
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

namespace DigitalBeacon.SiteBase.Mobile.Contacts
{
	public static class ContactsModule
	{
		static ContactsModule()
		{
			Angular.module("contacts", new[] { "sitebase", "ui.router", "contactService" })
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
								templateUrl = ControllerHelper.getTemplateUrl("~/contacts"),
								controller = "contactListController"
							})
							.state("list.new", new
							{
								url = "/new",
								templateUrl = ControllerHelper.getTemplateUrl("~/contacts/new"),
								controller = "contactDetailsController"
							})
							.state("list.edit", new
							{
								url = "/{id:[0-9]{1,4}}",
								templateUrl = ControllerHelper.getTemplateUrl("~/contacts/0/edit"),
								controller = "contactDetailsController",
							});
					})
				})
				.controller("contactListController",
					new object[] { "$scope", "$state", "$location", "contactService", 
						(Action<Scope, object, ILocation, ContactService>)
						((scope, state, location, contactService) => 
							BaseController.extend(scope, new ContactListController(scope, state, location, contactService))) })
				.controller("contactDetailsController",
					new object[] { "$scope", "$state", "$location", "contactService", 
						(Action<Scope, object, ILocation, ContactService>)
						((scope, state, location, contactService) => 
							BaseController.extend(scope, new ContactDetailsController(scope, state, location, contactService))) })
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