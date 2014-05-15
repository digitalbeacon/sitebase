// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using NHibernate;
using DigitalBeacon.Data.NHibernate;
using DigitalBeacon.SiteBase.Model;

namespace DigitalBeacon.SiteBase.Data.NHibernate
{
	public class AuditLogDao : BaseDao<AuditLogEntity>, IAuditLogDao
	{
		#region IAuditLogDao Members

		public void DeleteEntries(long userId)
		{
			Delete(GetIdProperty(AuditLogEntity.UserProperty), userId, NHibernateUtil.Int64);
		}

		#endregion
	}
}