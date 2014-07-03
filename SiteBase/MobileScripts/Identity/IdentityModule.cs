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
							BaseController.initScope(scope, new SignInController(scope, identityService))) })
				.controller("changePasswordController",
					new object[] { "$scope", "identityService", 
						(Action<dynamic, IdentityService>)
						((scope, identityService) => 
							BaseController.initScope(scope, new ChangePasswordController(scope, identityService))) })
				.controller("registrationController",
					new object[] { "$scope", "identityService", 
						(Action<dynamic, IdentityService>)
						((scope, identityService) => 
							BaseController.initScope(scope, new RegistrationController(scope, identityService))) })
				.controller("recoverUsernameController",
					new object[] { "$scope", "identityService", 
						(Action<dynamic, IdentityService>)
						((scope, identityService) => 
							BaseController.initScope(scope, new RecoverUsernameController(scope, identityService))) })
				.controller("resetPasswordController",
					new object[] { "$scope", "identityService", 
						(Action<dynamic, IdentityService>)
						((scope, identityService) => 
							BaseController.initScope(scope, new ResetPasswordController(scope, identityService))) });
		}
	}
}
