// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using FluentValidation;
using FluentValidation.Validators;
using System.Reflection;

namespace DigitalBeacon.Web
{
	public class FluentValidationModelMetadataProvider : DataAnnotationsModelMetadataProvider
	{
		private readonly IValidatorFactory _factory;
		private MethodInfo _toAttribute;

		public FluentValidationModelMetadataProvider(IValidatorFactory factory)
		{
			_factory = factory;
		}

		protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
		{
			return base.CreateMetadata(attributes.Concat(ConvertFVMetaDataToAttributes(containerType, propertyName)), containerType, modelAccessor, modelType, propertyName);
		}

		IEnumerable<Attribute> ConvertFVMetaDataToAttributes(Type type, string name)
		{
			var validator = _factory.GetValidator(type);
			if (validator == null)
			{
				return Enumerable.Empty<Attribute>();
			}
			var validators = validator.CreateDescriptor().GetValidatorsForMember(name);

			//var attributes = validators.OfType<IAttributeMetadataValidator>()
			//				.Select(x => x.ToAttribute())
			//				.Concat(SpecialCaseValidatorConversions(validators));

			var attributes = new List<Attribute>();
			var parameters = new object[0];
			foreach (var x in validators.Where(x => x.GetType().Name == "AttributeMetadataValidator"))
			{
				if (_toAttribute == null)
				{
					_toAttribute = x.GetType().GetMethod("ToAttribute");
				}
				attributes.Add((Attribute)_toAttribute.Invoke(x, parameters));
			}
			return attributes.Concat(SpecialCaseValidatorConversions(validators));
		}

		IEnumerable<Attribute> SpecialCaseValidatorConversions(IEnumerable<IPropertyValidator> validators)
		{
			//Email Validator should be convertible to DataType EmailAddress.
			return validators
				.OfType<IEmailValidator>()
				.Select(x => new DataTypeAttribute(DataType.EmailAddress))
				.Cast<Attribute>();
		}
	}
}