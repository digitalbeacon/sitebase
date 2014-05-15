// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using DigitalBeacon.Model;
using NHibernate.Criterion;
using DigitalBeacon.Data.NHibernate;
using DigitalBeacon.SiteBase.Model;

namespace DigitalBeacon.SiteBase.Data.NHibernate
{
	public class RoleHomeDao : BaseDao<RoleHomeEntity>, IRoleHomeDao
	{
		#region IRoleHomeDao Members

		public RoleHomeEntity GetRoleHome(long associationId, long? userId)
		{
			return HibernateTemplate.Execute(session =>
			{
				var dcUserRoles = DetachedCriteria.For(typeof(UserRoleEntity))
					.SetProjection(Projections.Constant(1))
					.Add(Restrictions.EqProperty(UserRoleEntity.RoleProperty, GetPropertyName(DefaultAlias, RoleHomeEntity.EntityIdProperty)))
					.CreateCriteria(UserRoleEntity.UserProperty)
					.Add(Restrictions.Eq(UserEntity.IdProperty, userId ?? 0));
				var c = session.CreateCriteria(typeof(RoleHomeEntity), DefaultAlias)
					.Add(Restrictions.Eq(RoleHomeEntity.AssociationIdProperty, associationId))
					.Add(Restrictions.Gt(RoleHomeEntity.DisplayOrderProperty, 0));
				if (userId.HasValue)
				{
					c.Add(Restrictions.Not(
							 Restrictions.And(
								 Restrictions.Eq(RoleHomeEntity.EntityTypeProperty, EntityType.RoleGroup),
								 Restrictions.Eq(RoleHomeEntity.EntityIdProperty, (long)RoleGroup.Unauthenticated))))
					 .Add(Restrictions.Or(
							Subqueries.Exists(DetachedCriteria.For(typeof(UserEntity))
								.Add(Restrictions.Eq(BaseEntity.IdProperty, userId ?? 0))
								.Add(Restrictions.Eq(UserEntity.SuperUserProperty, true)).SetProjection(Projections.Constant(1))),
							Restrictions.Or(
								Restrictions.And(
									 Restrictions.Eq(RoleHomeEntity.EntityTypeProperty, EntityType.User),
									 Restrictions.Eq(RoleHomeEntity.EntityIdProperty, userId ?? 0)),
								Restrictions.Or(
									 Restrictions.And(
										 Restrictions.Eq(RoleHomeEntity.EntityTypeProperty, EntityType.RoleGroup),
										 Restrictions.Or(
											 Restrictions.Eq(RoleHomeEntity.EntityIdProperty, (long)RoleGroup.Everyone),
											 Restrictions.Eq(RoleHomeEntity.EntityIdProperty, (long)RoleGroup.Authenticated))),
									 Restrictions.And(
										 Restrictions.Eq(RoleHomeEntity.EntityTypeProperty, EntityType.Role),
										 Subqueries.Exists(dcUserRoles))))));
				}
				else
				{
					c.Add(Restrictions.Eq(RoleHomeEntity.EntityTypeProperty, EntityType.RoleGroup))
					 .Add(Restrictions.Or(
							Restrictions.Eq(RoleHomeEntity.EntityIdProperty, (long)RoleGroup.Everyone),
							Restrictions.Eq(RoleHomeEntity.EntityIdProperty, (long)RoleGroup.Unauthenticated)));
				}
				return c.AddOrder(Order.Asc(RoleHomeEntity.DisplayOrderProperty))
					.SetMaxResults(1)
					.UniqueResult<RoleHomeEntity>();
			});			
		}

		#endregion
	}
}