// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Collections.Generic;

namespace DigitalBeacon.SiteBase.Models
{
	public class ApiResponse
	{
		public bool Success;
		public string RedirectUrl;
		public string Message;
		public string ErrorMessage;
		public Dictionary<string,string[]> ValidationErrors = new Dictionary<string,string[]>();
	}
}
