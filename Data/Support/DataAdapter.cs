// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Reflection;
using System.Collections.Generic;
using Common.Logging;
using DigitalBeacon.Model;
using System.ComponentModel;

namespace DigitalBeacon.Data.Support
{
	/// <summary>
	/// a base class for data access objects using NHibernate persistence
	/// </summary>
	public class DataAdapter : IDataAdapter
	{
		#region private variables
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		#endregion

		#region IDal Members

		public T Fetch<T>(long id) where T:class, IBaseEntity, new()
		{
			IDao<T> dao = DaoFactory.Instance.GetDao<T>();
			return dao.Fetch(id);
		}

		#region IDataAdapter Members

		public T FetchWithLazyCollections<T>(long id) where T : class, IBaseEntity, new()
		{
			IDao<T> dao = DaoFactory.Instance.GetDao<T>();
			return dao.FetchWithLazyCollections(id);
		}

		#endregion

//		public T Fetch<T>(long id, long version) where T : class, IBaseEntity, new()
//		{
//			IDao<T> dao = DaoFactory.Instance.GetDao<T>();
//			return dao.Fetch(id);
//		}

		public T Fetch<T>(string propertyName, object value) where T : class, IBaseEntity, new()
		{
			IDao<T> dao = DaoFactory.Instance.GetDao<T>();
			return dao.Fetch(propertyName, value);
		}

		public IList<T> FetchAll<T>() where T:class, IBaseEntity, new()
		{
			IDao<T> dao = DaoFactory.Instance.GetDao<T>();
			return dao.FetchAll();
		}

		public IList<T> FetchAll<T>(string sortByPropertyName) where T : class, IBaseEntity, new()
		{
			return FetchAll<T>(sortByPropertyName, ListSortDirection.Ascending);
		}

		public IList<T> FetchAll<T>(string sortByPropertyName, ListSortDirection sortDirection) where T : class, IBaseEntity, new()
		{
			IDao<T> dao = DaoFactory.Instance.GetDao<T>();
			return dao.FetchAll(sortByPropertyName, sortDirection);
		}

		public IList<T> FetchList<T>(string propertyName, object value) where T : class, IBaseEntity, new()
		{
			IDao<T> dao = DaoFactory.Instance.GetDao<T>();
			return dao.FetchList(propertyName, value);
		}

		public IList<T> FetchList<T>(string propertyName, object value, string sortByPropertyName, bool asc) where T : class, IBaseEntity, new()
		{
			IDao<T> dao = DaoFactory.Instance.GetDao<T>();
			return dao.FetchList(propertyName, value, sortByPropertyName, asc);
		}

		public IList<T> FetchList<T>(SearchInfo<T> searchInfo) where T : class, IBaseEntity, new()
		{
			IDao<T> dao = DaoFactory.Instance.GetDao<T>();
			return dao.FetchList(searchInfo);
		}

		public long FetchCount<T>(SearchInfo<T> searchInfo) where T : class, IBaseEntity, new()
		{
			IDao<T> dao = DaoFactory.Instance.GetDao<T>();
			return dao.FetchCount(searchInfo);
		}

		public T Save<T>(T entity) where T : class, IBaseEntity, new()
		{
			IDao<T> dao = DaoFactory.Instance.GetDao<T>();
			return dao.Save(entity);
		}

		public void Delete<T>(long id, long version) where T:class, IBaseEntity, new()
		{
			IDao<T> dao = DaoFactory.Instance.GetDao<T>();
			dao.Delete(id, version);
		}

		public void Delete<T>(long id) where T:class, IBaseEntity, new()
		{
			IDao<T> dao = DaoFactory.Instance.GetDao<T>();
			dao.Delete(id);
		}

		public void Delete<T>(T entity) where T:class, IBaseEntity, new()
		{
			IDao<T> dao = DaoFactory.Instance.GetDao<T>();
			dao.Delete(entity);
		}

//		public void InitializeLazyCollections<T>(T entity) where T:class, IBaseEntity, new()
//		{
//			IDao<T> dao = DaoFactory.Instance.GetDao<T>();
//		}

		public IList<INamedEntity> FetchNameList<T>() where T : class, INamedEntity, new()
		{
			INameDao<T> dao = DaoFactory.Instance.GetNameDao<T>();
			return dao.FetchNameList();
		}

		public T FetchByName<T>(string name) where T : class, INamedEntity, new()
		{
			INameDao<T> dao = DaoFactory.Instance.GetNameDao<T>();
			return dao.FetchByName(name);
		}

		public T FetchByCode<T>(string code) where T : class, ICodedEntity, new()
		{
			ICodeDao<T> dao = DaoFactory.Instance.GetDao<T, ICodeDao<T>>();
			return dao.FetchByCode(code);
		}

		public IList<T> FetchAllByRefId<T>(long refId) where T : class, IArchivedEntity, new()
		{
			IArchiveDao<T> dao = DaoFactory.Instance.GetArchiveDao<T>();
			return dao.FetchAllByRefId(refId);
		}

		//public void Initialize<T>(T entity, object member) where T:class, IBaseEntity, new()
		//{
		//	IDao<T> dao = DaoFactory.Instance.GetDao<T>();
		//	dao.Initialize(entity, member);
		//}

		public void Evict<T>(object entity) where T : class, IBaseEntity, new()
		{
			IDao<T> dao = DaoFactory.Instance.GetDao<T>();
			dao.Evict(entity);
		}

		#endregion
	}
}