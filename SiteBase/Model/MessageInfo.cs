// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
namespace DigitalBeacon.SiteBase.Model
{
	public class MessageInfo
	{
		private long _userId;
		private string _subject;
		private string _body;
		private DateTime _sendDate = DateTime.Now;

		public long UserId
		{
			get { return _userId; }
			set { _userId = value; }
		}

		public string Subject
		{
			get { return _subject; }
			set { _subject = value; }
		}

		public string Body
		{
			get { return _body; }
			set { _body = value; }
		}

		public DateTime SendDate
		{
			get { return _sendDate; }
			set { _sendDate = value; }
		}
	}
}
