// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using DigitalBeacon.Business;
using DigitalBeacon.Util;
using FluentValidation;
using FluentValidation.Results;

namespace DigitalBeacon.Web.Validation
{
	public class BaseValidator<T> : AbstractValidator<T>
	{
		public const string NoProperty = "NoProperty";

		/// <summary>
		/// Validates the specified instance.
		/// </summary>
		/// <param name="instance">The instance.</param>
		/// <returns></returns>
		public override ValidationResult Validate(T instance)
		{
			var retVal = base.Validate(instance);
			for (var i = 0; i < retVal.Errors.Count; i++)
			{
				var error = retVal.Errors[i];
				if (error.PropertyName == NoProperty)
				{
					retVal.Errors[i] = new ValidationFailure(String.Empty, error.ErrorMessage, error.AttemptedValue);
				}
			}
			return retVal;
		}

		/// <summary>
		/// Gets the localized string.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="args">The args.</param>
		/// <returns></returns>
		protected string GetLocalizedString(string key, params object[] args)
		{
			var retVal = ResourceManager.Instance.GetString(key);
			return args != null ? retVal.FormatWith(args) : retVal;
		}
	}
}
