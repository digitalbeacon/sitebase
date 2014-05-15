// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Linq;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using DigitalBeacon.Model;
using DigitalBeacon.Data.NHibernate;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Model.Content;
using System.ComponentModel;
using System.Text;
using DigitalBeacon.Data;

namespace DigitalBeacon.SiteBase.Data.Content.NHibernate
{
	/// <summary>
	/// Data access object for content groups
	/// </summary>
	public class ContentGroupDao : NameDao<ContentGroupEntity>, IContentGroupDao
	{
		#region IContentGroupDao Members

		public ContentGroupEntity Fetch(long associationId, string name)
		{
			return HibernateTemplate.Execute(
				delegate(ISession session)
				{
					return session.CreateCriteria(typeof(ContentGroupEntity))
						.Add(Expression.Eq(ContentGroupEntity.AssociationIdProperty, associationId))
						.Add(Expression.Eq(ContentGroupEntity.NameProperty, name))
						.UniqueResult<ContentGroupEntity>();
				});
		}

		public int FetchMaxDisplayOrder(long id)
		{
			return HibernateTemplate.Execute(session => 
			{
				return session.Query<ContentGroupEntity>().Where(x => x.Id != id).Max(x => x.DisplayOrder);
			});
		}

		public override ContentGroupEntity Save(ContentGroupEntity entity)
		{
			var retVal = base.Save(entity);
			if (entity.DisplayOrder > 0)
			{
				// reorder content entries
				HibernateTemplate.Execute(
					delegate(ISession session)
					{
						var list = from x in session.Query<ContentGroupEntity>()
								   where x.DisplayOrder >= entity.DisplayOrder
									  && x.Id != retVal.Id
								   orderby x.DisplayOrder
								   select x;
						int displayOrder = entity.DisplayOrder;
						foreach (var x in list)
						{
							x.DisplayOrder = ++displayOrder;
						}
						return 0;
					});
			}
			return retVal;
		}

		#endregion

		#region NameDao<ContentGroupEntity> Members

		public override ContentGroupEntity FetchWithLazyCollections(long id)
		{
			return HibernateTemplate.Execute(
				delegate(ISession session)
				{
					ContentGroupEntity entity = session.Get<ContentGroupEntity>(id);
					if (entity != null)
					{
						NHibernateUtil.Initialize(entity.ContentEntries);
						entity.AreLazyCollectionsInitialized = true;
					}
					return entity;
				});
		}

		#endregion
	}
}