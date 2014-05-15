// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Collections.Generic;
using NHibernate.Criterion;
using DigitalBeacon.Data.NHibernate;
using DigitalBeacon.SiteBase.Model;

namespace DigitalBeacon.SiteBase.Data.NHibernate
{
	public class RoleGroupDao : BaseDao<RoleGroupEntity>, IRoleGroupDao
	{
		#region IRoleGroupDao Members

		public IList<RoleGroupEntity> FetchInUse(long associationId)
		{
			return HibernateTemplate.ExecuteFind(session => 
				session.CreateCriteria(typeof(RoleGroupEntity), DefaultAlias)
					.Add(Restrictions.Gt(RoleGroupEntity.DisplayOrderProperty, 0))
					.Add(Restrictions.Or(
							 Restrictions.IsNull(RoleGroupEntity.AssociationIdProperty),
							 Restrictions.Eq(RoleGroupEntity.AssociationIdProperty, associationId)))
					.Add(Subqueries.Exists(
							 DetachedCriteria.For(typeof(RoleEntity))
								 .SetProjection(Projections.Constant(1))
								 .Add(Restrictions.Or(
										 Restrictions.IsNull(RoleEntity.AssociationIdProperty),
										 Restrictions.Eq(RoleEntity.AssociationIdProperty, associationId)))
								 .Add(Restrictions.EqProperty(GetIdProperty(RoleEntity.RoleGroupProperty), GetIdProperty(DefaultAlias)))))
					.AddOrder(Order.Asc(RoleGroupEntity.DisplayOrderProperty))
					.List<RoleGroupEntity>());
		}

		#endregion
	}
}