// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using Iesi.Collections.Generic;
using DigitalBeacon.Model;

namespace DigitalBeacon.SiteBase.Model.Contacts
{
	public class ContactCommentEntity : GeneratedContactCommentEntity
	{
		public virtual bool Flagged
		{
			get { return CommentType != null ? CommentType.Flagged : false; }
		}
	}
}
