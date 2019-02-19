// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

namespace DigitalBeacon
{
	export class StringUtils
	{
		static readonly DateRegex : RegExp = new RegExp("\/Date\((\-{0,1}\d+)\)\/", "gm");

		public static formatWith(format : string, arg1, arg2 = null, arg3 = null, arg4 = null, arg5 = null) : string
	{
			let s = format;
			let args = [ arg1, arg2, arg3, arg4, arg5 ];
			if (arg2 && arg3 && arg4 && arg5)
			{
				args.length = 1;
			}
			else if (arg3 && arg4 && arg5)
			{
				args.length = 2;
			}
			else if (arg4 && arg5)
			{
				args.length = 3;
			}
			else if (arg5)
			{
				args.length = 4;
			}
			for (let i = 0; i < args.length; i++)
			{
				let reg = new RegExp("\\{" + i + "\\}", "gm");
				s = s.replace(reg, "" + args[i]);
			}
			return s;
		}

		public static hasText(str : string) : boolean 
		{
			return jQuery.trim(str).length > 0;
		}

		public static isDateString(dateStr : string) : boolean
		{
			return StringUtils.DateRegex.test(dateStr);
		}

		public static toDate(dateStr : string) : Date
		{
			if (!dateStr)
			{
				return null;
			}
			var intDateStr = dateStr.replace(StringUtils.DateRegex, "$1");
			if (intDateStr)
			{
				return new Date(parseInt(intDateStr));
			}
			var intDateVal = Date.parse(dateStr);
			return !isNaN(intDateVal) ? new Date(intDateVal) : null;
		}

		public static expandSiteRelativeText(text : string) : string
		{
			var regex = new RegExp("\"~/", "gm");
			return StringUtils.hasText(text) ? text.replace(regex, "\"" + ($.digitalbeacon.appContextPath == "/" ? "" : $.digitalbeacon.appContextPath) + "/") : text;
		}

		public static toSiteRelativeText(text : string) : string
		{
			if ($.digitalbeacon.appContextPath == "/")
			{
				return text;
			}
			let regex = new RegExp("\"" + $.digitalbeacon.appContextPath + "/", "gm");
			return (StringUtils.hasText($.digitalbeacon.appContextPath) && StringUtils.hasText(text)) ? text.replace(regex, "\"~/") : text;
		}

	}
}
