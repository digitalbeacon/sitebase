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
	public class PredicateGroupDao : BaseDao<PredicateGroupEntity>, IPredicateGroupDao
	{
		#region IPredicateGroupDao Members

		public IList<PredicateGroupEntity> Fetch(EntityType entityType, long associationId, long? userId)
		{
			return HibernateTemplate.ExecuteFind<PredicateGroupEntity>(
				delegate(ISession session)
				{
					ICriteria c = session.CreateCriteria(typeof(PredicateGroupEntity))
						.Add(Expression.Eq(PredicateGroupEntity.TypeProperty, entityType))
						.Add(Expression.Or(
							Expression.IsNull(PredicateGroupEntity.AssociationIdProperty),
							Expression.Eq(PredicateGroupEntity.AssociationIdProperty, associationId)));
					if (userId.HasValue)
					{
						c.Add(Expression.Or(
							Expression.IsNull(PredicateGroupEntity.UserIdProperty),
							Expression.Eq(PredicateGroupEntity.UserIdProperty, userId.Value)));
					}
					else
					{
						c.Add(Expression.IsNull(PredicateGroupEntity.UserIdProperty));
					}
					return c.AddOrder(Order.Asc(PredicateGroupEntity.DisplayOrderProperty)).List<PredicateGroupEntity>();
				});
		}

		#endregion
	}
}