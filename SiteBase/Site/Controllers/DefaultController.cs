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
	[Precompile("Delete")]
	[Precompile("LookupEdit")]
	[Precompile("LookupList")]
	[Precompile("Message")]
	[Precompile("PartialNav")]
	[Precompile("EditorTemplates/DateTime")]
	[Precompile("EditorTemplates/ReadOnlyField")]
	[Precompile("EditorTemplates/String")]
	[Precompile("Mobile/Index")]
	[Precompile("Mobile/Message")]
	[Precompile("Mobile/PartialNav")]
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
