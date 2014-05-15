// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;

namespace DigitalBeacon.Util
{
	public static class ObjectExtensions
	{
		public static void Guard(this object obj, string message, params object[] args)
		{
			if (obj == null)
			{
				throw new ArgumentNullException(message.FormatWith(args));
			}
		}

		public static T DefaultTo<T>(this T obj, T defaultVal)
		{
			if (typeof(T).IsValueType)
			{
				return obj.Equals(default(T)) ? defaultVal : obj;
			}
			return obj == null ? defaultVal : obj;
		}

		public static U DefaultTo<T, U>(this T obj, Func<T, U> fn)
		{
			if (typeof(T).IsValueType)
			{
				return obj.Equals(default(T)) ? fn(obj) : default(U);
			}
			return obj == null ? fn(obj) : default(U);
		}

		public static T DefaultIfNull<T>(this T obj, T defaultVal) where T : class
		{
			return DefaultTo(obj, defaultVal);
		}

		public static U DefaultIfNull<T, U>(this T obj, Func<T, U> fn) where T : class
		{
			return DefaultTo(obj, fn);
		}

		public static T IfNotNull<T>(this T obj, T newObj) where T : class
		{
			return obj != null ? newObj : obj;
		}

		public static void IfNotNull<T>(this T obj, Action<T> action) where T : class
		{
			if (obj != null) { action(obj); }
		}

		public static U IfNotNull<T, U>(this T obj, Func<T, U> fn) where T : class
		{
			return obj != null ? fn(obj) : default(U);
		}

		public static T IfNotDefault<T>(this T val, T newVal) where T : struct
		{
			return !val.Equals(default(T)) ? newVal : val;
		}

		public static U IfNotDefault<T, U>(this T val, Func<T, U> fn) where T : struct
		{
			return !val.Equals(default(T)) ? fn(val) : default(U);
		}

		public static string ToSafeString(this object anObject)
		{
			return anObject == null ? String.Empty : anObject.ToString();
		}

		public static string ToJson(this object anObject)
		{
			return TextUtil.ToJson(anObject);
		}

		public static string ToJson(this object anObject, bool quotePropertyNames, bool applyCamelCaseToPropertyNames)
		{
			return TextUtil.ToJson(anObject, quotePropertyNames, applyCamelCaseToPropertyNames);
		}

		public static string ToString(this object anObject, string aFormat)
		{
			return ToString(anObject, aFormat, null);
		}

		public static string ToString(this object anObject, string aFormat, IFormatProvider formatProvider)
		{
			var sb = new StringBuilder();
			var type = anObject.GetType();
			var reg = new Regex(@"({)([^}]+)(})", RegexOptions.IgnoreCase);
			var mc = reg.Matches(aFormat);
			var startIndex = 0;
			foreach (Match m in mc)
			{
				var g = m.Groups[2]; //it's second in the match between { and }
				var length = g.Index - startIndex - 1;
				sb.Append(aFormat.Substring(startIndex, length));

				string toGet;
				var toFormat = String.Empty;
				var formatIndex = g.Value.IndexOf(":"); //formatting would be to the right of a :
				if (formatIndex == -1) //no formatting, no worries
				{
					toGet = g.Value;
				}
				else //pickup the formatting
				{
					toGet = g.Value.Substring(0, formatIndex);
					toFormat = g.Value.Substring(formatIndex + 1);
				}

				//first try properties
				var retrievedProperty = type.GetProperty(toGet);
				Type retrievedType = null;
				object retrievedObject = null;
				if (retrievedProperty != null)
				{
					retrievedType = retrievedProperty.PropertyType;
					retrievedObject = retrievedProperty.GetValue(anObject, null);
				}
				else //try fields
				{
					var retrievedField = type.GetField(toGet);
					if (retrievedField != null)
					{
						retrievedType = retrievedField.FieldType;
						retrievedObject = retrievedField.GetValue(anObject);
					}
				}

				if (retrievedType != null) //Cool, we found something
				{
					string result;
					if (toFormat == String.Empty) //no format info
					{
						result = retrievedType.InvokeMember("ToString",
						  BindingFlags.Public | BindingFlags.NonPublic |
						  BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.IgnoreCase
						  , null, retrievedObject, null) as string;
					}
					else //format info
					{
						result = retrievedType.InvokeMember("ToString",
						  BindingFlags.Public | BindingFlags.NonPublic |
						  BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.IgnoreCase
						  , null, retrievedObject, new object[] { toFormat, formatProvider }) as string;
					}
					sb.Append(result);
				}
				else //didn't find a property with that name, so be gracious and put it back
				{
					sb.Append("{");
					sb.Append(g.Value);
					sb.Append("}");
				}
				startIndex = g.Index + g.Length + 1;
			}
			if (startIndex < aFormat.Length) //include the rest (end) of the string
			{
				sb.Append(aFormat.Substring(startIndex));
			}
			return sb.ToString();
		}

		public static string PropertyName<TObject, TProperty>(this TObject anObject, Expression<Func<TObject, TProperty>> memberExpression) where TObject : class
		{
			memberExpression.Guard("memberExpression");
			return memberExpression.GetMember().Name;
		}

		public static bool HasProperty<T>(this object anObject, string memberName)
		{
			if (anObject == null)
			{
				return false;
			}
			memberName.Guard("memberName");
			var prop = GetPropertyInfo(anObject, memberName);
			return prop != null && prop.PropertyType == typeof(T);
		}

		public static T GetPropertyValue<T, TObject, TProperty>(this TObject anObject, Expression<Func<TObject, TProperty>> memberExpression) where TObject : class
		{
			memberExpression.Guard("memberExpression");
			return GetPropertyValue<T>(anObject, memberExpression.GetMember().Name);
		}

		public static T GetPropertyValue<T>(this object anObject, string memberName)
		{
			if (anObject == null)
			{
				return default(T);
			}
			memberName.Guard("memberName");
			var prop = GetPropertyInfo(anObject, memberName);
			if (prop == null)
			{
				throw new ArgumentException("{0} does not exist".FormatWith(memberName));
			}
			if (prop.PropertyType != typeof(T))
			{
				throw new ArgumentException("{0} is not of type {1}".FormatWith(memberName, typeof(T).Name));
			}
			var value = prop.GetValue(anObject, null);
			return value is T ? (T)value : default(T);
		}

		public static void SetPropertyValue<T>(this object theObject, string memberName, T value)
		{
			theObject.Guard("theObject");
			memberName.Guard("memberName");
			var prop = GetPropertyInfo(theObject, memberName);
			if (prop == null)
			{
				throw new ArgumentException("{0} does not exist".FormatWith(memberName));
			}
			if (prop.PropertyType != typeof(T))
			{
				throw new ArgumentException("{0} is not of type {1}".FormatWith(memberName, typeof(T).Name));
			}
			prop.SetValue(theObject, value, null);
		}

		private static PropertyInfo GetPropertyInfo(object obj, string property)
		{
			var type = obj.GetType();
			// Need to perform reflection in isolation on the object and its supertype due to a problem
			// with proxies throwing ambiguous match exception.
			return type.BaseType.GetProperty(property) ?? type.GetProperty(property, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
		}
	}
}
