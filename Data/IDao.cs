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
	public interface IDao<T> where T:IBaseEntity, new()
	{
		/// <summary>
		/// Retreive a single entity by Id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		T Fetch(long id);

		///// <summary>
		///// Retrieve a single entity by Id with the given version
		///// </summary>
		///// <param name="id"></param>
		///// <param name="version"></param>
		///// <returns></returns>
		//T Fetch(long id, long version);

		/// <summary>
		/// Query single entity by property value
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		T Fetch(string propertyName, object value);

		/// <summary>
		/// Retrieve a single entity by Id and initialize all lazy members
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		T FetchWithLazyCollections(long id);

		/// <summary>
		/// Retrieve all entities associated with this DAO
		/// </summary>
		/// <returns></returns>
		IList<T> FetchAll();

		/// <summary>
		/// Retrieve all entities associated with this DAO sorted by the specified parameters
		/// </summary>
		/// <param name="sortByPropertyName"></param>
		/// <returns></returns>
		IList<T> FetchAll(string sortByPropertyName);

		/// <summary>
		/// Retrieve all entities associated with this DAO sorted by the specified parameters
		/// </summary>
		/// <param name="sortByPropertyName"></param>
		/// <param name="sortDirection"></param>
		/// <returns></returns>
		IList<T> FetchAll(string sortByPropertyName, ListSortDirection sortDirection);

		/// <summary>
		/// Query by property value
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		IList<T> FetchList(string propertyName, object value);

		/// <summary>
		/// Query by property value with specified sort
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="value"></param>
		/// <param name="sortByPropertyName"></param>
		/// <param name="asc"></param>
		/// <returns></returns>
		IList<T> FetchList(string propertyName, object value, string sortByPropertyName, bool asc);

		/// <summary>
		/// Query by specified search parameters
		/// </summary>
		/// <param name="searchInfo">The search info.</param>
		/// <returns></returns>
		IList<T> FetchList(SearchInfo<T> searchInfo);

		/// <summary>
		/// Fetches the count of items that would be returned by the specified search parameters
		/// </summary>
		/// <param name="searchInfo">The search info.</param>
		/// <returns></returns>
		long FetchCount(SearchInfo<T> searchInfo);

		/// <summary>
		/// Save the given entity. This method will try create or update the associated entity
		/// depending on version property of the entity. An Id value of zero indicates that the
		/// entity is new, and a create operation will be performed. If the Id value is not zero,
		/// the entity is assumed exist and an update operation will be performed.
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		T Save(T entity);

		/// <summary>
		/// Deletes the entity with the given Id and version
		/// </summary>
		/// <param name="id"></param>
		/// <param name="version"></param>
		void Delete(long id, long version);

		/// <summary>
		/// Deletes the entity with the given Id
		/// </summary>
		/// <param name="id"></param>
		void Delete(long id);

		/// <summary>
		/// Deletes the given entity
		/// </summary>
		/// <param name="entity"></param>
		void Delete(T entity);

		/// <summary>
		/// Initializes a lazily initialized property for the given entity
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="propertyName"></param>
		void Initialize(T entity, string propertyName);

		/// <summary>
		/// Discards the entity from the associated entity manager
		/// </summary>
		/// <param name="entity"></param>
		void Evict(object entity);

	}
}
