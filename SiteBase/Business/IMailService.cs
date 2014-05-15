// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Collections.Generic;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Model;

namespace DigitalBeacon.SiteBase.Business
{
	public interface IMailService
	{
		/// <summary>
		/// Queue email message
		/// </summary>
		/// <param name="associationId">The association id.</param>
		/// <param name="msg">The message.</param>
		long QueueEmail(long associationId, MessageInfo msg);

		/// <summary>
		/// Queue email message
		/// </summary>
		/// <param name="associationId">The association id.</param>
		/// <param name="msg">The message.</param>
		/// <param name="isSystemError">if set to <c>true</c> message is system error.</param>
		long QueueEmail(long associationId, MessageInfo msg, bool isSystemError);

		/// <summary>
		/// Queue email message
		/// </summary>
		/// <param name="associationId">The association id.</param>
		/// <param name="qe">The qe.</param>
		void QueueEmail(long associationId, QueuedEmailEntity qe);

		/// <summary>
		/// Process email queue
		/// </summary>
		void ProcessEmailQueue();

		/// <summary>
		/// Send queued email
		/// </summary>
		/// <param name="qe"></param>
		QueuedEmailEntity SendQueuedEmail(QueuedEmailEntity qe);

		/// <summary>
		/// Gets the queued emails.
		/// </summary>
		/// <param name="searchInfo">The search info.</param>
		/// <returns></returns>
		IList<QueuedEmailEntity> GetQueuedEmails(QueuedEmailSearchInfo searchInfo);

		/// <summary>
		/// Gets the queued email count.
		/// </summary>
		/// <param name="searchInfo">The search info.</param>
		/// <returns></returns>
		long GetQueuedEmailCount(QueuedEmailSearchInfo searchInfo);

		/// <summary>
		/// Gets the queued email.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns></returns>
		QueuedEmailEntity GetQueuedEmail(long id);

		/// <summary>
		/// Saves the queued email.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns></returns>
		QueuedEmailEntity SaveQueuedEmail(QueuedEmailEntity entity);

		/// <summary>
		/// Deletes the queued email.
		/// </summary>
		/// <param name="id">The id.</param>
		void DeleteQueuedEmail(long id);
	}
}
