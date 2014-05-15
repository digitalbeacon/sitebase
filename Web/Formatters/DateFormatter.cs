// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using System.Text;
using Spring.Util;
using Spring.Globalization;
using Spring.Globalization.Formatters;
using System.Globalization;

namespace DigitalBeacon.Web.Formatters
{
	public class DateFormatter : DateTimeFormatter
	{
		private string _format;

		public DateFormatter(string format) : base(format)
		{
			_format = format;
		}

		public bool TryParseExact(string value, out DateTime result)
		{
			return DateTime.TryParseExact(value, _format, CultureInfo.CurrentCulture.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces, out result);
		}
	}

}
