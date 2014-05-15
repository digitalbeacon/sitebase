// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;

namespace DigitalBeacon.Data
{
	/// <summary>
	/// A trivial exception class used to identify data exceptions
	/// </summary>
	public class DataException : BaseException
	{
		public DataException(string msg) : base(msg) {}
		public DataException(string msg, Exception innerException) : base(msg, innerException) {}
		public DataException(string format, params string[] args) : base(format, args) {}
		public DataException(Exception innerException, string format, params string[] args) : base(innerException, format, args) { }
	}
}
