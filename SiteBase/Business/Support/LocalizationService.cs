// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DigitalBeacon.Business;
using DigitalBeacon.Data;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Data;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.Util;
using Spring.Context;
using Spring.Context.Support;

namespace DigitalBeacon.SiteBase.Business.Support
{
	public class LocalizationService : ILocalizationService
	{
		private static readonly IDataAdapter DataAdapter = ServiceFactory.Instance.GetService<IDataAdapter>();
		private static readonly IAuditingService AuditingService = ServiceFactory.Instance.GetService<IAuditingService>();
		private static readonly IResourceDao ResourceDao = ServiceFactory.Instance.GetService<IResourceDao>();

		#region ILocalizationService implementation

		public ResourceEntity GetResource(long id)
		{
			return DataAdapter.Fetch<ResourceEntity>(id);
		}

		public IList<ResourceEntity> GetResources(SearchInfo<ResourceEntity> searchInfo)
		{
			return DataAdapter.FetchList(PrepareSearch(searchInfo));
		}

		public long GetResourceCount(SearchInfo<ResourceEntity> searchInfo)
		{
			return DataAdapter.FetchCount(PrepareSearch(searchInfo));
		}

		public ResourceEntity SaveResource(ResourceEntity resource)
		{
			var isNew = resource.IsNew;
			var retVal = DataAdapter.Save(resource);
			if (isNew)
			{
				AuditingService.CreateAuditLogEntry(AuditAction.CreateEntity, 0, retVal);
			}
			else
			{
				AuditingService.CreateAuditLogEntry(AuditAction.UpdateEntity, 0, retVal);
			}
			return retVal;
		}

		public void SaveResources(IDictionary<long, string> resources)
		{
			foreach (var id in resources.Keys)
			{
				var resource = GetResource(id);
				if (resource == null)
				{
					throw new ServiceException("Could not find {0} with Id [{1}] for update.", typeof(ResourceEntity).Name, id);
				}
				resource.Value = resources[id].DefaultTo(String.Empty);
				SaveResource(resource);
			}
		}

		public void DeleteResource(long id)
		{
			DeleteResource(GetResource(id));
		}

		public void DeleteResource(ResourceEntity resource)
		{
			resource.Guard("resource");
			AuditingService.CreateAuditLogEntry(AuditAction.DeleteEntity, 0, resource);
			DataAdapter.Delete(resource);
		}

		public ResourceEntity GetResource(CultureInfo culture, string key)
		{
			return GetResource(culture, null, key);
		}

		public ResourceEntity GetResource(CultureInfo culture, string type, string key)
		{
			var search = new SearchInfo<ResourceEntity>();
			search.AddFilter(x => x.Language.Code, (culture ?? CultureInfo.CurrentUICulture).Name);
			if (type.HasText())
			{
				search.AddFilter(x => x.Type, type);
				search.AddFilter(x => x.Property, ComparisonOperator.Null);
			}
			else
			{
				search.AddFilter(x => x.Type, ComparisonOperator.Null);
			}
			search.AddFilter(x => x.Key, key);
			var resources = DataAdapter.FetchList(search);
			if (resources.Count > 1)
			{
				throw new ServiceException("Request for unique resource returned multiple results. {0}/{1}/{2}", (culture ?? CultureInfo.CurrentUICulture).Name, type, key);
			}
			return resources.Count == 1 ? resources[0] : null;
		}

		public IList<ResourceEntity> GetResources(CultureInfo culture, string type)
		{
			return GetResources(culture, type, null, null, false);
		}

		public IList<ResourceEntity> GetDefaultResources(CultureInfo culture, string type)
		{
			return GetResources(culture, type, null, null, true);
		}

		public IList<ResourceEntity> GetResources(CultureInfo culture, string type, string key, string property)
		{
			return GetResources(culture, type, key, property, false);
		}

		public T Localize<T>(T entity, string defaultProperty) where T : class, IBaseEntity
		{
			return Localize(ResourceManager.ClientCulture, entity, defaultProperty);
		}

		public T Localize<T>(CultureInfo culture, T entity, string defaultProperty) where T : class, IBaseEntity
		{
			if (entity != null)
			{
				var resources = GetResources(culture, ResourceManager.GetTypeKey<T>(), entity.Id.ToString(), null, false);
				foreach (var resource in resources)
				{
					if (resource.Value.IsNullOrBlank())
					{
						continue;
					}
					if (resource.Property.HasText())
					{
						entity.SetPropertyValue(resource.Property, resource.Value);
					}
					else if (defaultProperty.HasText())
					{
						entity.SetPropertyValue(defaultProperty, resource.Value);
					}
				}
			}
			return entity;
		}

