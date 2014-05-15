// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using DigitalBeacon.Business;
using DigitalBeacon.Business.Support;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Data;
using DigitalBeacon.SiteBase.Model;

namespace DigitalBeacon.SiteBase.Business.Support
{
	public class FileService : BaseService, IFileService
	{
		#region Private Members

		//private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private static readonly VersionedEntityHelper VersionHelper = new VersionedEntityHelper();
		private static readonly IFolderDao FolderDao = ServiceFactory.Instance.GetService<IFolderDao>();
		private static readonly IFileDao FileDao = ServiceFactory.Instance.GetService<IFileDao>();
		private static readonly IPermissionService PermissionService = ServiceFactory.Instance.GetService<IPermissionService>();

		#endregion

		#region IFileService Members

		public IList<FolderEntity> GetFolders(long associationId, long? userId)
		{
			return FolderDao.FetchForDisplay(associationId, FolderType.File, userId);
		}

		public FolderEntity GetFolder(long id)
		{
			return DataAdapter.Fetch<FolderEntity>(id);
		}

		public FolderEntity SaveFolder(FolderEntity folder)
		{
			var isNew = folder.IsNew;
			folder.Type = FolderType.File;
			ValidateFolder(folder);
			var retVal = SaveWithAudit(folder);
			if (isNew)
			{
				var p = new PermissionEntity
							{
								Key1 = typeof(FolderEntity).Name, 
								Key2 = folder.Id, 
								EntityType = EntityType.RoleGroup, 
								EntityId = (long)RoleGroup.Authenticated, 
								Mask = 1
							};
				PermissionService.SavePermission(p);
			}
			return retVal;
		}

		public void DeleteFolder(long id)
		{
			var f = FolderDao.Fetch(id);
			if (f == null)
			{
				throw new ServiceException("Could not find {0} with Id [{1}] for deletion.", typeof(FolderEntity).FullName, id);
			}
			var permissions = PermissionService.GetPermissions(typeof(FolderEntity).Name, id, null, null, false);
			foreach (var p in permissions)
			{
				DeleteWithAudit(p);
			}
			DeleteFolder(f);
		}

		public int GetMaxFolderDisplayOrder(long associationId, long? parentFolderId)
		{
			return FolderDao.FetchMaxDisplayOrder(associationId, FolderType.File, parentFolderId);
		}

		public IList<FileEntity> GetFilesByFolder(long folderId)
		{
			return FileDao.FetchByFolder(folderId);
		}

		public FileEntity GetFile(long id)
		{
			return DataAdapter.Fetch<FileEntity>(id);
		}

		public FileEntity SaveFile(FileEntity file, bool archiveExisting)
		{
			ValidateFile(file);
			if (file.DataChanged)
			{
				if (!file.DataCompressed)
				{
					var loweredFilename = file.Filename.ToLower();
					if (!loweredFilename.EndsWith(".jpg")
						&& !loweredFilename.EndsWith(".jpeg")
						&& !loweredFilename.EndsWith(".zip")
						&& !loweredFilename.EndsWith(".gz")
						&& !loweredFilename.EndsWith(".rar"))
					{
						var ms = new MemoryStream();
						using (var zs = new GZipStream(ms, CompressionMode.Compress, true))
						{
							zs.Write(file.FileData.Data, 0, file.CachedSize);
							zs.Close();
						}
						file.FileData.Data = ms.ToArray();
						file.DataCompressed = true;
					}
				}
				file.LastModified = DateTime.Now;
				file.FileData = DataAdapter.Save(file.FileData);
			}
			//file.AssociationId = file.Folder.AssociationId;
			FileEntity retVal;
			if (archiveExisting && !file.IsNew)
			{
				if (file.DataChanged)
				{
					FileDao.Evict(file.FileData);
					var fileData = new FileDataEntity { Data = file.FileData.Data };
					file.FileData = fileData;
					DataAdapter.Save(file.FileData);
				}
				retVal = VersionHelper.Save<FileEntity, FileArchiveEntity>(file);
			}
			else
			{
				retVal = SaveWithAudit(file);
			}
			return retVal;
		}

