// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;

namespace DigitalBeacon.SiteBase.Mobile
{
	[ScriptObjectLiteral]
	public class PostalCodeData
	{
		public bool Success;
		public int Id;
		public string Code;
		public string City;
		public string StateCode;
		public int StateId;
		public string County;
	}
}