// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using DigitalBeacon.Data.NHibernate;
using DigitalBeacon.SiteBase.Model;

namespace DigitalBeacon.SiteBase.Data.NHibernate
{
	public class PermissionDao : BaseDao<PermissionEntity>, IPermissionDao
	{
		#region IPermissionDao Members

		public IList<PermissionEntity> FetchList(string key1, long? key2, string key3, Permission? mask)
		{
			return FetchList(key1, key2, key3, mask, true, false);
		}

		public IList<PermissionEntity> FetchList(string key1, long? key2, string key3, Permission? mask, bool matchNulls, bool partialMatchForKey3)
		{
			return HibernateTemplate.ExecuteFind(session =>
			{
				var c = session.CreateCriteria(typeof(PermissionEntity))
					.Add(Restrictions.Eq(PermissionEntity.Key1Property, key1));
				if (key2.HasValue)
				{
					c.Add(Restrictions.Eq(PermissionEntity.Key2Property, key2.Value));
				}
				else if (matchNulls)
				{
					c.Add(Restrictions.IsNull(PermissionEntity.Key2Property));
				}
				if (key3 != null)
				{
					if (partialMatchForKey3)
					{
						c.Add(Restrictions.Like(PermissionEntity.Key3Property, key3, MatchMode.Start));
					}
					else
					{
						c.Add(Restrictions.Eq(PermissionEntity.Key3Property, key3));
					}
				}
				else if (matchNulls)
				{
					c.Add(Restrictions.IsNull(PermissionEntity.Key3Property));
				}
				if (mask.HasValue)
				{
					c.Add(Expression.Sql(String.Format("{{alias}}.{0} & ? > 0", PermissionEntity.MaskProperty), (int)mask.Value, NHibernateUtil.Int32));
				}
				return c.List<PermissionEntity>();
			});
		}

		public PermissionEntity Fetch(string key1, long? key2, string key3, EntityType entityType, long entityId)
		{
			return HibernateTemplate.Execute(session =>
			{
				var c = session.CreateCriteria(typeof(PermissionEntity))
					.Add(Restrictions.Eq(PermissionEntity.Key1Property, key1));
				if (key2.HasValue)
				{
					c.Add(Restrictions.Eq(PermissionEntity.Key2Property, key2.Value));
				}
				else
				{
					c.Add(Restrictions.IsNull(PermissionEntity.Key2Property));
				}
				if (key3 != null)
				{
					c.Add(Restrictions.Eq(PermissionEntity.Key3Property, key3));
				}
				else
				{
					c.Add(Restrictions.IsNull(PermissionEntity.Key3Property));
				}
				c.Add(Restrictions.Eq(PermissionEntity.EntityTypeProperty, entityType));
				c.Add(Restrictions.Eq(PermissionEntity.EntityIdProperty, entityId));
				return c.UniqueResult<PermissionEntity>();
			});
		}

		#endregion
	}
}