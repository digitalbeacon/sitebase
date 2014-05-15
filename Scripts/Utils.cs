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
	}
}
