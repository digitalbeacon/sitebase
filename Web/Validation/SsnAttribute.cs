// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Globalization;
using DigitalBeacon.Business;
using DigitalBeacon.Util;

namespace DigitalBeacon.Web.Validation
{
	public class SsnAttribute : RegularExpressionAttribute
	{
		public const string Regex = @"^\d{3}-?\d{2}-?\d{4}$";

		public SsnAttribute() : base(Regex)
		{
		}

		public SsnAttribute(string errorKey) : base(Regex, errorKey)
		{
		}

		public override string FormatErrorMessage(string name)
		{
			return String.Format(CultureInfo.CurrentCulture, ResourceManager.Instance.GetString(ErrorMessage), name);
		}
	}
}
