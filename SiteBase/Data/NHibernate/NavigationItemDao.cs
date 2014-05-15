// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Collections.Generic;
using System.Linq;
using DigitalBeacon.Model;
using NHibernate.Criterion;
using NHibernate.Linq;
using DigitalBeacon.Data.NHibernate;
using DigitalBeacon.SiteBase.Model;

namespace DigitalBeacon.SiteBase.Data.NHibernate
{
	public class NavigationItemDao : BaseDao<NavigationItemEntity>, INavigationItemDao
	{
		#region INavigationItemDao Members

		public IList<NavigationItemEntity> FetchChildren(long id)
		{
			return HibernateTemplate.ExecuteFind(session =>
				 (from x in session.Query<NavigationItemEntity>() where x.Parent.Id == id select x).ToList());
		}

		public IList<NavigationItemEntity> FetchParentCandidates(long id, long associationId, long navigationId)
		{
			return HibernateTemplate.ExecuteFind(session =>
			{
				var c = session.CreateCriteria(typeof(NavigationItemEntity), DefaultAlias)
					.Add(Restrictions.Not(Restrictions.Eq(BaseEntity.IdProperty, id)))
					.Add(Restrictions.Or(
							 Restrictions.Eq(NavigationItemEntity.AssociationIdProperty, associationId),
							 Restrictions.IsNull(NavigationItemEntity.AssociationIdProperty)))
					.Add(Restrictions.Eq(NavigationItemEntity.EnabledProperty, true))
					.Add(Restrictions.IsNull(NavigationItemEntity.ParentProperty))
					.Add(Restrictions.Eq(GetIdProperty(NavigationItemEntity.NavigationProperty), navigationId));
				if (id > 0)
				{
					c.Add(Subqueries.NotExists(
							  DetachedCriteria.For(typeof(NavigationItemEntity))
								  .SetProjection(Projections.Constant(1))
								  .Add(Restrictions.Eq(GetIdProperty(NavigationItemEntity.ParentProperty), id))));
				}
				return c.AddOrder(Order.Asc(NavigationItemEntity.DisplayOrderProperty))
					.List<NavigationItemEntity>();
			});
		}

		public IList<NavigationItemEntity> FetchItems(long associationId, Navigation nav, long? userId)
		{
			return HibernateTemplate.ExecuteFind(session =>
			{
				var dcUserRoles = DetachedCriteria.For(typeof(UserRoleEntity))
					.SetProjection(Projections.Constant(1))
					.Add(Restrictions.EqProperty(UserRoleEntity.RoleProperty, GetPropertyName(typeof(PermissionEntity).Name, PermissionEntity.EntityIdProperty)))
					.CreateCriteria(UserRoleEntity.UserProperty)
					.Add(Restrictions.Eq(UserEntity.IdProperty, userId ?? 0));

				var dcAuthenticatedPermissions = DetachedCriteria.For(typeof(PermissionEntity), typeof(PermissionEntity).Name)
					.SetProjection(Projections.Constant(1))
					.Add(Restrictions.Or(
							Restrictions.And(
								Restrictions.Eq(PermissionEntity.Key1Property, SiteBaseConstants.SitePathKey),
								Restrictions.EqProperty(PermissionEntity.Key3Property, GetPropertyName(DefaultAlias, NavigationItemEntity.UrlProperty))),
							Restrictions.And(
								Restrictions.Eq(PermissionEntity.Key1Property, typeof(NavigationItemEntity).Name),
								Restrictions.EqProperty(PermissionEntity.Key2Property, GetIdProperty(DefaultAlias)))))
					.Add(Restrictions.Gt(PermissionEntity.MaskProperty, 0))
					.Add(Restrictions.Not(
							 Restrictions.And(
								 Restrictions.Eq(PermissionEntity.EntityTypeProperty, EntityType.RoleGroup),
								 Restrictions.Eq(PermissionEntity.EntityIdProperty, (long)RoleGroup.Unauthenticated))))
					.Add(Restrictions.Or(
							Subqueries.Exists(DetachedCriteria.For(typeof(UserEntity))
								.Add(Restrictions.Eq(BaseEntity.IdProperty, userId ?? 0))
								.Add(Restrictions.Eq(UserEntity.SuperUserProperty, true)).SetProjection(Projections.Constant(1))),
							Restrictions.Or(
								Restrictions.And(
									 Restrictions.Eq(PermissionEntity.EntityTypeProperty, EntityType.User),
									 Restrictions.Eq(PermissionEntity.EntityIdProperty, userId ?? 0)),
								Restrictions.Or(
									 Restrictions.And(
										 Restrictions.Eq(PermissionEntity.EntityTypeProperty, EntityType.RoleGroup),
										 Restrictions.Or(
											 Restrictions.Eq(PermissionEntity.EntityIdProperty, (long)RoleGroup.Everyone),
											 Restrictions.Eq(PermissionEntity.EntityIdProperty, (long)RoleGroup.Authenticated))),
									 Restrictions.And(
										 Restrictions.Eq(PermissionEntity.EntityTypeProperty, EntityType.Role),
										 Subqueries.Exists(dcUserRoles))))));

				var dcUnauthenticatedPermissions = DetachedCriteria.For(typeof(PermissionEntity))
					.SetProjection(Projections.Constant(1))
					.Add(Restrictions.Or(
							Restrictions.And(
								Restrictions.Eq(PermissionEntity.Key1Property, SiteBaseConstants.SitePathKey),
								Restrictions.EqProperty(PermissionEntity.Key3Property, GetPropertyName(DefaultAlias, NavigationItemEntity.UrlProperty))),
							Restrictions.And(
								Restrictions.Eq(PermissionEntity.Key1Property, typeof(NavigationItemEntity).Name),
								Restrictions.EqProperty(PermissionEntity.Key2Property, GetIdProperty(DefaultAlias)))))
					.Add(Restrictions.Gt(PermissionEntity.MaskProperty, 0))
					.Add(Restrictions.Eq(PermissionEntity.EntityTypeProperty, EntityType.RoleGroup))
					.Add(Restrictions.Or(
							 Restrictions.Eq(PermissionEntity.EntityIdProperty, (long)RoleGroup.Everyone),
							 Restrictions.Eq(PermissionEntity.EntityIdProperty, (long)RoleGroup.Unauthenticated)));

				var c = session.CreateCriteria(typeof(NavigationItemEntity), DefaultAlias)
					.Add(Restrictions.Eq(NavigationItemEntity.EnabledProperty, true))
					.Add(Restrictions.Or(
							 Restrictions.Eq(NavigationItemEntity.AssociationIdProperty, associationId),
							 Restrictions.IsNull(NavigationItemEntity.AssociationIdProperty)))
					.Add(Restrictions.Eq(GetIdProperty(NavigationItemEntity.NavigationProperty), (long)nav))
					.Add(Restrictions.Gt(NavigationItemEntity.DisplayOrderProperty, 0));
				if (userId.HasValue)
				{
					c.Add(Subqueries.Exists(dcAuthenticatedPermissions));
				}
				else
				{
					c.Add(Subqueries.Exists(dcUnauthenticatedPermissions));
				}
				return c.AddOrder(Order.Asc(GetIdProperty(NavigationItemEntity.ParentProperty)))
					.AddOrder(Order.Asc(NavigationItemEntity.DisplayOrderProperty))
					.AddOrder(Order.Asc(NavigationItemEntity.TextProperty))
					.List<NavigationItemEntity>();
			});
		}

		#endregion
	}
}