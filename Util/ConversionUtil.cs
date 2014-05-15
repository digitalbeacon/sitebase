// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;

namespace DigitalBeacon.Util
{
	public class ConversionUtil
	{
		/// <summary>
		/// Gets the optional value casted .
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static T? GetOptionalValue<T>(string value) where T : struct
		{
			T? retVal = null;
			if (TextUtil.HasText(value) && (typeof(T).IsEnum || typeof(T) == typeof(Int32) || typeof(T) == typeof(Int64)))
			{
				long val;
				if (Int64.TryParse(value, out val))
				{
					retVal = (T)val;
				}
				else if (typeof(T).IsEnum && Enum.IsDefined(typeof(T), value))
				{
					retVal = (T)Enum.Parse(typeof(T), value);
				}
			}
			return retVal;
		}

		/// <summary>
		/// Gets the enum value.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static T GetEnumValue<T>(string value) where T : struct
		{
			T retVal = default(T);
			if (TextUtil.HasText(value) && typeof(T).IsEnum)
			{
				long val;
				if (Int64.TryParse(value, out val))
				{
					retVal = (T)((object)val);
				}
				else if (Enum.IsDefined(typeof(T), value))
				{
					retVal = (T)Enum.Parse(typeof(T), value);
				}
			}
			return retVal;
		}
	}
}
