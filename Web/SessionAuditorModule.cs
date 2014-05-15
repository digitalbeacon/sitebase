// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

// =========================================================================
// Adapted from AsynchronousSessionAuditor code from Sky Sanders
// http://spikes.codeplex.com
// Revision May 17, 2003
// =========================================================================

using System;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using DigitalBeacon.Util;

namespace DigitalBeacon.Web
{
	/// <summary>
	/// SessionAuditorModule provides a mechanism for auditing the lifecycle
	/// of an ASP.Net session and/or an ASP.Net FormsAuthentication cookie in the interest
	/// of providing better monitoring and control of a user session. This is becoming more
	/// important as more and more web apps are single page apps that may not cycle a page for
	/// many minutes or hours.
	/// 
	/// Storing a token in the Asp.net Session was for many years the default authentication strategy.
	/// The are still valid applications for this techique although FormsAuthentication has far
	/// surpassed in security and functionality.
	/// 
	/// What I am providing is a manner in which to transparently monitor the expiration status of each
	/// by implementing a module that recognizes two virtual endpoints:
	/// 
	///   http://mysite/.session.ashx
	///   http://mysite/.authsession.ashx
	/// 
	/// By making a request to these urls you will be delivered a javascript date in numeric form that
	/// represents the expiration dateTime of either the current ASP.Net session, if any, or the current
	/// FormsAuthentication ticket expiration, if any.
	/// 
	/// If the requested item does not exists, zero is returned. Any value served by this module should
	/// be cast to a date and compared with Now. If less than you should take action. You should have
	/// taken action on the client before the session timed out, aided by the output of this module, but
	/// hey, nobody is perfect.
	/// </summary>
	public class SessionAuditorModule : IHttpModule
	{
		// note: these must remain in sync with the string keys in the javascript
		private const string AspSessionAuditKey = ".SESSION.ASHX";
		private const string FormsAuthAuditKey = ".AUTHSESSION.ASHX";

		#region IHttpModule Members

		public void Init(HttpApplication context)
		{
			// this is our audit hook. get the request before anyone else does
			// and if it is for us handle it and end. no one is the wiser.
			// otherwise just let it pass...
			context.BeginRequest += HandleAuditRequest;

			// this is as early as we can access session. 
			// it is also the latest we can get in, as the script handler is coming
			// right after and we want to beat the script handler to the request
			// will have to set a cookie for the next audit request to read in Begin request.
			// the cookie is used nowhere else.
			context.PostAcquireRequestState += AuditSession;
		}

		public void Dispose()
		{
		}

		#endregion

		private static void HandleAuditRequest(object sender, EventArgs e)
		{
			HttpContext context = ((HttpApplication)sender).Context;
			bool formsAudit = context.Request.Url.PathAndQuery.ToUpperInvariant().EndsWith(FormsAuthAuditKey);
			bool aspSessionAudit = context.Request.Url.PathAndQuery.ToUpperInvariant().EndsWith(AspSessionAuditKey);

			if (!formsAudit && !aspSessionAudit)
			{
				// your are not the droids i am looking for, you may move along...
				return;
			}

			long timeout;
			//var expiration = DateTime.Now;
			// want to know forms auth status
			if (formsAudit)
			{
				HttpCookie formsAuthCookie = context.Request.Cookies[FormsAuthentication.FormsCookieName];
				if (formsAuthCookie != null)
				{
					//FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(formsAuthCookie.Value);
					//expiration = ticket.Expiration;
					timeout = (long)FormsAuthentication.Decrypt(formsAuthCookie.Value).Expiration.Subtract(DateTime.Now).TotalSeconds;
				}
				else
				{
					timeout = 0;
				}
			}
			// want to know session status
			else
			{
				// no session here, just take the word of SetAuditBugs
				HttpCookie sessionTimeoutCookie = context.Request.Cookies[AspSessionAuditKey];
				timeout = sessionTimeoutCookie == null ? 0 : (long)new DateTime(sessionTimeoutCookie.Value.ToInt64() ?? 0).Subtract(DateTime.Now).TotalSeconds;
				//if (timeout > 0)
				//{
				//	expiration = new DateTime(1970, 1, 1).ToLocalTime().AddMilliseconds(timeout);
				//}
			}

			// ensure that the response is not cached. That would defeat the whole purpose
			context.Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
			context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
			context.Response.Cache.SetNoStore();
			// the money shot. a javascript date.
			context.Response.Write(timeout > 0 ? timeout : 0);
			//context.Response.Write(expiration);
			//context.Response.Write(expiration.Subtract(DateTime.Now).TotalSeconds);
			context.Response.Flush();
			context.Response.End();
		}

		private static void AuditSession(object sender, EventArgs e)
		{
			HttpApplication app = (HttpApplication)sender;
			if ((app.Context.Handler is IRequiresSessionState || app.Context.Handler is IReadOnlySessionState))
			{
				HttpCookie sessionTimeoutCookie = new HttpCookie(AspSessionAuditKey);
				// check to see if there is a session cookie
				string cookieHeader = app.Context.Request.Headers["Cookie"];
				if (cookieHeader != null && cookieHeader.Contains("ASP.NET_SessionId"))
				{
					if (!app.Context.Session.IsNewSession)
					{
						// session is live and this is a request so lets ensure the life span
						app.Context.Session["__________SessionKicker"] = DateTime.Now;
					}
					sessionTimeoutCookie.Expires = DateTime.Now.AddMinutes(app.Session.Timeout).AddSeconds(2);
					sessionTimeoutCookie.Value = sessionTimeoutCookie.Expires.Ticks.ToString();
				}
				else
				{
					// session has timed out; don't fiddle with it
					sessionTimeoutCookie.Expires = DateTime.Now.AddDays(-30);
					sessionTimeoutCookie.Value = 0.ToString();
				}
				app.Response.Cookies.Add(sessionTimeoutCookie);
			}
		}
	}
}