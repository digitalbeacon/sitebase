// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;

namespace DigitalBeacon.SiteBase.Mobile.Identity
{
	[ScriptIgnoreNamespace]
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
		}

		public void signIn(string username, string password)
		{
			_identityService.signIn(new { Username = username, Password = password },
				(Action<dynamic>)(response => ApiResponseHelper.handleResponse(response, Scope)));
		}
	}
}
