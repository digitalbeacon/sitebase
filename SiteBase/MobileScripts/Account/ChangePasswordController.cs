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
	public class ChangePasswordController : BaseController
	{
		private AccountService _accountService;

		public ChangePasswordController(dynamic scope, AccountService accountService)
		{
			Scope = scope;
			_accountService = accountService;
		}

		protected override void submit(string modelName)
		{
			if (ScopeData.model.NewPasswordConfirm != ScopeData.model.NewPassword)
			{
				setAlert(sitebase.localization.passwordConfirmNotMatched);
				return;
			}
			if (ScopeData.model.NewPassword == ScopeData.model.Username)
			{
				setAlert(sitebase.localization.passwordInvalid);
				return;
			}
			_accountService.changePassword(ScopeData.model, DefaultHandler);
		}
	}
}
