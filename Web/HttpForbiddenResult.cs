// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Web.Mvc;

namespace DigitalBeacon.Web
{
	public class HttpForbiddenResult : ActionResult
	{
		public override void ExecuteResult(ControllerContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			context.HttpContext.Response.StatusCode = 0x193; // 403
		}
	}
}
