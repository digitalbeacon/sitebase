// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Common.Logging;
using NHibernate;
using NHibernate.Criterion;
using DigitalBeacon.Data.NHibernate;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Model.Messaging;

namespace DigitalBeacon.SiteBase.Data.Messaging.NHibernate
{
	/// <summary>
	/// Data access object for message templates
	/// </summary>
	public class MessageTemplateDao : BaseDao<MessageTemplateEntity>, IMessageTemplateDao
	{
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		#region IMessageTemplateDao Members

		public IList<MessageTemplateEntity> FetchNameList(long associationId)
		{
			return HibernateTemplate.ExecuteFind<MessageTemplateEntity>(
				delegate(ISession session)
				{
					IList<MessageTemplateEntity> retVal = null;
					ICriteria c = session.CreateCriteria(typeof(MessageTemplateEntity))
						.Add(Expression.Eq(MessageTemplateEntity.AssociationIdProperty, associationId))
						.AddOrder(Order.Asc(MessageTemplateEntity.NameProperty))
						.SetProjection(Projections.ProjectionList()
							.Add(Projections.Property(MessageTemplateEntity.IdProperty))
							.Add(Projections.Property(MessageTemplateEntity.VersionProperty))
							.Add(Projections.Property(MessageTemplateEntity.NameProperty)));
					IList<object[]> list = c.List<object[]>();
					if (list != null)
					{
						retVal = CreateNamedEntityList(list);
					}
					return retVal;
				});
		}

		private static MessageTemplateEntity CreateNamedEntity(object[] result)
		{
			MessageTemplateEntity entity = null;
			if (result != null)
			{
				entity = new MessageTemplateEntity();
				entity.Id = Int64.Parse(result[0].ToString());
				entity.ModificationCounter = Int64.Parse(result[1].ToString());
				entity.Name = result[2] as string;
			}
			return entity;
		}

		private static List<MessageTemplateEntity> CreateNamedEntityList(ICollection<object[]> resultList)
		{
			List<MessageTemplateEntity> retVal = new List<MessageTemplateEntity>(resultList.Count);
			foreach (object[] result in resultList)
			{
				retVal.Add(CreateNamedEntity(result));
			}
			return retVal;
		}

		#endregion
	}
}