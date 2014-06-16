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
using DigitalBeacon.SiteBase.Web.Models;
using DigitalBeacon.Util;
using DigitalBeacon.Web;

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
		protected bool AllowJsonGet { get; set; }
		
		#endregion

		#region Helper Methods

		protected JsonResult ApiResponse(BaseViewModel model = null)
		{
			var response = new ApiResponse();
			response.Success = ModelState.IsValid && (model == null || model.Errors == null || model.Errors.Count == 0);
			if (response.Success)
			{
				response.Data = model;
				//if (model != null && model.Messages != null && model.Messages.Count > 0)
				//{
				//	response.Message = model.Messages[0];
				//}
			}
			else
			{
				if (model != null && model.Errors != null && model.Errors.Count > 0)
				{
					response.ErrorMessage = model.Errors[0];
				}
				else
				{
					foreach (var key in ModelState.Keys)
					{
						var errors = ModelState[key].Errors.Select(x => x.ErrorMessage).ToArray();
						if (errors.Length > 0)
						{
							response.ValidationErrors[key] = errors;
						}
					}
				}
			}
			return Json(response, AllowJsonGet ? JsonRequestBehavior.AllowGet : JsonRequestBehavior.DenyGet);
		}

		protected virtual void SetHeading(string heading, BaseViewModel model = null)
		{
			ViewBag.Heading = heading;
			if (model != null)
			{
				model.Heading = heading;
			}
		}

		/// <summary>
		/// Gets the name of the view.
		/// </summary>
		/// <param name="baseViewName">Name of the base view.</param>
		/// <returns></returns>
		protected virtual string GetViewName(string baseViewName)
		{
			if (!IsMobile && !RenderTemplate)
			{
				return baseViewName;
			}
			if (baseViewName.IsNullOrBlank())
			{
				baseViewName = RouteData.Values["action"].ToStringSafe();
			}
			return "{0}{1}{2}".FormatWith(
				(IsMobile && MobileModuleName != null) ? "Mobile/" : string.Empty, 
				baseViewName,
				RenderTemplate ? "Template" : string.Empty);
		}

		#endregion

		#region Overrides

		protected override ViewResult View(string viewName, string masterName, object model)
		{
			var bvm = model as BaseViewModel;
			if (bvm != null && bvm.Heading.HasText())
			{
				ViewBag.Heading = bvm.Heading;
			}
			return base.View(SuppressViewNameTranslation ? viewName : GetViewName(viewName), masterName, model);
		}

		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			base.OnActionExecuted(filterContext);
			if (RenderJson && !(filterContext.Result is TransferResult))
			{
				string url = null;
				if (filterContext.Result is RedirectToRouteResult)
				{
					var result = (RedirectToRouteResult)filterContext.Result;
					url = Url.RouteUrl(result.RouteName, result.RouteValues);
				}
				else if (filterContext.Result is RedirectResult)
				{
					url = ((RedirectResult)filterContext.Result).Url;
				}
				if (url.HasText())
				{
					filterContext.Result = Json(new ApiResponse { RedirectUrl = url });
				}
				else if (filterContext.Result is ViewResultBase)
				{
					filterContext.Result = ApiResponse(((ViewResultBase)filterContext.Result).Model as BaseViewModel);
				}
			}
		}

		protected override void OnResultExecuting(ResultExecutingContext filterContext)
		{
			if (filterContext.Result is ViewResult && !RenderPartial && !ControllerContext.IsChildAction)
			{
				LoadUserData();
				LoadNavigationData();
				if (IsMobile && MobileModuleName == null)
				{
					MobileModuleName = ModuleService.GetGlobalSetting("MobileAppName").IfNotNull(x => x.Value).DefaultTo("sitebase");
				}
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
			var navs = IsMobile ? new[] { Navigation.TopLeft, Navigation.TopRight } : new[] { Navigation.TopLeft, Navigation.TopRight, Navigation.Left };
			foreach (Navigation nav in navs)
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
