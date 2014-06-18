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
	public class ChangeSecurityQuestionController : BaseController
	{
		private AccountService _accountService;

		public ChangeSecurityQuestionController(dynamic scope, AccountService accountService)
		{
			Scope = scope;
			_accountService = accountService;
		}

		protected override void submit(string modelName)
		{
			_accountService.changeSecurityQuestion(ScopeData.model, DefaultHandler);
		}
	}
}
