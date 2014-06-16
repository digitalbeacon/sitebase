// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using System.Html;

namespace ng
{
	[ScriptIgnoreNamespace]
	[ScriptExternal]
	public abstract class NgModelController
	{
		[ScriptName("$parsers")]
		public dynamic parsers;

		[ScriptName("$formatters")]
		public dynamic formatters;

		[ScriptName("$setValidity")]
		public extern void setValidity(string validationErrorKey, bool isValid);
	}
}