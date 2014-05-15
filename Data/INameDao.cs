// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Collections.Generic;
using DigitalBeacon.Model;

namespace DigitalBeacon.Data
{
	/// <summary>
	/// Data access interface for named entities
	/// </summary>
	public interface INameDao<T> where T:INamedEntity, new()
	{
		/// <summary>
		/// Retrieve a list of INamedEntity's
		/// </summary>
		/// <returns></returns>
		IList<INamedEntity> FetchNameList();

		/// <summary>
		/// Retrieve an entity of the associated type with the given name
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		T FetchByName(string name);
	}
}
