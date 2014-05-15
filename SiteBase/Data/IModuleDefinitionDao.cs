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
	/// The interface for accessing data associated with module definitions
	/// </summary>
	public interface IModuleDefinitionDao : IDao<ModuleDefinitionEntity>, INameDao<ModuleDefinitionEntity>
	{
		/// <summary>
		/// Retrieve all module definitions to display
		/// </summary>
		/// <returns></returns>
		IList<ModuleDefinitionEntity> FetchListToDisplay();
	}
}
