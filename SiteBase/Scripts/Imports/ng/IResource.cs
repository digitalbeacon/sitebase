// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;

namespace ng
{
	[ScriptIgnoreNamespace]
	public interface IResource
	{
		dynamic get(object data, dynamic responseHandler = null);
		void save(object data, dynamic responseHandler = null);
		void delete(object data, dynamic responseHandler = null);
	}
}
