// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using jQueryLib;
using ng;

namespace DigitalBeacon.SiteBase.Mobile.Identity
{
	[ScriptIgnoreNamespace]
	public static class IdentityModule
	{
		static IdentityModule()
		{
			Angular.module("identity", new[] { "ui.bootstrap", "identityService" })
				.controller("identityController",
					new object[] { "$scope", "identityService", 
						(Action<dynamic, IdentityService>)
						((scope, identityService) => 
							jQuery.extend(scope, new IdentityController(scope, identityService)).init()) });

		}
	}
}
