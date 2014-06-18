// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using ng;

namespace DigitalBeacon.SiteBase.Mobile.Account
{
	public static class AccountModule
	{
		static AccountModule()
		{
			Angular.module("account", new[] { "sitebase", "accountService" })
				.controller("updateProfileController",
					new object[] { "$scope", "accountService", 
						(Action<dynamic, AccountService>)
						((scope, accountService) => 
							BaseController.initScope(scope, new UpdateProfileController(scope, accountService))) })
				.controller("changePasswordController",
					new object[] { "$scope", "accountService", 
						(Action<dynamic, AccountService>)
						((scope, accountService) => 
							BaseController.initScope(scope, new ChangePasswordController(scope, accountService))) })
				.controller("changeSecurityQuestionController",
					new object[] { "$scope", "accountService", 
						(Action<dynamic, AccountService>)
						((scope, accountService) => 
							BaseController.initScope(scope, new ChangeSecurityQuestionController(scope, accountService))) });
		}
	}
}
