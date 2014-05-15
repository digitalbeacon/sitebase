// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Reflection;
using Common.Logging;
using DigitalBeacon.Model;
using Spring.Context;
using Spring.Context.Support;

namespace DigitalBeacon.Data
{
	/// <summary>
	/// A factory class to get a DAL instance
	/// </summary>
	public class DataAdapterFactory
	{
		#region private variables
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private static readonly IApplicationContext _ctx = ContextRegistry.GetContext();
		private static IDataAdapter _dataAdapter;
		#endregion

		static DataAdapterFactory()
		{
			_log.Debug("DataAdapterFactory static constructor");
		}

		/// <summary>
		/// Constructor non-public 
		/// </summary>
		protected DataAdapterFactory() { }

		/// <summary>
		/// Accessor for singleton instance
		/// </summary>
		public static IDataAdapter DataAdapter
		{
			get
			{
				if (_dataAdapter == null)
				{
					_dataAdapter = _ctx.GetObject(typeof(IDataAdapter).Name) as IDataAdapter;
					if (_dataAdapter == null)
					{
						throw new Exception("Could not locate an instance of IDataAdapter");
					}
				}
				return _dataAdapter;
			}
		}
   }
}