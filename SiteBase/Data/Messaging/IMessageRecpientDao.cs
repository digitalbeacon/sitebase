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
	/// The interface for accessing data associated with message recipients
	/// </summary>
	public interface IMessageRecipientDao : IDao<MessageRecipientEntity>
	{
		/// <summary>
		/// Get message recipient by message and recipient
		/// </summary>
		/// <param name="messageId">The message id.</param>
		/// <param name="recipientType">Type of the recipient.</param>
		/// <param name="recipientId">The recipient id.</param>
		/// <returns></returns>
		MessageRecipientEntity Fetch(long messageId, EntityType recipientType, long recipientId);

		/// <summary>
		/// fetch the most recent read date for the specified user
		/// </summary>
		/// <param name="associationId"></param>
		/// <param name="userId"></param>
		/// <returns></returns>
		DateTime? FetchMostRecentReadDate(long associationId, long userId);
	}
}
