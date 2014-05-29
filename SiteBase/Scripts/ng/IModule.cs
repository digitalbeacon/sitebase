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
		IModule config(dynamic[] inlineAnnotadedFunction);

		IModule factory(string name, dynamic[] inlineAnnotadedFunction);
		IModule factory(dynamic obj);
		
		IModule filter(string name, dynamic filterFactoryFunction);
		IModule filter(string name, dynamic[] inlineAnnotadedFunction);
		IModule filter(dynamic obj);

		IModule controller(string name, dynamic controllerFactoryFunction);
	}
}