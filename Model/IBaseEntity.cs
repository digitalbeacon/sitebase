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
	///	This is the interface for all business objects
	/// </summary>
	public interface IBaseEntity
	{
		/// <summary>
		/// The primary key for the entity
		/// </summary>
		long Id { get; set; }

		/// <summary>
		/// The version of the entity, used for optimistic concurrency
		/// </summary>
		long ModificationCounter { get; set; }

		/// <summary>
		/// Flag that designates whether lazy collections have been initialized
		/// </summary>		
		bool AreLazyCollectionsInitialized { get; set; }

		/// <summary>
		/// Determine whether entity has been previously persisted
		/// </summary>
		bool IsNew { get; }
  
	}
}
