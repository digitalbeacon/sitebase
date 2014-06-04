// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Collections.Generic;

namespace DigitalBeacon.SiteBase
{
	public class ApiResponse
	{
		public bool Success;
		public string Message;
		public string ErrorMessage;
		public Dictionary<string[]> ValidationErrors;
		public int Id;
		public string RedirectUrl;
	}
}
