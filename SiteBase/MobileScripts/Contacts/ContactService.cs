// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Dom;
using System.Html;
using ng;
using DigitalBeacon;
using DigitalBeacon.SiteBase.Mobile;
using DigitalBeacon.SiteBase;
using System.Collections.Generic;

namespace DigitalBeacon.SiteBase.Mobile.Contacts
{
	public class ContactService : BaseEntityService
	{
		private dynamic _http;

		static ContactService()
		{
			Angular.module("contactService", new [] { "ngResource" })
				.factory("contactService", new dynamic[] 
				{ 
					"$http",
					"$resource",
					(Func<dynamic, dynamic, dynamic>)
					((http, resource) =>
					{
						return new ContactService(http, resource);
					})
				});
		}

		public ContactService(dynamic http, dynamic resource)
		{
			_http = http;
			Resource = resource(
				digitalbeacon.resolveUrl("~/contacts/:id/:action/json"),
				new { id = "@id" },
				new
				{
					update = new { method = "PUT" },
					search = new { method = "POST", @params = new { action = "search" } },
					deletePhoto = new { method = "POST", @params = new { action = "deletePhoto" } },
					rotatePhotoCounterclockwise = new { method = "POST", @params = new { action = "rotatePhotoCounterclockwise" } },
					rotatePhotoClockwise = new { method = "POST", @params = new { action = "rotatePhotoClockwise" } }
				});
		}

		public void saveWithFileData(string id, object model, object[] files, Action<ApiResponse> responseHandler = null)
		{
			sendFormData(_http, "contacts", id, model, files, responseHandler);
		}

		public void deletePhoto(string id, Action<ApiResponse> responseHandler = null)
		{
			Resource.deletePhoto(new { id = id }, responseHandler);
		}

		public void rotatePhotoCounterclockwise(string id, Action<ApiResponse> responseHandler = null)
		{
			Resource.rotatePhotoCounterclockwise(new { id = id }, responseHandler);
		}

		public void rotatePhotoClockwise(string id, Action<ApiResponse> responseHandler = null)
		{
			Resource.rotatePhotoClockwise(new { id = id }, responseHandler);
		}
	}
}