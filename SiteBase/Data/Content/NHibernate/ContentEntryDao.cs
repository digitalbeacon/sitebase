// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using DigitalBeacon.Model;
using DigitalBeacon.Data.NHibernate;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Model.Content;

namespace DigitalBeacon.SiteBase.Data.Content.NHibernate
{
	/// <summary>
	/// Data access object for content entries
	/// </summary>
	public class ContentEntryDao : BaseDao<ContentEntryEntity>, IContentEntryDao
	{
		#region IContentEntryDao Members

		public IList<ContentEntryEntity> FetchForDisplay(long contentGroupId)
		{
			return HibernateTemplate.ExecuteFind<ContentEntryEntity>(
				delegate(ISession session)
				{
					ICriteria criteria = session.CreateCriteria(typeof(ContentEntryEntity))
						.Add(Expression.Eq(String.Format("{0}.{1}", ContentEntryEntity.ContentGroupProperty, BaseEntity.IdProperty), contentGroupId))
						.Add(Expression.Gt(BaseEntity.DisplayOrderProperty, 0))
						.AddOrder(Order.Asc(BaseEntity.DisplayOrderProperty));
					return criteria.List<ContentEntryEntity>();
				});
		}

		public int FetchMaxDisplayOrder(long contentGroupId)
		{
			return HibernateTemplate.Execute(session =>
			{
				return session.Query<ContentEntryEntity>().Where(x => x.ContentGroup.Id == contentGroupId).Max(x => x.DisplayOrder);
			});
		}

		#endregion

		public override ContentEntryEntity Save(ContentEntryEntity entity)
		{
			ContentEntryEntity retVal = base.Save(entity);
			// reorder content entries
			HibernateTemplate.Execute<long>(
				delegate(ISession session)
				{
					if (entity.DisplayOrder > 0)
					{
						IList<ContentEntryEntity> list = FetchForDisplay(entity.ContentGroup.Id);
						for (int i = 0; i < list.Count; i++)
						{
							if (list[i].Id != entity.Id && list[i].DisplayOrder >= entity.DisplayOrder)
							{
								list[i].DisplayOrder++;
							}
						}
					}
					return 0;
				});
			return retVal;
		}
	}
}