// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Web.Mvc;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Models;
using DigitalBeacon.SiteBase.Web;

namespace DigitalBeacon.SiteBase.Controllers
{
	public class AdminController : SiteBaseController
	{
		[Authorization(Role.Administrator)]
		public ActionResult Index()
		{
			return View("PartialNav", AddTransientMessages(new PartialNavModel { Url = "~/admin" }));
		}
	}

}
