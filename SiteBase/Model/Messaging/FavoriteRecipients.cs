// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using DigitalBeacon.Model;

namespace DigitalBeacon.SiteBase.Model.Messaging
{
	/// <summary>
	/// A struct used to aggregate favorite recipients
	/// </summary>
	public class FavoriteRecipients
	{
		public ISet<long> Users = new HashSet<long>();
		public ISet<long> Roles = new HashSet<long>();
	}
}