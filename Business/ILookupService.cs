// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Collections.Generic;
using DigitalBeacon.Model;

namespace DigitalBeacon.Business
{
	/// <summary>
	/// interface for business logic pertaining to lookup values
	/// </summary>
	public interface ILookupService
	{
		/// <summary>
		/// Get an entity by id
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="id"></param>
		/// <returns></returns>
		T Get<T>(long id) where T : class, IBaseEntity, new();

		/// <summary>
		/// Get a list of named entities
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		IList<INamedEntity> GetNameList<T>() where T : class, INamedEntity, new();

		/// <summary>
		/// Get name of named entity by Id
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="id"></param>
		/// <returns></returns>
		string GetName<T>(long id) where T : class, INamedEntity, new();

		/// <summary>
		/// Get a named entity by name
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name"></param>
		/// <returns></returns>
		T GetByName<T>(string name) where T : class, INamedEntity, new();

		/// <summary>
		/// Get code of coded entity by Id
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="id"></param>
		/// <returns></returns>
		string GetCode<T>(long id) where T : class, ICodedEntity, new();

		/// <summary>
		/// Get a coded entity by code
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="code"></param>
		/// <returns></returns>
		T GetByCode<T>(string code) where T : class, ICodedEntity, new();
	}
}
