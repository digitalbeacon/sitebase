// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using DigitalBeacon.Model;

namespace DigitalBeacon.Data
{
	/// <summary>
	/// Data access interface for coded entities
	/// </summary>
	public interface ICodeDao<T> where T:ICodedEntity, new()
	{
		/// <summary>
		/// Retrieve an entity of the associated type with the given code
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		T FetchByCode(string code);
	}
}
