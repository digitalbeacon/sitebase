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
	public interface IArchivedEntity : IBaseEntity
	{
		/// <summary>
		/// The Id for the referenced entity
		/// </summary>
		long RefId { get; set; }

		/// <summary>
		/// The creation time of the entity
		/// </summary>
		DateTime Created { get; set; }

		/// <summary>
		/// The date/time the entity was archived
		/// </summary>
		DateTime Archived { get; set; }
	}
}
