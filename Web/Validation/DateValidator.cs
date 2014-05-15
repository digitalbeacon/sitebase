// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DigitalBeacon.Business;
using DigitalBeacon.Util;

namespace DigitalBeacon.Web.Validation
{
	public class DateValidator : ModelValidator 
	{
		public DateValidator(ModelMetadata metadata, ControllerContext controllerContext)
			: base(metadata, controllerContext) 
		{
		}

		public override IEnumerable<ModelClientValidationRule> GetClientValidationRules() 
		{
			return new[]
			{ 
				new ModelClientValidationRule 
				{
					ValidationType = "date",
					ErrorMessage = MakeErrorString(Metadata.GetDisplayName())
				}
			};
		}

		private static string MakeErrorString(string displayName) 
		{
			return ResourceManager.Instance.GetString("Validation.Error.Date").FormatWith(displayName);
		}

		public override IEnumerable<ModelValidationResult> Validate(object container) 
		{
			// this is not a server-side validator
			return Enumerable.Empty<ModelValidationResult>();
		}
	}
}