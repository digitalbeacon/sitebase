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
	public class IdentityService
	{
		static IdentityService()
		{
			Angular.module("identityService", new [] { "ngResource" })
				.factory("identityService", new dynamic[] 
				{ 
					"$resource",
					(Func<dynamic, dynamic>)
					(resource =>
					{
						return resource(
							ControllerHelper.getJsonUrl("~/identity/:operation"),
							new { },
							new
							{
								signIn = new { method = "POST", @params = new { operation = "signIn" } }
							});
					})
				});
		}

		[ScriptExternal]
		public extern void signIn(dynamic parameters, Action<ApiResponse> responseHandler);
	}
}
