// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using ng;

namespace DigitalBeacon.SiteBase.Mobile.Identity
{
	public static class IdentityModule
	{
		static IdentityModule()
		{
			Angular.module("identity", new[] { "sitebase", "identityService" })
				.controller("signInController",
					new object[] { "$scope", "identityService", 
						(Action<dynamic, IdentityService>)
						((scope, identityService) => 
							BaseController.extend(scope, new SignInController(scope, identityService))) })
				.controller("registrationController",
					new object[] { "$scope", "identityService", 
						(Action<dynamic, IdentityService>)
						((scope, identityService) => 
							BaseController.extend(scope, new RegistrationController(scope, identityService))) });
		}
	}
}
