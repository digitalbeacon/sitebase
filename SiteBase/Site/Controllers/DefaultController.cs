// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Web.Mvc;
using DigitalBeacon.Web;
using DigitalBeacon.Util;
using Spark;

namespace DigitalBeacon.SiteBase.Controllers
{
	[Precompile("Blank")]
	[Precompile("PartialNav")]
	[Precompile("Message")]
	[Precompile("Delete")]
	[Precompile("LookupList")]
	[Precompile("LookupEdit")]
	[Precompile("EditorTemplates/String")]
	[Precompile("EditorTemplates/ReadOnlyField")]
	[Precompile("EditorTemplates/DateTime")]
	public class DefaultController : SiteBaseController
	{
		public DefaultController()
		{
			MobileModuleName = "sitebase";
		}
		
		public ActionResult Index(string id)
		{
			return new TransferResult(Url.Action("flexible", "content", new { id = id.HasText() ? id : "default" }));
		}

		public ActionResult Unavailable()
		{
			return ErrorAction("Error.ActionUnavailable.Heading", "Error.ActionUnavailable");
		}

		public ActionResult NotFound()
		{
			return ErrorAction("Error.ActionNotFound.Heading", "Error.ActionNotFound");
		}
	}

}
