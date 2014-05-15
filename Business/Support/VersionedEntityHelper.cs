// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Common.Logging;
using DigitalBeacon.Data;
using DigitalBeacon.Model;

namespace DigitalBeacon.Business.Support
{
	/// <summary>
	/// 
	/// </summary>
	public class VersionedEntityHelper
	{
		#region private variables

		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly IDataAdapter _dataAdapter = ServiceFactory.Instance.GetService<IDataAdapter>();
		private static readonly Hashtable _propertyCache = new Hashtable();
		private static readonly object[] EmptyObjectArray = new object[] {};
		#endregion

		public T Fetch<T>(long id)
			where T : class, IVersionedEntity, new()
		{
			T retVal = null;
			T entity = _dataAdapter.Fetch<T>(id);
			if (retVal != null && retVal.Deleted != null)
			{
				retVal = entity;
			}
			return retVal;
		}

		/// <summary>
		/// This method implements a versioned save which updates the entity
		/// and saves the previous version in an archive table.
		/// </summary>
		/// <param name="entity"></param>
		public T Save<T, K>(T entity)
			where T : class, IVersionedEntity, new()
			where K : class, IArchivedEntity, new()
		{
			if (entity.Id > 0)
			{
				// fetch previous entity
				T previous = _dataAdapter.Fetch<T>(entity.Id);
				// create archived entity
				K archived = new K();
				archived.Archived = DateTime.Now;
				archived.RefId = previous.Id;
				archived.Created = previous.Created;
				MemberwiseCopy(previous, archived);
				_dataAdapter.Save(archived);
				//_dataAdapter.Evict<T>(previous);
				//_dataAdapter.Evict<K>(archived);
			}
			// save new changes
			return _dataAdapter.Save(entity);
		}

		/// <summary>
		/// Soft delete implementation for versioned entities
		/// </summary>
		/// <param name="entity"></param>
		public void Delete<T, K>(long id)
			where T : class, IVersionedEntity, new()
			where K : class, IArchivedEntity, new()
		{
			T entity = _dataAdapter.Fetch<T>(id);
			if (entity != null)
			{
				entity.Deleted = DateTime.Now;
				//Save<T, K>(entity);
			}
		}

		/// <summary>
		/// Purge a versioned entity
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="K"></typeparam>
		/// <param name="entity"></param>
		public void Purge<T, K>(T entity)
			where T : class, IVersionedEntity, new()
			where K : class, IArchivedEntity, new()
		{
			IList<K> archivedList = _dataAdapter.FetchAllByRefId<K>(entity.Id);
			foreach (K archived in archivedList)
			{
				_dataAdapter.Delete(archived);
			}
			_dataAdapter.Delete(entity);
		}

		/// <summary>
		/// Helper method to copy properties from a versioned entity to
		/// an archived entity.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="dest"></param>
		private static void MemberwiseCopy<T, K>(T source, K dest)
			where T : class, IVersionedEntity, new()
			where K : class, IArchivedEntity, new()
		{
			foreach (PropertyInfo sourceProp in GetProperties(typeof(T)))
			{
				if (sourceProp.Name != "Id"
					&& sourceProp.Name != "ModificationCounter"
					&& sourceProp.Name != "Deleted")
				{
					foreach (PropertyInfo destProp in GetProperties(typeof(K)))
					{
						if (destProp.Name == sourceProp.Name
							&& destProp.PropertyType == sourceProp.PropertyType
							&& destProp.CanWrite)
						{
							destProp.SetValue(dest, sourceProp.GetValue(source, EmptyObjectArray), EmptyObjectArray);
							break;
						}
					}
				}
				//if (sourceProp.PropertyType.IsSubclassOf(typeof(BaseEntity)))
				//{
				//	_dataAdapter.Evict<T>(sourceProp.GetValue(source, EmptyObjectArray));
				//}
			}
		}

		/// <summary>
		/// Returns an array of properties supported for the given type
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		private static PropertyInfo[] GetProperties(Type type)
		{
			lock (_propertyCache)
			{
				PropertyInfo[] properties = (PropertyInfo[])_propertyCache[type];
				if (properties == null)
				{
					ArrayList propertyList = new ArrayList();
					CollectPropertiesRecursive(type, propertyList);
					properties = (PropertyInfo[])propertyList.ToArray(typeof(PropertyInfo));
					_propertyCache[type] = properties;
				}
				return properties;
			}
		}

		/// <summary>
		/// Builds a list of properties for a give type including properties
		/// defined in all super-classes.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="propertyList"></param>
		private static void CollectPropertiesRecursive(Type type, ArrayList propertyList)
		{
			if (type != typeof(object))
			{
				PropertyInfo[] properties = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
				propertyList.AddRange(properties);
				CollectPropertiesRecursive(type.BaseType, propertyList);
			}
		}
	}
}