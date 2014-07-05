// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using jQueryLib;
using ng;

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

		public override void submitForm(string modelName, bool isValid)
		{
			//base.submitForm(modelName, isValid);
			if (!isValid)
			{
				// handle browser autocomplete implementations that do not trigger a change event
				// to update the model binding
				if (!ScopeData.model.Username)
				{
					ScopeData.model.Username = jQuery.Select("#Username").val();
				}
				if (!ScopeData.model.Password)
				{
					ScopeData.model.Password = jQuery.Select("#Password").val();
				}
			}
			submit(modelName);
		}
	}
}
