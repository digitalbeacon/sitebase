// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Configuration;
using System.Reflection;
using System.Web.Compilation;
using DigitalBeacon.Util;

namespace DigitalBeacon.SiteBase.Web
{
	public static class WebConstants
	{
		private static string _assetVersion;

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
		public const string AvailableSortFieldsKey = "AvailableSortFields";

		public const string MessageModelKey = "SiteBase.MessageModel";
		public const string TransientMessagesKey = "SiteBase.TransientMessagesKey";
		public const string RenderTypeKey = "RenderType";
		public const string RenderTypeDefault = "default";
		public const string RenderTypeMobile = "mobile";
		public const string RenderTypePartial = "partial";
		public const string RenderTypePartialWrapped = "partialWrapped";
		public const string RenderTypeJson = "json";
		public const string RenderTypeTemplate = "template";

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
		public const string HtmlToPdfTempPathKey = "HtmlToPdfTempPath"; 
		public const string SupportsMobileKey = "SupportsMobile";
		public const string UseEmailForUsernameKey = "UseEmailForUsername";
		public const string AssetVersionKey = "AssetVersion";

		public const string DefaultErrorHeadingKey = "Error.Heading";

		public static bool UseEmailForUsername
		{
			get { return ConfigurationManager.AppSettings[UseEmailForUsernameKey].ToBoolean() ?? false; }
		}

		public static bool IsPdfGenerationEnabled
		{
			get { return ConfigurationManager.AppSettings[HtmlToPdfExePathKey].HasText(); }
		}

		public static string AssetVersion
		{
			get 
			{
				if (_assetVersion != null)
				{
					return _assetVersion;
				}
				_assetVersion = ConfigurationManager.AppSettings[AssetVersionKey] ?? new AssemblyName(BuildManager.GetGlobalAsaxType().BaseType.Assembly.FullName).Version.Revision.ToString();
				return _assetVersion;
			}
		}
	}
}
