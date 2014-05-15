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
	[ScriptExternal]
	[ScriptIgnoreNamespace]
	public static class StringExtensions
	{
		[ScriptMixin]
		public static string formatWith(this string format, object arg1, object arg2 = null, object arg3 = null, object arg4 = null, object arg5 = null)
		{
			return StringUtils.formatWith(format, arg1, arg2, arg3, arg4, arg5);
		}

		[ScriptMixin]
		public static bool hasText(this string str)
		{
			return StringUtils.hasText(str);
		}

		[ScriptMixin]
		public static bool toDate(this string str)
		{
			return StringUtils.toDate(str);
		}

		[ScriptMixin]
		public static string expandSiteRelativeText(this string str)
		{
			return StringUtils.expandSiteRelativeText(str);
		}

		[ScriptMixin]
		public static string toSiteRelativeText(this string str)
		{
			return StringUtils.toSiteRelativeText(str);
		}
	}
}
