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
	[ScriptName("angular")]
	public class Angular
	{
		public static extern IModule module(string name, string[] requires = null, IFunction configFunction = null);
		public static extern bool isDate(object obj);
		public static extern object extend(object target, object source1, object source2 = null, object source3 = null);
	}
}