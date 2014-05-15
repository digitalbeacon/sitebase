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
	public interface IModuleDao : IDao<ModuleEntity>
	{
		/// <summary>
		/// Retrieve all modules with the given module definition
		/// </summary>
		/// <param name="associationId"></param>
		/// <param name="moduleDefinition"></param>
		/// <returns></returns>
		IList<ModuleEntity> FetchByModuleDefinition(long associationId, ModuleDefinition moduleDefinition);

		/// <summary>
		/// Get the default instance of a particular module type
		/// </summary>
		/// <param name="associationId"></param>
		/// <param name="moduleDefinition"></param>
		/// <returns></returns>
		ModuleEntity FetchDefaultInstance(long associationId, ModuleDefinition moduleDefinition);

		/// <summary>
		/// Fetches the instance by name for the specified association.
		/// </summary>
		/// <param name="associationId">The association id.</param>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		ModuleEntity FetchInstance(long associationId, string name);

		/// <summary>
		/// Get the instance of a module type with the given name
		/// </summary>
		/// <param name="associationId"></param>
		/// <param name="moduleDefinition"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		ModuleEntity FetchInstance(long associationId, ModuleDefinition moduleDefinition, string name);
	}
}
