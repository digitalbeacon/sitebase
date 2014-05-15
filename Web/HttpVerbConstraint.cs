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
	public class HttpVerbConstraint : IRouteConstraint
	{
		private HttpVerbs _verb;

		/// <summary>
		/// Initializes a new instance of the <see cref="HttpVerbConstraint"/> class.
		/// </summary>
		/// <param name="routeVerbs">The route verbs.</param>
		public HttpVerbConstraint(HttpVerbs routeVerb)
		{
			this._verb = routeVerb;
		}

		/// <summary>
		/// Determines whether the URL parameter contains a valid value for this constraint.
		/// </summary>
		/// <param name="httpContext">An object that encapsulates information about the HTTP request.</param>
		/// <param name="route">The object that this constraint belongs to.</param>
		/// <param name="parameterName">The name of the parameter that is being checked.</param>
		/// <param name="values">An object that contains the parameters for the URL.</param>
		/// <param name="routeDirection">An object that indicates whether the constraint check is being performed when an incoming request is being handled or when a URL is being generated.</param>
		/// <returns>
		/// true if the URL parameter contains a valid value; otherwise, false.
		/// </returns>
		public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection
		)
		{
			switch (httpContext.Request.HttpMethod)
			{
				case "DELETE":
					return ((_verb & HttpVerbs.Delete) == HttpVerbs.Delete);
				case "PUT":
					return ((_verb & HttpVerbs.Put) == HttpVerbs.Put);
				case "GET":
					return ((_verb & HttpVerbs.Get) == HttpVerbs.Get);
				case "HEAD":
					return ((_verb & HttpVerbs.Head) == HttpVerbs.Head);
				case "POST":

					// First, check whether it's a real post.
					if ((_verb & HttpVerbs.Post) == HttpVerbs.Post) return (true);

					// If not, check for special magic HttpMethodOverride hidden fields.
					switch (httpContext.Request.Form["X-HTTP-Method-Override"])
					{
						case "DELETE":
							return ((_verb & HttpVerbs.Delete) == HttpVerbs.Delete);
						case "PUT":
							return ((_verb & HttpVerbs.Put) == HttpVerbs.Put);
					}
					break;
			}
			return (false);
		}
	}
}