// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections;
using System.Reflection;
using Common.Logging;
using Spring.Context;
using Spring.Context.Support;

namespace DigitalBeacon.Business
{
	/// <summary>
	/// A factory class to get service instances
	/// </summary>
	public class ServiceFactory
	{
		#region private variables
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private static readonly IApplicationContext _ctx = ContextRegistry.GetContext();
		private static ServiceFactory _instance;
		private static readonly Hashtable _serviceCache = new Hashtable();
		#endregion

		/// <summary>
		/// Constructor non-public 
		/// </summary>
		protected ServiceFactory() {}

		/// <summary>
		/// Accessor for singleton instance
		/// </summary>
		public static ServiceFactory Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new ServiceFactory();
				}
				return _instance;
			}
		}

		/// <summary>
		/// This method will attempt to locate a service with the name
		/// of the specified type. It will return null if the service
		/// can not be located.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T QueryService<T>() where T : class
		{
			T retVal;
			if (_serviceCache.ContainsKey(typeof(T)))
			{
				retVal = _ctx.GetObject(typeof(T).Name) as T;
			}
			else
			{
				retVal = _ctx.GetObject(typeof(T).Name) as T;
				if (retVal != null)
				{
					_serviceCache[typeof(T)] = retVal;
				}
			}
			return retVal;
		}


		/// <summary>
		/// Returns a service with the name of the specified type. This
		/// method throws and exception if the requested service can not
		/// be located.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T GetService<T>() where T:class
		{
			T retVal = QueryService<T>();
			if (retVal == null)
			{
				throw new Exception("Could not find requested service: " + typeof(T).FullName);
			}
			return retVal;
		}

		/// <summary>
		/// Gets the managed object.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		public T GetManagedObject<T>(string key) where T : class
		{
			return _ctx.GetObject(key) as T;
		}
	}
}