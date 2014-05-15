// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;

namespace DigitalBeacon
{
	[ScriptExternal]
	[ScriptNamespace("$")]
	[ScriptName("digitalbeacon")]
	public static class digitalbeacon
	{
		public static string appContextPath;

		public static void log(string message) { }
		public static string resolveUrl(string relativeUrl) { return null; }
		public static void loadCssFile(string cssFile) { }
		public static string htmlEncode(string s) { return null; }
		public static string htmlDecode(string s) { return null; }
		public static void registerJQueryPlugin(string name, Func<object,object,object> fn) { }
	}
}