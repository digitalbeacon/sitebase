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
	public interface IModule
	{
		IModule config(dynamic f);

		IModule factory(string name, dynamic f);
		IModule factory(dynamic obj);
		
		IModule filter(string name, dynamic f);
		IModule filter(dynamic obj);

		IModule controller(string name, dynamic f);

		IModule directive(string name, dynamic f);

		IModule run(dynamic f);
	}
}