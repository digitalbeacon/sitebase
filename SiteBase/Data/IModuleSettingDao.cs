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
	/// The interface for accessing data associated with module settings
	/// </summary>
	public interface IModuleSettingDao : IDao<ModuleSettingEntity>
	{
		/// <summary>
		/// Get module setting by module setting defintion
		/// </summary>
		/// <param name="moduleId"></param>
		/// <param name="moduleSettingDefinition"></param>
		/// <returns></returns>
		ModuleSettingEntity Fetch(long? moduleId, ModuleSettingDefinition moduleSettingDefinition);

		/// <summary>
		/// Retrieve the given module setting
		/// </summary>
		/// <param name="moduleId"></param>
		/// <param name="settingKey"></param>
		/// <returns></returns>
		ModuleSettingEntity Fetch(long? moduleId, string settingKey);
	}
}
