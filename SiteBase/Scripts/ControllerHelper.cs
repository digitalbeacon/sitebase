// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using System.Dom;
using System.Html;

namespace DigitalBeacon.SiteBase
{
	public static class ControllerHelper
	{
		public static string getTemplateUrl(string targetUrl)
		{
			return Utils.mergeParams(digitalbeacon.resolveUrl(targetUrl), new { renderType = "Template" });
		}

		public static string getJsonUrl(string targetUrl, object parameters = null)
		{
			return Utils.mergeParams(digitalbeacon.resolveUrl(targetUrl), new { renderType = "Json" });
		}

		public static Alert[] getAlerts(ApiResponse response)
		{
			var alerts = new Alert[0];
			if (response.Message)
			{
				alerts.push(new Alert { type = "success", msg = response.Message });
			}
			if (response.ErrorMessage)
			{
				alerts.push(new Alert { type = "danger", msg = response.ErrorMessage });
			}
			if (response.ValidationErrors)
			{
				foreach (var key in Object.keys(response.ValidationErrors))
				{
					foreach (var msg in response.ValidationErrors[key])
					{
						alerts.push(new Alert { type = "danger", msg = msg });
					}
				}
			}
			return alerts;
		}

		public static void handleResponse(ApiResponse response, dynamic scope = null)
		{
			if (response)
			{
				if (response.RedirectUrl)
				{
					window.location.assign(response.RedirectUrl);
				}
				else if (scope == null)
				{
					window.alert(response.Message ?? response.ErrorMessage ?? toString(response.ValidationErrors));
				}
				else
				{
					scope.alerts = getAlerts(response);
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
