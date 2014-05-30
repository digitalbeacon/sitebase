// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Web.Mvc;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.Util;
using DigitalBeacon.Web;

namespace DigitalBeacon.SiteBase.Controllers
{
	public class HomeController : SiteBaseController
	{
		public ActionResult Index(string id)
		{
			var roleHome = IdentityService.GetRoleHome(CurrentAssociationId, IsAuthenticated ? CurrentUserId : (long?)null);
			if (roleHome != null)
			{
				if (roleHome.Redirect)
				{
					return new RedirectResult(roleHome.Url);
				}
				return new TransferResult(roleHome.Url);
			}
			return new TransferResult(Url.Action("flexible", "content", new { id = id.HasText() ? id : "default" }));
		}
	}

}
