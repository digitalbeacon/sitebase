// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;

namespace DigitalBeacon.SiteBase.Mobile.Identity
{
	public class RecoverUsernameController : BaseController
	{
		private IdentityService _identityService;

		public RecoverUsernameController(dynamic scope, IdentityService identityService)
		{
			Scope = scope;
			_identityService = identityService;
		}

		protected override void submit(string modelName)
		{
			_identityService.recoverUsername(ScopeData.model, DefaultHandler);
		}
	}
}
