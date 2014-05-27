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

namespace DigitalBeacon.SiteBase.Mobile.Contacts
{
	[ScriptIgnoreNamespace]
	public class DetailsController : BaseDetailsController
	{
		private ContactService _contactService;
		private dynamic _routeParams;
		private dynamic _location;

		public dynamic contact;

		//private dynamic _routeParams;

		//static DetailsController()
		//{
		//	((Action<dynamic, dynamic, dynamic>)instance)["$inject"] = new[] { "$scope", "$routeParams", "contactService" };
		//}

		//public static void instance(dynamic scope, dynamic routeParams, dynamic contactService)
		//{
		//	//scope.contact = contactService.get(new { id = routeParams.id });

		//	scope.contact = contactService.get(new { id = routeParams.id },
		//		(Action<dynamic>)(response =>
		//		{
		//			scope.contactJson = window.JSON.stringify(response);
		//		}));

		//	scope.filteredProperties = (Func<object, object>)
		//		(x =>
		//		{
		//			var retVal = new object();
		//			foreach (var key in Object.keys(x))
		//			{
		//				var val = x[key];
		//				if (!val || (val is Array && val.length == 0))
		//				{
		//					continue;
		//				}
		//				retVal[key] = x[key];
		//			}
		//			return retVal;
		//		});

		public DetailsController(dynamic scope, dynamic routeParams, dynamic location, ContactService contactService)
		{
			Scope = scope;
			_routeParams = routeParams;
			_location = location;
			_contactService = contactService;

			//, 
			//	(Action<dynamic>)
			//	(response =>
			//	{
			//		scope.contactJson = window.JSON.stringify(response);
			//	}));
		}

		public override void init()
		{
			contact = _contactService.get(new { id = _routeParams.id });
		}

		public override void submit()
		{
			_contactService.save(formData,
				(Action<dynamic>)(response =>
					{
						if (response.Success)
						{
							_location.path("/");
						}
						ApiResponseHelper.handleResponse(response, Scope);
					}));
		}

		public void cancel()
		{
			_location.path("/");
		}
	}
}
