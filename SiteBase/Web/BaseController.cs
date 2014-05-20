// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Common.Logging;
using DigitalBeacon.Business;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Business;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Web.Models;
using DigitalBeacon.Util;
using DigitalBeacon.Web;
using DigitalBeacon.Web.Util;
using MarkdownSharp;
using Newtonsoft.Json.Linq;
using Spark;
using Spring.Data.NHibernate.Support;

namespace DigitalBeacon.SiteBase.Web
{
	[Precompile]
	public abstract class BaseController : Controller
	{
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public const string ListViewName = "List";
		public const string EditViewName = "Edit";
		public const string DisplayViewName = "Display";
		public const string NewViewName = "New";
		public const string DeleteViewName = "Delete";
		public const string DefaultActionName = "index";
		public const string ListActionName = "index";
		public const string DeleteActionName = "delete";
		public const string EditActionName = "edit";
		public const string NewActionName = "new";

		private const string HtmlToPdfOptionsKey = "HtmlToPdfOptions";
		private const string DefaultHtmlToPdfOptions = "--print-media-type --page-size Letter --javascript-delay 100 --margin-bottom 12mm --footer-font-size 8 --footer-right \"Page [page] of [toPage]\"";

		private JObject _jsonData;

		protected internal static readonly object[] EmptyParams = new object[] { };


		#region Common Services

		private ILocalizationService _localizationService;
		private IPermissionService _permissionService;
		private IPreferenceService _preferenceService;
		private ILookupAdminService _lookupService;
		private IModuleService _moduleService;
		private IIdentityService _identityService;
		private IMailService _mailService;

		protected internal ILocalizationService LocalizationService
		{
			get
			{
				if (_localizationService == null)
				{
					_localizationService = ServiceFactory.Instance.GetService<ILocalizationService>();
				}
				return _localizationService;
			}
		}

		protected internal IPermissionService PermissionService
		{
			get
			{
				if (_permissionService == null)
				{
					_permissionService = ServiceFactory.Instance.GetService<IPermissionService>();
				}
				return _permissionService;
			}
		}

		protected internal IPreferenceService PreferenceService
		{
			get
			{
				if (_preferenceService == null)
				{
					_preferenceService = ServiceFactory.Instance.GetService<IPreferenceService>();
				}
				return _preferenceService;
			}
		}

		protected internal ILookupAdminService LookupService
		{
			get
			{
				if (_lookupService == null)
				{
					_lookupService = ServiceFactory.Instance.GetService<ILookupAdminService>();
				}
				return _lookupService;
			}
		}

		protected internal IModuleService ModuleService
		{
			get
			{
				if (_moduleService == null)
				{
					_moduleService = ServiceFactory.Instance.GetService<IModuleService>();
				}
				return _moduleService;
			}
		}

		protected internal IIdentityService IdentityService
		{
			get
			{
				if (_identityService == null)
				{
					_identityService = ServiceFactory.Instance.GetService<IIdentityService>();
				}
				return _identityService;
			}
		}

		protected internal IMailService MailService
		{
			get
			{
				if (_mailService == null)
				{
					_mailService = ServiceFactory.Instance.GetService<IMailService>();
				}
				return _mailService;
			}
		}

		#endregion

		#region Overrides

		/// <summary>
		/// Called when authorization occurs.
		/// </summary>
		/// <param name="filterContext">Information about the current request and action.</param>
		protected override void OnAuthorization(AuthorizationContext filterContext)
		{
			if (!ControllerContext.IsChildAction)
			{
				OpenNHibernateSession();
				SetRenderType();
				SetCurrentUserInfo();
			}
			base.OnAuthorization(filterContext);
		}

		/// <summary>
		/// Called after the action method is invoked.
		/// </summary>
		/// <param name="filterContext">Information about the current request and action.</param>
		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			base.OnActionExecuted(filterContext);
			if (RenderPartial)
			{
				if (filterContext.Result is RedirectToRouteResult)
				{
					var result = (RedirectToRouteResult)filterContext.Result;
					result.RouteValues[WebConstants.RenderTypeKey] = RenderType;
					// setting the X-Requested-With=XMLHttpRequest parameter in the URL is a work-around
					// for a Firefox issue where the X-Request-With header does not get properly set when
					// the XHR processes the redirect
					result.RouteValues[WebConstants.RequestedWithKey] = WebConstants.XmlHttpRequest;
				}
				else if (filterContext.Result is RedirectResult)
				{
					var url = ((RedirectResult)filterContext.Result).Url;
					if (url.ToUpperInvariant().IndexOf(WebConstants.RenderTypeKey.ToUpperInvariant()) < 0)
					{
						filterContext.Result = new RedirectResult(
							String.Format("{0}{1}{2}={3}&{4}={5}", url, url.Contains("?") ? '&' : '?',
										  WebConstants.RenderTypeKey, RenderType, WebConstants.RequestedWithKey, WebConstants.XmlHttpRequest));
					}
				}
				else if (filterContext.Result is TransferResult)
				{
					var transferResult = (TransferResult)filterContext.Result;
					if (transferResult.Url.ToUpperInvariant().IndexOf(WebConstants.RenderTypeKey.ToUpperInvariant()) < 0)
					{
						filterContext.Result = new TransferResult(
							"{0}{1}{2}={3}&{4}={5}".FormatWith(transferResult.Url, transferResult.Url.Contains("?") ? '&' : '?',
							 WebConstants.RenderTypeKey, RenderType, WebConstants.RequestedWithKey, WebConstants.XmlHttpRequest));
					}
				}
			}
		}

