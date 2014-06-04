// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Browser;
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
		public string photoUrl;

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
				load(RouterParams.id);
			}
		}

		private void load(int id)
		{
			model = _contactService.get(new { id = id },
				new Action<dynamic>(response => photoUrl = digitalbeacon.resolveUrl("~/contacts/{0}/photo?x={1}".formatWith((int)response.Id, (int)response.PhotoId))));
		}

		public override void submit(bool isValid)
		{
			model.submitted = true;
			if (!isValid)
			{
				window.scrollTo(0, 0);
				return;
			}

			if (data.fileInput)
			{
				_contactService.save(model.Id, model, data.fileInput.files, new Action<ApiResponse>(response =>
					{
						ApiResponseHelper.handleResponse(response, Scope);
						load(model.Id);
					}));
			}
			else
			{
				_contactService.save(model.Id, model, ReturnToList);
			}
		}

		public override void delete()
		{
			if (model.Id && window.confirm(localization.confirmText))
			{
				_contactService.delete(new { id = model.Id }, ReturnToList);
			}
		}

		public void deletePhoto()
		{
			if (model.Id && window.confirm(localization.confirmText))
			{
				_contactService.deletePhoto(model.Id, ResponseHandler);
			}
		}

		public void rotatePhotoCounterclockwise()
		{
			if (model.Id && window.confirm(localization.confirmText))
			{
				_contactService.rotatePhotoCounterclockwise(model.Id, ResponseHandler);
			}
		}

		public void rotatePhotoClockwise()
		{
			if (model.Id && window.confirm(localization.confirmText))
			{
				_contactService.rotatePhotoClockwise(model.Id, ResponseHandler);
			}
		}

		protected override void handleResponse(ApiResponse response)
		{
			ApiResponseHelper.handleResponse(response, Scope);
			if (response.Success)
			{
				load(model.Id);
			}
		}
	}
}
