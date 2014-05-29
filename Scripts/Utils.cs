// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using System.Html;

namespace DigitalBeacon
{
	public static class Utils
	{
		public static string mergeParams(string url, dynamic args)
		{
			foreach (var key in Object.keys(args))
			{
				var regExp = new RegExp("({0})=([^&]*)".formatWith((string)key), "gi");
				if (regExp.test(url))
				{
					url = url.replace(regExp, "$1=" + args[key]);
				}
				else {
					var sb = new StringBuilder();
					sb.cat(url);
					if (url.indexOf("?") < 0) {
						sb.cat("?");
					}
					else {
						sb.cat("&");
					}
					url = sb.cat(key).cat("=").cat(window.encodeURIComponent(args[key])).build();
				}
			}
			return url;
		}

		public static object convertDateStringsToDates(object input, int level = 0)
		{
			if (!digitalbeacon.isObject(input))
			{
				return input;
			}
			foreach (var key in Object.keys(input))
			{
				if (!((dynamic)input).hasOwnProperty(key)) continue;
				var value = input[key];
				if (digitalbeacon.isString(value) && StringUtils.isDateString(value))
				{
					input[key] = StringUtils.toDate(value);
				}
				else if (value && digitalbeacon.isObject(value) && level < 10)
				{
					convertDateStringsToDates(value, level++);
				}
			}
			return input;
		}
	}
}
