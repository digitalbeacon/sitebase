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
							digitalbeacon.resolveUrl("~/identity/:operation/json"),
							new { },
							new
							{
								signIn = new { method = "POST", @params = new { operation = "signIn" } },
								changePassword = new { method = "POST", @params = new { operation = "changePassword" } },
								register = new { method = "POST", @params = new { operation = "register" } },
								recoverUsername = new { method = "POST", @params = new { operation = "recoverUsername" } },
								resetPassword = new { method = "POST", @params = new { operation = "resetPassword" } }
							});
					})
				});
		}

		[ScriptExternal]
		public extern void signIn(dynamic parameters, Action<ApiResponse> responseHandler);

		[ScriptExternal]
		public extern void changePassword(dynamic parameters, Action<ApiResponse> responseHandler);

		[ScriptExternal]
		public extern void register(dynamic parameters, Action<ApiResponse> responseHandler);

		[ScriptExternal]
		public extern void recoverUsername(dynamic parameters, Action<ApiResponse> responseHandler);

		[ScriptExternal]
		public extern void resetPassword(dynamic parameters, Action<ApiResponse> responseHandler);
	}
}
