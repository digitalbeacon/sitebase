// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Xml;
using System.Xml.XPath;
using DigitalBeacon.Business;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Data;
using DigitalBeacon.SiteBase.Data.Messaging;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Model.Messaging;
using DigitalBeacon.Util;

namespace DigitalBeacon.SiteBase.Business.Support
{
	public class MessageService : BaseService, IMessageService
	{
		#region Private Members

		private static readonly IMessageDao MsgDao = ServiceFactory.Instance.GetService<IMessageDao>();
		private static readonly IFolderDao FolderDao = ServiceFactory.Instance.GetService<IFolderDao>();
		private static readonly IMessageRecipientDao RecipientDao = ServiceFactory.Instance.GetService<IMessageRecipientDao>();
		private static readonly IMessageTemplateDao TemplateDao = ServiceFactory.Instance.GetService<IMessageTemplateDao>();
		private static readonly ILookupService LookupService = ServiceFactory.Instance.GetService<ILookupService>();
		private static readonly IIdentityService IdentityService = ServiceFactory.Instance.GetService<IIdentityService>();
		private static readonly IPreferenceService PreferenceService = ServiceFactory.Instance.GetService<IPreferenceService>();
		private static readonly IQueuedEmailDao QueuedEmailDao = ServiceFactory.Instance.GetService<IQueuedEmailDao>();
		private static readonly IMailService MailService = ServiceFactory.Instance.GetService<IMailService>();

		#endregion

		#region IMessageService Members

		public FolderEntity GetFolder(long folderId)
		{
			return DataAdapter.Fetch<FolderEntity>(folderId);
		}

		public IList<FolderEntity> GetFolders(long associationId, long userId)
		{
			return FolderDao.FetchForDisplay(associationId, FolderType.Message, userId);
		}

		public IDictionary<FolderEntity, int> GetUnreadMessageCounts(long associationId, long userId)
		{
			IDictionary<FolderEntity, int> map = new Dictionary<FolderEntity, int>();
			IList<FolderEntity> folders = GetFolders(associationId, userId);
			foreach (FolderEntity folder in folders)
			{
				map[folder] = GetUnreadMessageCount(associationId, userId, folder.Id);
			}
			return map;
		}

		public int GetUnreadMessageCount(long associationId, long userId, long folderId)
		{
			return MsgDao.FetchUnreadCount(associationId, userId, folderId);
		}

		public FavoriteRecipients GetFavoriteRecipients(long associationId, long userId)
		{
			var fr = new FavoriteRecipients();
			var up = PreferenceService.GetUserPreference(associationId, userId, MessagingConstants.FavoriteRecipientsKey);
			if (up != null)
			{
				var doc = new XPathDocument(new StringReader(up.Value));
				var nav = doc.CreateNavigator();
				var nodes = nav.Select("/FavoriteRecipients/Users/Id");
				while (nodes.MoveNext())
				{
					fr.Users.Add(Convert.ToInt64(nodes.Current.Value));
				}
				nodes = nav.Select("/FavoriteRecipients/Roles/Id");
				while (nodes.MoveNext())
				{
					fr.Roles.Add(Convert.ToInt64(nodes.Current.Value));
				}
			}
			return fr;
		}

		public void SaveFavoriteRecipients(long associationId, long userId, FavoriteRecipients recipients)
		{
			var doc = new XmlDocument();
			XmlNode favoriteRecipients = doc.CreateElement("FavoriteRecipients");
			doc.AppendChild(favoriteRecipients);
			XmlNode users = doc.CreateElement("Users");
			favoriteRecipients.AppendChild(users);
			if (recipients.Users != null && recipients.Users.Count > 0)
			{
				foreach (long id in recipients.Users)
				{
					XmlNode ah = doc.CreateElement("Id");
					ah.InnerText = id.ToString();
					users.AppendChild(ah);
				}
			}
			XmlNode roles = doc.CreateElement("Roles");
			favoriteRecipients.AppendChild(roles);
			if (recipients.Roles != null && recipients.Roles.Count > 0)
			{
				foreach (long id in recipients.Roles)
				{
					XmlNode ah = doc.CreateElement("Id");
					ah.InnerText = id.ToString();
					roles.AppendChild(ah);
				}
			}
			var pref = PreferenceService.GetUserPreference(associationId, userId, MessagingConstants.FavoriteRecipientsKey);
			if (pref == null)
			{
				pref = new PreferenceEntity
			   {
				   AssociationId = associationId, 
				   UserId = userId, 
				   Key = MessagingConstants.FavoriteRecipientsKey
			   };
			}
			pref.Value = doc.OuterXml;
			PreferenceService.SaveUserPreference(pref);
		}

