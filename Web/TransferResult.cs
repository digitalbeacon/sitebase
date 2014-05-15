// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DigitalBeacon.Web
{
	public class TransferResult : RedirectResult
	{
		public TransferResult(string url) : base(url)
		{
		}

		public TransferResult(object routeValues) : base(GetRouteUrl(routeValues))
		{
		}

		private static string GetRouteUrl(object routeValues)
		{
			UrlHelper url = new UrlHelper(new RequestContext(new HttpContextWrapper(HttpContext.Current), new RouteData()), RouteTable.Routes);
			return url.RouteUrl(routeValues);
		}

		public override void ExecuteResult(ControllerContext context)
		{
			var httpContext = HttpContext.Current;
			// ASP.NET MVC 3.0
			if (context.Controller.TempData != null && context.Controller.TempData.Count > 0)
			{
				context.Controller.TempData.Keep();
				context.Controller.TempData.Save(context, ((Controller)context.Controller).TempDataProvider);
			}
			httpContext.Server.TransferRequest(Url, true); // change to false to pass query string parameters if you have already processed them
		}
	}
}
