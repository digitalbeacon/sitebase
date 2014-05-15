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
	public class GuidAttribute : RegularExpressionAttribute
	{
		public GuidAttribute() : base(TextUtil.GuidRegex.ToString())
		{
		}

		public GuidAttribute(string errorKey)
			: base(TextUtil.GuidRegex.ToString(), errorKey)
		{
		}

		public override string FormatErrorMessage(string name)
		{
			return String.Format(CultureInfo.CurrentCulture, ResourceManager.Instance.GetString(ErrorMessage), name);
		}
	}
}
