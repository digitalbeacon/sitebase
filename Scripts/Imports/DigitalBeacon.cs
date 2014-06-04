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

		public extern static void log(string message);
		public extern static string resolveUrl(string relativeUrl);
		public extern static void loadCssFile(string cssFile);
		public extern static string htmlEncode(string s);
		public extern static string htmlDecode(string s);
		public extern static void registerJQueryPlugin(string name, Func<object, object, object> fn);
		public extern static bool isOfType(object obj, string type);
	}
}