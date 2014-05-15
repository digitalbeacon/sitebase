// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;
using System.Configuration;
using Common.Logging;
using DigitalBeacon.Util;
using Microsoft.SqlServer.Management.Common;

namespace DigitalBeacon.SqlUpdate
{
	public class SqlUpdater
	{
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public const string ConnectionKey = "SqlUpdateConnection";
		public const string TargetVersionKey = "SqlUpdateTargetVersion";
		public const string ClassKey = "SqlUpdateClass";
		public const string BaseDirectoryKey = "SqlUpdateBaseDirectory";
		public const string DefaultTargetVersion = "100.0";

		private static readonly Regex VersionRegex = new Regex(@"^.*((\d)+\.(\d)+(\.(\d)+){0,2})$", RegexOptions.Compiled);
		private static readonly Regex EmbeddedFileRegex = new Regex(@"^([.a-zA-Z]*\.(_\d+\._\d+(\._\d+){0,2}))\.(\d{3}\.sql)$", RegexOptions.Compiled);

		private readonly string _connectionString;
		private readonly string _baseDir;
		private readonly string _targetVersion;
		private string _moduleName = String.Empty;
		private Assembly _resourceAssembly;
		private ServerConnection _serverConnection;

		#region Constructors

		public SqlUpdater()
		{
			if (ConfigurationManager.ConnectionStrings.Count == 0)
			{
				throw new Exception("No connection strings defined in the application configuration file.");
			}
			var sqlUpdateConnection = ConfigurationManager.AppSettings[ConnectionKey];
			if (String.IsNullOrEmpty(sqlUpdateConnection))
			{
				throw new BaseException("{0} must be specified in the app settings section of the application configuration file.", ConnectionKey);
			}
			if (ConfigurationManager.ConnectionStrings[sqlUpdateConnection] == null)
			{
				throw new BaseException("ConnectionString with name [{0}] could not be found in the application configuration file.", sqlUpdateConnection);
			}
			_connectionString = ConfigurationManager.ConnectionStrings[sqlUpdateConnection].ConnectionString;
			_targetVersion = ConfigurationManager.AppSettings[TargetVersionKey] ?? DefaultTargetVersion;
			var sqlUpdateClass = ConfigurationManager.AppSettings[ClassKey];
			if (!String.IsNullOrEmpty(sqlUpdateClass))
			{
				_resourceAssembly = Assembly.GetAssembly(Type.GetType(sqlUpdateClass, true));
			}
			if (_resourceAssembly == null)
			{
				_baseDir = ConfigurationManager.AppSettings[BaseDirectoryKey];
			}
		}

		public SqlUpdater(string connectionString, string targetVersion, string baseDirectory, string moduleName)
		{
			if (String.IsNullOrEmpty(connectionString))
			{
				throw new Exception("The connectionString parameter is required.");
			}
			_connectionString = connectionString;
			if (String.IsNullOrEmpty(targetVersion))
			{
				throw new Exception("The targetVersion parameter is required.");
			}
			_targetVersion = targetVersion;
			if (String.IsNullOrEmpty(baseDirectory))
			{
				throw new Exception("The baseDirectory parameter is required.");
			}
			_baseDir = baseDirectory;
			_moduleName = moduleName;
		}

		public SqlUpdater(string connectionString, string targetVersion, Assembly resourceAssembly, string moduleName)
		{
			if (String.IsNullOrEmpty(connectionString))
			{
				throw new Exception("The connectionString parameter is required.");
			}
			_connectionString = connectionString;
			if (String.IsNullOrEmpty(targetVersion))
			{
				throw new Exception("The targetVersion parameter is required.");
			}
			_targetVersion = targetVersion;
			if (resourceAssembly == null)
			{
				throw new Exception("The resourceAssembly parameter cannot be null.");
			}
			_resourceAssembly = resourceAssembly;
			_moduleName = moduleName;
		}

		#endregion

		#region Public Methods

		protected string ModuleName
		{
			get { return _moduleName; }
			set { _moduleName = value; }
		}

		protected Assembly ResourceAssembly
		{
			get { return _resourceAssembly; }
			set { _resourceAssembly = value; }
		}

		public void Run()
		{
			if (_resourceAssembly == null && _baseDir.IsNullOrBlank())
			{
				throw new BaseException("{0} or {1} must be specified in the app settings section of the application configuration file.", ClassKey, BaseDirectoryKey);
			}
			if (_resourceAssembly != null)
			{
				Log.Info("SqlUpdater resource assembly: " + _resourceAssembly.GetName().Name);
			}
			else
			{
				Log.Info("SqlUpdater base directory: " + _baseDir);
			}
			if (!VersionRegex.IsMatch(_targetVersion))
			{
				throw new BaseException("Requested target version [{0}] is not valid.", _targetVersion);
			}
			Log.Info("Target version: " + _targetVersion);
			using (var conn = new SqlConnection(_connectionString))
			{
				conn.Open();
				_serverConnection = new ServerConnection(conn);
				_serverConnection.BeginTransaction();
				try
				{
					GenerateDataUpdateObjects();
					var latestVersion = GetLatestUpdateVersion();
					Log.Info("Last patch version: " + latestVersion);
					var patchNumber = GetLatestPatchNumber(latestVersion);
					Log.Info(String.Format("Last patch number for version {0}: {1}", latestVersion, patchNumber));
					var versionMap = ParseBaseDirectory(latestVersion, new Version(_targetVersion));
					var versionList = new List<Version>(versionMap.Keys);
					versionList.Sort();
					foreach (var v in versionList)
					{
						var patches = ParsePatchDirectory(v, versionMap[v]);
						foreach (var patch in patches)
						{
							ExcecutePatch(patch);
						}
					}
					_serverConnection.CommitTransaction();
				}
				catch (Exception)
				{
					_serverConnection.RollBackTransaction();
					throw;
				}
				conn.Close();
			}
		}

