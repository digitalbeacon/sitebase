// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using System.Html;

namespace DigitalBeacon.SiteBase
{
	public static class ApiResponseHelper
	{
		public static void handleResponse(object obj, dynamic scope = null)
		{
			var response = (ApiResponse)obj;
			if (response != null)
			{
				if (response.RedirectUrl != null)
				{
					window.location.assign(response.RedirectUrl);
				}
				else if (scope == null)
				{
					window.alert(response.Message ?? response.ErrorMessage ?? toString(response.ValidationErrors));
				}
				else
				{
					var alerts = new dynamic[0];
					if (response.Message != null)
					{
						alerts.push(new { type = "success", msg = response.Message });
					}
					if (response.ErrorMessage != null)
					{
						alerts.push(new { type = "danger", msg = response.ErrorMessage });
					}
					foreach (var key in Object.keys(response.ValidationErrors))
					{
						foreach (var msg in response.ValidationErrors[key])
						{
							alerts.push(new { type = "danger", msg = msg });
						}
					}
					scope.alerts = alerts;
					scope.validationErrors = response.ValidationErrors ?? new Dictionary<string[]>();
					window.scrollTo(0, 0);
				}
			}
		}

		public static string toString(dynamic validationErrors)
		{
			var s = "";
			foreach (var key in Object.keys(validationErrors))
			{
				//s += key + " => ";
				s += ((string[])validationErrors[key]).join("\n");
				s += "\n";
			}
			return s;
		}
	}
}
