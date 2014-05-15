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
	public class PhoneNumberAttribute : RegularExpressionAttribute
	{
		public const string Regex = @"^((\(\d{3}\)[ ]?)|(\d{3}-?))\d{3}-?\d{4}$";

		public PhoneNumberAttribute() : base(Regex)
		{
		}

		public PhoneNumberAttribute(string errorKey) : base(Regex, errorKey)
		{
		}

		public override string FormatErrorMessage(string name)
		{
			return String.Format(CultureInfo.CurrentCulture, ResourceManager.Instance.GetString(ErrorMessage), name);
		}
	}
}
