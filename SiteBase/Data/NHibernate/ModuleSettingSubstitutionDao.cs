// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using NHibernate;
using DigitalBeacon.Data.NHibernate;
using DigitalBeacon.SiteBase.Model;

namespace DigitalBeacon.SiteBase.Data.NHibernate
{
	/// <summary>
	/// Data access object for modules
	/// </summary>
	public class ModuleSettingSubstitutionDao : BaseDao<ModuleSettingSubstitutionEntity>, IModuleSettingSubstitutionDao
	{
		#region IModuleSettingSubstitutionDao Members

		public IList<ModuleSettingSubstitutionEntity> FetchByModuleSettingDefinition(ModuleSettingDefinition moduleSettingDefinition)
		{
			return FetchList(ModuleSettingSubstitutionEntity.ModuleSettingDefinitionProperty, moduleSettingDefinition);
		}

		#endregion

	}
}