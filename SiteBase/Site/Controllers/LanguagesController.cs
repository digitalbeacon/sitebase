// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Linq;
using System.Web.Mvc;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Web;

namespace DigitalBeacon.SiteBase.Controllers
{
	[Authorization(Role.Administrator)]
	public class LanguagesController : LookupEntityController<LanguageEntity>
	{
		/// <summary>
		/// Get list of languages
		/// </summary>
		/// <returns></returns>
		public ActionResult List()
		{
			return Json(LookupService.GetNameList<LanguageEntity>()
							.Select(x => new { x.Id, x.Name }), JsonRequestBehavior.AllowGet);
		}
	}
}
