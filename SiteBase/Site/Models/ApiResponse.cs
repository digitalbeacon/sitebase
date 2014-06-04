// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Collections.Generic;
using System.Web;

namespace DigitalBeacon.SiteBase.Models
{
	public class ApiResponse
	{
		public bool Success;
		public string Message;
		public string ErrorMessage;
		public Dictionary<string,string[]> ValidationErrors = new Dictionary<string,string[]>();
		public long Id;
		public string RedirectUrl;
	}
}
