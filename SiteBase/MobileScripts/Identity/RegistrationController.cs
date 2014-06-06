// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;

namespace DigitalBeacon.SiteBase.Mobile.Identity
{
	public class RegistrationController : BaseController
	{
		private IdentityService _identityService;

		public RegistrationController(dynamic scope, IdentityService identityService)
		{
			Scope = scope;
			_identityService = identityService;
		}

		public override void init()
		{
			base.init();
		}

		public override void submit()
		{ 
			_identityService.register(model,
				(Action<dynamic>)(response => ControllerHelper.handleResponse(response, Scope)));
		}
	}
}
