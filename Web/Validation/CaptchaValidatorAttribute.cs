// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Web.Mvc;
using DigitalBeacon.Util;
using DigitalBeacon.Web.Util;
using Recaptcha.Web.Mvc;

namespace DigitalBeacon.Web.Validation
{
	public class CaptchaValidatorAttribute : ActionFilterAttribute
	{
		/// <summary>
		/// Called before the action method is invoked
		/// </summary>
		/// <param name="filterContext">Information about the current request and action</param>
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (CaptchaConstants.IsCaptchaEnabled && filterContext.HttpContext.Request.Form[CaptchaConstants.CaptchaFieldName] != null)
			{
				var captchaValidator = (filterContext.Controller as Controller).GetRecaptchaVerificationHelper();
				if (captchaValidator.Response.HasText())
				{
					// this will push the result value into a parameter in our Action
					//filterContext.ActionParameters[CaptchaValid] = recaptchaResponse.IsValid;
					filterContext.RouteData.Values[CaptchaConstants.CaptchaValidKey] = captchaValidator.VerifyRecaptchaResponse() == Recaptcha.Web.RecaptchaVerificationResult.Success;
				}
			}
			base.OnActionExecuting(filterContext);
			
			// Add string to Trace for testing
			//filterContext.HttpContext.Trace.Write("Log: OnActionExecuting", String.Format("Calling {0}", filterContext.ActionDescriptor.ActionName));
		}
	}
}