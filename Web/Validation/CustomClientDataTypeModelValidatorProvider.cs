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
using DigitalBeacon.Util;

namespace DigitalBeacon.Web.Validation
{
	public class CustomClientDataTypeModelValidatorProvider : ModelValidatorProvider 
	{
		private static readonly HashSet<Type> NumericTypes = new HashSet<Type>(
			new[] { typeof(byte), typeof(sbyte), typeof(short), typeof(ushort), typeof(int), typeof(uint), 
					typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal) });

		public override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, ControllerContext context)
		{
			metadata.Guard("metadata");
			context.Guard("context");
			return GetValidatorsImpl(metadata, context);
		}

		private static IEnumerable<ModelValidator> GetValidatorsImpl(ModelMetadata metadata, ControllerContext context)
		{
			var type = Nullable.GetUnderlyingType(metadata.ModelType) ?? metadata.ModelType;
			if (type == typeof(DateTime))
			{
				yield return new DateValidator(metadata, context);
			}
			else if (NumericTypes.Contains(type))
			{
				yield return new NumericValidator(metadata, context, type);
			}
		}
	}
}