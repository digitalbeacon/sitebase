// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;

namespace DigitalBeacon.SiteBase.Model
{
	[Flags]
	public enum Permission
	{
		None = 0,
		Access = 1,
		Create = 2,
		Update = 4,
		Delete = 8,
		Admin = 16
	}
}
