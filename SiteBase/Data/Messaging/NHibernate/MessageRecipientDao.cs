// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using DigitalBeacon.Data.NHibernate;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Model.Messaging;

namespace DigitalBeacon.SiteBase.Data.Messaging.NHibernate
{
	/// <summary>
	/// Data access object for message recipient
	/// </summary>
	public class MessageRecipientDao : BaseDao<MessageRecipientEntity>, IMessageRecipientDao
	{
		#region IMessageRecipientDao Members

		public MessageRecipientEntity Fetch(long messageId, EntityType recipientType, long recipientId)
		{
			return HibernateTemplate.Execute<MessageRecipientEntity>(
				delegate(ISession session)
				{
					DetachedCriteria c2 = DetachedCriteria.For(typeof(UserRoleEntity))
						.SetProjection(Projections.Property(UserRoleEntity.RoleProperty))
						.CreateCriteria(UserRoleEntity.UserProperty)
							.Add(Expression.Eq(UserEntity.IdProperty, recipientId));
					ICriteria c = session.CreateCriteria(typeof(MessageRecipientEntity))
						.Add(Expression.Eq(GetPropertyName(MessageRecipientEntity.MessageProperty, MessageEntity.IdProperty), messageId))
						.Add(Expression.Or(
								Expression.And(
									Expression.Eq(MessageRecipientEntity.RecipientTypeProperty, EntityType.User),
									Expression.Eq(MessageRecipientEntity.RecipientIdProperty, recipientId)),
								Expression.And(
									Expression.Eq(MessageRecipientEntity.RecipientTypeProperty, EntityType.Role),
									Subqueries.PropertyIn(MessageRecipientEntity.RecipientIdProperty, c2))));
					return c.UniqueResult<MessageRecipientEntity>();

				});
		}

		public DateTime? FetchMostRecentReadDate(long associationId, long userId)
		{
			return HibernateTemplate.Execute<DateTime?>(
				delegate(ISession session)
				{
					ICriteria c = session.CreateCriteria(typeof(MessageRecipientEntity))
						.Add(Expression.Eq(MessageRecipientEntity.RecipientTypeProperty, EntityType.User))
						.Add(Expression.Eq(MessageRecipientEntity.RecipientIdProperty, userId));
					c.CreateCriteria(MessageRecipientEntity.MessageProperty)
						.Add(Expression.Eq(MessageEntity.AssociationIdProperty, associationId));
					return c.SetProjection(Projections.Max(MessageRecipientEntity.DateFirstReadProperty)).UniqueResult<DateTime?>();
				});
		}

		#endregion
	}
}