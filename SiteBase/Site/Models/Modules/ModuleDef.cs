// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Collections.Generic;
using DigitalBeacon.SiteBase.Model;

namespace DigitalBeacon.SiteBase.Models.Modules
{
	public class ModuleDef
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public IList<ModuleEntity> Instances { get; set; }
	}
}