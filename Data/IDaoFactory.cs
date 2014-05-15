// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;

using DigitalBeacon.Model;

namespace DigitalBeacon.Data
{
	/// <summary>
	/// An interface to get DAO implementations
	/// </summary>
	public interface IDaoFactory
	{
		/// <summary>
		/// Returns an implementation for the requested type
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		IDao<T> GetDao<T>() where T:class, IBaseEntity, new();

		/// <summary>
		/// Returns an implementation for the requested named type
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		INameDao<T> GetNameDao<T>() where T : class, INamedEntity, new();

		/// <summary>
		/// Returns an implementation for the requested archived type
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		IArchiveDao<T> GetArchiveDao<T>() where T : class, IArchivedEntity, new();

		/// <summary>
		/// Returns an implementation for the requested type which also
		/// satisfies the given interface.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="K">The requested interface.</typeparam>
		/// <returns></returns>
		K GetDao<T, K>() where T : class, IBaseEntity, new() where K : class;

	}
}
