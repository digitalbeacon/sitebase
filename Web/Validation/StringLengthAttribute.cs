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
	public class StringLengthAttribute : System.ComponentModel.DataAnnotations.StringLengthAttribute
	{
		public StringLengthAttribute(int maxLength) : base(maxLength)
		{
			ErrorMessage = "Validation.Error.StringLength";
		}

		public StringLengthAttribute(int maxLength, string errorKey) : base(maxLength)
		{
			ErrorMessage = errorKey;
		}

		public override string FormatErrorMessage(string name)
		{
			return String.Format(CultureInfo.CurrentCulture, ResourceManager.Instance.GetString(ErrorMessage), name, MaximumLength);
		}
	}
}