		/// <summary>
		/// Called after the action result that is returned by an action method is executed.
		/// </summary>
		/// <param name="filterContext">Information about the current request and action result</param>
		protected override void OnResultExecuted(ResultExecutedContext filterContext)
		{
			base.OnResultExecuted(filterContext);
			if (!ControllerContext.IsChildAction)
			{
				CloseNHibernateSession();
			}
		}

		/// <summary>
		/// Called when an unhandled exception occurs in the action.
		/// </summary>
		/// <param name="filterContext">Information about the current request and action.</param>
		protected override void OnException(ExceptionContext filterContext)
		{
			try
			{
				if (filterContext.Exception is EntityDependencyException)
				{
					filterContext.Result = RedirectToErrorAction("Common.Delete.Label", "Error.DeleteEntity.Dependency");
					if (!RenderPartial)
					{
						MessageModel.ReturnUrl = Url.Action(EditActionName);
					}
					filterContext.ExceptionHandled = true;
				}
				else if (filterContext.HttpContext.IsCustomErrorEnabled)
				{
					Log.Error(filterContext.Exception);
					var ms = ModuleService.GetGlobalModuleSetting(ModuleSettingDefinition.GlobalAdminUsername);
					var user = IdentityService.GetUser(ms.Value);
					if (user == null)
					{
						throw new Exception(String.Format("Could not location site admin user for username [{0}].", ms.Value));
					}
					ms = ModuleService.GetGlobalModuleSetting(ModuleSettingDefinition.GlobalEmailException);
					var msg = new MessageInfo { UserId = user.Id, Subject = ms.Subject };
					var sb = new StringBuilder();
					sb.AppendFormat(ResourceManager.SystemCulture, "<p>Timestamp: {0}<br/>", DateTime.Now);
					sb.AppendFormat("Requested Url: {0}<br/>", Request.Url);
					sb.AppendFormat("Client User Agent: {0}<br/>", Request.UserAgent);
					sb.AppendFormat("Client Host: {0}<br/>", Request.UserHostName);
					sb.AppendFormat("Client IP: {0}<br/>", Request.UserHostAddress);
					if (Request.UrlReferrer != null)
					{
						sb.AppendFormat("Referrer Url: {0}<br/>", Request.UrlReferrer);
					}
					if (IsAuthenticated)
					{
						sb.AppendFormat("User: {0}<br/>", CurrentUsername);
					}
					sb.Append("</p>");
					var ex = filterContext.Exception;
					while (ex != null)
					{
						sb.AppendFormat("<p>{0}: {1}</p>", ex.GetType(), ex.Message);
						if (!String.IsNullOrEmpty(ex.StackTrace))
						{
							sb.AppendFormat("<p>{0}</p>", ex.StackTrace.Replace("\r\n", "<br/>"));
						}
						ex = ex.InnerException;
					}
					IDictionary<SubstitutionDefinition, string> substitutions = new Dictionary<SubstitutionDefinition, string>();
					substitutions[SubstitutionDefinition.DynamicContent] = sb.ToString();
					msg.Body = ModuleService.GetModuleSettingValueWithSubstitutions(ms, substitutions);
					MailService.QueueEmail(CurrentAssociationId, msg, true);
					filterContext.Result = RedirectToDefaultErrorAction();
					filterContext.ExceptionHandled = true;
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex);
				filterContext.Result = RedirectToDefaultErrorAction();
				filterContext.ExceptionHandled = true;
			}
			finally
			{
				if (!ControllerContext.IsChildAction)
				{
					try
					{
						CloseNHibernateSession();
					}
					catch { }
				}
			}
		}

		/// <summary>
		/// Opens the NHibernate session.
		/// </summary>
		private void OpenNHibernateSession()
		{
			HttpContext.Items["sessionScope"] = new SessionScope("appSettings", typeof(OpenSessionInViewModule), true);
		}

