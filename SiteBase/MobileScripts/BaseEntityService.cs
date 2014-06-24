// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using System.Html;
using ng;

namespace DigitalBeacon.SiteBase.Mobile
{
	public abstract class BaseEntityService : IEntityService
	{
		protected dynamic Resource { get; set; }

		public virtual dynamic get(string id, Action<dynamic> responseHandler = null)
		{
			return Resource.get(new { id = id }, responseHandler);
		}

		public virtual void search(object parameters, dynamic response)
		{
			Resource.search(parameters, response);
		}

		public virtual void save(string id, object postData, Action<ApiResponse> responseHandler = null)
		{
			if (id)
			{
				Resource.update(new { id = id }, postData, responseHandler);
			}
			else
			{
				Resource.save(postData, responseHandler);
			}
		}

		[ScriptName("$delete")] // work-around for issue with minification
		public virtual void delete(string id, Action<ApiResponse> responseHandler = null)
		{
			((IResource)Resource).delete(new { id = id }, responseHandler);
		}

		protected void sendFormData(dynamic http, string entityTarget, string id, object model, object[] files, Action<ApiResponse> responseHandler = null)
		{
			var headers = new Dictionary<object>();
			headers["Content-Type"] = window.undefined; // angular will populate content-type with "multipart/form-data" and the generated boundary parameter
			http(new
			{
				method = id ? "PUT" : "POST",
				url = digitalbeacon.resolveUrl("~/" + entityTarget + (id ? ("/" + id) : "") + "/json"),
				headers = headers,
				data = constructFormData(model, files),
				transformRequest = new Func<object, object>(x => x)
			})
			.success(responseHandler);
		}

		private static FormData constructFormData(object model, object[] files)
		{
			var data = new FormData();
			foreach (var k in Object.keys(model))
			{
				if (k.slice(0, 1) == "$")
				{
					continue;
				}
				if (model[k] || digitalbeacon.isOfType(model[k], "boolean"))
				{
					if (Angular.isDate(model[k]))
					{
						data.append(k, window.JSON.stringify(model[k]).replace("\"", "").replace("\"", ""));
					}
					else
					{
						data.append(k, model[k]);
					}
				}
			}
			if (files)
			{
				for (var i = 0; i < files.length; i++)
				{
					data.append("file" + i, files[i]);
				}
			}
			return data;
		}
	}
}