		public void SendMessage(MessageEntity msg)
		{
			if (msg.Id > 0 && msg.DateSent != null)
			{
				throw new Exception("Can not send a message that was previously sent");
			}
			if (msg.SenderId <= 0)
			{
				throw new Exception("Message must have a sender.");
			}
			if (msg.Recipients == null || msg.Recipients.Count == 0)
			{
				throw new Exception("Message has no recipients.");
			}
			AssociateExistingRecipients(msg);
			foreach (MessageRecipientEntity r in msg.Recipients)
			{
				if (r.RecipientType == EntityType.Role)
				{
					if (msg.Recipients.Count > 1)
					{
						throw new Exception("Message can only have one recipient if recipient is a role.");
					}
					//UserEntity sender = IdentityService.GetUser(msg.SenderId);
					if (msg.ExpandRoleRecipient)
					{
						// expand role
						//if (r.Role.Value == Role.PropertyOwner)
						//{
						//	if (!msg.Email)
						//	{
						//		throw new Exception("Message recipient can only be Property Owners roles when message is sent via Email.");
						//	}
						//	IList<PropertyOwnerEntity> owners = _propertyService.FindPropertyOwnersWithEmail(msg.AssociationId, new PropertySearchInfo());
						//	foreach (PropertyOwnerEntity owner in owners)
						//	{
						//		msg.AddPersonRecipient(owner.Id, String.Format("{0} ({1})", owner.DisplayName, owner.Address.Email), false, true);
						//	}
						//}
						//else
						//{
							IList<UserEntity> users = IdentityService.GetUsersByRole(msg.AssociationId, (Role)r.RecipientId);
							foreach (UserEntity user in users)
							{
								if (user.Id != msg.SenderId)
								{
									msg.AddRecipient(EntityType.User, user.Id, msg.Email ? String.Format("{0} ({1})", user.DisplayName, user.Email) : user.DisplayName, false, true);
								}
							}
						//}
						r.RecipientType = EntityType.User;
						r.RecipientId = msg.SenderId;
						AssociateExistingRecipients(msg);
					}
					break;
				}
			}
			if (msg.ReplyToId != null && msg.RelatedId == null)
			{
				MessageEntity replyToMsg = MsgDao.Fetch(msg.ReplyToId.Value);
				if (replyToMsg == null)
				{
					throw new Exception("Could not locate replyToMessageId: " + msg.ReplyToId.Value);
				}
				if (replyToMsg.RelatedId != null)
				{
					msg.RelatedId = replyToMsg.RelatedId;
				}
				else
				{
					msg.RelatedId = msg.ReplyToId;
				}
			}
			//else if (msg.WorkItemId != null && msg.RelatedId == null)
			//{
			//	// set RelatedId to the first message associated with this work item
			//	List<MessageEntity> relatedMessages = new List<MessageEntity>(GetRelatedMessagesForWorkItem(msg.WorkItemId.Value));
			//	if (relatedMessages.Count > 0)
			//	{
			//		relatedMessages.Sort();
			//		if (relatedMessages[0].DateSent != null)
			//		{
			//			msg.RelatedId = relatedMessages[0].Id;
			//		}
			//	}
			//}
			msg.DateSent = DateTime.Now;
			SaveMessage(msg);
			if (msg.Email)
			{
				SendEmail(msg);
			}
			else
			{
				SendNotifications(msg);
			}
		}

		public void SaveDraft(MessageEntity msg)
		{
			msg.DateSent = null;
			AssociateExistingRecipients(msg);
			SaveMessage(msg);
		}

		public void DeleteDraft(long messageId)
		{
			MessageEntity msg = MsgDao.Fetch(messageId);
			if (msg == null)
			{
				throw new Exception("Could not find message with Id " + messageId);
			}
			if (msg.DateSent != null)
			{
				throw new Exception(String.Format("Message with Id {0} is not a draft message.", messageId));
			}
			DeleteWithAudit<MessageEntity>(messageId);
		}

		public MessageEntity GetMessage(long messageId)
		{
			return MsgDao.Fetch(messageId);
		}

		public IList<MessageEntity> GetMessages(long associationId, long userId, long folderId)
		{
			return MsgDao.Fetch(associationId, userId, folderId);
		}

		public IList<MessageEntity> GetSentMessages(long associationId, long userId)
		{
			return MsgDao.FetchSent(associationId, userId);
		}

		public IList<MessageEntity> GetDraftMessages(long associationId, long userId)
		{
			return MsgDao.FetchDrafts(associationId, userId);
		}

