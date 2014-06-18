﻿// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using jQueryLib;

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

		protected override void submit(string modelName)
		{
			_identityService.register(ScopeData.model, DefaultHandler);
		}
	}
}
