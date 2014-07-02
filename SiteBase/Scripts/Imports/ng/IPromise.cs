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
	public interface IPromise
	{
		IPromise then(Action<object> successHandler, Action<object> errorHandler = null);
	}
}