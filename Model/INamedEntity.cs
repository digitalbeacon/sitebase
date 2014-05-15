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
	/// <summary>
	///	This is the interface for all business objects that have a name property
	/// </summary>
	public interface INamedEntity : IBaseEntity
	{
		/// <summary>
		/// The name of the entity
		/// </summary>
		string Name { get; set; }
	}
}
