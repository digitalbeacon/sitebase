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
	public class ChangePasswordController : BaseController
	{
		private IdentityService _identityService;

		public ChangePasswordController(dynamic scope, IdentityService identityService)
		{
			Scope = scope;
			_identityService = identityService;
		}

		protected override void submit(string modelName)
		{
			if (ScopeData.model.PasswordConfirm != ScopeData.model.Password)
			{
				setAlert(sitebase.localization.passwordConfirmNotMatched);
				return;
			}
			if (ScopeData.model.Password == ScopeData.model.Username)
			{
				setAlert(sitebase.localization.passwordInvalid);
				return;
			}
			_identityService.changePassword(ScopeData.model, DefaultHandler);
		}
	}
}
