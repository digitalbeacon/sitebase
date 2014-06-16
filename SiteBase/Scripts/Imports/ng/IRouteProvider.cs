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
	public interface IRouteProvider : IServiceProvider
	{
		IRouteProvider when(string path, Route route);
		IRouteProvider otherwise(dynamic args);
	}
}