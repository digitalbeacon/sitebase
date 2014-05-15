// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Collections.Generic;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.Model;

namespace DigitalBeacon.SiteBase.Business
{
	public interface IContactService
	{
		#region Contacts

		/// <summary>
		/// Gets the contact count.
		/// </summary>
		/// <param name="searchInfo">The search info.</param>
		long GetContactCount(SearchInfo<ContactEntity> searchInfo);

		/// <summary>
		/// Gets the contacts.
		/// </summary>
		/// <param name="searchInfo">The search info.</param>
		IList<ContactEntity> GetContacts(SearchInfo<ContactEntity> searchInfo);

		/// <summary>
		/// Saves the contact.
		/// </summary>
		/// <param name="contact">The contact.</param>
		/// <returns></returns>
		ContactEntity SaveContact(ContactEntity contact);

		/// <summary>
		/// Deletes the contact photo.
		/// </summary>
		/// <param name="contactId">The contact id.</param>
		void DeleteContactPhoto(long contactId);

		/// <summary>
		/// Deletes the contact.
		/// </summary>
		/// <param name="id">The id.</param>
		void DeleteContact(long id);

		#endregion
	}
}
