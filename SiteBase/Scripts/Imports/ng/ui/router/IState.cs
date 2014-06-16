// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using System.Html;

namespace ng.ui.router
{
	[ScriptExternal]
	[ScriptIgnoreNamespace]
	public class State : IServiceProvider
	{
		public dynamic current;
		public extern dynamic get(string stateName = null);
		public extern void go(string stateName, object args = null);
		public extern bool @is(string stateName, object args = null);
	}
}