		public IList<MessageEntity> GetRelatedMessages(long messageId)
		{
			return GetRelatedMessages(messageId, false);
		}

		public IList<MessageEntity> GetRelatedMessages(long messageId, bool includeSpecifiedMessage)
		{
			ISet<MessageEntity> set = new HashSet<MessageEntity>();
			MessageEntity msg = MsgDao.Fetch(messageId);
			if (msg == null)
			{
				throw new Exception("Could not find message with Id: " + messageId);
			}
			if (msg.RelatedId != null)
			{
				IList<MessageEntity> list = MsgDao.FetchRelated(msg.RelatedId.Value);
				foreach (MessageEntity relatedMsg in list)
				{
					if (includeSpecifiedMessage || relatedMsg.Id != messageId)
					{
						set.Add(relatedMsg);
					}
				}
				set.Add(MsgDao.Fetch(msg.RelatedId.Value));
			}
			else
			{
				MsgDao.FetchRelated(msg.Id).ForEach(x => set.Add(x));
				if (includeSpecifiedMessage)
				{
					set.Add(msg);
				}
			}
			return  new List<MessageEntity>(set);
		}

		public MessageRecipientEntity GetMessageRecipient(long messageId, long recipientId)
		{
			return RecipientDao.Fetch(messageId, EntityType.User, recipientId);
		}

		public string GetDisplayName(long userId)
		{
			string retVal = String.Empty;
			UserEntity user = IdentityService.GetUser(userId);
			if (user != null)
			{
				retVal = user.DisplayName;
			}
			return retVal;
		}

		public bool HasAttachment(long messageId)
		{
			return MsgDao.HasAttachment(messageId);
		}

		public IDictionary<string, bool> GetMessageRecipientNames(long messageId)
		{
			IDictionary<string, bool> map = new Dictionary<string, bool>();
			MessageEntity msg = MsgDao.Fetch(messageId);
			if (msg == null)
			{
				throw new Exception("Could not find message with Id: " + messageId);
			}
			foreach (MessageRecipientEntity recipient in msg.Recipients)
			{
				if (!recipient.Bcc)
				{
					map[recipient.Name] = recipient.Cc;
				}
			}
			return map;
		}

		public IList<NotificationPreferenceEntity> GetNotificationPreferences()
		{
			return DataAdapter.FetchAll<NotificationPreferenceEntity>(BaseEntity.IdProperty); 
		}

		public void ProcessNotifications()
		{

		}

		public void SetMessageFlag(long messageId, long recipientId, bool flag)
		{
			MessageRecipientEntity r = RecipientDao.Fetch(messageId, EntityType.User, recipientId);
			MessageEntity msg = MsgDao.Fetch(messageId);
			if (r == null && msg.SenderId != recipientId)
			{
				throw new Exception(String.Format("Could not find message recipient or sender with Id {0} for message with Id {1}.", recipientId, messageId));
			}
			if (r != null)
			{
				r.Flagged = flag;
				SaveWithAudit(r);
			}
			if (msg.SenderId == recipientId)
			{
				msg.Flagged = flag;
				SaveWithAudit(msg);
			}
		}

		public void SetMessagesFlag(ICollection<long> messageIds, long recipientId, bool flag)
		{
			if (messageIds == null)
			{
				throw new ArgumentNullException("messageIds");
			}
			if (messageIds.Count == 0)
			{
				throw new ArgumentException("messageIds should not be empty");
			}
			foreach (long messageId in messageIds)
			{
				SetMessageFlag(messageId, recipientId, flag);
			}
		}

		public void SetMessageRead(long messageId, long recipientId, bool read)
		{
			MessageRecipientEntity r = RecipientDao.Fetch(messageId, EntityType.User, recipientId);
			if (r == null)
			{
				throw new Exception(String.Format("Could not find message recipient with Id {0} for message with Id {1}.", recipientId, messageId));
			}
			r.Read = read;
			if (read && r.DateFirstRead == null)
			{
				r.DateFirstRead = DateTime.Now;
			}
			SaveWithAudit(r);
		}

		public void SetMessagesRead(ICollection<long> messageIds, long recipientId, bool read)
		{
			if (messageIds == null)
			{
				throw new ArgumentNullException("messageIds");
			}
			if (messageIds.Count == 0)
			{
				throw new ArgumentException("messageIds should not be empty");
			}
			foreach (long messageId in messageIds)
			{
				SetMessageRead(messageId, recipientId, read);
			}
		}

		public void MoveMessage(long messageId, long recipientId, long folderId)
		{
			MessageRecipientEntity r = RecipientDao.Fetch(messageId, EntityType.User, recipientId);
			if (r == null)
			{
				throw new Exception(String.Format("Could not find message recipient with Id {0} for message with Id {1}.", recipientId, messageId));
			}
			r.FolderId = folderId;
			SaveWithAudit(r);
		}

