// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Dom;
using System.Html;
using DigitalBeacon;
using DigitalBeacon.SiteBase;
using DigitalBeacon.SiteBase.Mobile;
using jQueryLib;
using ng;

namespace DigitalBeacon.SiteBase.Mobile.Contacts
{
	public class ContactDetailsController : BaseDetailsController
	{
		private ContactService _contactService;

		public dynamic contact;

		public ContactDetailsController(Scope scope, dynamic state, ILocation location, ContactService contactService)
		{
			Scope = scope;
			State = state;
			Location = location;
			_contactService = contactService;
		}

		public override void init()
		{
			base.init();
			if (StateParams.id)
			{
				formData = _contactService.get(new { id = StateParams.id });
			}
		}

		public override void submit()
		{
			if (formData.Id)
			{
				_contactService.update(new { id = formData.Id }, formData, new Action<dynamic>(response => handleResponse(response)));
				return;
			}
			_contactService.save(formData, new Action<dynamic>(response => handleResponse(response)));
		}

		public override void delete()
		{
			if (formData.Id && window.confirm(localization.confirmText))
			{
				_contactService.delete(new { id = formData.Id }, new Action<dynamic>(response => handleResponse(response)));
			}
		}

		private void handleResponse(dynamic response)
		{
			if (response.Success)
			{
				showList(response);
			}
			else
			{
				ApiResponseHelper.handleResponse(response, Scope);
			}
		}

		public void cancel()
		{
			showList();
		}
	}
}
