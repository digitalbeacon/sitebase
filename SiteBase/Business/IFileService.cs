// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Collections.Generic;
using DigitalBeacon.SiteBase.Model;

namespace DigitalBeacon.SiteBase.Business
{
	public interface IFileService
	{
		/// <summary>
		/// Get list of folders for association
		/// </summary>
		/// <param name="associationId">The association id.</param>
		/// <param name="userId">The user id.</param>
		/// <returns></returns>
		IList<FolderEntity> GetFolders(long associationId, long? userId);

		/// <summary>
		/// Get folder by Id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		FolderEntity GetFolder(long id);

		/// <summary>
		/// Save folder
		/// </summary>
		/// <param name="folder"></param>
		/// <returns></returns>
		FolderEntity SaveFolder(FolderEntity folder);

		/// <summary>
		/// Delete folder
		/// </summary>
		/// <param name="id"></param>
		void DeleteFolder(long id);

		/// <summary>
		/// Get max display order for folder
		/// </summary>
		/// <param name="associationId"></param>
		/// <param name="parentFolderId"></param>
		/// <returns></returns>
		int GetMaxFolderDisplayOrder(long associationId, long? parentFolderId);

		/// <summary>
		/// Get all files for folder
		/// </summary>
		/// <param name="folderId"></param>
		/// <returns></returns>
		IList<FileEntity> GetFilesByFolder(long folderId);

		/// <summary>
		/// Get file by Id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		FileEntity GetFile(long id);

		/// <summary>
		/// Save file
		/// </summary>
		/// <param name="file"></param>
		/// <param name="archiveExisting"></param>
		/// <returns></returns>
		FileEntity SaveFile(FileEntity file, bool archiveExisting);

		/// <summary>
		/// Delete file
		/// </summary>
		/// <param name="id"></param>
		/// <param name="archiveExisting"></param>
		void DeleteFile(long id, bool archiveExisting);

		/// <summary>
		/// Get file archive entity by Id
		/// </summary>
		/// <param name="archiveId"></param>
		/// <returns></returns>
		FileArchiveEntity GetFileRevision(long archiveId);

		/// <summary>
		/// Get previous version of a file
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		IList<FileArchiveEntity> GetPreviousFileVersions(long id);

		/// <summary>
		/// Delete an archived version of a file
		/// </summary>
		/// <param name="id"></param>
		void DeleteArchivedFile(long id);

		/// <summary>
		/// Get count of deleted files for an association
		/// </summary>
		/// <param name="associationId"></param>
		/// <returns></returns>
		int GetDeletedFileCount(long associationId);

		/// <summary>
		/// Get list of deleted files for association
		/// </summary>
		/// <param name="associationId"></param>
		/// <returns></returns>
		IList<FileEntity> GetDeletedFiles(long associationId);

		/// <summary>
		/// Restore specified files to given folder
		/// </summary>
		/// <param name="fileIds"></param>
		/// <param name="folderId"></param>
		void RestoreDeletedFiles(IList<long> fileIds, long? folderId);

		/// <summary>
		/// Purge specified files
		/// </summary>
		/// <param name="fileIds"></param>
		void PurgeDeletedFiles(IList<long> fileIds);

		/// <summary>
		/// Purge all deleted files for association
		/// </summary>
		/// <param name="associationId"></param>
		void PurgeDeletedFiles(long associationId);
	}
}
