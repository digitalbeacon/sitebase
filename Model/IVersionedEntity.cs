// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;

namespace DigitalBeacon.Model
{
	/// <summary>
	///	This is the interface for all business objects that support versioning
	/// </summary>
	public interface IVersionedEntity : IBaseEntity
	{
		/// <summary>
		/// The creation time of the entity
		/// </summary>
		DateTime Created { get; }

		/// <summary>
		/// The timestamp that the entity was deleted. The value should be null
		/// for entities that are currently active.
		/// </summary>
		DateTime? Deleted { get; set; }
	}
}
