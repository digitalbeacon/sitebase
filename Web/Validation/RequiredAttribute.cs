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
	public class RequiredAttribute : System.ComponentModel.DataAnnotations.RequiredAttribute
	{
		public RequiredAttribute()
		{
			ErrorMessage = "Validation.Error.Required";
		}

		public RequiredAttribute(string errorKey)
		{
			ErrorMessage = errorKey;
		}

		public override string FormatErrorMessage(string name)
		{
			return String.Format(CultureInfo.CurrentCulture, ResourceManager.Instance.GetString(ErrorMessage), name);
		}
	}
}
