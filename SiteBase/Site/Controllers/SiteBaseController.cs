// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Models;
using DigitalBeacon.SiteBase.Web;
using DigitalBeacon.Util;

namespace DigitalBeacon.SiteBase.Controllers
{
	public abstract class SiteBaseController : BaseController
	{
		//private string _mobileModuleName;

		#region Protected Members

		protected string MobileModuleName
		{
			get 
			{
				//return _mobileModuleName;
				return ViewBag.MobileModuleName as string; 
			}
			set
			{
				//_mobileModuleName = value;
				ViewBag.MobileModuleName = value;
			}
		}

		protected bool SuppressViewNameTranslation { get; set; }
		
		#endregion

		#region Helper Methods

		/// <summary>
		/// Gets the name of the view.
		/// </summary>
		/// <param name="baseViewName">Name of the base view.</param>
		/// <returns></returns>
		protected virtual string GetViewName(string baseViewName)
		{
			return "{0}{1}{2}".FormatWith(
				(IsMobile && MobileModuleName != null) ? "Mobile/" : string.Empty, 
				baseViewName,
				IsTemplateRequest ? "Template" : string.Empty);
		}

		#endregion

		#region Overrides

		protected override ViewResult View(string viewName, string masterName, object model)
		{
			return base.View(SuppressViewNameTranslation ? viewName : GetViewName(viewName), masterName, model);
		}

		protected override void OnResultExecuting(ResultExecutingContext filterContext)
		{
			if (filterContext.Result is ViewResult && !RenderPartial && !ControllerContext.IsChildAction)
			{
				LoadUserData();
				LoadNavigationData();
			}
			base.OnResultExecuting(filterContext);
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Loads the user data.
		/// </summary>
		private void LoadUserData()
		{
			if (IsAuthenticated)
			{
				ViewData["CurrentUser"] = new User(CurrentUser);
			}
		}

		/// <summary>
		/// Loads the navigation data.
		/// </summary>
		private void LoadNavigationData()
		{
			foreach (Navigation nav in Enum.GetValues(typeof(Navigation)))
			{
				var navItems = new List<NavigationItem>();
				var navEntities = ModuleService.GetNavigationItems(CurrentAssociationId, nav, IsAuthenticated ? CurrentUserId : (long?)null);
				foreach (var e in navEntities)
				{
					var item = new NavigationItem
					{
						Id = e.Id,
						Text = e.Text,
						Url = e.Url.IfHasText(e.Url[0] == '/' ? '~' + e.Url : e.Url),
						ImageUrl = e.ImageUrl
					};
					if (e.Parent == null)
					{
						navItems.Add(item);
					}
					else
					{
						var parent = navItems.AsQueryable().Where(x => x.Id == e.Parent.Id).SingleOrDefault();
						if (parent != null)
						{
							parent.Items.Add(item);
						}
					}
				}
				if (IsAuthenticated && nav == Navigation.TopRight)
				{
					navItems.Insert(0, new NavigationItem
					{
						Text = GetLocalizedText("Common.Welcome.Text", CurrentUser.Person.FirstName)
					});
				}
				ViewData[nav + "Nav"] = navItems;
			}
			var languageSearch = new SearchInfo<LanguageEntity> { ApplyDefaultFilters = false };
			var languages = LookupService.GetEntityList(CurrentAssociationId, languageSearch);
			if (languages.Count > 1)
			{
				ViewData[WebConstants.LanguagesKey] = languages.Select(x => x.Code).ToList();
			}
		}

		#endregion
	}

}