		/// <summary>
		/// Closes the NHibernate session.
		/// </summary>
		private void CloseNHibernateSession()
		{
			((SessionScope)HttpContext.Items["sessionScope"]).Close();
		}

		/// <summary>
		/// Sets the render type.
		/// </summary>
		/// <returns></returns>
		private void SetRenderType()
		{
			var renderType = GetParamAsString(WebConstants.RenderTypeKey)
				.DefaultTo(Request.IsAjaxRequest() ? WebConstants.RenderTypePartial : null);
			if (renderType != null)
			{
				HttpContext.Items[WebConstants.RenderTypeKey] = renderType;
			}
		}

		/// <summary>
		/// Sets the current user.
		/// </summary>
		private void SetCurrentUserInfo()
		{
			Language? userLanguage = null;
			if (IsAuthenticated)
			{
				var user = IdentityService.GetCurrentUser();
				userLanguage = user.Language;
				// initialize person
				//var temp = user.Person.FirstName;
				// initialize roles
				//int roleCount = user.Roles.Count;
				ControllerContext.HttpContext.Items[WebConstants.CurrentUserKey] = user;
			}

			// association
			ControllerContext.HttpContext.Items[WebConstants.CurrentAssociationIdKey] = IdentityService.GetCurrentAssociationId();

			// mobile
			var mobileParam = GetParamAsString(WebConstants.MobileKey).ToBoolean();
			if (mobileParam != null)
			{
				SetCookieValue(WebConstants.MobileKey, mobileParam.Value);
			}
			else
			{
				mobileParam = GetCookieValue(WebConstants.MobileKey).ToBoolean();
			}
			ControllerContext.HttpContext.Items[WebConstants.MobileKey] = mobileParam ?? false;

			// language
			CultureInfo culture = null;
			var persistLanguage = false;
			var languageParam = GetParamAsString(BusinessConstants.PersistentLanguageKey);
			if (languageParam.HasText())
			{
				persistLanguage = true;
			}
			else
			{
				languageParam = GetParamAsString(BusinessConstants.RequestLanguageKey);
			}
			if (languageParam.HasText())
			{
				LanguageEntity language;
				if (languageParam.IsInt64())
				{
					language = LookupService.Get<LanguageEntity>(languageParam.ToInt64().Value);
				}
				else
				{
					language = LookupService.GetByCode<LanguageEntity>(languageParam);
				}
				if (language != null)
				{
					culture = CultureInfo.GetCultureInfo(language.Code);
					if (persistLanguage)
					{
						var cookie = new HttpCookie(WebConstants.PreferencesKey);
						cookie[WebConstants.LanguageCodeKey] = language.Code;
						cookie.Expires = DateTime.MaxValue;
						cookie.Path = Request.ApplicationPath;
						Response.Cookies.Add(cookie);
					}
				}
			}
			else
			{
				string languageCode = null;
				var cookie = Request.Cookies[WebConstants.PreferencesKey];
				if (cookie != null)
				{
					languageCode = cookie[WebConstants.LanguageCodeKey];
				}
				if (languageCode.HasText())
				{
					culture = CultureInfo.GetCultureInfo(languageCode);
				}
				else if (userLanguage.HasValue)
				{
					culture = CultureInfo.GetCultureInfo(LookupService.Get<LanguageEntity>((long)userLanguage.Value).Code);
				}
			}
			if (culture != null)
			{
				ResourceManager.ClientCulture = culture;
			}
			Thread.CurrentThread.CurrentCulture = ResourceManager.ClientCulture;
			Thread.CurrentThread.CurrentUICulture = ResourceManager.ClientCulture;
		}

		#endregion

		#region Identity Methods

		/// <summary>
		/// Get UserId of authenticated user
		/// </summary>
		protected internal long CurrentUserId
		{
			get
			{
				long? retVal = null;
				if (IsAuthenticated)
				{
					if (CurrentUser != null)
					{
						retVal = CurrentUser.Id;
					}
				}
				if (retVal == null)
				{
					throw new Exception("Could not determine Id for current user.");
				}
				return retVal.Value;
			}
		}

		/// <summary>
		/// Get the username of the logged in user
		/// </summary>
		static protected internal string CurrentUsername
		{
			get { return System.Web.HttpContext.Current.User != null ? System.Web.HttpContext.Current.User.Identity.Name : String.Empty; }
		}

		/// <summary>
		/// Gets the current user entity.
		/// </summary>
		/// <value>The user entity.</value>
		static protected internal UserEntity CurrentUser
		{
			get
			{
				UserEntity retVal = null;
				if (IsAuthenticated)
				{
					retVal = System.Web.HttpContext.Current.Items[WebConstants.CurrentUserKey] as UserEntity;
					if (retVal == null)
					{
						throw new BaseException("CurrentUser should not be null.");
					}
				}
				return retVal;
			}
		}

