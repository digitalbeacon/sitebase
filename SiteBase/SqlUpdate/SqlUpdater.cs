// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using DigitalBeacon.SqlUpdate;
using Common.Logging;

namespace DigitalBeacon.SiteBase.SqlUpdate
{
	public class SqlUpdater : DigitalBeacon.SqlUpdate.SqlUpdater
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private const string Module = "SiteBase";

		public SqlUpdater()
		{
			ModuleName = Module;
			ResourceAssembly = GetType().Assembly;
		}

		static void Main(string[] args)
		{
			try
			{
				SqlUpdater updater = new SqlUpdater();
				updater.Run();
			}
			catch (Exception ex)
			{
				_log.Error(ex.Message);
				_log.Error(ex.StackTrace);
				if (ex.InnerException != null)
				{
					_log.Error(ex.InnerException.Message);
					_log.Error(ex.InnerException.StackTrace);
				}
			}
		}
	}
}
