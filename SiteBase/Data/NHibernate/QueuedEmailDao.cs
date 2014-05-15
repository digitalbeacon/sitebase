// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Linq;
using System.Collections.Generic;
using DigitalBeacon.Util;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using DigitalBeacon.Data.NHibernate;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.Model;

namespace DigitalBeacon.SiteBase.Data.NHibernate
{
	public class QueuedEmailDao : BaseDao<QueuedEmailEntity>, IQueuedEmailDao
	{
		#region IQueuedEmailDaoDao Members

		public IList<QueuedEmailEntity> FetchMessagesToSend()
		{
			return HibernateTemplate.ExecuteFind(session => 
				session.CreateCriteria(typeof(QueuedEmailEntity))
				   .Add(Restrictions.IsNull(QueuedEmailEntity.DateProcessedProperty))
				   .Add(Restrictions.Le(QueuedEmailEntity.SendDateProperty, DateTime.Now))
				   .List<QueuedEmailEntity>());
		}

		public DateTime? FetchMostRecentSentDate(long associationId, long userId, ModuleSettingDefinition template)
		{
			return HibernateTemplate.Execute(session =>
			{
				var c = session.CreateCriteria(typeof(QueuedEmailEntity))
					.Add(Restrictions.Eq(QueuedEmailEntity.AssociationIdProperty, associationId))
					.Add(Restrictions.Eq(QueuedEmailEntity.TemplateProperty, template));
				c.CreateCriteria(QueuedEmailEntity.RecipientsProperty)
					.Add(Restrictions.Eq(QueuedEmailRecipientEntity.UserIdProperty, userId));
				return c.SetProjection(Projections.Max(QueuedEmailEntity.SendDateProperty)).UniqueResult<DateTime?>();
			});
		}

		public void ClearSession()
		{
			HibernateTemplate.Clear();
		}

		//protected override ICriteria ApplyFilters(ICriteria c, SearchInfo<QueuedEmailEntity> searchInfo)
		//{
		//	//var recipients = DetachedCriteria.For(typeof(QueuedEmailRecipientEntity))
		//	//	.Add(Restrictions.Like(QueuedEmailRecipientEntity.EmailProperty, searchInfo.SearchText, MatchMode.Anywhere));
		//	if (searchInfo.SearchText.HasText())
		//	{
		//		c.CreateCriteria(QueuedEmailEntity.RecipientsProperty)
		//			.Add(Restrictions.Like(QueuedEmailRecipientEntity.EmailProperty, searchInfo.SearchText, MatchMode.Anywhere));
		//	}
		//	return base.ApplyFilters(c, searchInfo);
		//}

		#endregion
	}
}