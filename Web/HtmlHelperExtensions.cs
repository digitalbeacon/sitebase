// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Web;
using System.Web.Mvc;
using Recaptcha.Web;
using Recaptcha.Web.Mvc;

namespace DigitalBeacon.Web
{
	public static class HtmlHelperExtensions
	{
		/// <summary>
		/// Html Helper to build and render the Captcha control
		/// </summary>
		/// <param name="helper">HtmlHelper class provides a set of helper methods whose purpose is to help you create HTML controls programmatically</param>
		/// <returns></returns>
		public static IHtmlString GenerateCaptcha(this HtmlHelper helper)
		{
			return helper.Recaptcha(theme: RecaptchaTheme.Clean);
		}
	}
}