		public IList<T> Localize<T>(IList<T> list, string defaultProperty) where T : class, IBaseEntity
		{
			return list.Select(x => Localize(x, defaultProperty)).ToList();
		}

		public T LocalizeRegistered<T>(T entity) where T : class, IBaseEntity
		{
			var property = ResourceManager.Instance.GetLocalizedProperty<T>();
			if (property.HasText())
			{
				entity = LocalizeProperty(entity, property);
			}
			return entity;
		}

		public IList<T> LocalizeRegistered<T>(IList<T> list) where T : class, IBaseEntity
		{
			var property = ResourceManager.Instance.GetLocalizedProperty<T>();
			if (property.HasText())
			{
				list = LocalizeProperty(list, property);
			}
			return list;
		}

		public T LocalizeName<T>(T entity) where T : class, IBaseEntity
		{
			if (entity != null && typeof(INamedEntity).IsAssignableFrom(typeof(T)))
			{
				var namedEntity = (INamedEntity)entity;
				var resource = GetResource(ResourceManager.ClientCulture, ResourceManager.GetTypeKey<T>(), namedEntity.Name);
				if (resource != null && resource.Value.HasText())
				{
					namedEntity.Name = resource.Value;
				}
			}
			return entity;
		}

		public IList<T> LocalizeName<T>(IList<T> list) where T : class, IBaseEntity
		{
			if (typeof(INamedEntity).IsAssignableFrom(typeof(T)))
			{
				var resources = GetDefaultResources(ResourceManager.ClientCulture, ResourceManager.GetTypeKey<T>());
				foreach (INamedEntity e in list)
				{
					var resource = resources.Where(x => x.Key.EqualsIgnoreCase(e.Name)).SingleOrDefault();
					if (resource != null && resource.Value.HasText())
					{
						e.Name = resource.Value;
					}
				}
			}
			return list;
		}

		public T LocalizeProperty<T>(T entity, string propertyName) where T : class, IBaseEntity
		{
			if (entity != null)
			{
				var resource = GetResource(ResourceManager.ClientCulture, ResourceManager.GetTypeKey<T>(), entity.GetPropertyValue<string>(propertyName));
				if (resource != null && resource.Value.HasText())
				{
					entity.SetPropertyValue(propertyName, resource.Value);
				}
			}
			return entity;
		}

		public IList<T> LocalizeProperty<T>(IList<T> list, string propertyName) where T : class, IBaseEntity
		{
			var prop = typeof(T).GetProperty(propertyName);
			if (prop == null)
			{
				throw new ArgumentException("property {0} for type {1} does not exist".FormatWith(propertyName, typeof(T).FullName));
			}
			if (prop.PropertyType != typeof(string))
			{
				throw new ArgumentException("property {0} for type {1} is not of type string".FormatWith(propertyName, typeof(T).FullName));
			}
			var resources = GetDefaultResources(ResourceManager.ClientCulture, ResourceManager.GetTypeKey<T>());
			foreach (var e in list)
			{
				var resource = resources.Where(x => x.Key.EqualsIgnoreCase((string)prop.GetValue(e, null))).SingleOrDefault();
				if (resource != null && resource.Value.HasText())
				{
					prop.SetValue(e, resource.Value, null);
				}
			}
			return list;
		}

		public IList<string> GetTypes()
		{
			return ResourceDao.FetchUniqueTypes(); 
		}

		public IList<string> GetResourceSetNames()
		{
			return ((ResourceSetMessageSource)ServiceFactory.Instance.GetService<IMessageSource>())
				.ResourceManagers.Cast<System.Resources.ResourceManager>()
				.Where(x => x.BaseName != null)
				.Select(x => x.BaseName).ToList();
		}

