// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DigitalBeacon.Business;
using DigitalBeacon.Util;

namespace DigitalBeacon.Web.Validation
{
	public class NumericValidator : ModelValidator 
	{
		private Type NumericType { get; set; }

		public NumericValidator(ModelMetadata metadata, ControllerContext controllerContext, Type numericType)
			: base(metadata, controllerContext) 
		{
			NumericType = numericType;
		}

		public override IEnumerable<ModelClientValidationRule> GetClientValidationRules() 
		{
			var validationType = "number";
			if (NumericType == typeof(Int32))
			{
				validationType = "integer";
			}
			else if (NumericType == typeof(Decimal))
			{
				validationType = "currency";
			}
			return new[] 
			{ 
				new ModelClientValidationRule 
				{
					ValidationType = validationType,
					ErrorMessage = MakeErrorString(Metadata.GetDisplayName())
				}
			};
		}

		private string MakeErrorString(string displayName) 
		{
			var validationMessage = "Validation.Error.Number";
			if (NumericType == typeof(Int32))
			{
				validationMessage = "Validation.Error.Integer";
			}
			else if (NumericType == typeof(Decimal))
			{
				validationMessage = "Validation.Error.Currency";
			}
			return ResourceManager.Instance.GetString(validationMessage).FormatWith(displayName);
		}

		public override IEnumerable<ModelValidationResult> Validate(object container) 
		{
			// this is not a server-side validator
			return Enumerable.Empty<ModelValidationResult>();
		}
	}
}