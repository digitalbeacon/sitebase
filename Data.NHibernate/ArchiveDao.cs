// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Collections.Generic;
using DigitalBeacon.Data.NHibernate;
using DigitalBeacon.Model;

namespace DigitalBeacon.Data.NHibernate
{
	/// <summary>
	/// Base class for data access objects for archived entities using NHibernate persistence
	/// </summary>
	public class ArchiveDao<T> : BaseDao<T>, IArchiveDao<T> where T:class, IBaseEntity, IArchivedEntity, new()
	{
		#region static variables

		protected const string WhereClauseByRefId = " where x.RefId = :refId";
		protected const string RefIdParam = "refId";

		#endregion

		#region IArchiveDao<T> Members

		public IList<T> FetchAllByRefId(long refId)
		{
			return HibernateTemplate.FindByNamedParam<T>(FromClause + WhereClauseByRefId, RefIdParam, refId);
		}

		#endregion
	}
}
