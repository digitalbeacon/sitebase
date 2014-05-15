// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using System.Text;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Model.Messaging;

namespace DigitalBeacon.SiteBase.Business
{
	/// <summary>
	/// interface for business logic pertaining to messaging
	/// </summary>
	public interface IMessageService
	{
		/// <summary>
		/// Get message folder by Id
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns></returns>
		FolderEntity GetFolder(long id);

		/// <summary>
		/// Get message folders for user
		/// </summary>
		/// <param name="associationId">The association id.</param>
		/// <param name="userId">The user id.</param>
		/// <returns></returns>
		IList<FolderEntity> GetFolders(long associationId, long userId);

		/// <summary>
		/// Get a map of message folders and the current number of unread messages
		/// for each folder
		/// </summary>
		/// <param name="associationId">The association id.</param>
		/// <param name="userId">The user id.</param>
		/// <returns></returns>
		IDictionary<FolderEntity, int> GetUnreadMessageCounts(long associationId, long userId);

		/// <summary>
		/// Get unread message count for specified folder for given user
		/// </summary>
		/// <param name="associationId">The association id.</param>
		/// <param name="userId">The user id.</param>
		/// <param name="folderId">The folder id.</param>
		/// <returns></returns>
		int GetUnreadMessageCount(long associationId, long userId, long folderId);

		/// <summary>
		/// Get the favorite recipients for user
		/// </summary>
		/// <param name="associationId">The association id.</param>
		/// <param name="userId">The user id.</param>
		/// <returns></returns>
		FavoriteRecipients GetFavoriteRecipients(long associationId, long userId);

		/// <summary>
		/// Save the favorite recipients
		/// </summary>
		/// <param name="associationId">The association id.</param>
		/// <param name="userId">The user id.</param>
		/// <param name="recipients">The recipients.</param>
		void SaveFavoriteRecipients(long associationId, long userId, FavoriteRecipients recipients);

		/// <summary>
		/// Send a secure message
		/// </summary>
		/// <param name="msg">The message.</param>
		void SendMessage(MessageEntity msg);

		/// <summary>
		/// Saves a draft secure message
		/// </summary>
		/// <param name="msg">The message.</param>
		void SaveDraft(MessageEntity msg);

		/// <summary>
		/// Delete a draft message
		/// </summary>
		/// <param name="messageId">The message id.</param>
		void DeleteDraft(long messageId);

		/// <summary>
		/// Get secure message by Id
		/// </summary>
		/// <param name="messageId">The message id.</param>
		/// <returns></returns>
		MessageEntity GetMessage(long messageId);

		/// <summary>
		/// Get list of messages in specified folder for user
		/// </summary>
		/// <param name="associationId">The association id.</param>
		/// <param name="userId">The user id.</param>
		/// <param name="folderId">The folder id.</param>
		/// <returns></returns>
		IList<MessageEntity> GetMessages(long associationId, long userId, long folderId);

		/// <summary>
		/// Get list of sent messages for user
		/// </summary>
		/// <param name="associationId">The association id.</param>
		/// <param name="userId">The user id.</param>
		/// <returns></returns>
		IList<MessageEntity> GetSentMessages(long associationId, long userId);

		/// <summary>
		/// Get list of draft messages for user
		/// </summary>
		/// <param name="associationId">The association id.</param>
		/// <param name="userId">The user id.</param>
		/// <returns></returns>
		IList<MessageEntity> GetDraftMessages(long associationId, long userId);

		/// <summary>
		/// Get list of messages in the same thread as the given message
		/// </summary>
		/// <param name="messageId">The message id.</param>
		/// <returns></returns>
		IList<MessageEntity> GetRelatedMessages(long messageId);

		/// <summary>
		/// Get list of messages in the same thread as the given message
		/// </summary>
		/// <param name="messageId">The message id.</param>
		/// <param name="includeSpecifiedMessage">if set to <c>true</c> [include specified message].</param>
		/// <returns></returns>
		IList<MessageEntity> GetRelatedMessages(long messageId, bool includeSpecifiedMessage);

