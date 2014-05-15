// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using DigitalBeacon.Model;
using DigitalBeacon.Data;
using DigitalBeacon.SiteBase.Model;

namespace DigitalBeacon.SiteBase.Data
{
	/// <summary>
	/// The interface for accessing data associated with secure files
	/// </summary>
	public interface IFileDao : IDao<FileEntity>
	{
		/// <summary>
		/// Fetch entity with given folder and filename
		/// </summary>
		/// <param name="folderId">The folder id.</param>
		/// <param name="filename">The filename.</param>
		/// <returns></returns>
		FileEntity Fetch(long? folderId, string filename);

		/// <summary>
		/// Fetch list of files for given folder
		/// </summary>
		/// <param name="folderId"></param>
		/// <returns></returns>
		IList<FileEntity> FetchByFolder(long folderId);

		/// <summary>
		/// Fetch count of delete file for association
		/// </summary>
		/// <param name="associationId"></param>
		/// <returns></returns>
		int FetchDeletedCount(long associationId);

		/// <summary>
		/// Fetch list of deleted files for association
		/// </summary>
		/// <param name="associationId"></param>
		/// <returns></returns>
		IList<FileEntity> FetchDeleted(long associationId);
	}
}
