// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;

namespace DigitalBeacon.SiteBase.Mobile.Identity
{
	public class IdentityController : BaseController
	{
		private IdentityService _identityService;

		public IdentityController(dynamic scope, IdentityService identityService)
		{
			Scope = scope;
			_identityService = identityService;
		}

		public override void init()
		{
			base.init();
		}

		public void signIn()
		{
			_identityService.signIn(formData,
				(Action<dynamic>)(response => ApiResponseHelper.handleResponse(response, Scope)));
		}
	}
}
