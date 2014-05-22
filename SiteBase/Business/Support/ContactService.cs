// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Collections.Generic;
using DigitalBeacon.Business;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Model.Contacts;
using DigitalBeacon.Util;

namespace DigitalBeacon.SiteBase.Business.Support
{
	public class ContactService : BaseService, IContactService
	{
		#region Private Members

		private static readonly IFileService FileService = ServiceFactory.Instance.GetService<IFileService>();

		#endregion

		#region IContactService Members

		public ContactEntity GetContact(long contactId)
		{
			return DataAdapter.Fetch<ContactEntity>(contactId);
		}

		public long GetContactCount(ContactSearchInfo searchInfo)
		{
			return DataAdapter.FetchCount(ProcessSearchInfo(searchInfo));
		}

		public IList<ContactEntity> GetContacts(ContactSearchInfo searchInfo)
		{
			return DataAdapter.FetchList(ProcessSearchInfo(searchInfo));
		}

		public ContactEntity SaveContact(ContactEntity contact)
		{
			long? orphanedPhotoId = null;
			if (!contact.IsNew)
			{
				DataAdapter.Evict<ContactEntity>(contact);
				var existing = DataAdapter.Fetch<ContactEntity>(contact.Id);
				if (existing.Photo != null && (contact.Photo == null || contact.Photo.DataChanged))
				{
					orphanedPhotoId = existing.Photo.Id;
				}
			}
			if (contact.Photo != null && contact.Photo.DataChanged)
			{
				contact.Photo = FileService.SaveFile(contact.Photo, false);
			}
			var retVal = SaveWithAudit(contact);
			if (orphanedPhotoId.HasValue)
			{
				FileService.DeleteFile(orphanedPhotoId.Value, false);
			}
			return retVal;
		}

		public void DeleteContactPhoto(long contactId)
		{
			var contact = DataAdapter.Fetch<ContactEntity>(contactId);
			if (contact != null && contact.Photo != null)
			{
				var photo = contact.Photo;
				contact.Photo = null;
				contact.PhotoWidth = null;
				contact.PhotoHeight = null;
				FileService.DeleteFile(photo.Id, false);
			}
		}

		public void DeleteContact(long id)
		{
			DeleteContactPhoto(id);
			DataAdapter.FetchList<ContactCommentEntity>(ContactCommentEntity.ContactIdProperty, id).ForEach(c => DeleteWithAudit(c));
			DeleteWithAudit<ContactEntity>(id);
		}

		#endregion

		#region Private Methods

		private static SearchInfo<ContactEntity> ProcessSearchInfo(SearchInfo<ContactEntity> searchInfo)
		{
			if (searchInfo.ApplyDefaultFilters)
			{
				if (searchInfo.SearchText.HasText())
				{
					searchInfo.AddFilter(x => x.FirstName, ComparisonOperator.Contains, searchInfo.SearchText).Grouping = 1;
					searchInfo.AddFilter(x => x.LastName, ComparisonOperator.Contains, searchInfo.SearchText).Grouping = 2;
				}
				searchInfo.ApplyDefaultFilters = false;
			}
			return searchInfo;
		}

		#endregion
	}
}
