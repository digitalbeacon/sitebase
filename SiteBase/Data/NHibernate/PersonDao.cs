// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using NHibernate;
using NHibernate.Criterion;
using Spring.Collections;
using DigitalBeacon.Data.NHibernate;
using DigitalBeacon.SiteBase.Model;

namespace DigitalBeacon.SiteBase.Data.NHibernate
{
	public class PersonDao : BaseDao<PersonEntity>, IPersonDao
	{
		#region IPersonDao Members

		public IList<PersonEntity> Find(PredicateGroupEntity criteria)
		{
			return HibernateTemplate.ExecuteFind<PersonEntity>(
				delegate(ISession session)
				{
					ICriteria c = session.CreateCriteria(typeof(PersonEntity));
					PredicateDao.AddPredicates(criteria.Predicates, c);
					return c.List<PersonEntity>();
				});
		}

		#endregion
	}
}