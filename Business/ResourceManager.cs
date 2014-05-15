// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using DigitalBeacon.Model;
using DigitalBeacon.Util;
using Spring.Context;
using System.Globalization;
using Spring.Threading;

namespace DigitalBeacon.Business
{
	/// <summary>
	/// A manager class to get resources
	/// </summary>
	public class ResourceManager
	{
		public const string ClientCultureKey = "ResourceManager.ClientCulture";

		#region private variables

		private static readonly Regex KeyRegex = new Regex(@"^\w*(\.\w*)+$", RegexOptions.Compiled);
		private static ResourceManager _instance;
		private IMessageSource _messageSource;
		private static readonly CultureInfo _systemCulture;
		private readonly IDictionary<string, LocalizedEntity> _localizedEntityTypes = new Dictionary<string, LocalizedEntity>();

		#endregion

		static ResourceManager()
		{
			_systemCulture = Thread.CurrentThread.CurrentUICulture;
		}

		/// <summary>
		/// Constructor non-public 
		/// </summary>
		protected ResourceManager() { }

		/// <summary>
		/// Accessor for singleton instance
		/// </summary>
		public static ResourceManager Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new ResourceManager();
				}
				return _instance;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the database should be checked for resource lookups.
		/// </summary>
		/// <value><c>true</c> if the database should be checked for resource lookups; otherwise, <c>false</c>.</value>
		public bool UseDatabaseResources { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to fallback to the system culture.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if to fallback to the system culture; otherwise, <c>false</c>.
		/// </value>
		public bool FallbackToSystemCulture { get; set; }

		/// <summary>
		/// Clears the resource cache.
		/// </summary>
		public void ClearCache()
		{
			_messageSource = null;
		}

		/// <summary>
		/// Gets the localized string for the default culture.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="args">The args.</param>
		/// <returns></returns>
		public string GetString(string key, params object[] args)
		{
			return GetString(ClientCulture, key, args);
		}

		/// <summary>
		/// Gets the localized string for the specified culture.
		/// </summary>
		/// <param name="culture">The culture.</param>
		/// <param name="key">The key.</param>
		/// <param name="args">The args.</param>
		/// <returns></returns>
		public string GetString(CultureInfo culture, string key, params object[] args)
		{
			if (KeyRegex.IsMatch(key))
			{
				if (_messageSource == null)
				{
					_messageSource = ServiceFactory.Instance.GetService<IMessageSource>();
				}
				return _messageSource.GetMessage(key, culture).FormatWith(culture, args);
			}
			return key.FormatWith(culture, args);
		}

		/// <summary>
		/// Gets or sets the current culture.
		/// </summary>
		/// <value>The current culture.</value>
		public static CultureInfo ClientCulture
		{
			get { return LogicalThreadContext.GetData(ClientCultureKey) as CultureInfo ?? CultureInfo.CurrentUICulture; }
			set { LogicalThreadContext.SetData(ClientCultureKey, value); }
		}

		/// <summary>
		/// Gets or sets the system culture.
		/// </summary>
		/// <value>The system culture.</value>
		public static CultureInfo SystemCulture
		{
			get { return _systemCulture; }
		}

		/// <summary>
		/// Gets the localized entity types.
		/// </summary>
		/// <value>The localized entity types.</value>
		public IEnumerable<LocalizedEntity> LocalizedEntityTypes
		{
			get { return _localizedEntityTypes.Values; }
		}

		/// <summary>
		/// Determines whether given entity type has been registered to be localized.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns>
		/// 	<c>true</c> if given entity type has been registered to be localized; otherwise, <c>false</c>.
		/// </returns>
		public bool IsEntityLocalized<T>() where T : class, IBaseEntity
		{
			return _localizedEntityTypes.ContainsKey(GetTypeKey<T>());
		}

		/// <summary>
		/// Gets the localized property name for the given localizable entity type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public string GetLocalizedProperty<T>() where T : class, IBaseEntity
		{
			if (_localizedEntityTypes.ContainsKey(GetTypeKey<T>()))
			{
				return _localizedEntityTypes[GetTypeKey<T>()].LocalizedProperty;
			}
			return null;
		}

		/// <summary>
		/// Gets the type key for the specified type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static string GetTypeKey<T>() where T : IBaseEntity
		{
			return GetTypeKey(typeof(T));
		}

		/// <summary>
		/// Gets the type key for the specified type.
		/// </summary>
		/// <returns></returns>
		public static string GetTypeKey(Type type)
		{
			return type.Name.Replace("Entity", String.Empty);
		}

		/// <summary>
		/// Registers the localized entity.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		public static void RegisterLocalizedEntity<T>() where T : class, INamedEntity
		{
			Instance._localizedEntityTypes[GetTypeKey<T>()] = LocalizedEntity.Create<T>();
		}

		/// <summary>
		/// Registers the localized entity.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="property">The property.</param>
		public static void RegisterLocalizedEntity<T>(string property) where T : class, IBaseEntity
		{
			Instance._localizedEntityTypes[GetTypeKey<T>()] = LocalizedEntity.Create<T>(property);
		}
	}
}