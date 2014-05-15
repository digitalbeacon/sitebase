// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using System.Text;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Model.Content;
using DigitalBeacon.Model;

namespace DigitalBeacon.SiteBase.Business
{
	public interface IContentService
	{
		/// <summary>
		/// Get content group type
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		ContentGroupTypeEntity GetContentGroupType(ContentGroupType type);

		/// <summary>
		/// Gets the content groups.
		/// </summary>
		/// <param name="associationId">The association id.</param>
		/// <param name="forDisplay">if set to <c>true</c> [for display].</param>
		/// <param name="searchInfo">The search info.</param>
		/// <returns></returns>
		IList<ContentGroupEntity> GetContentGroups(long associationId, bool forDisplay, SearchInfo<ContentGroupEntity> searchInfo);

		/// <summary>
		/// Gets the content group count.
		/// </summary>
		/// <param name="associationId">The association id.</param>
		/// <param name="forDisplay">if set to <c>true</c> [for display].</param>
		/// <param name="searchInfo">The search info.</param>
		/// <returns></returns>
		long GetContentGroupCount(long associationId, bool forDisplay, SearchInfo<ContentGroupEntity> searchInfo);

		/// <summary>
		/// Get content group by Id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		ContentGroupEntity GetContentGroup(long id);

		/// <summary>
		/// Get content group by association and name
		/// </summary>
		/// <param name="associationId"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		ContentGroupEntity GetContentGroup(long associationId, string name);

		/// <summary>
		/// Get content entries for content group
		/// </summary>
		/// <param name="groupId">The group id.</param>
		/// <param name="forDisplay">if set to <c>true</c> [for display].</param>
		/// <param name="searchInfo">The search info.</param>
		/// <returns></returns>
		IList<ContentEntryEntity> GetEntries(long groupId, bool forDisplay, SearchInfo<ContentEntryEntity> searchInfo);

		/// <summary>
		/// Gets the entry count for content group.
		/// </summary>
		/// <param name="groupId">The group id.</param>
		/// <param name="forDisplay">if set to <c>true</c> [for display].</param>
		/// <param name="searchInfo">The search info.</param>
		/// <returns></returns>
		long GetEntryCount(long groupId, bool forDisplay, SearchInfo<ContentEntryEntity> searchInfo);

		/// <summary>
		/// Save content group
		/// </summary>
		/// <param name="contentGroup"></param>
		/// <returns></returns>
		ContentGroupEntity SaveContentGroup(ContentGroupEntity contentGroup);

		/// <summary>
		/// Deletes the content group.
		/// </summary>
		/// <param name="id">The id.</param>
		void DeleteContentGroup(long id);

		/// <summary>
		/// Get content entry by Id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		ContentEntryEntity GetEntry(long id);

		/// <summary>
		/// Get max entry display order for specified group
		/// </summary>
		/// <param name="groupId"></param>
		/// <returns></returns>
		int GetMaxEntryDisplayOrder(long groupId);

		/// <summary>
		/// Save content entry
		/// </summary>
		/// <param name="entry"></param>
		/// <returns></returns>
		ContentEntryEntity SaveEntry(ContentEntryEntity entry);

		/// <summary>
		/// Delete content entry
		/// </summary>
		/// <param name="id"></param>
		void DeleteEntry(long id);
	}
}
