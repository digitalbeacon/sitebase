// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;

namespace DigitalBeacon
{
	//[ScriptExternal]
	//[ScriptIgnoreNamespace]
	public static class ObjectExtensions
	{
		//[ScriptMixin]
		public static bool hasValue(this object obj)
		{
			return (bool)obj;
		}
	}
}
