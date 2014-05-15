// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using System.ComponentModel;

using DigitalBeacon.Model;

namespace DigitalBeacon.Data
{
	/// <summary>
	/// a generic interface for data access objects
	/// </summary>
	public interface IDataAdapter
	{
		/// <summary>
		/// Retreive a single entity by Id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		T Fetch<T>(long id) where T:class, IBaseEntity, new();

//		/// <summary>
//		/// Retrieve a single entity by Id with the given version
//		/// </summary>
//		/// <param name="id"></param>
//		/// <param name="version"></param>
//		/// <returns></returns>
//		T Fetch<T>(long id, long version) where T:class, IBaseEntity, new();

		/// <summary>
		/// Query single entity by property value
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="propertyName"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		T Fetch<T>(string propertyName, object value) where T : class, IBaseEntity, new();

		/// <summary>
		/// Retreive a single entity by Id with lazy members initialized
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		T FetchWithLazyCollections<T>(long id) where T : class, IBaseEntity, new();

		/// <summary>
		/// Retrieve all entities associated with this DAO
		/// </summary>
		/// <returns></returns>
		IList<T> FetchAll<T>() where T:class, IBaseEntity, new();

		/// <summary>
		/// Retrieve all entities associated with this DAO sorted by the specified parameters
		/// </summary>
		/// <param name="sortByPropertyName"></param>
		/// <returns></returns>
		IList<T> FetchAll<T>(string sortByPropertyName) where T : class, IBaseEntity, new();

		/// <summary>
		/// Retrieve all entities associated with this DAO sorted by the specified parameters
		/// </summary>
		/// <param name="sortByPropertyName"></param>
		/// <param name="sortDirection"></param>
		/// <returns></returns>
		IList<T> FetchAll<T>(string sortByPropertyName, ListSortDirection sortDirection) where T : class, IBaseEntity, new();

		/// <summary>
		/// Query by property value
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="propertyName"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		IList<T> FetchList<T>(string propertyName, object value) where T : class, IBaseEntity, new();

		/// <summary>
		/// Query by property value with specified sort
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="propertyName"></param>
		/// <param name="value"></param>
		/// <param name="sortByPropertyName"></param>
		/// <param name="asc"></param>
		/// <returns></returns>
		IList<T> FetchList<T>(string propertyName, object value, string sortByPropertyName, bool asc) where T : class, IBaseEntity, new();

		/// <summary>
		/// Query by specified search parameters
		/// </summary>
		/// <param name="searchInfo">The search info.</param>
		/// <returns></returns>
		IList<T> FetchList<T>(SearchInfo<T> searchInfo) where T : class, IBaseEntity, new();

		/// <summary>
		/// Fetches the count of items that would be returned by the specified search parameters
		/// </summary>
		/// <param name="searchInfo">The search info.</param>
		/// <returns></returns>
		long FetchCount<T>(SearchInfo<T> searchInfo) where T : class, IBaseEntity, new();

		/// <summary>
		/// Save the given entity. This method will try create or update the associated entity
		/// depending on version property of the entity. An Id value of zero indicates that the
		/// entity is new, and a create operation will be performed. If the Id value is not zero,
		/// the entity is assumed exist and an update operation will be performed.
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		T Save<T>(T entity) where T : class, IBaseEntity, new();

		/// <summary>
		/// Deletes the entity with the given Id and version
		/// </summary>
		/// <param name="id"></param>
		/// <param name="version"></param>
		void Delete<T>(long id, long version) where T:class, IBaseEntity, new();

		/// <summary>
		/// Deletes the entity with the given Id
		/// </summary>
		/// <param name="id"></param>
		void Delete<T>(long id) where T:class, IBaseEntity, new();

		/// <summary>
		/// Deletes the given entity
		/// </summary>
		/// <param name="entity"></param>
		void Delete<T>(T entity) where T:class, IBaseEntity, new();

		/// <summary>
		/// Retrieve a list of all entities of the associated type with only the
		/// primary key and name field populated
		/// </summary>
		/// <returns></returns>
		IList<INamedEntity> FetchNameList<T>() where T : class, INamedEntity, new();

		/// <summary>
		/// Retrieve an entity of the associated type with the given name
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		T FetchByName<T>(string name) where T:class, INamedEntity, new();

		/// <summary>
		/// Retrieve an entity of the associated type with the given code
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		T FetchByCode<T>(string code) where T : class, ICodedEntity, new();

		/// <summary>
		/// Retrieve a list of all entities of the specified type by the
		/// reference Id.
		/// </summary>
		/// <returns></returns>
		IList<T> FetchAllByRefId<T>(long refId) where T : class, IArchivedEntity, new();

		///// <summary>
		///// Initialize a lazily initialized member of the given entity
		///// </summary>
		///// <param name="entity"></param>
		///// <param name="member"></param>
		//void Initialize<T>(T entity, object member) where T : class, IBaseEntity, new();

		/// <summary>
		/// Discards the entity from the associated entity manager
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="entity"></param>
		void Evict<T>(object entity) where T : class, IBaseEntity, new();

	}
}
