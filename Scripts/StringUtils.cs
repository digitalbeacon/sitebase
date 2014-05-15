// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using jQueryLib;

namespace DigitalBeacon
{
	public static class StringUtils
	{
		private static RegExp DateRegex = new RegExp(@"\/Date\((\-{0,1}\d+)\)\/", "gm");

		public static string formatWith(string format, object arg1, object arg2 = null, object arg3 = null, object arg4 = null, object arg5 = null)
		{
			var s = format;
			var args = new [] { arg1, arg2, arg3, arg4, arg5 };
			if ((bool)arg2 && (bool)arg3 && (bool)arg4 && (bool)arg5)
			{
				args.length = 1;
			}
			else if ((bool)arg3 && (bool)arg4 && (bool)arg5)
			{
				args.length = 2;
			}
			else if ((bool)arg4 && (bool)arg5)
			{
				args.length = 3;
			}
			else if ((bool)arg5)
			{
				args.length = 4;
			}
			for (var i = 0; i < args.length; i++)
			{
				var reg = new RegExp("\\{" + i + "\\}", "gm");
				s = s.replace(reg, "" + args[i]);
			}
			return s;
		}

		public static bool hasText(string str)
		{
			return jQuery.trim(str).length > 0;
		}

		public static Date toDate(string dateStr)
		{
			if (!dateStr)
			{
				return null;
			}
			var intDateStr = dateStr.replace(DateRegex, "$1");
			if (intDateStr)
			{
				return new Date(window.parseInt(intDateStr));
			}
			var intDateVal = Date.parse(dateStr);
			return intDateVal != window.NaN ? new Date(intDateVal) : null;
		}

		public static string expandSiteRelativeText(string text)
		{
			var regex = new RegExp("\"~/", "gm");
			return hasText(text) ? text.replace(regex, "\"" + (digitalbeacon.appContextPath == "/" ? "" : digitalbeacon.appContextPath) + "/") : text;
		}

		public static string toSiteRelativeText(string text)
		{
			if (digitalbeacon.appContextPath == "/")
			{
				return text;
			}
			var regex = new RegExp("\"" + digitalbeacon.appContextPath + "/", "gm");
			return (hasText(digitalbeacon.appContextPath) && hasText(text)) ? text.replace(regex, "\"~/") : text;
		}

	}
}