		public void DeleteFile(long id, bool archiveExisting)
		{
			if (archiveExisting)
			{
				VersionHelper.Delete<FileEntity, FileArchiveEntity>(id);
			}
			else
			{
				var f = FileDao.Fetch(id);
				if (f == null)
				{
					throw new ServiceException("Could not find {0} with Id [{1}] for deletion.", typeof(FileEntity).FullName, id);
				}
				AuditingService.CreateAuditLogEntry(AuditAction.DeleteEntity, 0, f);
				VersionHelper.Purge<FileEntity, FileArchiveEntity>(f);
				DataAdapter.Delete(f.FileData);
			}
		}

		public FileArchiveEntity GetFileRevision(long archiveId)
		{
			return DataAdapter.Fetch<FileArchiveEntity>(archiveId);
		}

		public IList<FileArchiveEntity> GetPreviousFileVersions(long id)
		{
			return DataAdapter.FetchList<FileArchiveEntity>(FileArchiveEntity.RefIdProperty, id, FileArchiveEntity.CreatedProperty, true);
		}

		public void DeleteArchivedFile(long id)
		{
			DeleteWithAudit<FileArchiveEntity>(id);
		}

		public int GetDeletedFileCount(long associationId)
		{
			return FileDao.FetchDeletedCount(associationId);
		}

		public IList<FileEntity> GetDeletedFiles(long associationId)
		{
			return FileDao.FetchDeleted(associationId);
		}

		public void RestoreDeletedFiles(IList<long> fileIds, long? folderId)
		{
			FolderEntity folder = null;
			if (folderId.HasValue)
			{
				folder = FolderDao.Fetch(folderId.Value);
				if (folder == null)
				{
					throw new ServiceException("Could not find {0} with Id [{1}] to restore files.", typeof(FolderEntity).FullName, folderId);
				}
			}
			foreach (var fileId in fileIds)
			{
				var file = FileDao.Fetch(fileId);
				if (file == null || file.Deleted == null)
				{
					throw new ServiceException("Could not find deleted {0} with Id [{1}] to restore.", typeof(FileEntity).FullName, fileId);
				}
				var temp = FileDao.Fetch(folderId, file.Filename);
				if (temp != null)
				{
					var i = file.Filename.LastIndexOf(".");
					if (i > 0)
					{
						file.Filename = String.Format("{0}-{1}.{2}", file.Filename.Substring(0, i), file.Deleted.Value.ToString("yyyyMMddHHmmss"), file.Filename.Substring(i + 1));
					}
					else
					{
						file.Filename = String.Format("{0}-{1}", file.Filename, file.Deleted.Value.ToString("yyyyMMddHHmmss"));
					}
				}
				file.Folder = folder;
				file.Deleted = null;
				FileDao.Save(file);
			}
		}

		public void PurgeDeletedFiles(IList<long> fileIds)
		{
			foreach (var fileId in fileIds)
			{
				DeleteFile(fileId, false);
			}
		}

		public void PurgeDeletedFiles(long associationId)
		{
			var files = FileDao.FetchDeleted(associationId);
			foreach (var file in files)
			{
				DeleteFile(file.Id, false);
			}
		}

		#endregion

		#region Private Methods

		private void DeleteFolder(FolderEntity folder)
		{
			var files = DataAdapter.FetchList<FileEntity>(GetPropertyName(FileEntity.FolderProperty, BaseEntity.IdProperty), folder.Id);
			foreach (var file in files)
			{
				file.Folder = null;
				DeleteFile(file.Id, true);
			}
			var children = FolderDao.FetchChildren(folder.Id);
			foreach (var childFolder in children)
			{
				DeleteFolder(childFolder);
			}
			DeleteWithAudit<FolderEntity>(folder.Id);
		}

		private static void ValidateFolder(FolderEntity folder)
		{
			var messages = new List<string>();
			var f = FolderDao.Fetch(folder.AssociationId, FolderType.File, folder.ParentFolderId, folder.Name);
			if (f != null && f.Id != folder.Id)
			{
				messages.Add(FilesConstants.ErrorDuplicateFolder);
				throw new ServiceValidationException(messages);
			}
		}

		private static void ValidateFile(FileEntity file)
		{
			var messages = new List<string>();
			var f = FileDao.Fetch(file.Folder != null ? file.Folder.Id : (long?)null, file.Filename);
			if (f != null && f.Id != file.Id)
			{
				messages.Add(FilesConstants.ErrorDuplicateFile);
				throw new ServiceValidationException(messages);
			}
		}

		#endregion
	}
}
