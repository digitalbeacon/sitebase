// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalBeacon.Business
{
	public class ServiceValidationException : ServiceException
	{
		private List<string> _validationMessages = new List<string>();

		public ServiceValidationException(IList<string> validationMessages)
			: base(String.Join(", ", new List<string>(validationMessages).ToArray()))
		{
			AddMessages(validationMessages);
		}

		public ServiceValidationException(string msg, IList<string> validationMessages)
			: base(msg)
		{
			AddMessages(validationMessages);
		}

		public ServiceValidationException(string validationMessage)
			: base(validationMessage)
		{
			AddMessages(validationMessage);
		}

		public IList<string> ValidationMessages
		{
			get { return _validationMessages; }
		}

		public void AddMessages(params string[] messages)
		{
			foreach (string message in messages)
			{
				_validationMessages.Add(message);
			}
		}

		public void AddMessages(IList<string> messages)
		{
			foreach (string message in messages)
			{
				_validationMessages.Add(message);
			}
		}
	}
}
