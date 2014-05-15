// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;

namespace DigitalBeacon.Util
{
	public static class DateExtensions
	{
		/// <summary>
		/// Converts the given date to the corresponding JSON date string
		/// </summary>
		/// <param name="dt">The date.</param>
		/// <returns></returns>
		public static string ToJson(this DateTime dt)
		{
			return "/Date({0})/".FormatWith(dt.ToJavaScriptMilliseconds());
		}

		/// <summary>
		/// Converts the given date to the corresponding milliseconds representation
		/// that can be used in the JavaScript date constructor
		/// http://forums.asp.net/t/1044408.aspx
		/// </summary>
		/// <param name="dt">The date.</param>
		/// <returns></returns>
		public static long ToJavaScriptMilliseconds(this DateTime dt)
		{
			return (long)new TimeSpan(dt.ToUniversalTime().Ticks - new DateTime(1970, 1, 1).Ticks).TotalMilliseconds;
		}

		/// <summary>
		/// Returns the age for the given date of birth
		/// </summary>
		/// <param name="dateOfBirth">The date of birth.</param>
		/// <returns></returns>
		public static int Age(this DateTime dateOfBirth)
		{
			return Age(dateOfBirth, DateTime.Today);
		}

		/// <summary>
		/// Returns the age for the given date of birth on the specified date
		/// </summary>
		/// <param name="dateOfBirth">The date of birth.</param>
		/// <param name="date">The date.</param>
		/// <returns></returns>
		public static int Age(this DateTime dateOfBirth, DateTime date)
		{
			int age = date.Year - dateOfBirth.Year;
			if (dateOfBirth > date.AddYears(-age))
			{
				age--;
			}
			return age;
		}

		/// <summary>
		/// Returns the first day of the week based on the specified date
		/// </summary>
		/// <param name="dt">The dt.</param>
		/// <param name="startOfWeek">The start of week.</param>
		/// <returns></returns>
		public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
		{
			int diff = dt.DayOfWeek - startOfWeek;
			if (diff < 0)
			{
				diff += 7;
			}
			return dt.AddDays(-1 * diff).Date;
		}
	}
}
