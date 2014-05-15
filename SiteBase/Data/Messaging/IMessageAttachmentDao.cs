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
	/// The interface for accessing data associated with message attachments
	/// </summary>
	public interface IMessageAttachmentDao : IDao<MessageAttachmentEntity>
	{
		/// <summary>
		/// Get message attachments for message
		/// </summary>
		/// <param name="messageId"></param>
		/// <returns></returns>
		IList<MessageAttachmentEntity> FetchByMessage(long messageId);
	}
}
