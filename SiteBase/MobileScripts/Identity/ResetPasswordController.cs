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
	public class ResetPasswordController : BaseController
	{
		private IdentityService _identityService;

		public ResetPasswordController(dynamic scope, IdentityService identityService)
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

			_identityService.resetPassword(ScopeData.model, createHandler(response =>
			{
				if (response.Data.Step > 1)
				{
					Angular.extend(ScopeData.model, response.Data);
					resetForm("resetPasswordPanel");
				}
			}));
		}

		public void back()
		{
			ScopeData.model.Step = 1;
		}
	}
}
