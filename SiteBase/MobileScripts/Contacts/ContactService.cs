// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Dom;
using System.Html;
using ng;
using DigitalBeacon;
using DigitalBeacon.SiteBase.Mobile;
using DigitalBeacon.SiteBase;

namespace DigitalBeacon.SiteBase.Mobile.Contacts
{
	[ScriptIgnoreNamespace]
	public class ContactService : BaseService
	{
		static ContactService()
		{
			Angular.module("contactService", new [] { "ngResource" })
				.factory("contactService", new dynamic[] 
				{ 
					"$resource",
					(Func<dynamic, dynamic>)
					(resource =>
					{
						return resource(
							ControllerHelper.getApiUrl("~/contacts/:id"),
							new { },
							new
							{
								update = new { method = "PUT" },
								search = new { method = "POST", @params = new { id = "search" } }
							});
					})
				});
		}
	}
}