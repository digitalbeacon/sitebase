// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Collections.Generic;
using DigitalBeacon.Model;
using DigitalBeacon.Business;

namespace DigitalBeacon.SiteBase.Business
{
	/// <summary>
	/// interface for business logic pertaining to administering lookup values
	/// </summary>
	public interface ILookupAdminService : ILookupService
	{
		/// <summary>
		/// Get localized name of named entity by Id
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="id"></param>
		/// <returns></returns>
		//string GetLocalizedName<T>(long id) where T : class, INamedEntity, new();

		/// <summary>
		/// Get a list of localized named entities
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		//IList<INamedEntity> GetLocalizedNameList<T>() where T : class, INamedEntity, new();

		/// <summary>
		/// Gets the named entity by name.
		/// </summary>
		/// <param name="associationId">The association id.</param>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		T GetByName<T>(long associationId, string name) where T : class, INamedEntity, new();

		/// <summary>
		/// Gets the entity by code.
		/// </summary>
		/// <param name="associationId">The association id.</param>
		/// <param name="code">The code.</param>
		/// <returns></returns>
		T GetByCode<T>(long associationId, string code) where T : class, IBaseEntity, new();

		/// <summary>
		/// Gets the named entity list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="searchInfo">The search info.</param>
		/// <returns></returns>
		IList<T> GetEntityList<T>(SearchInfo<T> searchInfo) where T : class, IBaseEntity, new();

		/// <summary>
		/// Gets the named entity list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="associationId">The association id.</param>
		/// <param name="searchInfo">The search info.</param>
		/// <returns></returns>
		IList<T> GetEntityList<T>(long associationId, SearchInfo<T> searchInfo) where T : class, IBaseEntity, new();

		/// <summary>
		/// Gets the entity list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		IList<T> GetEntityList<T>() where T : class, IBaseEntity, new();

		/// <summary>
		/// Gets the entity list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="associationId">The association id.</param>
		/// <returns></returns>
		IList<T> GetEntityList<T>(long associationId) where T : class, IBaseEntity, new();

		/// <summary>
		/// Gets the count.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="searchInfo">The search info.</param>
		/// <returns></returns>
		long GetEntityCount<T>(SearchInfo<T> searchInfo) where T : class, IBaseEntity, new();

		/// <summary>
		/// Gets the count.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="associationId">The association id.</param>
		/// <param name="searchInfo">The search info.</param>
		/// <returns></returns>
		long GetEntityCount<T>(long associationId, SearchInfo<T> searchInfo) where T : class, IBaseEntity, new();

		/// <summary>
		/// Gets the entity count.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		long GetEntityCount<T>() where T : class, IBaseEntity, new();

		/// <summary>
		/// Gets the entity count.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="associationId">The association id.</param>
		/// <returns></returns>
		long GetEntityCount<T>(long associationId) where T : class, IBaseEntity, new();

		/// <summary>
		/// Saves the entity.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="entity">The entity.</param>
		/// <returns></returns>
		T SaveEntity<T>(T entity) where T : class, IBaseEntity, new();

		/// <summary>
		/// Saves the entity.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="associationId">The association id.</param>
		/// <param name="entity">The entity.</param>
		/// <returns></returns>
		T SaveEntity<T>(long associationId, T entity) where T : class, IBaseEntity, new();

		/// <summary>
		/// Deletes the entity.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="id">The id.</param>
		void DeleteEntity<T>(long id) where T : class, IBaseEntity, new();
	}
}