		public void MoveMessages(ICollection<long> messageIds, long recipientId, long folderId)
		{
			if (messageIds == null)
			{
				throw new ArgumentNullException("messageIds");
			}
			if (messageIds.Count == 0)
			{
				throw new ArgumentException("messageIds should not be empty");
			}
			foreach (long messageId in messageIds)
			{
				MoveMessage(messageId, recipientId, folderId);
			}
		}

		public MessageAttachmentEntity GetAttachment(long attachmentId)
		{
			return DataAdapter.Fetch<MessageAttachmentEntity>(attachmentId);
		}

		public void SaveAttachment(MessageAttachmentEntity attachment)
		{
			if (attachment.DateCreated == default(DateTime))
			{
				attachment.DateCreated = DateTime.Now;
			}
			SaveWithAudit(attachment);
		}

		public void DeleteAttachment(long attachmentId)
		{
			DeleteWithAudit<MessageAttachmentEntity>(attachmentId);
		}

		public IList<MessageTemplateEntity> GetTemplateNames(long associationId)
		{
			return TemplateDao.FetchNameList(associationId);
		}

		public MessageTemplateEntity GetTemplate(long id)
		{
			return DataAdapter.Fetch<MessageTemplateEntity>(id);
		}

		public void SaveTemplate(MessageTemplateEntity template)
		{
			SaveWithAudit(template);
		}

		public void DeleteTemplate(long id)
		{
			DeleteWithAudit<MessageTemplateEntity>(id);
		}

		#endregion

		#region Private Methods

		private void SaveMessage(MessageEntity msg)
		{
			if (String.IsNullOrEmpty(msg.SenderName))
			{
				msg.SenderName = GetDisplayName(msg.SenderId);
			}
			if (msg.Recipients != null && msg.Recipients.Count > 0)
			{
				foreach (MessageRecipientEntity recipient in msg.Recipients)
				{
					if (String.IsNullOrEmpty(recipient.Name))
					{
						if (recipient.RecipientType == EntityType.User)
						{
							recipient.Name = GetDisplayName(recipient.RecipientId);
						}
						else if (recipient.RecipientType == EntityType.Role)
						{
							recipient.Name = DataAdapter.Fetch<RoleEntity>(recipient.RecipientId).Name;
						}
					}
				}
			}
			SaveWithAudit(msg);
		}

		private static void AssociateExistingRecipients(MessageEntity msg)
		{
			// developer note: tan.do @ digitalbeacon.com - 2007.09.14
			// If we do not handle existing child relationships, we can end up with
			// a constraint violation because NHibernate will insert new child
			// records before trying delete orphaned ones.
			if (msg.Id > 0)
			{
				MsgDao.Evict(msg);
				MessageEntity savedMsg = MsgDao.Fetch(msg.Id);
				if (msg.Recipients != null && msg.Recipients.Count > 0)
				{
					for (int i = 0; i < msg.Recipients.Count; i++)
					{
						foreach (MessageRecipientEntity r in savedMsg.Recipients)
						{
							if (r.RecipientType == msg.Recipients[i].RecipientType && r.RecipientId == msg.Recipients[i].RecipientId)
							{
								msg.Recipients[i].Id = r.Id;
								msg.Recipients[i].ModificationCounter = r.ModificationCounter;
								break;
							}
						}
					}
				}
				MsgDao.Evict(savedMsg);
			}
		}

