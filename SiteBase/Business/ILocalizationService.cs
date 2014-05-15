// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using System.Globalization;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Model;

namespace DigitalBeacon.SiteBase.Business
{
	public interface ILocalizationService
	{
		#region Resources

		/// <summary>
		/// Gets the resource.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns></returns>
		ResourceEntity GetResource(long id);

		/// <summary>
		/// Gets the resources.
		/// </summary>
		/// <param name="searchInfo">The search info.</param>
		/// <returns></returns>
		IList<ResourceEntity> GetResources(SearchInfo<ResourceEntity> searchInfo);

		/// <summary>
		/// Gets the resource count.
		/// </summary>
		/// <param name="searchInfo">The search info.</param>
		/// <returns></returns>
		long GetResourceCount(SearchInfo<ResourceEntity> searchInfo);

		/// <summary>
		/// Saves the resource.
		/// </summary>
		/// <param name="resource">The resource.</param>
		/// <returns></returns>
		ResourceEntity SaveResource(ResourceEntity resource);

		/// <summary>
		/// Saves the resources.
		/// </summary>
		/// <param name="resources">The resources.</param>
		void SaveResources(IDictionary<long, string> resources);

		/// <summary>
		/// Deletes the resource.
		/// </summary>
		/// <param name="id">The id.</param>
		void DeleteResource(long id);

		/// <summary>
		/// Deletes the resource.
		/// </summary>
		/// <param name="resource">The resource.</param>
		void DeleteResource(ResourceEntity resource);

		/// <summary>
		/// Gets the resource.
		/// </summary>
		/// <param name="culture">The culture.</param>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		ResourceEntity GetResource(CultureInfo culture, string key);

		/// <summary>
		/// Gets the resource.
		/// </summary>
		/// <param name="culture">The culture.</param>
		/// <param name="type">The type.</param>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		ResourceEntity GetResource(CultureInfo culture, string type, string key);

		/// <summary>
		/// Gets the resources.
		/// </summary>
		/// <param name="culture">The culture.</param>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		IList<ResourceEntity> GetResources(CultureInfo culture, string type);

		/// <summary>
		/// Gets only the resources with null property keys.
		/// </summary>
		/// <param name="culture">The culture.</param>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		IList<ResourceEntity> GetDefaultResources(CultureInfo culture, string type);

		/// <summary>
		/// Gets the resources.
		/// </summary>
		/// <param name="culture">The culture.</param>
		/// <param name="type">The type.</param>
		/// <param name="key">The key.</param>
		/// <param name="property">The property.</param>
		/// <returns></returns>
		IList<ResourceEntity> GetResources(CultureInfo culture, string type, string key, string property);

		/// <summary>
		/// Localizes the specified entity.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="entity">The entity.</param>
		/// <param name="defaultProperty">The default property.</param>
		/// <returns></returns>
		T Localize<T>(T entity, string defaultProperty) where T : class, IBaseEntity;

		/// <summary>
		/// Localizes the specified entity to the given culture.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="culture">The culture.</param>
		/// <param name="entity">The entity.</param>
		/// <param name="defaultProperty">The default property.</param>
		/// <returns></returns>
		T Localize<T>(CultureInfo culture, T entity, string defaultProperty) where T : class, IBaseEntity;

		/// <summary>
		/// Localizes the entities in the specified list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list">The list.</param>
		/// <param name="defaultProperty">The default property.</param>
		/// <returns></returns>
		IList<T> Localize<T>(IList<T> list, string defaultProperty) where T : class, IBaseEntity;

		/// <summary>
		/// Localizes the registered property of specified entity.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="entity">The entity.</param>
		/// <returns></returns>
		T LocalizeRegistered<T>(T entity) where T : class, IBaseEntity;

		/// <summary>
		/// Localizes the registered property of the entities in the specified list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list">The list.</param>
		/// <returns></returns>
		IList<T> LocalizeRegistered<T>(IList<T> list) where T : class, IBaseEntity;

		/// <summary>
		/// Localizes the name property of specified entity.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="entity">The entity.</param>
		/// <returns></returns>
		T LocalizeName<T>(T entity) where T : class, IBaseEntity;

		/// <summary>
		/// Localizes the name property of the entities in the specified list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list">The list.</param>
		/// <returns></returns>
		IList<T> LocalizeName<T>(IList<T> list) where T : class, IBaseEntity;

		/// <summary>
		/// Localizes a single property of the specified entity.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="entity">The entity.</param>
		/// <param name="propertyName">Name of the property.</param>
		/// <returns></returns>
		T LocalizeProperty<T>(T entity, string propertyName) where T : class, IBaseEntity;

		/// <summary>
		/// Localizes a single property for each entity in the specified list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list">The list.</param>
		/// <param name="propertyName">Name of the property.</param>
		/// <returns></returns>
		IList<T> LocalizeProperty<T>(IList<T> list, string propertyName) where T : class, IBaseEntity;

		/// <summary>
		/// Gets list of unique type values.
		/// </summary>
		/// <returns></returns>
		IList<string> GetTypes();

		/// <summary>
		/// Gets the resource set names.
		/// </summary>
		/// <returns></returns>
		IList<string> GetResourceSetNames();

		/// <summary>
		/// Creates the resource set.
		/// </summary>
		/// <param name="languageId">The language id.</param>
		/// <param name="resourceSetName">Name of the resource set.</param>
		void CreateResourceSet(long languageId, string resourceSetName);

		/// <summary>
		/// Deletes the resource set.
		/// </summary>
		/// <param name="languageId">The language id.</param>
		/// <param name="resourceSetName">Name of the resource set.</param>
		/// <param name="emptyValuesOnly">if set to <c>true</c> [empty values only].</param>
		void DeleteResourceSet(long languageId, string resourceSetName, bool emptyValuesOnly);

		/// <summary>
		/// Creates the entity resource set.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="languageId">The language id.</param>
		/// <param name="propertyName">Name of the property.</param>
		void CreateEntityResourceSet<T>(long languageId, string propertyName) where T : class, IBaseEntity, new();

		/// <summary>
		/// Deletes the entity resource set.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="languageId">The language id.</param>
		/// <param name="emptyValuesOnly">if set to <c>true</c> [empty values only].</param>
		void DeleteEntityResourceSet<T>(long languageId, bool emptyValuesOnly) where T : class, IBaseEntity, new();

		#endregion
	}
}
