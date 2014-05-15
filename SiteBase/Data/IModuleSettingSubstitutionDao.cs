// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using DigitalBeacon.Data;
using DigitalBeacon.SiteBase.Model;

namespace DigitalBeacon.SiteBase.Data
{
	public interface IModuleSettingSubstitutionDao : IDao<ModuleSettingSubstitutionEntity>
	{
		IList<ModuleSettingSubstitutionEntity> FetchByModuleSettingDefinition(ModuleSettingDefinition moduleSettingDefinition);
	}
}
