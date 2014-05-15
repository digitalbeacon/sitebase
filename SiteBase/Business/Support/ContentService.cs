// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Collections.Generic;
using DigitalBeacon.Business;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Data.Content;
using DigitalBeacon.SiteBase.Model.Content;
using DigitalBeacon.Util;

namespace DigitalBeacon.SiteBase.Business.Support
{
	public class ContentService : BaseService, IContentService
	{
		#region Private Members

		private static readonly IContentEntryDao EntryDao = ServiceFactory.Instance.GetService<IContentEntryDao>();
		private static readonly IContentGroupDao GroupDao = ServiceFactory.Instance.GetService<IContentGroupDao>();

		#endregion

		#region IContentService Members

		public ContentGroupTypeEntity GetContentGroupType(ContentGroupType type)
		{
			return DataAdapter.Fetch<ContentGroupTypeEntity>((long)type);
		}

		public IList<ContentGroupEntity> GetContentGroups(long associationId, bool forDisplay, SearchInfo<ContentGroupEntity> searchInfo)
		{
			if (searchInfo == null)
			{
				searchInfo = new SearchInfo<ContentGroupEntity>();
			}
			return GroupDao.FetchList(PrepareSearch(searchInfo, associationId, forDisplay));
		}

		public long GetContentGroupCount(long associationId, bool forDisplay, SearchInfo<ContentGroupEntity> searchInfo)
		{
			if (searchInfo == null)
			{
				searchInfo = new SearchInfo<ContentGroupEntity>();
			}
			return GroupDao.FetchCount(PrepareSearch(searchInfo, associationId, forDisplay));
		}

		public ContentGroupEntity GetContentGroup(long id)
		{
			return GroupDao.Fetch(id);
		}

		public ContentGroupEntity GetContentGroup(long associationId, string name)
		{
			return GroupDao.Fetch(associationId, name);
		}

		public IList<ContentEntryEntity> GetEntries(long groupId, bool forDisplay, SearchInfo<ContentEntryEntity> searchInfo)
		{
			if (searchInfo == null)
			{
				searchInfo = new SearchInfo<ContentEntryEntity>();
			}
			return EntryDao.FetchList(PrepareSearch(searchInfo, groupId, forDisplay));
		}

		public long GetEntryCount(long groupId, bool forDisplay, SearchInfo<ContentEntryEntity> searchInfo)
		{
			if (searchInfo == null)
			{
				searchInfo = new SearchInfo<ContentEntryEntity>();
			}
			return EntryDao.FetchCount(PrepareSearch(searchInfo, groupId, forDisplay));
		}

		public ContentGroupEntity SaveContentGroup(ContentGroupEntity contentGroup)
		{
			if (contentGroup.CalculateDisplayOrder)
			{
				contentGroup.DisplayOrder = GroupDao.FetchMaxDisplayOrder(contentGroup.Id) + 1;
			}
			return SaveWithAudit(contentGroup);
		}

		public void DeleteContentGroup(long id)
		{
			foreach (var x in EntryDao.FetchList(
				GetPropertyName(ContentEntryEntity.ContentGroupProperty, ContentGroupEntity.IdProperty), id))
			{
				DeleteWithAudit(x);
			}
			DeleteWithAudit<ContentGroupEntity>(id);
		}

		public ContentEntryEntity GetEntry(long id)
		{
			return EntryDao.Fetch(id);
		}

		public int GetMaxEntryDisplayOrder(long groupId)
		{
			return EntryDao.FetchMaxDisplayOrder(groupId);
		}

		public ContentEntryEntity SaveEntry(ContentEntryEntity entry)
		{
			if (entry.CalculateDisplayOrder)
			{
				entry.DisplayOrder = EntryDao.FetchMaxDisplayOrder(entry.ContentGroup.Id) + 1;
			}
			return SaveWithAudit(entry);
		}

		public void DeleteEntry(long id)
		{
			DeleteWithAudit<ContentEntryEntity>(id);
		}

		#endregion

		#region Private Methods

		protected SearchInfo<ContentGroupEntity> PrepareSearch(SearchInfo<ContentGroupEntity> searchInfo, long associationId, bool forDisplay)
		{
			if (searchInfo.ApplyDefaultFilters)
			{
				searchInfo.AddFilter(x => x.AssociationId, associationId);
				if (forDisplay)
				{
					searchInfo.AddFilter(x => x.DisplayOrder, ComparisonOperator.GreaterThan, 0);
				}
				if (searchInfo.SearchText.HasText())
				{
					searchInfo.AddFilter(x => x.Name, ComparisonOperator.Contains, searchInfo.SearchText).Grouping = 1;
					searchInfo.AddFilter(x => x.Title, ComparisonOperator.Contains, searchInfo.SearchText).Grouping = 2;
					searchInfo.AddFilter(x => x.ContentGroupType.Name, ComparisonOperator.Contains, searchInfo.SearchText).Grouping = 3;
				}
				searchInfo.ApplyDefaultFilters = false;
			}
			return searchInfo;
		}

		protected SearchInfo<ContentEntryEntity> PrepareSearch(SearchInfo<ContentEntryEntity> searchInfo, long groupId, bool forDisplay)
		{
			if (searchInfo.ApplyDefaultFilters)
			{
				searchInfo.AddFilter(x => x.ContentGroup.Id, groupId);
				if (forDisplay)
				{
					searchInfo.AddFilter(x => x.DisplayOrder, ComparisonOperator.GreaterThan, 0);
				}
				if (searchInfo.SearchText.HasText())
				{
					searchInfo.AddFilter(x => x.Title, ComparisonOperator.Contains, searchInfo.SearchText).Grouping = 1;
					searchInfo.AddFilter(x => x.Body, ComparisonOperator.Contains, searchInfo.SearchText).Grouping = 2;
				}
				searchInfo.ApplyDefaultFilters = false;
			}
			return searchInfo;
		}

		#endregion
	}
}
