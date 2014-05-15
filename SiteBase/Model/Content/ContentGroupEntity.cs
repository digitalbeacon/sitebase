// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using Iesi.Collections.Generic;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Model;

namespace DigitalBeacon.SiteBase.Model.Content
{
	[Serializable]
	public class ContentGroupEntity : GeneratedContentGroupEntity
	{
		public virtual bool CalculateDisplayOrder { get; set; }
	}
}
