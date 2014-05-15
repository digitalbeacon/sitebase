// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Text.RegularExpressions;
using System.Globalization;

namespace DigitalBeacon.Util
{
	public class ValidationUtil
	{
		public static readonly Regex GuidRegex = new Regex(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", RegexOptions.Compiled);
		public static readonly Regex EmailRegex = new Regex(@"^([\w\.!#\$%\-+.'_]+@[A-Za-z0-9\-]+(\.[A-Za-z0-9\-]+)+)", RegexOptions.Compiled);

		public static bool IsGuid(string candidate)
		{
			Guid temp;
			return TryParseGuid(candidate, out temp);
		}

		public static bool TryParseGuid(string candidate, out Guid output)
		{
			bool retVal = false;
			output = Guid.Empty;
			if (!String.IsNullOrEmpty(candidate))
			{
				if (GuidRegex.IsMatch(candidate))
				{
					output = new Guid(candidate);
					retVal = true;
				}
			}
			return retVal;
		}

		public static bool IsEmail(string candidate)
		{
			return IsValid(candidate, EmailRegex);
		}

		public static bool IsCurrency(string candidate)
		{
			return IsValid(candidate, new Regex(@"^(\+|-)?(\" + CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol + @")?[0-9](,[0-9]{3}|[0-9])*(\" + CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator + @"[0-9]{1,2})?$", RegexOptions.Compiled));
		}

		public static bool IsInt32(string candidate)
		{
			int value;
			return Int32.TryParse(candidate, out value);
		}

		public static bool IsInt64(string candidate)
		{
			long value;
			return Int64.TryParse(candidate, out value);
		}

		public static bool IsDouble(string candidate)
		{
			double value;
			return Double.TryParse(candidate, out value);
		}

		public static bool IsDate(string candidate)
		{
			DateTime date;
			return DateTime.TryParse(candidate, out date);
		}

		public static bool IsDate(string candidate, string format)
		{
			DateTime date;
			return DateTime.TryParseExact(candidate, format, CultureInfo.CurrentCulture.DateTimeFormat, DateTimeStyles.None, out date);
		}

		public static bool IsNullOrEmpty(string candidate)
		{
			return candidate.IsNullOrBlank();
		}

		private static bool IsValid(string candidate, Regex expression)
		{
			bool retVal = false;
			if (!String.IsNullOrEmpty(candidate))
			{
				retVal = expression.IsMatch(candidate);
			}
			return retVal;
		}
	}
}
