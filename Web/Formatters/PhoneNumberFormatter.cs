// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using System.Text;
using Spring.Globalization;
using Spring.Util;

namespace DigitalBeacon.Web.Formatters
{
	public class PhoneNumberFormatter : IFormatter
	{
		private const string DefaultFormat = "(000) 000-0000";

		private string _format;

		public PhoneNumberFormatter()
		{
			_format = DefaultFormat;
		}

		public PhoneNumberFormatter(string format)
		{
			_format = format;
		}

		public string Format(object value)
		{
			string retVal = String.Empty;
			if (!String.IsNullOrEmpty(value as string))
			{
				retVal = value.ToString();
				if (value != null && value.ToString().Trim().Length == 10)
				{
					retVal = Convert.ToInt64(value).ToString(_format);
				}
			}
			return retVal;
		}

		public string Parse(string value)
		{
			var sb = new StringBuilder(value);
			for (int i = sb.Length - 1; i >= 0; i--)
			{
				if (!Char.IsDigit(sb[i]))
				{
					sb.Remove(i, 1);
				}
			}
			return sb.ToString();
		}

		#region IFormatter Members

		object IFormatter.Parse(string value)
		{
			return ((PhoneNumberFormatter)this).Parse(value);
		}

		#endregion
	}

}
