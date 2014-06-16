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

		protected override void submit(string modelName)
		{
			_identityService.signIn(ScopeData.model, DefaultHandler);
		}
	}
}
