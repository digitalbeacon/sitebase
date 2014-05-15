// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Collections.Generic;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Model;

namespace DigitalBeacon.SiteBase.Business
{
	public interface IAuditingService
	{
		/// <summary>
		/// Create new audit log entry
		/// </summary>
		/// <param name="action">The action.</param>
		/// <param name="associationId">The association id.</param>
		/// <param name="userId">The user id.</param>
		/// <param name="entity">The entity.</param>
		/// <param name="details">The details.</param>
		void CreateAuditLogEntry(AuditAction action, long? associationId, long? userId, IBaseEntity entity, string details);

		/// <summary>
		/// Create new audit log entry
		/// </summary>
		/// <param name="action">The action.</param>
		/// <param name="associationId">The association id.</param>
		/// <param name="userId">The user id.</param>
		/// <param name="details">The details.</param>
		void CreateAuditLogEntry(AuditAction action, long? associationId, long? userId, string details);

		/// <summary>
		/// Create new audit log entry
		/// </summary>
		/// <param name="action">The action.</param>
		/// <param name="associationId">The association id.</param>
		/// <param name="entity">The entity.</param>
		/// <param name="details">The details.</param>
		void CreateAuditLogEntry(AuditAction action, long? associationId, IBaseEntity entity, string details);

		/// <summary>
		/// Create new audit log entrys
		/// </summary>
		/// <param name="action">The action.</param>
		/// <param name="associationId">The association id.</param>
		/// <param name="details">The details.</param>
		void CreateAuditLogEntry(AuditAction action, long? associationId, string details);

		/// <summary>
		/// Create new audit log entry
		/// </summary>
		/// <param name="action">The action.</param>
		/// <param name="associationId">The association id.</param>
		/// <param name="entity">The entity.</param>
		void CreateAuditLogEntry(AuditAction action, long? associationId, IBaseEntity entity);

		/// <summary>
		/// Create new audit log entry
		/// </summary>
		/// <param name="associationId">The association id.</param>
		/// <param name="entity">The entity.</param>
		void CreateSaveOrUpdateAuditLogEntry(long? associationId, IBaseEntity entity);

		/// <summary>
		/// Create new audit log entry
		/// </summary>
		/// <param name="entry"></param>
		void CreateAuditLogEntry(AuditLogEntity entry);

		/// <summary>
		/// Gets the audit log entry.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns></returns>
		AuditLogEntity GetAuditLogEntry(long id);

		/// <summary>
		/// Gets the audit log entries.
		/// </summary>
		/// <param name="searchInfo">The search info.</param>
		/// <returns></returns>
		IList<AuditLogEntity> GetAuditLogEntries(SearchInfo<AuditLogEntity> searchInfo);

		/// <summary>
		/// Gets the audit log count.
		/// </summary>
		/// <param name="searchInfo">The search info.</param>
		/// <returns></returns>
		long GetAuditLogCount(SearchInfo<AuditLogEntity> searchInfo);
	}
}
