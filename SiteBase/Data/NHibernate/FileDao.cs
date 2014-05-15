// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Criterion;
using DigitalBeacon.Data.NHibernate;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Model;

namespace DigitalBeacon.SiteBase.Data.NHibernate
{
	/// <summary>
	/// Data access object for secure files
	/// </summary>
	public class FileDao : BaseDao<FileEntity>, IFileDao
	{
		#region IFileDao Members

		public FileEntity Fetch(long? folderId, string filename)
		{
			return HibernateTemplate.Execute(session =>
			{
				var q = from x in session.Query<FileEntity>()
						where x.Deleted == null
							  && x.Filename == filename 
						select x;
				if (folderId != null)
				{
					q = q.Where(x => x.Folder.Id == folderId);
				}
				else
				{
					q = q.Where(x => x.Folder == null);
				}
				return q.SingleOrDefault();
				//var c = session.CreateCriteria(typeof(FileEntity))
				//	.Add(Restrictions.IsNull(FileEntity.DeletedProperty))
				//	.Add(Restrictions.Eq(FileEntity.FilenameProperty, filename));
				//if (folder != null)
				//{
				//	c.Add(Restrictions.Eq(GetPropertyName(FileEntity.FolderProperty, BaseEntity.IdProperty), folder.Id));
				//}
				//else
				//{
				//	c.Add(Restrictions.IsNull(FileEntity.FolderProperty));
				//}
				//return c.UniqueResult<FileEntity>();
			});
		}

		public IList<FileEntity> FetchByFolder(long folderId)
		{
			return HibernateTemplate.ExecuteFind<FileEntity>(
				delegate(ISession session)
				{
					return session.CreateCriteria(typeof(FileEntity))
						.Add(Expression.Eq(GetPropertyName(FileEntity.FolderProperty, BaseEntity.IdProperty), folderId))
						.Add(Expression.IsNull(FileEntity.DeletedProperty))
						.List<FileEntity>();
				});
		}

		public int FetchDeletedCount(long associationId)
		{
			return HibernateTemplate.Execute<int>(
				delegate(ISession session)
				{
					return session.CreateCriteria(typeof(FileEntity))
						.Add(Expression.Eq(FileEntity.AssociationIdProperty, associationId))
						.Add(Expression.IsNotNull(FileEntity.DeletedProperty))
						.SetProjection(Projections.Count(BaseEntity.IdProperty))
						.UniqueResult<int>();
				});
		}

		public IList<FileEntity> FetchDeleted(long associationId)
		{
			return HibernateTemplate.ExecuteFind<FileEntity>(
				delegate(ISession session)
				{
					return session.CreateCriteria(typeof(FileEntity))
						.Add(Expression.Eq(FileEntity.AssociationIdProperty, associationId))
						.Add(Expression.IsNotNull(FileEntity.DeletedProperty))
						.List<FileEntity>();
				});
		}

		#endregion
	}
}