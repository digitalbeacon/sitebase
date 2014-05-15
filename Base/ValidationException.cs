// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;

namespace DigitalBeacon
{
	public class ValidationException : BaseException
	{
		public ValidationException(string message) 
			: base(message)
		{
		}

		public ValidationException(string format, params object[] args)
			: base(format, args)
		{
		}

		public ValidationException(Exception innerException, string format, params object[] args)
			: base(innerException, format, args)
		{
		}
	}
}
