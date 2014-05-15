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
	/// Data access object for secure messages
	/// </summary>
	public class MessageDao : BaseDao<MessageEntity>, IMessageDao
	{
		private static readonly string FetchSentQuery = FromClause + 
			" where x.AssociationId = ? and x.SenderId = ? and x.Type = ? and x.DateSent is not null order by x.DateSent";
		private static readonly string FetchDraftQuery = FromClause +
			" where x.AssociationId = ? and x.SenderId = ? and x.Type = ? and x.DateSent is null order by x.Id";
		private static readonly string FetchDraftCountQuery = "select count(x.Id) " + FromClause +
			" where x.AssociationId = ? and x.SenderId = ? and x.Type = ? and x.DateSent is null";
		private static readonly string FetchRelatedQuery = FromClause + 
			" where x.RelatedId = ? and x.DateSent is not null order by x.DateSent desc";
		private static readonly string FetchFileAttachmentCountQuery = 
			"select count(x) from MessageAttachmentEntity x where x.Message.Id = ?";

		#region IMessageDao Members

		public IList<MessageEntity> Fetch(long associationId, long userId, long folderId)
		{
			return HibernateTemplate.ExecuteFind<MessageEntity>(
				delegate(ISession session)
				{
					//ISQLQuery q = session.CreateSQLQuery(
					//	"SELECT {MessageEntity.*} FROM Message MessageEntity"
					//	+ " INNER JOIN MessageRecipient on MessageRecipient.MessageId = MessageEntity.Id"
					//	+ " WHERE MessageRecipient.FolderId = :folderId"
					//	+ " AND (MessageRecipient.UserId = :userId"
					//	+ "	  OR MessageRecipient.RoleId IN (SELECT RoleId FROM UserRole WHERE UserId = :userId))");
					//q.SetInt64("folderId", folderId);
					//q.SetInt64("userId", userId);
					//return q.AddEntity(typeof(MessageEntity)).List<MessageEntity>();

					//IQuery q = session.CreateQuery(
					//	"select x from MessageEntity as x inner join x.Recipients as y"
					//	+ " where y.Folder = :folder"
					//	+ " and (y.UserId = :userId or y.Role in (select z.Role from UserRoleEntity as z where z.User.Id = :userId))");
					//q.SetEnum("folder", (MessageFolder)folderId);
					//q.SetInt64("userId", userId);
					//return q.List<MessageEntity>();

					DetachedCriteria c2 = DetachedCriteria.For(typeof(UserRoleEntity))
						.SetProjection(Projections.Property(UserRoleEntity.RoleProperty))
						.CreateCriteria(UserRoleEntity.UserProperty)
							.Add(Expression.Eq(UserEntity.IdProperty, userId));

					ICriteria c = session.CreateCriteria(typeof(MessageEntity))
						.Add(Expression.Eq(MessageEntity.EmailProperty, false))
						.Add(Expression.IsNotNull(MessageEntity.DateSentProperty))
						.Add(Expression.Eq(MessageEntity.AssociationIdProperty, associationId))
						.Add(Expression.Eq(MessageEntity.TypeProperty, MessageType.SecureMessage))
						.CreateCriteria(MessageEntity.RecipientsProperty)
							.Add(Expression.Eq(MessageRecipientEntity.FolderIdProperty, folderId))
							.Add(Expression.Or(
									Expression.And(
										Expression.Eq(MessageRecipientEntity.RecipientTypeProperty, EntityType.User),
										Expression.Eq(MessageRecipientEntity.RecipientIdProperty, userId)),
									Expression.And(
										Expression.Eq(MessageRecipientEntity.RecipientTypeProperty, EntityType.Role),
										Subqueries.PropertyIn(MessageRecipientEntity.RecipientIdProperty, c2))));
					return c.List<MessageEntity>();
				});
		}

		public int FetchUnreadCount(long associationId, long userId, long folderId)
		{
			if (folderId == (long)MessageFolder.Drafts)
			{
				return HibernateTemplate.Execute<int>(
					delegate(ISession session)
					{
						IQuery query = session.CreateQuery(FetchDraftCountQuery);
						query.SetInt64(0, associationId);
						query.SetInt64(1, userId);
						query.SetEnum(2, MessageType.SecureMessage);
						return (int)query.UniqueResult<long>();
					});
			}
			else
			{
				return HibernateTemplate.Execute<int>(
					delegate(ISession session)
					{
						DetachedCriteria c2 = DetachedCriteria.For(typeof(UserRoleEntity))
							.SetProjection(Projections.Property(UserRoleEntity.RoleProperty))
							.CreateCriteria(UserRoleEntity.UserProperty)
								.Add(Expression.Eq(UserEntity.IdProperty, userId));
						ICriteria c = session.CreateCriteria(typeof(MessageEntity))
							.SetProjection(Projections.Count(MessageEntity.IdProperty))
							.Add(Expression.Eq(MessageEntity.EmailProperty, false))
							.Add(Expression.IsNotNull(MessageEntity.DateSentProperty))
							.Add(Expression.Eq(MessageEntity.AssociationIdProperty, associationId))
							.Add(Expression.Eq(MessageEntity.TypeProperty, MessageType.SecureMessage))
							.CreateCriteria(MessageEntity.RecipientsProperty)
								.Add(Expression.Eq(MessageRecipientEntity.FolderIdProperty, folderId))
								.Add(Expression.Eq(MessageRecipientEntity.ReadProperty, false))
								.Add(Expression.Or(
										Expression.And(
											Expression.Eq(MessageRecipientEntity.RecipientTypeProperty, EntityType.User),
											Expression.Eq(MessageRecipientEntity.RecipientIdProperty, userId)),
										Expression.And(
											Expression.Eq(MessageRecipientEntity.RecipientTypeProperty, EntityType.Role),
											Subqueries.PropertyIn(MessageRecipientEntity.RecipientIdProperty, c2))));
						return c.UniqueResult<int>();
					});
			}
		}

		public IList<MessageEntity> FetchSent(long associationId, long userId)
		{
			return HibernateTemplate.Find<MessageEntity>(FetchSentQuery, new object[] { associationId, userId, MessageType.SecureMessage });
		}

		public IList<MessageEntity> FetchDrafts(long associationId, long userId)
		{
			return HibernateTemplate.Find<MessageEntity>(FetchDraftQuery, new object[] { associationId, userId, MessageType.SecureMessage });
		}

		public IList<MessageEntity> FetchRelated(long relatedId)
		{
			return HibernateTemplate.Find<MessageEntity>(FetchRelatedQuery, relatedId);
		}

		public bool HasAttachment(long messageId)
		{
			bool retVal = false;
			HibernateTemplate.Execute<MessageEntity>(
				delegate(ISession session)
				{
					IQuery query = session.CreateQuery(FetchFileAttachmentCountQuery);
					query.SetInt64(0, messageId);
					long count = query.UniqueResult<long>();
					if (count > 0)
					{
						retVal = true;
					}
					return null;
				});
			return retVal;
		}

		#endregion
	}
}