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
	public class ManagedRegularExpressionAttribute : RegularExpressionAttribute
	{
		public ManagedRegularExpressionAttribute(string objectKey)
			: base(ServiceFactory.Instance.GetManagedObject<string>(objectKey))
		{
		}

		public ManagedRegularExpressionAttribute(string objectKey, string errorKey)
			: base(ServiceFactory.Instance.GetManagedObject<string>(objectKey), errorKey)
		{
		}

		public override string FormatErrorMessage(string name)
		{
			return String.Format(CultureInfo.CurrentCulture, ResourceManager.Instance.GetString(ErrorMessage), name);
		}
	}
}
