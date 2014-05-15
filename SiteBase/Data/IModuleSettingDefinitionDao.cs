// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Collections.Generic;
using DigitalBeacon.Data;
using DigitalBeacon.SiteBase.Model;

namespace DigitalBeacon.SiteBase.Data
{
	/// <summary>
	/// The interface for accessing data associated with modules
	/// </summary>
	public interface IModuleSettingDefinitionDao : IDao<ModuleSettingDefinitionEntity>
	{
		/// <summary>
		/// Retrieve all module instance setting definitions to display for the given module definition
		/// </summary>
		/// <param name="moduleDefinition"></param>
		/// <returns></returns>
		IList<ModuleSettingDefinitionEntity> FetchListToDisplayByModuleDefinition(ModuleDefinition moduleDefinition);

		/// <summary>
		/// Retrieve all module global setting definitions to display for the given module definition
		/// </summary>
		/// <param name="moduleDefinition"></param>
		/// <returns></returns>
		IList<ModuleSettingDefinitionEntity> FetchGlobalListToDisplayByModuleDefinition(ModuleDefinition moduleDefinition);

		/// <summary>
		/// Retrieve setting definition by module definition and setting key
		/// </summary>
		/// <param name="moduleDefinition"></param>
		/// <param name="settingKey"></param>
		/// <returns></returns>
		ModuleSettingDefinitionEntity Fetch(ModuleDefinition moduleDefinition, string settingKey);
	}
}
