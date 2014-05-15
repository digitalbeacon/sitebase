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
using DigitalBeacon.SiteBase.Model.Messaging;
using DigitalBeacon.Model;

namespace DigitalBeacon.SiteBase.Data.Messaging
{
	/// <summary>
	/// The interface for accessing data associated with secure message folders
	/// </summary>
	public interface IMessageTemplateDao : IDao<MessageTemplateEntity>
	{
		/// <summary>
		/// Fetch name list for association
		/// </summary>
		/// <param name="associationId"></param>
		/// <returns></returns>
		IList<MessageTemplateEntity> FetchNameList(long associationId);

	}
}
