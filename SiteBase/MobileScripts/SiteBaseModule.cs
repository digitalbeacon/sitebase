// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using jQueryLib;
using ng;

namespace DigitalBeacon.SiteBase.Mobile
{
	public static class SiteBaseModule
	{
		static SiteBaseModule()
		{
			Angular.module("sitebase", new[] { "ngSanitize", "ui.bootstrap", "ui.mask" })
				.config(new dynamic[] 
				{ 
					"$httpProvider",
					(Action<dynamic>)
					((httpProvider) =>
					{
						httpProvider.defaults.transformResponse.push(new Func<object, object>(data => Utils.convertDateStringsToDates(data)));
					})
				});
				//.directive("val", new object[]
				//{
				//	"$compile",
				//	new Func<dynamic, object>((compile) => {
				//		return new
				//		{
				//			restrict = "A",
				//			require = "ngModel",
				//			link = new Action<dynamic, jQuery, dynamic, NgModelController>((scope, elem, attr, ctrl) =>
				//			{
				//				//if (attr.valRequired)
				//				//{
				//				//	var e = jQuery.Select(StringUtils.formatWith(
				//				//		"<span ng-show=\"{0}.{1}.$error.required && (!{0}.{1}.$pristine || model.submitted)\" class=\"help-block\">{2}</span>",
				//				//		elem.closest("form").attr("name"), attr.name, attr.valRequired));
				//				//	var c = compile(e);
				//				//	c = c(scope);
				//				//	elem.after(e);
				//				//}

				//				//ctrl.parsers.unshift(new Func<string, bool>((value) =>
				//				//{
				//				//	if (attr.valRequired)
				//				//	{
				//				//		if (((dynamic)ctrl)["$invalid"])
				//				//		{
				//				//			if (!scope.hasAlert(attr.name))
				//				//			{
				//				//				scope.alerts.push(new Alert { key = attr.name, type = "danger", msg = attr.valRequired });
				//				//			}
				//				//		}
				//				//		else
				//				//		{
				//				//			scope.clearAlerts(attr.name);
				//				//		}
				//				//	}
				//				//	return false;
				//				//}));
				//				//ctrl.formatters.unshift(new Func<string, bool>((value) =>
				//				//{
				//				//	return value;
				//				//}));
				//			})
				//		};
				//	})
				//});
		}
	}
}
