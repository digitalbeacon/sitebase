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
	/// Data access object for message attachments
	/// </summary>
	public class MessageAttachmentDao : BaseDao<MessageAttachmentEntity>, IMessageAttachmentDao
	{
		private static readonly string FetchByMessageQuery = FromClause + " where x.MessageId = ? order by x.Id";

		#region IMessageAttachmentDao Members

		public IList<MessageAttachmentEntity> FetchByMessage(long messageId)
		{
			return HibernateTemplate.Find<MessageAttachmentEntity>(FetchByMessageQuery, messageId);
		}

		#endregion
	}
}