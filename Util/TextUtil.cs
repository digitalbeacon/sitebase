// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System.Text.RegularExpressions;

namespace DigitalBeacon.Util
{
	public static class TextUtil
	{
		private static readonly object[] EmptyObjectArray = new object[0];

		public const string DefaultEnclosingSubstitutionTag = "$$";

		public static readonly Regex GuidRegex = new Regex(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", RegexOptions.Compiled);
		public static readonly Regex EmailRegex = new Regex(@"^([\w\.!#\$%\-+.'_]+@[A-Za-z0-9\-]+(\.[A-Za-z0-9\-]+)+)", RegexOptions.Compiled);

		public static string Wrap(string text, int maxLength)
		{
			return Wrap(text, maxLength, 0);
		}

		public static string Wrap(string text, int maxLength, int paddingLength)
		{
			var sb = new StringBuilder(text.Length);
			var lines = text.Split('\n');
			foreach (var line in lines)
			{
				var s = line.Replace("\r", string.Empty).TrimEnd(new [] {' ', '\t'});
				if (s.Length <= maxLength)
				{
					sb.AppendLine(s);
				}
				else
				{
					s = line.Replace(".", ". ");
					s = s.Replace("\t", " ");
					s = s.Replace(",", ", ");
					s = s.Replace(";", "; ");
					var words = s.Split(new[] { ' ' });
					var currentLineLength = 0;
					foreach (var currentWord in words)
					{
						if (currentLineLength + currentWord.Length + 1 < maxLength)
						{
							if (currentLineLength > 0)
							{
								sb.Append(' ');
							}
							sb.Append(currentWord);
							currentLineLength += currentWord.Length + 1;
						}
						else
						{
							sb.AppendLine();
							for (var i = 0; i < paddingLength; i++)
							{
								sb.Append(' ');
							}
							sb.Append(currentWord);
							currentLineLength = currentWord.Length + paddingLength;
						}
					}
				}
			}
			return sb.ToString();
		}

		public static string SeparatePascalCaseText(string text, object separator)
		{
			if (IsNullOrBlank(text))
			{
				return text;
			}
			var sb = new StringBuilder(text);
			for (var i = 1; i < sb.Length; i++)
			{
				if (Char.IsUpper(sb[i]))
				{
					sb.Insert(i++, separator);
				}
			}
			return sb.ToString();
		}

		public static string ToCamelCase(string name)
		{
			if (IsNullOrBlank(name))
			{
				return name;
			}
			var str = String.Empty;
			var flag = false;
			var flag2 = true;
			var flag3 = true;
			foreach (var ch in name)
			{
				if (char.IsLower(ch))
				{
					flag3 = false;
					break;
				}
			}
			foreach (var ch2 in name)
			{
				switch (ch2)
				{
					case ' ':
						if (!flag2)
						{
							flag = true;
						}
						break;
					case '.':
						if (!flag2)
						{
							flag = true;
						}
						break;
					case '_':
						if (!flag2)
						{
							flag = true;
						}
						break;
					case '-':
						if (!flag2)
						{
							flag = true;
						}
						break;
					default:
						if (flag)
						{
							str = str + ch2.ToString().ToUpper();
							flag = false;
						}
						else if (flag2)
						{
							str = str + ch2.ToString().ToLower();
							flag2 = false;
						}
						else if (flag3)
						{
							str = str + ch2.ToString().ToLower();
						}
						else
						{
							str = str + ch2;
						}
						break;
				}
			}
			return str;
		}

		public static string ToPascalCase(string name)
		{
			if (IsNullOrBlank(name))
			{
				return name;
			}
			var str = String.Empty;
			var flag = true;
			var flag2 = true;
			foreach (var ch in name)
			{
				if (char.IsLower(ch))
				{
					flag2 = false;
					break;
				}
			}
			foreach (var ch2 in name)
			{
				switch (ch2)
				{
					case ' ':
						flag = true;
						break;
					case '.':
						flag = true;
						break;
					case '_':
						flag = true;
						break;
					case '-':
						flag = true;
						break;
					default:
						if (flag)
						{
							str = str + ch2.ToString().ToUpper();
							flag = false;
						}
						else if (flag2)
						{
							str = str + ch2.ToString().ToLower();
						}
						else
						{
							str = str + ch2;
						}
						break;
				}
			}
			return str;
		}

		public static string ToLowerHyphenated(string s)
		{
			return SeparatePascalCaseText(ToPascalCase(s), '-').ToLowerInvariant();
		}

		public static string SubstituteReplacements(string str, IDictionary<string, string> replacements)
		{
			return SubstituteReplacements(str, DefaultEnclosingSubstitutionTag, replacements);
		}

		public static string SubstituteReplacements(string str, string enclosingTags, IDictionary<string, string> replacements)
		{
			for (var i = replacements.Keys.GetEnumerator(); i.MoveNext(); )
			{
				str = str.Replace(String.Format("{0}{1}{0}", enclosingTags, i.Current), replacements[i.Current]);
			}
			return str;
		}

		public static bool IsNullOrBlank(string input)
		{
			return String.IsNullOrEmpty(input) || input.Trim().Length == 0;
		}

		public static bool HasText(string input)
		{
			return !IsNullOrBlank(input);
		}

		public static bool IsEqualIgnoreCase(object a, object b)
		{
			bool retVal;
			if (a == null && b == null)
			{
				retVal = true;
			}
			else if (a == null || b == null)
			{
				retVal = false;
			}
			else
			{
				retVal = String.Compare(a.ToString(), b.ToString(), true) == 0;
			}
			return retVal;
		}

		public static string ToJson(IEnumerable collection)
		{
			return ToJson(collection, false, true);
		}

		public static string ToJson(IEnumerable collection, bool quotePropertyNames, bool applyCamelCaseToPropertyNames)
		{
			if (collection != null)
			{
				return "[{0}]".FormatWith(String.Join(",", collection.Cast<object>().Select(x => ToJson(x, quotePropertyNames, applyCamelCaseToPropertyNames)).ToArray()));
			}
			return "null";
		}

		public static string ToJson(object x)
		{
			return ToJson(x, false, true);
		}

		public static string ToJson(object x, bool quotePropertyNames, bool applyCamelCaseToPropertyNames)
		{
			if (x == null)
			{
				return "null";
			}
			if (x is string)
			{
				return "\"{0}\"".FormatWith(x.ToString().Replace("\"", "\\\""));
			}
			if (x is bool)
			{
				return x.ToString().ToLowerInvariant();
			}
			if (x is DateTime)
			{
				return ((DateTime)x).ToJson();
			}
			if (x.GetType().IsPrimitive)
			{
				return x.ToString();
			}
			if (x is IEnumerable)
			{
				return ToJson((IEnumerable)x, quotePropertyNames, applyCamelCaseToPropertyNames);
			}
			var sb = new StringBuilder("{ ");
			foreach (var p in x.GetType().GetProperties())
			{
				if (quotePropertyNames)
				{
					sb.Append("\"");
				}
				if (applyCamelCaseToPropertyNames)
				{
					sb.Append(p.Name.ToCamelCase());
				}
				else
				{
					sb.Append(p.Name);
				}
				if (quotePropertyNames)
				{
					sb.Append("\"");
				}
				sb.Append(": ");
				var val = p.GetValue(x, EmptyObjectArray);
				if (val == null || val is string || val is DateTime || val.GetType().IsPrimitive)
				{
					sb.Append(ToJson(val));
				}
				else if (val is IEnumerable)
				{
					sb.Append("[ ");
					sb.Append(((IEnumerable)val).Cast<object>().Count());
					sb.Append(" ]");
				}
				else
				{
					sb.Append("{ type: ");
					sb.Append(val.GetType().Name);
					sb.Append(" }");
				}
				sb.Append(", ");
			}
			if (sb.Length > 2)
			{
				sb.Remove(sb.Length - 2, 2);
			}
			sb.Append(" }");
			return sb.ToString();
		}

		public static bool IsEmail(string candidate)
		{
			return IsValid(candidate, EmailRegex);
		}

		#region Conversion Methods

		public static bool? ToBoolean(string candidate)
		{
			if (candidate.IsNullOrBlank())
			{
				return null;
			}
			return !candidate.EqualsIgnoreCase(Boolean.FalseString) && candidate.Trim() != 0.ToString();
		}

		public static int? ToInt32(string candidate)
		{
			if (candidate.IsNullOrBlank())
			{
				return null;
			}
			int value;
			return Int32.TryParse(candidate, out value) ? value : (int?)null;
		}

		public static long? ToInt64(string candidate)
		{
			if (candidate.IsNullOrBlank())
			{
				return null;
			}
			long value;
			return Int64.TryParse(candidate, out value) ? value : (long?)null;
		}

		public static decimal? ToDecimal(string candidate)
		{
			if (candidate.IsNullOrBlank())
			{
				return null;
			}
			return IsValid(candidate, new Regex(@"^(\+|-)?(\" + CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol + @")?[0-9](,[0-9]{3}|[0-9])*(\" + CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator + @"[0-9]{1,2})?$", RegexOptions.Compiled)) ? Decimal.Parse(candidate) : (Decimal?)null;
		}

		public static double? ToDouble(string candidate)
		{
			if (candidate.IsNullOrBlank())
			{
				return null;
			}
			double value;
			return Double.TryParse(candidate, out value) ? value : (double?)null;
		}

		public static DateTime? ToDate(string candidate)
		{
			if (candidate.IsNullOrBlank())
			{
				return null;
			}
			DateTime date;
			return DateTime.TryParse(candidate, out date) ? date : (DateTime?)null;
		}

		public static DateTime? ToDate(string candidate, string format)
		{
			if (candidate.IsNullOrBlank())
			{
				return null;
			}
			DateTime date;
			return DateTime.TryParseExact(candidate, format, CultureInfo.CurrentCulture.DateTimeFormat, DateTimeStyles.None, out date) ? date : (DateTime?)null;
		}

		public static Guid? ToGuid(string candidate)
		{
			if (candidate.IsNullOrBlank())
			{
				return null;
			}
			Guid temp;
			return TryParseGuid(candidate, out temp);
		}

		private static Guid? TryParseGuid(string candidate, out Guid output)
		{
			Guid? retVal = null;
			output = Guid.Empty;
			if (!String.IsNullOrEmpty(candidate))
			{
				if (GuidRegex.IsMatch(candidate))
				{
					return new Guid(candidate);
				}
			}
			return retVal;
		}

		#endregion

		#region Private Methods

		private static bool IsValid(string candidate, Regex expression)
		{
			var retVal = false;
			if (!String.IsNullOrEmpty(candidate))
			{
				retVal = expression.IsMatch(candidate);
			}
			return retVal;
		}

		#endregion
	}
}
