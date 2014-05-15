// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Configuration;
using DigitalBeacon.Util;

namespace DigitalBeacon.SiteBase.Web
{
	public static class WebConstants
	{
		public const string PreferencesKey = "SiteBase.Preferences";
		public const string CurrentUserKey = "SiteBase.CurrentUser";
		public const string CurrentAssociationIdKey = "SiteBase.CurrentAssociationId";
		public const string LanguageCodeKey = "LanguageCode";
		public const string LanguagesKey = "SiteBase.Languages";

		public const string GeneralErrorId = "Error.Unspecified";
		public const string SuccessMsgId = "Message.Success";

		public const string ReturnUrl = "ReturnUrl";
		public const string FileUrl = "FileUrl";

		public const string DefaultDateFormat = "MM/dd/yyyy";
		public const string DefaultDateTimeFormat = "MM/dd/yyyy hh:mm tt";
		public const string PhoneMask = "(999) 999-9999";
		public const string SsnMask = "999-99-9999";
		public const string UsaPostalCodeMask = "99999";

		public const string IdKey = "Id";
		public const string CancelKey = "Cancel";
		public const string DeleteKey = "Delete";

		public const string MessageModelKey = "SiteBase.MessageModel";
		public const string TransientMessagesKey = "SiteBase.TransientMessagesKey";
		public const string RenderTypeKey = "RenderType";
		public const string RenderTypeDefault = "Default";
		public const string RenderTypeMobile = "Mobile";
		public const string RenderTypePartial = "Partial";
		public const string RenderTypePartialWrapped = "PartialWrapped";
		public const string RenderTypeJson = "Json";
		public const string RenderTypeTemplate = "Template";
		public const string RenderTypeApi = "Api";

		public const string ContentTypeJson = "application/json";

		public const string MobileKey = "m";

		public const string EnableJavaScriptKey = "EnableJavaScript";
		public const string RequestedWithKey = "X-Requested-With";
		public const string XmlHttpRequest = "XMLHttpRequest";

		public const string UsernameRegexKey = "usernameRegex";
		public const string PasswordRegexKey = "passwordRegex";
		public const string AppContextRelativeUrlRegexKey = "appContextRelativeUrlRegex";

		public const string ParentControllerRouteValueKey = "parentController";
		public const string ParentIdRouteValueKey = "parentId";
		public const string PageRouteValueKey = "page";

		public const string CreateDefaultUsersKey = "CreateDefaultUsers";
		public const string PrecompileViewsKey = "PrecompileViews";
		public const string PrecompiledViewsKey = "PrecompiledViews";
		public const string GoogleAnalyticsIdKey = "GoogleAnalyticsId";
		public const string UseDatabaseResourcesKey = "UseDatabaseResources";
		public const string HtmlToPdfExePathKey = "HtmlToPdfExePath";
		public const string SupportsMobileKey = "SupportsMobile";
		public const string UseEmailForUsernameKey = "UseEmailForUsername";

		public const string DefaultErrorHeadingKey = "Error.Heading";

		public static bool UseEmailForUsername
		{
			get { return ConfigurationManager.AppSettings[UseEmailForUsernameKey].ToBoolean() ?? false; }
		}

		public static bool IsPdfGenerationEnabled
		{
			get { return ConfigurationManager.AppSettings[HtmlToPdfExePathKey].HasText(); }
		}
	}
}
