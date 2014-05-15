// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DigitalBeacon.Business;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.Util;

namespace DigitalBeacon.SiteBase.Business.Support
{
	public class LookupAdminService : BaseService, ILookupAdminService
	{
		private const string AssociationIdProperty = "AssociationId";

		private static readonly IList<Type> AcceptedTypes = new List<Type>();

		public static void RegisterAcceptedType<T>()
		{
			if (!AcceptedTypes.Contains(typeof(T)))
			{
				AcceptedTypes.Add(typeof(T));
			}
		}

		#region ILookupService Members

		public T Get<T>(long id) where T : class, IBaseEntity, new()
		{
			return DataAdapter.Fetch<T>(id);
		}

		public IList<INamedEntity> GetNameList<T>() where T : class, INamedEntity, new()
		{
			if (ResourceManager.Instance.IsEntityLocalized<T>())
			{
				return GetLocalizedNameList<T>();
			}
			return DataAdapter.FetchNameList<T>();
		}

		public string GetName<T>(long id) where T : class, INamedEntity, new()
		{
			if (ResourceManager.Instance.IsEntityLocalized<T>())
			{
				return GetLocalizedName<T>(id);
			}
			return DataAdapter.Fetch<T>(id).Name;
		}

		public T GetByName<T>(string name) where T : class, INamedEntity, new()
		{
			return DataAdapter.FetchByName<T>(name);
		}

		public string GetCode<T>(long id) where T : class, ICodedEntity, new()
		{
			return DataAdapter.Fetch<T>(id).Code;
		}

		public T GetByCode<T>(string code) where T : class, ICodedEntity, new()
		{
			return DataAdapter.FetchByCode<T>(code);
		}

		#endregion

		#region ILookupAdminService Members

		public string GetLocalizedName<T>(long id) where T : class, INamedEntity, new()
		{
			return LocalizeName(DataAdapter.Fetch<T>(id)).Name;
		}

		public IList<INamedEntity> GetLocalizedNameList<T>() where T : class, INamedEntity, new()
		{
			return LocalizeName((IList<T>)DataAdapter.FetchNameList<T>().Cast<T>().ToList()).Cast<INamedEntity>().ToList();
		}

		public T GetByName<T>(long associationId, string name) where T : class, INamedEntity, new()
		{
			T retVal;
			if (typeof(T).GetProperty(AssociationIdProperty) != null)
			{
				var searchInfo = new SearchInfo<T>();
				searchInfo.AddFilter(AssociationIdProperty, associationId).Grouping = -1;
				searchInfo.AddFilter(AssociationIdProperty, ComparisonOperator.Null).Grouping = -1;
				searchInfo.AddFilter(BaseEntity.NameProperty, name);
				retVal = DataAdapter.FetchList(searchInfo).SingleOrDefault();
			}
			else
			{
				retVal = DataAdapter.FetchByName<T>(name);
			}
			return retVal;
		}

		public T GetByCode<T>(long associationId, string code) where T : class, IBaseEntity, new()
		{
			T retVal = null;
			if (typeof(T).GetProperty(BaseEntity.CodeProperty) != null)
			{
				var searchInfo = new SearchInfo<T>();
				searchInfo.AddFilter(BaseEntity.CodeProperty, code);
				if (typeof(T).GetProperty(AssociationIdProperty) != null)
				{
					searchInfo.AddFilter(AssociationIdProperty, associationId).Grouping = -1;
					searchInfo.AddFilter(AssociationIdProperty, ComparisonOperator.Null).Grouping = -1;
				}
				retVal = DataAdapter.FetchList(searchInfo).SingleOrDefault();
			}
			return retVal;
		}

		public IList<T> GetEntityList<T>(long associationId, SearchInfo<T> searchInfo) where T : class, IBaseEntity, new()
		{
			var list = DataAdapter.FetchList(ProcessSearchInfo(associationId, searchInfo));
			if (searchInfo.Localize)
			{
				list = LocalizeRegistered(list);
			}
			return list;
		}

		public IList<T> GetEntityList<T>(SearchInfo<T> searchInfo) where T : class, IBaseEntity, new()
		{
			return GetEntityList(0, searchInfo);
		}

