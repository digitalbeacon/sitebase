// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using jQueryLib;

namespace DigitalBeacon.SiteBase.Mobile.Account
{
	public class UpdateProfileController : BaseController
	{
		private AccountService _accountService;

		public UpdateProfileController(dynamic scope, AccountService accountService)
		{
			Scope = scope;
			_accountService = accountService;
		}

		protected override void init()
		{
			base.init();
			_accountService.getProfile(null, createHandler(response =>
				{
					var defaultLanguage = ScopeData.model.Language;
					response.Data.Language = response.Data.Language || defaultLanguage;
					var defaultCountry = ScopeData.model.Country;
					response.Data.Country = response.Data.Country || defaultCountry;
					response.Data.State = response.Data.State || "";
					ScopeData.model = response.Data;
				}));
		}

		protected override void submit(string modelName)
		{
			_accountService.updateProfile(ScopeData.model, DefaultHandler);
		}
	}
}