		/// <summary>
		/// Get message recipient
		/// </summary>
		/// <param name="messageId">The message id.</param>
		/// <param name="recipientId">The recipient id.</param>
		/// <returns></returns>
		MessageRecipientEntity GetMessageRecipient(long messageId, long recipientId);

		/// <summary>
		/// Get the display name for the user
		/// </summary>
		/// <param name="userId">The user id.</param>
		/// <returns></returns>
		string GetDisplayName(long userId);

		/// <summary>
		/// Get a dictionary of recipient names for message. The key is the
		/// recipient name and the value is true if the recipient was cc'ed
		/// and false otherwises
		/// </summary>
		/// <param name="messageId">The message id.</param>
		/// <returns></returns>
		IDictionary<string, bool> GetMessageRecipientNames(long messageId);

		/// <summary>
		/// Get list of notification preferences
		/// </summary>
		/// <returns></returns>
		IList<NotificationPreferenceEntity> GetNotificationPreferences();

		#region Message Action methods

		/// <summary>
		/// Sets the flag value for a message recipient
		/// </summary>
		/// <param name="messageId"></param>
		/// <param name="recipientId"></param>
		/// <param name="flag"></param>
		void SetMessageFlag(long messageId, long recipientId, bool flag);

		/// <summary>
		/// Sets the flag value for a message recipient
		/// </summary>
		/// <param name="messageIds"></param>
		/// <param name="recipientId"></param>
		/// <param name="flag"></param>
		void SetMessagesFlag(ICollection<long> messageIds, long recipientId, bool flag);

		/// <summary>
		/// Sets the read value for a message recipient
		/// </summary>
		/// <param name="messageId"></param>
		/// <param name="recipientId"></param>
		/// <param name="read"></param>
		void SetMessageRead(long messageId, long recipientId, bool read);

		/// <summary>
		/// Sets the read value for a message recipient
		/// </summary>
		/// <param name="messageIds"></param>
		/// <param name="recipientId"></param>
		/// <param name="read"></param>
		void SetMessagesRead(ICollection<long> messageIds, long recipientId, bool read);

		/// <summary>
		/// Move message to folder
		/// </summary>
		/// <param name="messageId">The message id.</param>
		/// <param name="recipientId">The recipient id.</param>
		/// <param name="folderId">The folder id.</param>
		void MoveMessage(long messageId, long recipientId, long folderId);

		/// <summary>
		/// Move message to folder
		/// </summary>
		/// <param name="messageIds">The message ids.</param>
		/// <param name="recipientId">The recipient id.</param>
		/// <param name="folderId">The folder id.</param>
		void MoveMessages(ICollection<long> messageIds, long recipientId, long folderId);

		#endregion

		#region Attachment methods

		/// <summary>
		/// Returns true if message has at least one file or clinical attachment
		/// </summary>
		/// <param name="messageId"></param>
		/// <returns></returns>
		bool HasAttachment(long messageId);

		/// <summary>
		/// Get message attachment by Id
		/// </summary>
		/// <param name="attachmentId"></param>
		/// <returns></returns>
		MessageAttachmentEntity GetAttachment(long attachmentId);

		/// <summary>
		/// Save the message attachment
		/// </summary>
		/// <param name="attachment"></param>
		void SaveAttachment(MessageAttachmentEntity attachment);

		/// <summary>
		/// Delete the message attachment for the given Id
		/// </summary>
		/// <param name="attachmentId"></param>
		void DeleteAttachment(long attachmentId);

		#endregion

		#region Template methods

		/// <summary>
		/// Get message template names for association
		/// </summary>
		/// <param name="associationId"></param>
		/// <returns></returns>
		IList<MessageTemplateEntity> GetTemplateNames(long associationId);

		/// <summary>
		/// Get message template by Id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		MessageTemplateEntity GetTemplate(long id);

		/// <summary>
		/// Save the message template
		/// </summary>
		/// <param name="template"></param>
		void SaveTemplate(MessageTemplateEntity template);

		/// <summary>
		/// Delete message template with Id
		/// </summary>
		/// <param name="id"></param>
		void DeleteTemplate(long id);

		#endregion	
	}
}
