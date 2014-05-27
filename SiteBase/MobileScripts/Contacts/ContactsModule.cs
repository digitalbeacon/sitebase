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

namespace DigitalBeacon.SiteBase.Mobile.Contacts
{
	[ScriptIgnoreNamespace]
	public static class ContactsModule
	{
		static ContactsModule()
		{
			Angular.module("contacts", new[] { "ngRoute", "ui.bootstrap", "contactService" })
				.config(new dynamic[] 
				{ 
					"$routeProvider",
					"$locationProvider",
					(Action<IRouteProvider, ILocationProvider>)
					((routeProvider, locationProvider) =>
					{
						routeProvider
							.when("/", new Route
							{
								templateUrl = ControllerHelper.getTemplateUrl("~/contacts/"),
								controller = getContactsController()
							})
							.when("/new", new Route
							{
								templateUrl = ControllerHelper.getTemplateUrl("~/contacts/new"),
								controller = getContactsController()
							})
							.when("/:id", new Route
							{
								templateUrl = ControllerHelper.getTemplateUrl("~/contacts/edit"),
								controller = getContactsController()
							})
							.otherwise(new { redirectTo = "/" });
						locationProvider.html5Mode(true);
					})
				});

			//var app = Angular.module("contacts", new[] { "ui.bootstrap", "contactService" });
			//app.controller("contactsController", (Action<dynamic, dynamic>)ClientsController.instance);
		}

		static object getContactsController()
		{
			return new object[] { "$scope", "$routeParams", "$location", "contactService", 
									(Action<dynamic, dynamic, ILocation, ContactService>)
									((scope, routeParams, location, contactService) => 
										extend(scope, new ContactsController(scope, routeParams, location, contactService))) };
		}

		static void extend(object target, object object1)
		{
			((BaseController)jQuery.extend(target, object1)).init();
		}
	}
}