		public IList<T> GetEntityList<T>(long associationId) where T : class, IBaseEntity, new()
		{
			return GetEntityList(associationId, new SearchInfo<T>());
		}

		public IList<T> GetEntityList<T>() where T : class, IBaseEntity, new()
		{
			return GetEntityList<T>(0);
		}

		public long GetEntityCount<T>(long associationId, SearchInfo<T> searchInfo) where T : class, IBaseEntity, new()
		{
			return DataAdapter.FetchCount(ProcessSearchInfo(associationId, searchInfo));
		}

		public long GetEntityCount<T>(SearchInfo<T> searchInfo) where T : class, IBaseEntity, new()
		{
			return GetEntityCount(0, searchInfo);
		}

		public long GetEntityCount<T>(long associationId) where T : class, IBaseEntity, new()
		{
			return GetEntityCount(associationId, new SearchInfo<T>());
		}

		public long GetEntityCount<T>() where T : class, IBaseEntity, new()
		{
			return GetEntityCount<T>(0);
		}

		public T SaveEntity<T>(long associationId, T entity) where T : class, IBaseEntity, new()
		{
			ValidateEntityType<T>();
			if (entity.IsNew)
			{
				var prop = typeof(T).GetProperty(AssociationIdProperty);
				if (prop != null)
				{
					prop.SetValue(entity, associationId, null);
				}
			}
			return SaveWithAudit(entity);
		}

		public T SaveEntity<T>(T entity) where T : class, IBaseEntity, new()
		{
			return SaveEntity(0, entity);
		}

		public void DeleteEntity<T>(long id) where T : class, IBaseEntity, new()
		{
			ValidateEntityType<T>();
			var resourceSearch = new SearchInfo<ResourceEntity> { ApplyDefaultFilters = false };
			resourceSearch.AddFilter(x => x.Type, ResourceManager.GetTypeKey<T>());
			resourceSearch.AddFilter(x => x.Key, id.ToString());
			var resources = DataAdapter.FetchList(resourceSearch);
			foreach (var resource in resources)
			{
				DeleteWithAudit(resource);
			}
			DeleteWithAudit<T>(id);
		}

		#endregion

		#region Private Methods

		private static void ValidateEntityType<T>() where T : class, IBaseEntity, new()
		{
			if (!typeof(INamedEntity).IsAssignableFrom(typeof(T)) && !AcceptedTypes.Contains(typeof(T)))
			{
				throw new ServiceException("{0} is not an accepted type for this method.", typeof(T).Name);
			}
		}

		private static SearchInfo<T> ProcessSearchInfo<T>(long associationId, SearchInfo<T> searchInfo) where T : class, IBaseEntity, new()
		{
			if (searchInfo.ApplyDefaultFilters)
			{
				if (typeof(T).GetProperty(AssociationIdProperty) != null)
				{
					searchInfo.AddFilter(AssociationIdProperty, associationId).Grouping = -1;
					if (searchInfo.MatchNullAssociations)
					{
						searchInfo.AddFilter(AssociationIdProperty, ComparisonOperator.Null).Grouping = -1;
					}
				}
				if (searchInfo.SearchText.HasText())
				{
					if (typeof(INamedEntity).IsAssignableFrom(typeof(T)))
					{
						searchInfo.AddFilter(BaseEntity.NameProperty, ComparisonOperator.Contains, searchInfo.SearchText).Grouping = 1;
					}
					if (typeof(ICodedEntity).IsAssignableFrom(typeof(T)))
					{
						searchInfo.AddFilter(BaseEntity.CodeProperty, ComparisonOperator.Contains, searchInfo.SearchText).Grouping = 2;
					}
				}
				searchInfo.ApplyDefaultFilters = false;
			}
			if (searchInfo.ApplyDefaultSorting)
			{
				if (typeof(T).GetProperty(BaseEntity.DisplayOrderProperty) != null)
				{
					searchInfo.AddSort(BaseEntity.DisplayOrderProperty, ListSortDirection.Ascending);
				}
				if (typeof(INamedEntity).IsAssignableFrom(typeof(T)))
				{
					searchInfo.AddSort(BaseEntity.NameProperty, ListSortDirection.Ascending);
				}
				searchInfo.ApplyDefaultSorting = false;
			}
			return searchInfo;
		}

		#endregion
	}
}
