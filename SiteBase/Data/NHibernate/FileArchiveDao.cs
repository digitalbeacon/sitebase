// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using DigitalBeacon.Data.NHibernate;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Model;

namespace DigitalBeacon.SiteBase.Data.NHibernate
{
	/// <summary>
	/// Data access object for secure file archives
	/// </summary>
	public class FileArchiveDao : ArchiveDao<FileArchiveEntity>
	{
		public override void Delete(FileArchiveEntity entity)
		{
			bool deleteFileData = HibernateTemplate.Execute<bool>(
				delegate(ISession session)
				{
					int dataReferenceCount = session.CreateCriteria(typeof(FileEntity))
						.Add(Expression.Eq(GetPropertyName(FileEntity.FileDataProperty, BaseEntity.IdProperty), entity.FileData.Id))
						.SetProjection(Projections.Count(BaseEntity.IdProperty))
						.UniqueResult<int>();
					dataReferenceCount += session.CreateCriteria(typeof(FileArchiveEntity))
						.Add(Expression.Eq(GetPropertyName(FileArchiveEntity.FileDataProperty, BaseEntity.IdProperty), entity.FileData.Id))
						.SetProjection(Projections.Count(BaseEntity.IdProperty))
						.UniqueResult<int>();
					return dataReferenceCount == 1;
				});
			base.Delete(entity);
			if (deleteFileData)
			{
				HibernateTemplate.Delete(entity.FileData);
			}
		}
	}
}