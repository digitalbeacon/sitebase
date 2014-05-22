// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using DigitalBeacon.Business;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Business;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Web;
using DigitalBeacon.SiteBase.Web.Models;

namespace DigitalBeacon.SiteBase.Controllers
{
	[Authorization(Role.Administrator,Role.SiteManager)]
	public class FilesController : EntityController<FileEntity, EntityModel>
	{
		private static readonly IFileService FileService = ServiceFactory.Instance.GetService<IFileService>();

		public static FileResult GetFileResult(long fileId)
		{
			var file = FileService.GetFile(fileId);
			return new FileStreamResult(new MemoryStream(file.FileData.Data), file.ContentType);
		}

		#region EntityController

		public ActionResult Download(long id)
		{
			var file = GetEntity(id);
			return new FileStreamResult(new MemoryStream(file.FileData.Data), file.ContentType) { FileDownloadName = file.Filename };
		}

		public override ActionResult Show(long id)
		{
			var file = GetEntity(id);
			return new FileStreamResult(new MemoryStream(file.FileData.Data), file.ContentType);
		}

		protected override string GetDescription(EntityModel model)
		{
			return String.Empty;
		}

		protected override IEnumerable<FileEntity> GetEntities(SearchInfo<FileEntity> searchInfo, ListModelBase model)
		{
			throw new NotImplementedException();
		}

		protected override long GetEntityCount(SearchInfo<FileEntity> searchInfo, ListModelBase model)
		{
			throw new NotImplementedException();
		}

		protected override FileEntity GetEntity(long id)
		{
			return FileService.GetFile(id);
		}

		protected override EntityModel ConstructModel(FileEntity entity)
		{
			throw new NotImplementedException();
		}

		protected override FileEntity ConstructEntity(EntityModel model)
		{
			throw new NotImplementedException();
		}

		protected override FileEntity SaveEntity(FileEntity entity, EntityModel model)
		{
			throw new NotImplementedException();
		}

		protected override void DeleteEntity(long id)
		{
			throw new NotImplementedException();
		}

		#endregion
	}

}
