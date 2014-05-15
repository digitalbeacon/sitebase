// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Configuration;
using DigitalBeacon.Util;

namespace DigitalBeacon.Web
{
	public static class CaptchaConstants
	{
		public const string CaptchaValidKey = "CaptchaValid";
		public const string CaptchaFieldName = "recaptcha_response_field";
		public const string RecaptchaPrivateKey = "RecaptchaPrivateKey";
		public const string RecaptchaPublicKey = "RecaptchaPublicKey";

		public static bool IsCaptchaEnabled
		{
			get
			{
				return ConfigurationManager.AppSettings[RecaptchaPrivateKey].HasText()
						&& ConfigurationManager.AppSettings[RecaptchaPublicKey].HasText();
			}
		}
	}
}
