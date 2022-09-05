// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Web.Mvc;
using System.Web.Security;
using DigitalBeacon.SiteBase.Web.Models;
using DigitalBeacon.Util;
using DigitalBeacon.Web;
using Spark;

namespace DigitalBeacon.SiteBase.Controllers
{
	[Precompile("Blank")]
	[Precompile("CommentsEdit")]
	[Precompile("CommentsList")]
	[Precompile("Delete")]
	[Precompile("LookupEdit")]
	[Precompile("LookupList")]
	[Precompile("Message")]
	[Precompile("PartialNav")]
	[Precompile("EditorTemplates/DateTime")]
	[Precompile("EditorTemplates/ReadOnlyField")]
	[Precompile("EditorTemplates/String")]
	[Precompile("EditorTemplates/Time")]
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
			AllowJsonGet = true;
			var model = new BaseViewModel("Error.ActionUnavailable.Heading");
			AddError(model, "Error.ActionUnavailable");
			if (!IsAuthenticated)
			{
				model.RedirectUrl = FormsAuthentication.DefaultUrl;
			}
			return MessageAction(model);
		}

		public ActionResult NotFound()
		{
			return ErrorAction("Error.ActionNotFound.Heading", "Error.ActionNotFound");
		}
	}

}
