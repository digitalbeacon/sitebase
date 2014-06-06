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

namespace DigitalBeacon.Web.Validation
{
	public static class ValidatorExtensions
	{
		public static IRuleBuilderOptions<T, TProperty> WithLocalizedMessage<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, string messageKey)
		{
			return rule.WithMessage(ResourceManager.Instance.GetString(messageKey));
		}

		public static IRuleBuilderOptions<T, TProperty> WithLocalizedMessage<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, string messageKey, params Func<T, object>[] funcs)
		{
			return rule.WithMessage(ResourceManager.Instance.GetString(messageKey), funcs);
		}

		public static IRuleBuilderOptions<T, TProperty> WithLocalizedMessage<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, string messageKey, params object[] args)
		{
			if (args.Length == 1 && args[0] is string)
			{
				return rule.WithMessage(ResourceManager.Instance.GetString(messageKey), ResourceManager.Instance.GetString((string)args[0]));
			}
			return rule.WithMessage(ResourceManager.Instance.GetString(messageKey), args);
		}

		public static IRuleBuilderOptions<T, TProperty> NotNullOrBlank<T, TProperty>(this IRuleBuilder<T, TProperty> rule)
		{
			if (typeof(TProperty) == typeof(string))
			{
				return rule.Must(x => !x.ToStringSafe().IsNullOrBlank());
			}
			return rule.NotEmpty();
		}

	}
}