		/// <summary>
		/// Get current association
		/// </summary>
		static protected internal long CurrentAssociationId
		{
			get
			{
				var retVal = System.Web.HttpContext.Current.Items[WebConstants.CurrentAssociationIdKey] as long?;
				if (retVal == null)
				{
					throw new Exception("Could not determine current AssociationId.");
				}
				return retVal.Value;
			}
		}

		/// <summary>
		/// Returns whether there is an authenticated user
		/// </summary>
		static protected internal bool IsAuthenticated
		{
			get { return CurrentUsername.HasText(); }
		}

		/// <summary>
		/// Returns whether the authenticated user is an admin user
		/// </summary>
		static protected internal bool IsAdmin
		{
			get { return CurrentUser.SuperUser || CurrentUser.HasRole(CurrentAssociationId, Role.Administrator); }
		}

		/// <summary>
		/// Return whether authenticated user is associated with the given role
		/// </summary>
		/// <param name="role"></param>
		/// <returns></returns>
		static protected internal bool HasRole(Role role)
		{
			var retVal = false;
			if (IsAuthenticated)
			{
				retVal = CurrentUser.HasRole(CurrentAssociationId, role);
			}
			return retVal;
		}

		#endregion

		#region Module Methods

		/// <summary>
		/// The module Id associated with this control
		/// </summary>
		public long? ModuleId { get; protected set; }

		/// <summary>
		/// Redirect to default instance of specified module definition
		/// </summary>
		/// <param name="moduleDefinition"></param>
		protected internal void RedirectToDefaultModule(ModuleDefinition moduleDefinition)
		{
			var m = GetDefaultModuleInstance(moduleDefinition);
			if (m != null)
			{
				Redirect(m.Url);
			}
		}

		/// <summary>
		/// Get the default instance of the given module definition
		/// </summary>
		/// <param name="moduleDefinition"></param>
		/// <returns></returns>
		protected internal ModuleEntity GetDefaultModuleInstance(ModuleDefinition moduleDefinition)
		{
			return ModuleService.GetDefaultInstance(IdentityService.GetCurrentAssociationId(), moduleDefinition);
		}

		///// <summary>
		///// Register the host module as an instance of the given module definition
		///// </summary>
		///// <param name="moduleDefinition"></param>
		//protected internal void RegisterModule(ModuleDefinition moduleDefinition)
		//{
		//	ModuleService.RegisterModule(moduleDefinition);
		//}

		/// <summary>
		/// Get module setting by setting def
		/// </summary>
		/// <param name="moduleSettingDef"></param>
		/// <returns></returns>
		protected internal string GetModuleSetting(ModuleSettingDefinition moduleSettingDef)
		{
			string retVal = null;
			var s = ModuleId.HasValue ? ModuleService.GetModuleSetting(ModuleId.Value, moduleSettingDef) : ModuleService.GetGlobalModuleSetting(moduleSettingDef);
			if (s != null)
			{
				retVal = s.Value;
			}
			return retVal;
		}

		/// <summary>
		/// Get module setting by setting def as Int64
		/// </summary>
		/// <param name="moduleSettingDef"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		protected internal long GetModuleSettingAsInt64(ModuleSettingDefinition moduleSettingDef, long defaultValue)
		{
			var retVal = defaultValue;
			var s = ModuleId.HasValue ? ModuleService.GetModuleSetting(ModuleId.Value, moduleSettingDef) : ModuleService.GetGlobalModuleSetting(moduleSettingDef);
			if (s != null)
			{
				var longVal = s.ValueAsInt64;
				if (longVal != null)
				{
					retVal = longVal.Value;
				}
			}
			return retVal;
		}

		/// <summary>
		/// Get module setting by setting def as DateTime
		/// </summary>
		/// <param name="moduleSettingDef"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		protected internal DateTime GetModuleSettingAsDateTime(ModuleSettingDefinition moduleSettingDef, DateTime defaultValue)
		{
			var retVal = defaultValue;
			var s = ModuleId.HasValue ? ModuleService.GetModuleSetting(ModuleId.Value, moduleSettingDef) : ModuleService.GetGlobalModuleSetting(moduleSettingDef);
			if (s != null)
			{
				var dateVal = s.ValueAsDateTime;
				if (dateVal != null)
				{
					retVal = dateVal.Value;
				}
			}
			return retVal;
		}

