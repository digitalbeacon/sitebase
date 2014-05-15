// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using DigitalBeacon.Data;
using DigitalBeacon.SiteBase.Model;

namespace DigitalBeacon.SiteBase.Data
{
	/// <summary>
	/// The interface for accessing data associated with secure message folders
	/// </summary>
	public interface IFolderDao : IDao<FolderEntity>
	{
		/// <summary>
		/// Fetch folder by specified parameters
		/// </summary>
		/// <param name="associationId">The association id.</param>
		/// <param name="folderType">Type of the folder.</param>
		/// <param name="parentFolderId">The parent folder id.</param>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		FolderEntity Fetch(long? associationId, FolderType folderType, long? parentFolderId, string name);

		///// <summary>
		///// Fetch list of folders for user
		///// </summary>
		///// <param name="associationId"></param>
		///// <param name="userId"></param>
		///// <returns></returns>
		//IList<FolderEntity> Fetch(long associationId, long userId);

		/// <summary>
		/// Fetch the max folder display order value for the give association and level
		/// </summary>
		/// <param name="associationId">The association id.</param>
		/// <param name="type">The type.</param>
		/// <param name="parentFolderId">The parent folder id.</param>
		/// <returns></returns>
		int FetchMaxDisplayOrder(long associationId, FolderType type, long? parentFolderId);

		/// <summary>
		/// Fetch list of folders to display for user
		/// </summary>
		/// <param name="associationId">The association id.</param>
		/// <param name="type">The type.</param>
		/// <param name="userId">The user id.</param>
		/// <returns></returns>
		IList<FolderEntity> FetchForDisplay(long associationId, FolderType type, long? userId);

		/// <summary>
		/// Fetch list of child folders for specified folder
		/// </summary>
		/// <param name="folderId"></param>
		/// <returns></returns>
		IList<FolderEntity> FetchChildren(long folderId);

	}
}
