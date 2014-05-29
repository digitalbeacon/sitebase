// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using System.Html;

namespace DigitalBeacon.SiteBase
{
	[ScriptExternal]
	[ScriptNamespace("$")]
	[ScriptName("sb")]
	public static class sitebase
	{
		public static void modalBox(Dictionary<object> args) { }
		public static void modalBox(string message) { }
		public static void displayMessage(string message, string heading, string cssClass, string additionalButtonText, Action additionalButtonAction) { }
	}

	[ScriptExternal]
	[ScriptNamespace("$.sb")]
	public static class localization
	{
		public static string confirmText;
	}

	[ScriptExternal]
	[ScriptNamespace("$.sb")]
	public abstract class FormPanel
	{
		public abstract void updateContent(string content);
		public abstract void resetSubmitFlag();
		public HTMLFormElement Form;
	}
}