// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using DigitalBeacon.Model;
using DigitalBeacon.Data;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Model.Messaging;

namespace DigitalBeacon.SiteBase.Data.Messaging

{
	/// <summary>
	/// The interface for accessing data associated with secure messages
	/// </summary>
	public interface IMessageDao : IDao<MessageEntity>
	{
		/// <summary>
		/// Get list of messages in specified folder for user
		/// </summary>
		/// <param name="associationId"></param>
		/// <param name="userId"></param>
		/// <param name="folderId"></param>
		/// <returns></returns>
		IList<MessageEntity> Fetch(long associationId, long userId, long folderId);

		/// <summary>
		/// Get unread message count in specified folder for user
		/// </summary>
		/// <param name="associationId"></param>
		/// <param name="userId"></param>
		/// <param name="folderId"></param>
		/// <returns></returns>
		int FetchUnreadCount(long associationId, long userId, long folderId);

		/// <summary>
		/// Get list of sent messages for user
		/// </summary>
		/// <param name="associationId"></param>
		/// <param name="userId"></param>
		/// <returns></returns>
		IList<MessageEntity> FetchSent(long associationId, long userId);

		/// <summary>
		/// Get list of draft messages for user
		/// </summary>
		/// <param name="associationId"></param>
		/// <param name="userId"></param>
		/// <returns></returns>
		IList<MessageEntity> FetchDrafts(long associationId, long userId);

		/// <summary>
		/// Get list of messages with the given relatedId
		/// </summary>
		/// <param name="relatedId"></param>
		/// <returns></returns>
		IList<MessageEntity> FetchRelated(long relatedMessageId);

		/// <summary>
		/// Returns true if message has at least one file or clinical attachment
		/// </summary>
		/// <param name="messageId"></param>
		/// <returns></returns>
		bool HasAttachment(long messageId);

	}
}
