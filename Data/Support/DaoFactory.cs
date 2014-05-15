// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections;
using DigitalBeacon.Model;
using Spring.Context;
using Spring.Context.Support;

namespace DigitalBeacon.Data.Support
{
	/// <summary>
	/// A factory class to get DAO instances
	/// </summary>
	public class DaoFactory : IDaoFactory
	{
		#region private variables
		private static readonly IApplicationContext _ctx = ContextRegistry.GetContext();
		private const string DaoSuffix = "Dao";
		private static DaoFactory _instance;
		private static readonly Hashtable _daoCache = new Hashtable();
		#endregion

		/// <summary>
		/// Constructor non-public 
		/// </summary>
		protected DaoFactory() {}

		/// <summary>
		/// Accessor for singleton instance
		/// </summary>
		public static DaoFactory Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new DaoFactory();
				}
				return _instance;
			}
		}

		#region IDaoFactory Members

		public IDao<T> GetDao<T>() where T:class, IBaseEntity, new()
		{
			IDao<T> retVal;
			if (_daoCache.ContainsKey(typeof(T)))
			{
				retVal = _daoCache[typeof(T)] as IDao<T>;
			}
			else
			{
				retVal = _ctx.GetObject(typeof(T).Name + DaoSuffix) as IDao<T>;
				if (retVal != null)
				{
					_daoCache[typeof(T)] = retVal;
				}
			}
			if (retVal == null)
			{
				throw new Exception("Could not find requested IDao: " + typeof(T).FullName);
			}
			return retVal;
		}

		public INameDao<T> GetNameDao<T>() where T : class, INamedEntity, new()
		{
			INameDao<T> retVal = GetDao<T>() as INameDao<T>;
			if (retVal == null)
			{
				throw new Exception("Could not find requested INameDao: " + typeof(T).FullName);
			}
			return retVal;
		}

		public IArchiveDao<T> GetArchiveDao<T>() where T : class, IArchivedEntity, new()
		{
			IArchiveDao<T> retVal = GetDao<T>() as IArchiveDao<T>;
			if (retVal == null)
			{
				throw new Exception("Could not find requested IArchiveDao: " + typeof(T).FullName);
			}
			return retVal;
		}

		public K GetDao<T, K>()
			where T : class, IBaseEntity, new()
			where K : class
		{
			K retVal = default(K);
			IDao<T> dao = GetDao<T>();
			if (dao is K)
			{
				retVal = dao as K;
			}
			else
			{
				throw new Exception(String.Format("Could not find requested IDao: {0} that implements interface: {1}", typeof(T).FullName, typeof(K).FullName));
			}
			return retVal;
		}

		#endregion
	}
}