		/// <summary>
		/// Get module setting by setting def as Boolean
		/// </summary>
		/// <param name="moduleSettingDef"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		protected internal bool GetModuleSettingAsBoolean(ModuleSettingDefinition moduleSettingDef, bool defaultValue)
		{
			var retVal = defaultValue;
			var s = ModuleId.HasValue ? ModuleService.GetModuleSetting(ModuleId.Value, moduleSettingDef) : ModuleService.GetGlobalModuleSetting(moduleSettingDef);
			if (s != null)
			{
				var boolVal = s.ValueAsBoolean;
				if (boolVal != null)
				{
					retVal = boolVal.Value;
				}
			}
			return retVal;
		}

		#endregion

		#region Utility Methods

		/// <summary>
		/// HTML encode text and convert markdown to HTML
		/// </summary>
		static protected internal string GetSafeFormattedText(string text, bool removeEnclosingParagraphTags = true)
		{
			if (text.IsNullOrBlank())
			{
				return text;
			}
			text = new Markdown().Transform(HttpUtility.HtmlEncode(text.Replace("****", string.Empty))).Trim();
			if (removeEnclosingParagraphTags && text.IndexOf("<p", 3) == -1 && text.StartsWith("<p>") && text.EndsWith("</p>"))
			{
				text = text.Substring(3, text.Length - 7);
			}
			return text.Replace("**", string.Empty);
		}

		/// <summary>
		/// Gets the localized string.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="args">The args.</param>
		/// <returns></returns>
		static protected internal string GetLocalizedText(string key, params object[] args)
		{
			return ResourceManager.Instance.GetString(key, args);
		}

		/// <summary>
		/// Gets the localized string with markdown formatting applied.
		/// </summary>
		static protected internal string GetLocalizedTextWithFormatting(string key, params object[] args)
		{
			return GetSafeFormattedText(ResourceManager.Instance.GetString(key, args));
		}

		/// <summary>
		/// Adds the model error.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="message">The message.</param>
		/// <param name="args">The args.</param>
		protected internal void AddPropertyValidationError(string key, string message, params object[] args)
		{
			ModelState.AddModelError(key, GetLocalizedText(message, args));
		}

		/// <summary>
		/// Adds the error.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="args">The args.</param>
		protected internal void AddValidationError(string message, params object[] args)
		{
			ModelState.AddModelError(String.Empty, GetLocalizedText(message, args));
		}

		/// <summary>
		/// Adds an entry to the select list.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <param name="key">The key.</param>
		/// <param name="items">The items.</param>
		static protected internal void AddSelectList(BaseViewModel model, string key, IEnumerable items)
		{
			model.ListItems[key] = new SelectList(items, BaseEntity.IdProperty, BaseEntity.NameProperty);
		}

		/// <summary>
		/// Adds an entry to the select list.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <param name="key">The key.</param>
		/// <param name="items">The items.</param>
		/// <param name="selectedItem">The selected item.</param>
		static protected internal void AddSelectList(BaseViewModel model, string key, IEnumerable items, object selectedItem)
		{
			model.ListItems[key] = new SelectList(items, BaseEntity.IdProperty, BaseEntity.NameProperty, selectedItem);
		}

		/// <summary>
		/// Redirect to login page
		/// </summary>
		protected internal void RedirectToLogin()
		{
			Redirect(String.Format("{0}?ReturnUrl={1}", FormsAuthentication.LoginUrl, Server.UrlEncode(Request.RawUrl)));
		}

		/// <summary>
		/// Merges the specified parameters with the url.
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <param name="paramNamesAndValues">The parameter names and values.</param>
		/// <returns></returns>
		protected internal string MergeParams(string url, params object[] paramNamesAndValues)
		{
			if (paramNamesAndValues.Length % 2 != 0)
			{
				throw new ArgumentException("Length of values must be an even number.");
			}
			var args = new Dictionary<string, string>();
			for (var i = 0; i < paramNamesAndValues.Length; i += 2)
			{
				args[paramNamesAndValues[i].ToString()] = paramNamesAndValues[i + 1].ToString();
			}
			return MergeParams(url, args);
		}