		private void SendEmail(MessageEntity msg)
		{
			var qe = new QueuedEmailEntity { MessageId = msg.Id, SendDate = DateTime.Now };
			if (msg.SenderRole.HasValue && msg.SenderRole.Value == Role.Administrator)
			{
				var association = LookupService.Get<AssociationEntity>(msg.AssociationId);
				var address = DataAdapter.Fetch<AddressEntity>(association.AddressId);
				if (!String.IsNullOrEmpty(address.Email))
				{
					qe.SenderEmail = new MailAddress(address.Email, String.IsNullOrEmpty(msg.SenderName) ? association.Name : msg.SenderName).ToString();
				}
			}
			else
			{
				UserEntity sender = IdentityService.GetUser(msg.SenderId);
				qe.SenderEmail = new MailAddress(sender.Email, String.IsNullOrEmpty(msg.SenderName) ? sender.DisplayName : msg.SenderName).ToString();
			}
			if (msg.Importance == MessageImportance.Low)
			{
				qe.Priority = (int)MailPriority.Low;
			}
			else if (msg.Importance == MessageImportance.High)
			{
				qe.Priority = (int)MailPriority.High;
			}
			qe.Recipients = new List<QueuedEmailRecipientEntity>();
			foreach (MessageRecipientEntity mr in msg.Recipients)
			{
				if (mr.RecipientType == EntityType.User || mr.RecipientType == EntityType.Person)
				{
					var r = new QueuedEmailRecipientEntity { QueuedEmail = qe };
					if (mr.RecipientType == EntityType.User)
					{
						UserEntity user = IdentityService.GetUser(mr.RecipientId);
						r.Email = new MailAddress(user.Email, mr.Name ?? user.DisplayName).ToString();
						r.UserId = mr.RecipientId;
					}
					else
					{
						PersonEntity person = IdentityService.GetPerson(mr.RecipientId);
						r.Email = new MailAddress(person.Address.Email, mr.Name ?? person.DisplayName).ToString();
						r.PersonId = mr.RecipientId;
					}
					if (mr.Bcc)
					{
						r.Bcc = true;
					}
					else if (mr.Cc)
					{
						r.Cc = true;
					}
					qe.Recipients.Add(r);
				}
			}
			if (qe.Recipients.Count == 0)
			{
				throw new Exception("Email has no recipients");
			}
			qe.Attachments = new List<QueuedEmailAttachmentEntity>();
			foreach (MessageAttachmentEntity ma in msg.Attachments)
			{
				var attachment = new QueuedEmailAttachmentEntity
				{
					QueuedEmail = qe, 
					ContentType = ma.ContentType, 
					FileName = ma.FileName, 
					Data = ma.Data
				};
				qe.Attachments.Add(attachment);
			}
			MailService.QueueEmail(IdentityService.GetCurrentAssociationId(), qe);
		}

		private static void SendNotifications(MessageEntity msg)
		{
			var userIds = new HashSet<long>();
			foreach (var r in msg.Recipients)
			{
				if (r.RecipientType == EntityType.User && r.RecipientId != msg.SenderId)
				{
					userIds.Add(r.RecipientId);
				}
				else if (r.RecipientType == EntityType.Role)
				{
					IList<UserEntity> users = IdentityService.GetUsersByRole(msg.AssociationId, (Role)r.RecipientId);
					foreach (UserEntity user in users)
					{
						if (user.Id != msg.SenderId)
						{
							userIds.Add(user.Id);
						}
					}
				}
			}
			if (userIds.Count > 0)
			{
				IList<long> userIdsToSendNotification = new List<long>();
				foreach (long userId in userIds)
				{
					NotificationPreference np = NotificationPreference.FirstNewMessage;
					PreferenceEntity pref = PreferenceService.GetUserPreference(msg.AssociationId, userId, MessagingConstants.NotificationPreferenceKey);
					if (pref != null && pref.ValueAsInt64.HasValue)
					{
						np = (NotificationPreference)pref.ValueAsInt64.Value;
					}
					if (np == NotificationPreference.EveryMessage)
					{
						userIdsToSendNotification.Add(userId);
					}
					else if (np == NotificationPreference.FirstNewMessage || np == NotificationPreference.OnceDaily)
					{
						DateTime? dateLastRead = RecipientDao.FetchMostRecentReadDate(msg.AssociationId, userId);
						DateTime? dateLastSent = QueuedEmailDao.FetchMostRecentSentDate(msg.AssociationId, userId, ModuleSettingDefinition.MessagingNotificationMessage);
						if (dateLastSent == null 
							|| (np == NotificationPreference.FirstNewMessage && dateLastRead != null && dateLastSent.Value < dateLastRead.Value)
							|| (np == NotificationPreference.OnceDaily && dateLastRead != null && dateLastSent.Value < dateLastRead.Value && dateLastSent.Value.Date != DateTime.Today))
						{
							userIdsToSendNotification.Add(userId);
						}
					}
				}
				long associationId = IdentityService.GetCurrentAssociationId();
				foreach (long userId in userIdsToSendNotification)
				{
					var qe = new QueuedEmailEntity
					{
						SendDate = DateTime.Now,
						AssociationId = msg.AssociationId,
						Template = ModuleSettingDefinition.MessagingNotificationMessage,
						Recipients = new List<QueuedEmailRecipientEntity>()
					};
					var r = new QueuedEmailRecipientEntity { QueuedEmail = qe, UserId = userId };
					UserEntity user = IdentityService.GetUser(userId);
					r.Email = new MailAddress(user.Email, user.DisplayName).ToString();
					qe.Recipients.Add(r);
					MailService.QueueEmail(associationId, qe);
				}
			}
		}

		#endregion
	}
}
