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
using DigitalBeacon.SiteBase.Web;
using DigitalBeacon.Util;
using Telerik.Web.Mvc.UI;

namespace DigitalBeacon.SiteBase.Views
{
	/// <summary>
	/// The strongly-typed base type for views for Spark/FluentHtml compatibility
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class ViewBase<T> : DigitalBeacon.SiteBase.Web.ViewBase<T> where T : class
	{
		private static readonly string _assetHandlerPath;

		static ViewBase()
		{
			_assetHandlerPath = "~/combined-assets{0}.axd".FormatWith(WebConstants.AssetVersion.HasText() ? "-" + WebConstants.AssetVersion : String.Empty);
		}

		/// <summary>
		/// Sets the view context.
		/// </summary>
		/// <param name="viewContext">The view context.</param>
		protected override void SetViewContext(ViewContext viewContext)
		{
			base.SetViewContext(viewContext);
			// This is a work-around to ensure that the Telerik stylesheet and script
			// registrars are associated with the initiating ViewContext. Spark master
			// layouts are handled in reverse order which causes the registrars to be
			// associated with the first child ViewContext when using the Html.Action
			// or Html.RenderAction helpers.
			Html.Telerik().ScriptRegistrar().AssetHandlerPath(_assetHandlerPath)
				.DefaultGroup(group => group.DefaultPath("~/resources/base/telerik"));
			Html.Telerik().StyleSheetRegistrar().AssetHandlerPath(_assetHandlerPath)
				.DefaultGroup(group => group.DefaultPath("~/resources/base/telerik"));
		}
	}

	/// <summary>
	/// The base type for views for Spark/FluentHtml compatibility
	/// </summary>
	public abstract class ViewBase : ViewBase<object> { }
}