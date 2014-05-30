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
			RouterState = state;
			Location = location;
			_contactService = contactService;
		}

		public override void init()
		{
			base.init();
			if (RouterParams.id)
			{
				model = _contactService.get(new { id = RouterParams.id });
			}
		}

		public override void submit(bool isValid)
		{
			model.submitted = true;
			if (!isValid)
			{
				window.scrollTo(0, 0);
				return;
			}
			if (model.Id)
			{
				_contactService.update(new { id = model.Id }, model, new Action<dynamic>(response => handleResponse(response)));
				return;
			}
			_contactService.save(model, new Action<dynamic>(response => handleResponse(response)));
		}

		public override void delete()
		{
			if (model.Id && window.confirm(localization.confirmText))
			{
				_contactService.delete(new { id = model.Id }, new Action<dynamic>(response => handleResponse(response)));
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
