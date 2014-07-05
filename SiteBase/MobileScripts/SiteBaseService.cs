// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using ng;

namespace DigitalBeacon.SiteBase.Mobile
{
	public class SiteBaseService
	{
		private dynamic _postalCodesResource;

		static SiteBaseService()
		{
			Angular.module("siteBaseService", new [] { "ngResource" })
				.factory("siteBaseService", new dynamic[] 
				{ 
					"$resource",
					(Func<dynamic, dynamic>)
					((resource) =>
					{
						return new SiteBaseService(resource);
					})
				});
		}

		public SiteBaseService(dynamic resource)
		{
			_postalCodesResource = resource(
				digitalbeacon.resolveUrl("~/postalCodes/:id/:action/json"),
				new { id = "@id" },
				new
				{
					code = new { method = "GET", @params = new { action = "code" } }
				});
		}

		public PostalCodeData getPostalCodeData(string postalCode, Action<PostalCodeData> responseHandler = null)
		{
			return _postalCodesResource.code(new { id = postalCode }, responseHandler);
		}
	}
}