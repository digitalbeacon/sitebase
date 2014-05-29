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
	public static class SiteBaseModule
	{
		static SiteBaseModule()
		{
			Angular.module("sitebase", new[] { "ngSanitize", "ui.bootstrap" })
				.config(new dynamic[] 
				{ 
					"$httpProvider",
					(Action<dynamic>)
					((httpProvider) =>
					{
						httpProvider.defaults.transformResponse.push(new Func<object, object>(data => Utils.convertDateStringsToDates(data)));
					})
				});
		}
	}
}
