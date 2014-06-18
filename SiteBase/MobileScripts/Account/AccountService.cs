// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using ng;

namespace DigitalBeacon.SiteBase.Mobile.Account
{
	public class AccountService
	{
		static AccountService()
		{
			Angular.module("accountService", new[] { "ngResource" })
				.factory("accountService", new dynamic[] 
				{ 
					"$resource",
					(Func<dynamic, dynamic>)
					(resource =>
					{
						return resource(
							digitalbeacon.resolveUrl("~/account/:operation/json"),
							new { },
							new
							{
								getProfile = new { method = "GET", @params = new { operation = "updateProfile" } },
								updateProfile = new { method = "POST", @params = new { operation = "updateProfile" } },
								changePassword = new { method = "POST", @params = new { operation = "changePassword" } },
								changeSecurityQuestion = new { method = "POST", @params = new { operation = "changeSecurityQuestion" } }
							});
					})
				});
		}

		[ScriptExternal]
		public extern dynamic getProfile(dynamic parameters, Action<ApiResponse> responseHandler);

		[ScriptExternal]
		public extern void updateProfile(dynamic parameters, Action<ApiResponse> responseHandler);

		[ScriptExternal]
		public extern void changePassword(dynamic parameters, Action<ApiResponse> responseHandler);

		[ScriptExternal]
		public extern void changeSecurityQuestion(dynamic parameters, Action<ApiResponse> responseHandler);
	}
}
