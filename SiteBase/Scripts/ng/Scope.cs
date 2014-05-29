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
	public class Scope
	{
		[ScriptName("$on")]
		public extern void on(string eventName, dynamic func);

		[ScriptName("$apply")]
		public extern void apply();

		[ScriptName("$emit")]
		public extern void emit(string eventName, object args = null);

		[ScriptName("$broadcast")]
		public extern void broadcast(string eventName, object args = null);

		[ScriptName("$watch")]
		public extern void watch(string expression, dynamic listener);
	}
}