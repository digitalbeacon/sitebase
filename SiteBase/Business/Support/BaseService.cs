// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DigitalBeacon.Business;
using DigitalBeacon.Data;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Model;

namespace DigitalBeacon.SiteBase.Business.Support
{
	/// <summary>
	/// Base service class
	/// </summary>
	public abstract class BaseService
	{
		private const int SqlFkErrorNumber = 547;

		#region Private Members

		private readonly IDataAdapter _dataAdapter = ServiceFactory.Instance.GetService<IDataAdapter>();
		private readonly IAuditingService _auditingService = ServiceFactory.Instance.GetService<IAuditingService>();
		private readonly ILocalizationService _localizationService = ServiceFactory.Instance.GetService<ILocalizationService>();

		#endregion

		#region Protected Members

		protected IDataAdapter DataAdapter
		{
			get { return _dataAdapter; }
		}

		protected IAuditingService AuditingService
		{
			get { return _auditingService; }
		}

		protected T SaveWithAudit<T>(T entity) where T : class, IBaseEntity, new()
		{
			var isNew = entity.IsNew;
			var retVal = _dataAdapter.Save(entity);
			if (isNew)
			{
				_auditingService.CreateAuditLogEntry(AuditAction.CreateEntity, 0, retVal);
			}
			else
			{
				_auditingService.CreateAuditLogEntry(AuditAction.UpdateEntity, 0, retVal);
			}
			return retVal;
		}

		protected void DeleteWithAudit<T>(long id) where T : class, IBaseEntity, new()
		{
			var entity = _dataAdapter.Fetch<T>(id);
			if (entity != null)
			{
				DeleteWithAudit(entity);
			}
			else
			{
				throw new ServiceException("Could not find {0} with Id [{1}] for deletion.", typeof(T).FullName, id);
			}
		}

		protected void DeleteWithAudit<T>(T entity) where T : class, IBaseEntity, new()
		{
			try
			{
				_auditingService.CreateAuditLogEntry(AuditAction.DeleteEntity, 0, entity);
				_dataAdapter.Delete(entity);
			}
			catch (Exception ex)
			{
				if (ex.InnerException != null && ex.InnerException is SqlException && (ex.InnerException as SqlException).Number == SqlFkErrorNumber)
				{
					throw new EntityDependencyException(entity);
				}
			}
		}

		protected static string GetPropertyName(string property, string subProperty)
		{
			return String.Format("{0}.{1}", property, subProperty);
		}

		protected T Localize<T>(T entity, string defaultProperty) where T : class, IBaseEntity
		{
			if (entity != null && ResourceManager.Instance.UseDatabaseResources)
			{
				entity = _localizationService.Localize(entity, defaultProperty);
			}
			return entity;
		}

		protected IList<T> Localize<T>(IList<T> list, string defaultProperty) where T : class, IBaseEntity
		{
			if (list != null && ResourceManager.Instance.UseDatabaseResources)
			{
				list = _localizationService.Localize(list, defaultProperty);
			}
			return list;
		}

		protected T LocalizeRegistered<T>(T entity) where T : class, IBaseEntity
		{
			if (entity != null && ResourceManager.Instance.UseDatabaseResources)
			{
				entity = _localizationService.LocalizeRegistered(entity);
			}
			return entity;
		}

		protected IList<T> LocalizeRegistered<T>(IList<T> list) where T : class, IBaseEntity
		{
			if (list != null && ResourceManager.Instance.UseDatabaseResources)
			{
				list = _localizationService.LocalizeRegistered(list);
			}
			return list;
		}

		protected T LocalizeName<T>(T entity) where T : class, IBaseEntity
		{
			if (entity != null && ResourceManager.Instance.UseDatabaseResources)
			{
				entity = _localizationService.LocalizeName(entity);
			}
			return entity;
		}

		protected IList<T> LocalizeName<T>(IList<T> list) where T : class, IBaseEntity
		{
			if (list != null && ResourceManager.Instance.UseDatabaseResources)
			{
				list = _localizationService.LocalizeName(list);
			}
			return list;
		}

		protected T LocalizeProperty<T>(T entity, string propertyName) where T : class, IBaseEntity
		{
			if (entity != null && ResourceManager.Instance.UseDatabaseResources)
			{
				entity = _localizationService.LocalizeProperty(entity, propertyName);
			}
			return entity;
		}

		protected IList<T> LocalizeProperty<T>(IList<T> list, string propertyName) where T : class, IBaseEntity
		{
			if (list != null && ResourceManager.Instance.UseDatabaseResources)
			{
				list = _localizationService.LocalizeProperty(list, propertyName);
			}
			return list;
		}


		#endregion
	}
}