		/// <summary>
		/// Merges the specified parameters with the url.
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <param name="values">The parameter values.</param>
		/// <returns></returns>
		protected internal string MergeParams(string url, object values)
		{
			var dictionary = new Dictionary<string, object>();
			if (values != null)
			{
				foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(values))
				{
					var obj2 = descriptor.GetValue(values);
					dictionary[descriptor.Name] = obj2;
				}
			}
			return MergeParams(url, dictionary);
		}

		/// <summary>
		/// Merges the specified parameters with the url.
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <param name="values">The parameter values.</param>
		/// <returns></returns>
		protected internal string MergeParams(string url, IDictionary values)
		{
			//var sb = new StringBuilder(baseUrl.Length + values.Count * 20);

			foreach (var key in values.Keys)
			{
				var regExp = new Regex("({0})=([^&]*)".FormatWith(key));
				if (regExp.IsMatch(url))
				{
					regExp.Replace(url, "{0}={1}".FormatWith(key, Url.Encode(values[key].ToSafeString())));
				}
				else
				{
					var sb = new StringBuilder();
					sb.Append(url);
					if (url.IndexOf('?') < 0)
					{
						sb.Append('?');
					}
					else
					{
						sb.Append('&');
					}
					url = sb.AppendFormat("{0}={1}", key, Url.Encode(values[key].ToSafeString())).ToString();
				}
			}
			return url;
		}

		/// <summary>
		/// Gets the qualified name of the subproperty.
		/// </summary>
		/// <param name="property">The property.</param>
		/// <param name="subProperty">The sub property.</param>
		/// <returns></returns>
		protected internal static string GetPropertyName(string property, string subProperty)
		{
			return String.Format("{0}.{1}", property, subProperty);
		}

		/// <summary>
		/// Gets the qualified id sub-property name for the specified property.
		/// </summary>
		/// <param name="property">The property.</param>
		/// <returns></returns>
		protected internal static string GetIdProperty(string property)
		{
			return GetPropertyName(property, BaseEntity.IdProperty);
		}

		/// <summary>
		/// Determines whether a value was specified for the given key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>
		/// 	<c>true</c> if the specified key has param; otherwise, <c>false</c>.
		/// </returns>
		protected internal bool HasParam(string key)
		{
			return GetParam<object>(key) != null;
		}

		/// <summary>
		/// Gets the parameter value from either the route data or request object
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		protected internal T GetParam<T>(string key)
		{
			var value = ControllerContext.IsChildAction ? RouteData.Values[key] : Request[key];
			if (value == null && !ControllerContext.IsChildAction)
			{
				value = RouteData.Values[key];
			}

			if (value == null)
			{
				ParseJsonContentData();
				value = _jsonData[key];
			}

			if (value is T)
			{
				return ((T)value);
			}
			return default(T);
		}

		private void ParseJsonContentData()
		{
			if (_jsonData != null)
			{
				return;
			}
			try
			{
				if (Request.ContentType.StartsWith(WebConstants.ContentTypeJson, StringComparison.InvariantCultureIgnoreCase))
				{
					Request.InputStream.Seek(0, SeekOrigin.Begin);
					string json;
					using (var r = new StreamReader(Request.InputStream, Encoding.UTF8, true, 1024, true))
					{
						json = r.ReadToEnd();
					}
					Request.InputStream.Seek(0, SeekOrigin.Begin);
					_jsonData = JObject.Parse(json) ?? new JObject();
				}
				else
				{
					_jsonData = new JObject();
				}
			}
			catch (Exception ex)
			{
				Log.Warn(ex);
				_jsonData = new JObject();
			}
		}

		/// <summary>
		/// Gets the parameter value from either the route data or request object
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		protected internal string GetParamAsString(string key)
		{
			var val = GetParam<object>(key);
			return val != null ? val.ToSafeString() : null;
		}

		/// <summary>
		/// Sets the cookie value.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <param name="expiration">The expiration.</param>
		protected void SetCookieValue(string key, object value, DateTime? expiration = null)
		{
			var c = new HttpCookie(key, value.ToSafeString());
			c.Expires = expiration ?? DateTime.MaxValue;
			Response.SetCookie(c);
		}

		/// <summary>
		/// Gets the cookie value.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		protected string GetCookieValue(string key)
		{
			var c = Request.Cookies[key];
			if (c != null)
			{
				return c.Value;
			}
			return null;
		}

		protected bool ValidateCaptcha(string propertyName = CaptchaConstants.CaptchaFieldName, string error = "Validation.Error.Captcha")
		{
			if (CaptchaConstants.IsCaptchaEnabled && !(GetParamAsString(CaptchaConstants.CaptchaValidKey).ToBoolean() ?? false))
			{
				AddPropertyValidationError(propertyName, error);
				return false;
			}
			return true;
		}

		#endregion

		#region Message and Error Actions

		/// <summary>
		/// Default action for showing a message
		/// </summary>
		/// <returns></returns>
		public virtual ActionResult Message()
		{
			var model = MessageModel;
			return model != null ? View(model) : Error();
		}

		/// <summary>
		/// Default unspecified error action
		/// </summary>
		/// <returns></returns>
		public virtual ActionResult Error()
		{
			return DefaultErrorAction();
		}

		#endregion

		#region Messages and Errors

		/// <summary>
		/// Gets or sets the message model.
		/// </summary>
		/// <value>The message model.</value>
		public BaseViewModel MessageModel
		{
			get { return TempData[WebConstants.MessageModelKey] as BaseViewModel; }
			set { TempData[WebConstants.MessageModelKey] = value; }
		}

		/// <summary>
		/// Gets or sets the transient messages that can be passed to a redirected view.
		/// </summary>
		/// <value>The transient messages.</value>
		protected internal IList<string> TransientMessages
		{
			get
			{
				var retVal = TempData[WebConstants.TransientMessagesKey] as IList<string>;
				if (retVal == null)
				{
					retVal = new List<string>();
					TempData[WebConstants.TransientMessagesKey] = retVal;
				}
				return retVal;
			}
		}

		/// <summary>
		/// Adds the transient messages to the specified model if a transient message exists.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <returns></returns>
		protected BaseViewModel AddTransientMessages(BaseViewModel model)
		{
			foreach (var msg in TransientMessages)
			{
				model.Messages.Add(msg);
			}
			return model;
		}

		/// <summary>
		/// Adds the message.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <param name="message">The message.</param>
		/// <param name="args">The args.</param>
		static protected internal void AddMessage(BaseViewModel model, string message, params object[] args)
		{
			model.Messages.Add(GetLocalizedText(message, args));
		}

		/// <summary>
		/// Adds the message.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <param name="error">The error.</param>
		/// <param name="args">The args.</param>
		static protected internal void AddError(BaseViewModel model, string error, params object[] args)
		{
			model.Errors.Add(GetLocalizedText(error, args));
		}

		/// <summary>
		/// Adds the message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="args">The args.</param>
		protected internal void AddTransientMessage(string message, params object[] args)
		{
			TransientMessages.Add(GetLocalizedText(message, args));
		}

		/// <summary>
		/// Redirects to message action.
		/// </summary>
		/// <param name="heading">The heading.</param>
		/// <param name="message">The message.</param>
		/// <param name="args">The args.</param>
		/// <returns></returns>
		protected ActionResult RedirectToMessageAction(string heading, string message, params object[] args)
		{
			var model = new BaseViewModel(heading);
			AddMessage(model, message, args);
			return RedirectToMessageAction(model);
		}

		/// <summary>
		/// Redirects to error action.
		/// </summary>
		/// <param name="heading">The heading.</param>
		/// <param name="error">The error.</param>
		/// <param name="args">The args.</param>
		/// <returns></returns>
		protected ActionResult RedirectToErrorAction(string heading, string error, params object[] args)
		{
			var model = new BaseViewModel(heading);
			AddError(model, error, args);
			return RedirectToMessageAction(model);
		}

		/// <summary>
		/// Redirects to default error action.
		/// </summary>
		/// <returns></returns>
		protected ActionResult RedirectToDefaultErrorAction()
		{
			return RedirectToMessageAction(null);
		}

		/// <summary>
		/// Redirects to message action.
		/// </summary>
		/// <returns></returns>
		protected ActionResult RedirectToMessageAction(BaseViewModel messageModel)
		{
			MessageModel = messageModel;
			return RedirectToAction("message", new { id = (string)null });
		}

		/// <summary>
		/// Transfers to message action.
		/// </summary>
		/// <param name="heading">The heading.</param>
		/// <param name="message">The message.</param>
		/// <param name="args">The args.</param>
		/// <returns></returns>
		protected ActionResult MessageAction(string heading, string message, params object[] args)
		{
			var model = new BaseViewModel(heading);
			AddMessage(model, message, args);
			return MessageAction(model);
		}

		/// <summary>
		/// Transfers to error action.
		/// </summary>
		/// <param name="heading">The heading.</param>
		/// <param name="error">The error.</param>
		/// <param name="args">The args.</param>
		/// <returns></returns>
		protected ActionResult ErrorAction(string heading, string error, params object[] args)
		{
			var model = new BaseViewModel(heading);
			AddError(model, error, args);
			return MessageAction(model);
		}

		/// <summary>
		/// Transfers to default error action.
		/// </summary>
		/// <returns></returns>
		protected ActionResult DefaultErrorAction()
		{
			return ErrorAction(WebConstants.DefaultErrorHeadingKey, "Error.Unspecified");
		}

		/// <summary>
		/// Transfers to message action.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <returns></returns>
		protected ActionResult MessageAction(BaseViewModel model)
		{
			return View("Message", model);
		}

		/// <summary>
		/// HTML to PDF action.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <param name="url">The URL.</param>
		/// <param name="landscape">if set to <c>true</c> use landscape orientation.</param>
		/// <param name="options">The options.</param>
		/// <returns></returns>
		protected ActionResult HtmlToPdfAction(string filename, string url, bool landscape = false, string options = null)
		{
			return HtmlToPdfAction(landscape, options, filename, url);
		}

		/// <summary>
		/// HTML to PDF action.
		/// </summary>
		/// <param name="landscape">if set to <c>true</c> use landscape orientation.</param>
		/// <param name="options">The options.</param>
		/// <param name="filename">The filename.</param>
		/// <param name="url">The URL.</param>
		/// <param name="additionalUrls">The additional urls.</param>
		/// <returns></returns>
		protected ActionResult HtmlToPdfAction(bool landscape, string options, string filename, string url, params string[] additionalUrls)
		{
			url.Guard("url");

			if (!WebConstants.IsPdfGenerationEnabled)
			{
				var webClient = new WebClient();
				using (var stream = webClient.OpenRead(url))
				using (var ms = new MemoryStream())
				{
					stream.CopyTo(ms);
					var result = new FileContentResult(ms.ToArray(), MediaTypeNames.Text.Html);
					if (filename.HasText())
					{
						result.FileDownloadName = filename;
					}
					return result;
				}
			}

			var p = new System.Diagnostics.Process();

			try
			{
				p.StartInfo.CreateNoWindow = true;
				p.StartInfo.RedirectStandardOutput = true;
				p.StartInfo.RedirectStandardError = true;
				//p.StartInfo.RedirectStandardInput = true;
				p.StartInfo.UseShellExecute = false;
				p.StartInfo.FileName = ConfigurationManager.AppSettings[WebConstants.HtmlToPdfExePathKey];
				p.StartInfo.WorkingDirectory = Path.GetDirectoryName(p.StartInfo.FileName);

				var sb = new StringBuilder();
				var pref = PreferenceService.GetPreference(CurrentAssociationId, HtmlToPdfOptionsKey);
				sb.Append(pref != null ? pref.Value : DefaultHtmlToPdfOptions);
				if (landscape)
				{
					sb.Append(" --orientation Landscape");
				}
				if (options.HasText())
				{
					sb.Append(" ");
					sb.Append(options);
				}
				sb.Append(" \"");
				sb.Append(url);
				sb.Append("\"");
				foreach (var x in additionalUrls)
				{
					sb.Append(" \"");
					sb.Append(x);
					sb.Append("\"");
				}
				sb.Append(" -");
				p.StartInfo.Arguments = sb.ToString();
				p.Start();

				//read output
				var buffer = new byte[32768];
				var ms = new MemoryStream();
				while (true)
				{
					int read = p.StandardOutput.BaseStream.Read(buffer, 0, buffer.Length);
					if (read <= 0)
					{
						break;
					}
					ms.Write(buffer, 0, read);
				}
				ms.Position = 0;

				// wait or exit
				p.WaitForExit(60000);
				// read the exit code, close process
				int returnCode = p.ExitCode;
				if (returnCode == 0)
				{
					var result = new FileStreamResult(ms, MediaTypeNames.Application.Pdf);
					if (filename.HasText())
					{
						result.FileDownloadName = filename;
					}
					return result;
				}
				else
				{
					throw new BaseException(p.StandardError.ReadToEnd());
				}
			}
			finally
			{
				p.Close();
			}
		}

		/// <summary>
		/// Gets the mobile flag.
		/// </summary>
		/// <value>The mobile flag.</value>
		protected bool IsMobile
		{
			get { return HttpContext.Items[WebConstants.MobileKey] as bool? ?? false; }
		}

		/// <summary>
		/// Gets the render type.
		/// </summary>
		/// <value>The render type.</value>
		protected string RenderType
		{
			get { return HttpContext.Items[WebConstants.RenderTypeKey] as string; }
		}

		/// <summary>
		/// Gets a value indicating whether request is for partial display.
		/// </summary>
		/// <value><c>true</c> if request is for partial display; otherwise, <c>false</c>.</value>
		protected bool RenderPartial
		{
			get { return RenderType.ToSafeString().ToLowerInvariant().StartsWith(WebConstants.RenderTypePartial.ToLowerInvariant()); }
		}

		/// <summary>
		/// Gets the JSON request flag
		/// </summary>
		/// <value>The JSON request flag</value>
		protected bool IsJsonRequest
		{
			get { return RenderType.ToSafeString().ToLowerInvariant().StartsWith(WebConstants.RenderTypeJson.ToLowerInvariant()); ; }
		}

		/// <summary>
		/// Gets the API request flag
		/// </summary>
		/// <value>The API request flag</value>
		protected bool IsApiRequest
		{
			get { return RenderType.ToSafeString().ToLowerInvariant().StartsWith(WebConstants.RenderTypeApi.ToLowerInvariant()); ; }
		}

		/// <summary>
		/// Gets a value indicating whether request is for client template data.
		/// </summary>
		/// <value><c>true</c> if request is for client template; otherwise, <c>false</c>.</value>
		protected bool IsTemplateRequest
		{
			get { return RenderType.ToSafeString().ToLowerInvariant().StartsWith(WebConstants.RenderTypeTemplate.ToLowerInvariant()); }
		}

		#endregion
	}
}