// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using jQueryLib;
using ng;

namespace DigitalBeacon.SiteBase.Mobile
{
	public static class SiteBaseModule
	{
		static SiteBaseModule()
		{
			Angular.module("sitebase", new[] { "ngSanitize", "ui.bootstrap", "ui.mask" })
				.config(new dynamic[] 
				{ 
					"$httpProvider",
					"$locationProvider",
					(Action<dynamic, ILocationProvider>)
					((httpProvider, locationProvider) =>
					{
						locationProvider.html5Mode(true);
						httpProvider.defaults.transformRequest.push(
							new Func<object, object>(data => 
							{
								sitebase.onAjaxStart();
								return data;
							}));
						httpProvider.defaults.transformResponse.push(
							new Func<object, object>(data => 
							{
								sitebase.onAjaxEnd();
								return Utils.convertDateStringsToDates(data);
							}));
					})
				});
		}
	}
}
