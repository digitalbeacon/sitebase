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
	public class RoleDao : BaseDao<RoleEntity>, IRoleDao
	{
		#region IRoleDao Members

		public IList<RoleEntity> FetchByGroup(long associationId, long? roleGroupId)
		{
			return HibernateTemplate.ExecuteFind(session =>
			{
				var c = session.CreateCriteria(typeof(RoleEntity))
					.Add(Restrictions.Or(
							 Restrictions.IsNull(RoleEntity.AssociationIdProperty),
							 Restrictions.Eq(RoleEntity.AssociationIdProperty, associationId)));
				if (roleGroupId.HasValue)
				{
					c.Add(Restrictions.Eq(GetPropertyName(RoleEntity.RoleGroupProperty, RoleEntity.IdProperty), roleGroupId.Value));
				}
				else
				{
					c.Add(Restrictions.IsNull(RoleEntity.RoleGroupProperty));
				}
				return c.AddOrder(Order.Asc(RoleEntity.NameProperty)).List<RoleEntity>();
			});
		}

		#endregion
	}
}