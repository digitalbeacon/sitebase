// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using DigitalBeacon.Data;
using DigitalBeacon.SiteBase.Model;

namespace DigitalBeacon.SiteBase.Data
{
	public interface IQueuedEmailDao : IDao<QueuedEmailEntity>
	{
		/// <summary>
		/// Fetch list of queued emails to send
		/// </summary>
		/// <returns></returns>
		IList<QueuedEmailEntity> FetchMessagesToSend();

		/// <summary>
		/// Fetch most recent sent date
		/// </summary>
		/// <param name="associationId"></param>
		/// <param name="userId"></param>
		/// <param name="template"></param>
		/// <returns></returns>
		DateTime? FetchMostRecentSentDate(long associationId, long userId, ModuleSettingDefinition template);

		/// <summary>
		/// Clear NHibernate session
		/// </summary>
		void ClearSession();
	}
}
