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

		public string photoUrl;

		public ContactDetailsController(Scope scope, object state, ILocation location, ContactService contactService) : base(scope, state, location)
		{
			_contactService = contactService;
		}

		protected override void load(int id)
		{
			model = _contactService.get(new { id = id },
				new Action<dynamic>(response =>
				{
					if (response.Id)
					{
						photoUrl = digitalbeacon.resolveUrl("~/contacts/{0}/photo?x={1}".formatWith((int)response.Id, (int)response.PhotoId));
					}
					else
					{
						ControllerHelper.handleResponse(response, Scope);
					}
				}));
		}

		protected override void save()
		{
			if (HasFileInput)
			{
				_contactService.saveWithFileData(model.Id, model, Files, SaveHandler);
			}
			else
			{
				_contactService.save(model.Id, model, SaveHandler);
			}
		}

		public override void delete()
		{
			if (model.Id && window.confirm(localization.confirmText))
			{
				detailsChanged();
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
	}
}
