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
	public class RegularExpressionAttribute : System.ComponentModel.DataAnnotations.RegularExpressionAttribute
	{
		public RegularExpressionAttribute(string pattern) : base(pattern)
		{
			ErrorMessage = "Validation.Error.RegularExpression";
		}

		public RegularExpressionAttribute(string pattern, string errorKey) : base(pattern)
		{
			ErrorMessage = errorKey;
		}

		public override string FormatErrorMessage(string name)
		{
			return String.Format(CultureInfo.CurrentCulture, ResourceManager.Instance.GetString(ErrorMessage), name);
		}
	}
}
