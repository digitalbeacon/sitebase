// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Collections.Generic;
using NHibernate.Criterion;
using DigitalBeacon.Data.NHibernate;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Model;

namespace DigitalBeacon.SiteBase.Data.NHibernate
{
	/// <summary>
	/// Data access object for secure file folders
	/// </summary>
	public class FolderDao : BaseDao<FolderEntity>, IFolderDao
	{
		public override FolderEntity Save(FolderEntity entity)
		{
			var retVal = base.Save(entity);
			if (entity.AssociationId.HasValue)
			{
				// reorder folders
				HibernateTemplate.Execute<long>(session =>
				{
					if (entity.DisplayOrder > 0)
					{
						var list = FetchForReorder(entity.AssociationId.Value, entity.Type, entity.ParentFolderId);
						for (var i = 0; i < list.Count; i++)
						{
							if (list[i].Id != entity.Id && list[i].DisplayOrder >= entity.DisplayOrder)
							{
								list[i].DisplayOrder++;
							}
						}
					}
					return 0;
				});
			}
			return retVal;
		}

		#region IFolderDao Members

		public FolderEntity Fetch(long? associationId, FolderType folderType, long? parentFolderId, string name)
		{
			return HibernateTemplate.Execute(session =>
			{
				var criteria = session.CreateCriteria(typeof(FolderEntity));
				if (associationId.HasValue)
				{
					criteria.Add(Restrictions.Eq(FolderEntity.AssociationIdProperty, associationId.Value));
				}
				else
				{
					criteria.Add(Restrictions.IsNull(FolderEntity.AssociationIdProperty));
				}
				criteria.Add(Restrictions.Eq(FolderEntity.TypeProperty, folderType));
				criteria.Add(Restrictions.Eq(FolderEntity.NameProperty, name));
				if (parentFolderId == null)
				{
					criteria.Add(Restrictions.IsNull(FolderEntity.ParentFolderIdProperty));
				}
				else
				{
					criteria.Add(Restrictions.Eq(FolderEntity.ParentFolderIdProperty, parentFolderId.Value));
				}
				return criteria.UniqueResult<FolderEntity>();
			});
		}

		public int FetchMaxDisplayOrder(long associationId, FolderType folderType, long? parentFolderId)
		{
			return HibernateTemplate.Execute(session =>
			{
				var c = session.CreateCriteria(typeof(FolderEntity))
					.Add(Restrictions.Eq(FolderEntity.AssociationIdProperty, associationId))
					.Add(Restrictions.Eq(FolderEntity.TypeProperty, folderType));
				if (parentFolderId.HasValue)
				{
					c.Add(Restrictions.Eq(FolderEntity.ParentFolderIdProperty, parentFolderId.Value));
				}
				else
				{
					c.Add(Restrictions.IsNull(FolderEntity.ParentFolderIdProperty));
				}
				var retVal = c.SetProjection(Projections.Max(BaseEntity.DisplayOrderProperty)).UniqueResult();
				return (retVal != null ? (int)retVal : 0);
			});
		}

		public IList<FolderEntity> FetchForDisplay(long associationId, FolderType folderType, long? userId)
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
					.Add(Restrictions.Eq(PermissionEntity.Key1Property, typeof(FolderEntity).Name))
					.Add(Restrictions.EqProperty(PermissionEntity.Key2Property, GetIdProperty(DefaultAlias)))
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
					.Add(Restrictions.Eq(PermissionEntity.Key1Property, typeof(FolderEntity).Name))
					.Add(Restrictions.EqProperty(PermissionEntity.Key2Property, GetIdProperty(DefaultAlias)))
					.Add(Restrictions.Gt(PermissionEntity.MaskProperty, 0))
					.Add(Restrictions.Eq(PermissionEntity.EntityTypeProperty, EntityType.RoleGroup))
					.Add(Restrictions.Or(
							 Restrictions.Eq(PermissionEntity.EntityIdProperty, (long)RoleGroup.Everyone),
							 Restrictions.Eq(PermissionEntity.EntityIdProperty, (long)RoleGroup.Unauthenticated)));

				var c = session.CreateCriteria(typeof(FolderEntity))
					//.Add(Restrictions.Eq(FolderEntity.EnabledProperty, true))
					.Add(Restrictions.Or(
							 Restrictions.Eq(FolderEntity.AssociationIdProperty, associationId),
							 Restrictions.IsNull(FolderEntity.AssociationIdProperty)))
					.Add(Restrictions.Eq(FolderEntity.TypeProperty, folderType))
					.Add(Restrictions.Gt(FolderEntity.DisplayOrderProperty, 0));
				if (userId.HasValue)
				{
					c.Add(Subqueries.Exists(dcAuthenticatedPermissions));
				}
				else
				{
					c.Add(Subqueries.Exists(dcUnauthenticatedPermissions));
				}
				return c.AddOrder(Order.Asc(FolderEntity.ParentFolderIdProperty))
					.AddOrder(Order.Asc(FolderEntity.DisplayOrderProperty))
					.List<FolderEntity>();
			});
		}

		public IList<FolderEntity> FetchChildren(long folderId)
		{
			return HibernateTemplate.ExecuteFind(session => 
				session.CreateCriteria(typeof(FolderEntity))
				   .Add(Restrictions.Eq(FolderEntity.ParentFolderIdProperty, folderId))
				   .List<FolderEntity>());
		}

		#endregion

		private IList<FolderEntity> FetchForReorder(long associationId, FolderType folderType, long? parentFolderId)
		{
			return HibernateTemplate.ExecuteFind(session =>
			{
				var c = session.CreateCriteria(typeof(FolderEntity))
					.Add(Restrictions.Eq(FolderEntity.AssociationIdProperty, associationId))
					.Add(Restrictions.Eq(FolderEntity.TypeProperty, folderType))
					.Add(Restrictions.Gt(FolderEntity.DisplayOrderProperty, 0));
				if (parentFolderId.HasValue)
				{
					c.Add(Restrictions.Eq(FolderEntity.ParentFolderIdProperty, parentFolderId.Value));
				}
				else
				{
					c.Add(Restrictions.IsNull(FolderEntity.ParentFolderIdProperty));
				}
				c.AddOrder(Order.Asc(FolderEntity.DisplayOrderProperty));
				return c.List<FolderEntity>();
			});
		}
	}
}