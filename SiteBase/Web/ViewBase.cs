// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Configuration;
using System.Linq.Expressions;
using System.Web.Mvc;
using DigitalBeacon.Business;
using DigitalBeacon.Util;
using MarkdownSharp;

namespace DigitalBeacon.SiteBase.Web
{
	/// <summary>
	/// The strongly-typed base type for views for Spark/FluentHtml compatibility
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class ViewBase<T> : DigitalBeacon.Web.ViewBase<T> where T : class
	{
		/// <summary>
		/// Gets the property name from the lambda expression
		/// </summary>
		/// <typeparam name="TProperty">The type of the property.</typeparam>
		/// <param name="propertyExpression">The property expression.</param>
		/// <returns></returns>
		protected string PropertyName<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
		{
			propertyExpression.Guard("propertyExpression");
			var property = propertyExpression.GetMember();
			if (property == null)
			{
				throw new ArgumentException("Could not get property for expression {0}.".FormatWith(propertyExpression));
			}
			return property.Name;
		}

		/// <summary>
		/// Alias for PropertyName method
		/// </summary>
		/// <typeparam name="TProperty">The type of the property.</typeparam>
		/// <param name="propertyExpression">The property expression.</param>
		/// <returns></returns>
		protected string P<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
		{
			return PropertyName(propertyExpression);
		}

		/// <summary>
		/// Determines whether a value was specified for the given key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>
		/// 	<c>true</c> if the specified key has param; otherwise, <c>false</c>.
		/// </returns>
		protected bool HasParam(string key)
		{
			return GetParam<object>(key) != null;
		}

		/// <summary>
		/// Gets the parameter value from either the route data or request object
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		protected TParam GetParam<TParam>(string key)
		{
			var value = ViewContext.IsChildAction ? ViewContext.RouteData.Values[key] : Request[key];
			if (value is TParam)
			{
				return ((TParam)value);
			}
			return default(TParam);
		}

		/// <summary>
		/// Gets the parameter value from either the route data or request object
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		protected string GetParamAsString(string key)
		{
			var val = GetParam<object>(key);
			return val != null ? val.ToSafeString() : null;
		}

		/// <summary>
		/// Gets a value indicating whether site supports alternate rendering for mobile devices.
		/// </summary>
		/// <value>
		///   <c>true</c> if site supports alternate rendering for mobile devices; otherwise, <c>false</c>.
		/// </value>
		protected bool SupportsMobile
		{
			get
			{
				var val = Context.Application[WebConstants.SupportsMobileKey] as bool?;
				if (val == null)
				{
					val = ConfigurationManager.AppSettings[WebConstants.SupportsMobileKey].ToBoolean() ?? false;
					Context.Application[WebConstants.SupportsMobileKey] = val;
				}
				return (bool)val;
			}
		}

		/// <summary>
		/// Gets the mobile flag.
		/// </summary>
		/// <value>The mobile flag.</value>
		protected bool IsMobile
		{
			get { return SupportsMobile && (Context.Items[WebConstants.MobileKey] as bool? ?? false); }
		}

		/// <summary>
		/// Gets the render type.
		/// </summary>
		/// <value>The render type.</value>
		public string RenderType
		{
			get { return Context.Items[WebConstants.RenderTypeKey] as string; }
		}

		/// <summary>
		/// Gets a value indicating whether request is for partial display.
		/// </summary>
		/// <value><c>true</c> if request is for partial display; otherwise, <c>false</c>.</value>
		public bool RenderPartial
		{
			get { return RenderType.ToSafeString().ToLowerInvariant().StartsWith(WebConstants.RenderTypePartial.ToLowerInvariant()); }
		}

		/// <summary>
		/// Gets localized text.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="args">The args.</param>
		/// <returns></returns>
		protected string Text(string key, params object[] args)
		{
			return ResourceManager.Instance.GetString(key, args);
		}

		/// <summary>
		/// Expands the site relative text.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns></returns>
		protected string ExpandSiteRelativeText(string text)
		{
			return text.HasText() ? text.Replace("\"~/", "\"" + (Request.ApplicationPath == "/" ? String.Empty : Request.ApplicationPath) + "/") : text;
		}

		/// <summary>
		/// Gets the site relative text.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns></returns>
		protected string GetSiteRelativeText(string text)
		{
			return (Request.ApplicationPath.HasText() && text.HasText()) ? text.Replace("\"" + (Request.ApplicationPath == "/" ? String.Empty : Request.ApplicationPath) + "/", "\"~/") : text;
		}

		/// <summary>
		/// HTML encode text and convert markdown to HTML
		/// </summary>
		protected string M(string text, bool removeEnclosingParagraphTags = true)
		{
			if (text.IsNullOrBlank())
			{
				return text;
			}
			text = new Markdown().Transform(H(text)).Trim();
			if (removeEnclosingParagraphTags && text.IndexOf("<p", 3) == -1 && text.StartsWith("<p>") && text.EndsWith("</p>"))
			{
				text = text.Substring(3, text.Length - 7);
			}
			return text;
		}

		/// <summary>
		/// Gets a value indicating whether JavaScript should be enabled for the associated session.
		/// </summary>
		/// <value><c>true</c> if JavaScript should be enabled for the associated session; otherwise, <c>false</c>.</value>
		protected bool EnableJavaScript
		{
			get
			{
				var val = GetParamAsString(WebConstants.EnableJavaScriptKey).ToBoolean();
				if (Context.Session != null)
				{
					if (val.HasValue)
					{
						Context.Session[WebConstants.EnableJavaScriptKey] = val.Value;
					}
					else
					{
						val = Context.Session[WebConstants.EnableJavaScriptKey] as bool?;
					}
				}
				return val ?? true;
			}
		}

		/// <summary>
		/// Gets the Google Analytics Id.
		/// </summary>
		/// <value>The Google Analytics Id.</value>
		protected string GoogleAnalyticsId
		{
			get
			{
				var val = Context.Application[WebConstants.GoogleAnalyticsIdKey] as string;
				if (val == null)
				{
					val = ConfigurationManager.AppSettings[WebConstants.GoogleAnalyticsIdKey] ?? String.Empty;
					Context.Application[WebConstants.GoogleAnalyticsIdKey] = val;
				}
				return val;
			}
		}

		/// <summary>
		/// Returns whether there is an authenticated user
		/// </summary>
		protected internal bool IsAuthenticated
		{
			get { return BaseController.IsAuthenticated; }
		}
	}

	/// <summary>
	/// The base type for views for Spark/FluentHtml compatibility
	/// </summary>
	public abstract class ViewBase : ViewBase<object> { }
}