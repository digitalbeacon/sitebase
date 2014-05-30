// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using jQueryLib;

namespace DigitalBeacon.SiteBase.Mobile.Identity
{
	public class SignInController : BaseController
	{
		private IdentityService _identityService;

		public SignInController(dynamic scope, IdentityService identityService)
		{
			Scope = scope;
			_identityService = identityService;
		}

		public override void init()
		{
			base.init();
		}

		public void submit(bool isValid)
		{
			model.submitted = true;
			if (!isValid)
			{
				return;
			}
			_identityService.signIn(model,
				(Action<dynamic>)(response => ApiResponseHelper.handleResponse(response, Scope)));
		}
	}
}
