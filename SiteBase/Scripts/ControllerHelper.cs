// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
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

		public static string getJsonUrl(string targetUrl)
		{
			return Utils.mergeParams(digitalbeacon.resolveUrl(targetUrl), new { renderType = "Json" });
		}
	}
}
