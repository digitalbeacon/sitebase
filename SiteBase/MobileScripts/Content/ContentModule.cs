// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using jQueryLib;
using ng;

namespace DigitalBeacon.SiteBase.Mobile.Content
{
	[ScriptIgnoreNamespace]
	public static class ContentModule
	{
		static ContentModule()
		{
			Angular.module("content", new[] { "ui.bootstrap" })
				.controller("contentController",
					new object[] { "$scope",
						(Action<dynamic>)
						((scope) => 
							jQuery.extend(scope, new ContentController(scope)).init()) });

		}
	}
}
