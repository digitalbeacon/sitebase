// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DigitalBeacon.Business;
using DigitalBeacon.SiteBase.Business;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.Util;
using DigitalBeacon.Web;

namespace DigitalBeacon.SiteBase.Web
{
	public class AuthorizationAttribute : AuthorizeAttribute
	{
		private static IPermissionService _permissionService;

		private string _roles;
		private string[] _rolesSplit;
		private string _exclude;
		private string[] _excludeSplit;

		/// <summary>
		/// Gets or sets a value indicating whether an unauthenticated request is required.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if an unauthenticated request is required; otherwise, <c>false</c>.
		/// </value>
		public bool RequireUnauthenticated { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether request requires authorization.
		/// </summary>
		/// <value><c>true</c> if request requires authorization; otherwise, <c>false</c>.</value>
		public bool RequireAuthorization { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the permissions service should be used.
		/// </summary>
		/// <value><c>true</c> if the permissions service should be used; otherwise, <c>false</c>.</value>
		public bool CheckPermissions { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether action requires a local request.
		/// </summary>
		/// <value><c>true</c> if action requires a local request; otherwise, <c>false</c>.</value>
		public bool RequireLocal { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="AuthorizationAttribute"/> class.
		/// </summary>
		public AuthorizationAttribute() : this(true)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AuthorizationAttribute"/> class.
		/// </summary>
		/// <param name="checkPermissions">if set to <c>true</c> [check permissions].</param>
		public AuthorizationAttribute(bool checkPermissions)
		{
			CheckPermissions = checkPermissions;
			RequireAuthorization = true;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AuthorizationAttribute"/> class.
		/// </summary>
		/// <param name="roles">The roles.</param>
		public AuthorizationAttribute(params Role[] roles) : this()
		{
			_rolesSplit = roles.Select(x => x.ToString()).ToArray();
			_roles = String.Join(",", _rolesSplit);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AuthorizationAttribute"/> class.
		/// </summary>
		/// <param name="roles">The roles.</param>
		public AuthorizationAttribute(params string[] roles) : this()
		{
			_rolesSplit = roles;
			_roles = String.Join(",", _rolesSplit);
		}

		/// <summary>
		/// Gets or sets the user roles.
		/// </summary>
		/// <value></value>
		/// <returns>The user roles.</returns>
		public new string Roles
		{
			get { return _roles; }
			set
			{
				_roles = value;
				_rolesSplit = SplitString(value);
			}
		}

		/// <summary>
		/// Gets or sets the excluded actions.
		/// </summary>
		/// <value>The excluded actions.</value>
		public string Exclude
		{
			get { return _exclude; }
			set
			{
				_exclude = value;
				_excludeSplit = SplitString(value);
			}
		}

		/// <summary>
		/// Gets the permission service.
		/// </summary>
		/// <value>The permission service.</value>
		private static IPermissionService PermissionService
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

		/// <summary>
		/// Called when a process requests authorization.
		/// </summary>
		/// <param name="filterContext">The filter context, which encapsulates information for using <see cref="T:System.Web.Mvc.AuthorizeAttribute"/>.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="filterContext"/> parameter is null.</exception>
		public override void OnAuthorization(AuthorizationContext filterContext)
		{
			if (filterContext == null)
			{
				throw new ArgumentNullException("filterContext");
			}

			if (!RequireAuthorization
				 || (_excludeSplit != null
					 && _excludeSplit.Length > 0
					 && _excludeSplit.Any(x => TextUtil.IsEqualIgnoreCase(filterContext.RouteData.Values["action"], x))))
			{
				SetCachePolicy(filterContext);
			}
			else if (RequireLocal && !filterContext.HttpContext.Request.IsLocal)
			{
				filterContext.Result = new HttpForbiddenResult();
			}
			else if (RequireUnauthenticated)
			{
				if (filterContext.HttpContext.User.Identity.IsAuthenticated)
				{
					filterContext.Result = new HttpForbiddenResult();
				}
				else
				{
					SetCachePolicy(filterContext);
				}
			}
			else if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
			{
				if (CheckPermissions && HasPermission(filterContext, null))
				{
					SetCachePolicy(filterContext);
				}
				else
				{
					filterContext.Result = RenderPartial ? (ActionResult)new HttpForbiddenResult() : new HttpUnauthorizedResult();
				}
			}
			else if (IsAuthorized(filterContext))
			{
				SetCachePolicy(filterContext);
			}
			else
			{
				filterContext.Result = new HttpForbiddenResult();
			}
		}

		/// <summary>
		/// Determines whether the request is authorized.
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if the request is authorized; otherwise, <c>false</c>.
		/// </returns>
		protected bool IsAuthorized(AuthorizationContext filterContext)
		{
			var authorized = false;
			var currentUser = filterContext.HttpContext.Items[WebConstants.CurrentUserKey] as UserEntity;
			if (currentUser == null)
			{
				throw new BaseException("CurrentUser should not be null.");
			}
			if (currentUser.SuperUser || (!CheckPermissions &&  _roles.IsNullOrBlank()))
			{
				authorized = true;
			}
			else if (_rolesSplit != null && _rolesSplit.Length > 0)
			{
				var associationId = (long)filterContext.HttpContext.Items[WebConstants.CurrentAssociationIdKey];
				foreach (var role in _rolesSplit)
				{
					if (currentUser.Roles.Any(x => x.AssociationId == associationId && x.Role.ToString() == role))
					{
						authorized = true;
						break;
					}
				}
			}
			if (!authorized && CheckPermissions)
			{
				authorized = HasPermission(filterContext, currentUser);
			}
			return authorized;
		}

		private static bool HasPermission(ControllerContext filterContext, UserEntity user)
		{
			var sb = new StringBuilder("/");
			sb.Append(filterContext.RouteData.Values["controller"]);
			sb.Append("/");
			sb.Append(filterContext.RouteData.Values["action"]);
			sb.Append("/");
			sb.Append(filterContext.RouteData.Values["id"]);
			return PermissionService.HasPermissionToSitePath(user, sb.ToString());
		}

		/// <summary>
		/// Gets the render type.
		/// </summary>
		/// <value>The render type.</value>
		public string RenderType
		{
			get { return HttpContext.Current.Items[WebConstants.RenderTypeKey] as string; }
		}

		/// <summary>
		/// Gets a value indicating whether request is for partial display.
		/// </summary>
		/// <value><c>true</c> if request is for partial display; otherwise, <c>false</c>.</value>
		public bool RenderPartial
		{
			get { return RenderType.ToSafeString().ToLowerInvariant().StartsWith(WebConstants.RenderTypePartial.ToLowerInvariant()) ; }
		}

		/// <summary>
		/// ** IMPORTANT **
		/// Since we're performing authorization at the action level, the authorization code runs
		/// after the output caching module. In the worst case this could allow an authorized user
		/// to cause the page to be cached, then an unauthorized user would later be served the
		/// cached page. We work around this by telling proxies not to cache the sensitive page,
		/// then we hook our custom authorization code into the caching mechanism so that we have
		/// the final say on whether a page should be served from the cache.
		/// </summary>
		/// <param name="filterContext">The filter context.</param>
		protected void SetCachePolicy(AuthorizationContext filterContext)
		{
			HttpCachePolicyBase cachePolicy = filterContext.HttpContext.Response.Cache;
			cachePolicy.SetProxyMaxAge(new TimeSpan(0));
			cachePolicy.AddValidationCallback(CacheValidationHandler, null /* data */);
		}

		/// <summary>
		/// Handles the cache validation event
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="data">The data.</param>
		/// <param name="validationStatus">The validation status.</param>
		protected void CacheValidationHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
		{
			validationStatus = OnCacheAuthorization(new HttpContextWrapper(context));
		}

		internal static string[] SplitString(string original)
		{
			if (string.IsNullOrEmpty(original))
			{
				return new string[0];
			}
			return original.Split(new[] { ',' })
				.Select(x => x.Trim())
				.Where(x => x.HasText())
				.ToArray();
		}
	}
}
