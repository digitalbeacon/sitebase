// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections;

namespace DigitalBeacon.Model
{
	///	This is the interface for all business objects that have a code property
	/// </summary>
	public interface ICodedEntity : IBaseEntity
	{
		/// <summary>
		/// The code property
		/// </summary>
		string Code { get; set; }
	}
}
