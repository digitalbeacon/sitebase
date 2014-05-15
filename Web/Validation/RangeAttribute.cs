// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Globalization;
using DigitalBeacon.Business;

namespace DigitalBeacon.Web.Validation
{
	public class RangeAttribute : System.ComponentModel.DataAnnotations.RangeAttribute
	{
		public const string DefaultErrorMessage = "Validation.Error.Range";

		public RangeAttribute(int min, int max, string errorKey)
			: base(min, max)
		{
			ErrorMessage = errorKey;
		}

		public RangeAttribute(int min, int max)
			: this(min, max, DefaultErrorMessage)
		{
		}

		public RangeAttribute(double min, double max, string errorKey)
			: base(min, max)
		{
			ErrorMessage = errorKey;
		}

		public RangeAttribute(double min, double max)
			: this(min, max, DefaultErrorMessage)
		{
		}

		public RangeAttribute(Type type, string min, string max, string errorKey)
			: base(type, min, max)
		{
			ErrorMessage = errorKey;
		}

		public RangeAttribute(Type type, string min, string max)
			: this(type, min, max, DefaultErrorMessage)
		{
		}

		public override string FormatErrorMessage(string name)
		{
			return String.Format(CultureInfo.CurrentCulture, ResourceManager.Instance.GetString(ErrorMessage), name, Minimum, Maximum);
		}
	}
}
