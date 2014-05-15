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
	public class ServiceException : BaseException
	{
		public ServiceException(string message) 
			: base(message)
		{
		}

		public ServiceException(string format, params object[] args)
			: base(format, args)
		{
		}

		public ServiceException(System.Exception innerException, string format, params object[] args)
			: base(innerException, format, args)
		{
		}
	}
}
