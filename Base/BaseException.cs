// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;

namespace DigitalBeacon
{
	public class BaseException : Exception
	{
		public BaseException(string message) 
			: base(message)
		{
		}

		public BaseException(string format, params object[] args)
			: base(String.Format(format, args))
		{
		}

		public BaseException(Exception innerException, string format, params object[] args)
			: base(String.Format(format, args), innerException)
		{
		}
	}
}