		#endregion

		#region Private Methods

		private void GenerateDataUpdateObjects()
		{
			if ((int)_serverConnection.ExecuteScalar(Resources.CheckForDataUpdateObjectsSql) == 0)
			{
				_serverConnection.ExecuteNonQuery(Resources.CreateDataUpdateObjectsSql);
				Log.Info("Created Data Update Sql Objects");
			}
		}

		private Version GetLatestUpdateVersion()
		{
			var retVal = new Version(0, 0);
			var list = new List<Version>();
			using (var cmd = _serverConnection.SqlConnectionObject.CreateCommand())
			{
				cmd.CommandText = Resources.QueryVersionSql;
				cmd.Parameters.Add(new SqlParameter("@module", _moduleName));
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						list.Add(new Version(reader[0].ToString()));
					}
				}
			}
			if (list.Count > 1)
			{
				list.Sort();
			}
			if (list.Count > 0)
			{
				retVal = list[list.Count - 1];
			}
			return retVal;
		}

		private int GetLatestPatchNumber(Version version)
		{
			int retVal;
			using (var cmd = _serverConnection.SqlConnectionObject.CreateCommand())
			{
				cmd.CommandText = Resources.QueryPatchNumberSql;
				cmd.Parameters.Add(new SqlParameter("@version", version.ToString()));
				cmd.Parameters.Add(new SqlParameter("@module", _moduleName));
				retVal = cmd.ExecuteScalar() as int? ?? 0;
			}
			return retVal;
		}

		private IDictionary<Version, string> ParseBaseDirectory(Version minVersion, Version maxVersion)
		{
			var retVal = new Dictionary<Version, string>();
			if (_resourceAssembly != null)
			{
				var resourceNames = _resourceAssembly.GetManifestResourceNames();
				//var assemblyName = _resourceAssembly.GetName().Name;
				foreach (var resourceName in resourceNames)
				{
					if (EmbeddedFileRegex.IsMatch(resourceName))
					{
						var versionStr = EmbeddedFileRegex.Replace(resourceName, "$2").Replace("_", String.Empty);
						var version = new Version(versionStr);
						if (version >= minVersion && version <= maxVersion)
						{
							retVal[version] = EmbeddedFileRegex.Replace(resourceName, "$1");
						}
					}
				}
			}
			else
			{
				if (!Directory.Exists(_baseDir))
				{
					throw new BaseException("The configured base directory for patch files [{0}] does not exist.", _baseDir);
				}
				var subdirectories = Directory.GetDirectories(_baseDir);
				foreach (var directory in subdirectories)
				{
					if (VersionRegex.IsMatch(directory))
					{
						var versionStr = VersionRegex.Replace(directory, "$1");
						var version = new Version(versionStr);
						if (version >= minVersion && version <= maxVersion)
						{
							retVal[version] = directory;
						}
					}
				}
			}
			return retVal;
		}

		private IList<PatchInfo> ParsePatchDirectory(Version version, string dirPath)
		{
			var retVal = new List<PatchInfo>();
			var patchNumber = GetLatestPatchNumber(version) + 1;
			if (_resourceAssembly != null)
			{
				var resourceNames = new List<string>(_resourceAssembly.GetManifestResourceNames());
				var patchFilename = String.Format("{0}.{1:000}.sql", dirPath, patchNumber);
				while (resourceNames.Contains(patchFilename))
				{
					Log.Debug(String.Format("Found patch file [{0}] for version [{1}].", patchFilename, version));
					retVal.Add(new PatchInfo(patchFilename, version, patchNumber));
					patchNumber++;
					patchFilename = String.Format("{0}.{1:000}.sql", dirPath, patchNumber);
				}
			}
			else
			{
				var patchFilename = Path.Combine(dirPath, String.Format("{0:000}.sql", patchNumber));
				while (File.Exists(patchFilename))
				{
					Log.Debug(String.Format("Found patch file [{0}] for version [{1}].", patchFilename, version));
					retVal.Add(new PatchInfo(patchFilename, version, patchNumber));
					patchNumber++;
					patchFilename = Path.Combine(dirPath, String.Format("{0:000}.sql", patchNumber));
				}
			}
			return retVal;
		}

		private void ExcecutePatch(PatchInfo patch)
		{
			string script;
			if (_resourceAssembly != null)
			{
				script = new StreamReader(_resourceAssembly.GetManifestResourceStream(patch.Filename)).ReadToEnd();
			}
			else
			{
				script = File.OpenText(patch.Filename).ReadToEnd();
			}
			Log.Info(String.Format("Executing [{0}]...", patch.Filename));
			Log.Debug(script);
			_serverConnection.ExecuteNonQuery(script);
			using (var cmd = _serverConnection.SqlConnectionObject.CreateCommand())
			{
				cmd.CommandText = Resources.InsertPatchSql;
				cmd.Parameters.Add(new SqlParameter("@version", patch.Version.ToString()));
				cmd.Parameters.Add(new SqlParameter("@patchNumber", patch.PatchNumber));
				cmd.Parameters.Add(new SqlParameter("@module", _moduleName));
				cmd.ExecuteScalar();
			}
		}

		#endregion

		#region Inner Classes

		private class PatchInfo
		{
			public PatchInfo(string filename, Version version, int patchNumber)
			{
				Filename = filename;
				Version = version;
				PatchNumber = patchNumber;
			}

			public string Filename { get; private set; }
			public Version Version { get; private set; }
			public int PatchNumber { get; private set; }
		}

		#endregion
	}
}
