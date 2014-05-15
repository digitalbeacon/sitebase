// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using System.Html;
using System.Collections;
using jQueryLib;

namespace DigitalBeacon
{
	public class StringBuilder
	{
		private string[] _parts = new string[0];
		
		public StringBuilder cat(string str)
		{
			_parts.push(str);
			return this;
		}

		public string build()
		{
			return _parts.join("");
		}
	}
}