		public void CreateResourceSet(long languageId, string resourceSetName)
		{
			resourceSetName.Guard("resourceSetName");
			var msgSrc = (ResourceSetMessageSource)ServiceFactory.Instance.GetService<IMessageSource>();
			foreach (System.Resources.ResourceManager mgr in msgSrc.ResourceManagers)
			{
				if (mgr.BaseName == resourceSetName)
				{
					var language = DataAdapter.Fetch<LanguageEntity>(languageId);
					var resources = GetResources(CultureInfo.GetCultureInfo(language.Code), null, null, resourceSetName);
					// GetResourceSet returns null if we do not try to get a value first
					mgr.GetString(String.Empty);
					var set = mgr.GetResourceSet(CultureInfo.CurrentUICulture, false, true);
					foreach (DictionaryEntry entry in set)
					{
						if (!(entry.Value is string))
						{
							continue;
						}
						var key = (string)entry.Key;
						var resource = resources.Where(x => x.Key.EqualsIgnoreCase(key)).SingleOrDefault();
						if (resource == null)
						{
							SaveResource(new ResourceEntity { Language = language, Key = key, Property = resourceSetName, Value = String.Empty });
						}
					}
					mgr.ReleaseAllResources();
				}
			}
		}

		public void DeleteResourceSet(long languageId, string resourceSetName, bool emptyValuesOnly)
		{
			resourceSetName.Guard("resourceSetName");
			var resources = GetResources(CultureInfo.GetCultureInfo(DataAdapter.Fetch<LanguageEntity>(languageId).Code), null, null, resourceSetName);
			foreach (var resource in resources)
			{
				if (!emptyValuesOnly || resource.Value.IsNullOrBlank())
				{
					DeleteResource(resource);
				}
			}
		}

		public void CreateEntityResourceSet<T>(long languageId, string propertyName) where T : class, IBaseEntity, new()
		{
			var language = DataAdapter.Fetch<LanguageEntity>(languageId);
			var prop = typeof(T).GetProperty(propertyName);
			if (prop == null)
			{
				throw new ArgumentException("property {0} for type {1} does not exist".FormatWith(propertyName, typeof(T).FullName));
			}
			if (prop.PropertyType != typeof(string))
			{
				throw new ArgumentException("property {0} for type {1} is not of type string".FormatWith(propertyName, typeof(T).FullName));
			}
			var type = ResourceManager.GetTypeKey<T>();
			var resources = GetResources(CultureInfo.GetCultureInfo(language.Code), type);
			var entities = DataAdapter.FetchAll<T>();
			foreach (var e in entities)
			{
				var key = (string)prop.GetValue(e, null);
				var resource = resources.Where(x => x.Key.EqualsIgnoreCase(key)).SingleOrDefault();
				if (resource == null)
				{
					SaveResource(new ResourceEntity { Language = language, Type = type, Key = key, Value = String.Empty });
				}
			}
		}

		public void DeleteEntityResourceSet<T>(long languageId, bool emptyValuesOnly) where T : class, IBaseEntity, new()
		{
			var resources = GetResources(CultureInfo.GetCultureInfo(DataAdapter.Fetch<LanguageEntity>(languageId).Code), ResourceManager.GetTypeKey<T>());
			foreach (var resource in resources)
			{
				if (!emptyValuesOnly || resource.Value.IsNullOrBlank())
				{
					DeleteResource(resource);
				}
			}
		}

		#endregion

		#region Private Methods

		private static SearchInfo<ResourceEntity> PrepareSearch(SearchInfo<ResourceEntity> searchInfo)
		{
			if (searchInfo.ApplyDefaultFilters)
			{
				if (searchInfo.SearchText.HasText())
				{
					searchInfo.AddFilter(x => x.Key, ComparisonOperator.Contains, searchInfo.SearchText).Grouping = 1;
					searchInfo.AddFilter(x => x.Value, ComparisonOperator.Contains, searchInfo.SearchText).Grouping = 2;
				}
				searchInfo.ApplyDefaultFilters = false;
			}
			return searchInfo;
		}

		private static IList<ResourceEntity> GetResources(CultureInfo culture, string type, string key, string property, bool matchNullProperty)
		{
			var search = new SearchInfo<ResourceEntity>();
			search.AddFilter(x => x.Language.Code, (culture ?? CultureInfo.CurrentUICulture).Name);
			if (type.HasText())
			{
				search.AddFilter(x => x.Type, type);
			}
			else
			{
				search.AddFilter(x => x.Type, ComparisonOperator.Null);
			}
			if (key.HasText())
			{
				search.AddFilter(x => x.Key, key);
			}
			if (matchNullProperty)
			{
				search.AddFilter(x => x.Property, ComparisonOperator.Null);
			}
			else if (property.HasText())
			{
				search.AddFilter(x => x.Property, property);
			}
			return DataAdapter.FetchList(search);
		}

		#endregion
	}
}
