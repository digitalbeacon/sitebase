// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Globalization;
using System.Web.Mvc;
using DigitalBeacon.Util;

namespace DigitalBeacon.Web
{
	/// <summary>
	/// Custom model binder for decimal values
	/// </summary>
	public class DecimalModelBinder : IModelBinder
	{
		bool _allowNull;

		public DecimalModelBinder(bool allowNull)
		{
			_allowNull = allowNull;
		}

		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			ValueProviderResult valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
			ModelState modelState = new ModelState { Value = valueResult };
			object actualValue = null;
			try
			{
				if (!_allowNull || valueResult.AttemptedValue.HasText())
				{
					actualValue = Convert.ToDecimal(valueResult.AttemptedValue, CultureInfo.CurrentCulture);
				}
			}
			catch (FormatException e)
			{
				modelState.Errors.Add(e);
			}

			bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
			return actualValue;
		}
	}
}