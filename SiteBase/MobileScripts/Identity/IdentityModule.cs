﻿// ---------------------------------------------------------------------- //
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
				.controller("identityController",
					new object[] { "$scope", "identityService", 
						(Action<dynamic, IdentityService>)
						((scope, identityService) => 
							BaseController.extend(scope, new IdentityController(scope, identityService))) });
		}
	}
}
