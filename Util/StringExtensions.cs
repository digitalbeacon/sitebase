// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Globalization;

namespace DigitalBeacon.Util
{
	public static class StringExtensions
	{
		public static void Guard(this string s, string message, params object[] args)
		{
			if (TextUtil.IsNullOrBlank(s))
			{
				throw new ArgumentNullException(String.Format(message, args));
			}
		}

		public static string DefaultTo(this string s, string defaultVal)
		{
			return TextUtil.IsNullOrBlank(s) ? defaultVal : s;
		}

		public static string DefaultTo(this string s, Func<string, string> fn)
		{
			return TextUtil.IsNullOrBlank(s) ? fn(s) : s;
		}

		public static string IfHasText(this string s, string val)
		{
			return TextUtil.HasText(s) ? val : s;
		}

		public static string IfHasText(this string s, Func<string, string> fn)
		{
			return TextUtil.HasText(s) ? fn(s) : s;
		}

		public static string FormatWith(this string s, params object[] args)
		{
			return FormatWith(s, CultureInfo.CurrentCulture, args);
		}

		public static string FormatWith(this string s, CultureInfo culture, params object[] args)
		{
			if (s == null)
			{
				return null;
			}
			if (args == null || args.Length == 0)
			{
				return s;
			}
			return String.Format(culture, s, args);
		}

		public static string EscapeForJavaScript(this string s)
		{
			return TextUtil.IsNullOrBlank(s) ? s : s.Replace("'", @"\'");
		}

		public static bool HasText(this string s)
		{
			return TextUtil.HasText(s);
		}

		public static bool IsNullOrBlank(this string s)
		{
			return TextUtil.IsNullOrBlank(s);
		}

		public static bool EqualsIgnoreCase(this string a, string b)
		{
			return TextUtil.IsEqualIgnoreCase(a, b);
		}

		public static bool IsEmail(this string candidate)
		{
			return TextUtil.IsEmail(candidate);
		}

		#region Conversion Methods

		public static bool? ToBoolean(this string candidate)
		{
			return TextUtil.ToBoolean(candidate);
		}

		public static bool IsBoolean(this string candidate)
		{
			return TextUtil.ToBoolean(candidate).HasValue;
		}

		public static int? ToInt32(this string candidate)
		{
			return TextUtil.ToInt32(candidate);
		}

		public static bool IsInt32(this string candidate)
		{
			return TextUtil.ToInt32(candidate).HasValue;
		}

		public static long? ToInt64(this string candidate)
		{
			return TextUtil.ToInt64(candidate);
		}

		public static bool IsInt64(this string candidate)
		{
			return TextUtil.ToInt64(candidate).HasValue;
		}

		public static decimal? ToDecimal(this string candidate)
		{
			return TextUtil.ToDecimal(candidate);
		}

		public static bool IsDecimal(this string candidate)
		{
			return TextUtil.ToDecimal(candidate).HasValue;
		}

		public static double? ToDouble(this string candidate)
		{
			return TextUtil.ToDouble(candidate);
		}

		public static bool IsDouble(this string candidate)
		{
			return TextUtil.ToDouble(candidate).HasValue;
		}

		public static DateTime? ToDate(this string candidate)
		{
			return TextUtil.ToDate(candidate);
		}

		public static bool IsDate(this string candidate)
		{
			return TextUtil.ToDate(candidate).HasValue;
		}

		public static DateTime? ToDate(this string candidate, string format)
		{
			return TextUtil.ToDate(candidate, format);
		}

		public static bool IsDate(this string candidate, string format)
		{
			return TextUtil.ToDate(candidate, format).HasValue;
		}

		public static Guid? ToGuid(this string candidate)
		{
			return TextUtil.ToGuid(candidate);
		}

		public static bool IsGuid(this string candidate)
		{
			return TextUtil.ToGuid(candidate).HasValue;
		}

		public static string ToCamelCase(this string s)
		{
			return TextUtil.ToCamelCase(s);
		}

		public static string ToPascalCase(this string s)
		{
			return TextUtil.ToPascalCase(s);
		}

		public static string ToLowerHyphenated(this string s)
		{
			return TextUtil.ToLowerHyphenated(s);
		}

		public static string ToDisplayCase(this string s)
		{
			return TextUtil.SeparatePascalCaseText(TextUtil.ToPascalCase(s));
		}

		#endregion
	}
}
