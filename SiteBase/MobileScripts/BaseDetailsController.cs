﻿// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using System.Dom;
using System.Html;
using DigitalBeacon.SiteBase;
using DigitalBeacon.SiteBase.Mobile;

namespace DigitalBeacon.SiteBase.Mobile
{
	public abstract class BaseDetailsController : BaseController
	{
		public int id;

		public override void init()
		{
			base.init();
			hideList();
		}

		public object getDisplayObject(dynamic x)
		{
			var retVal = new object();
			foreach (var key in Object.keys(x))
			{
				var val = x[key];
				if (!val || (val is Array && val.length == 0))
				{
					continue;
				}
				retVal[key] = x[key];
			}
			return retVal;
		}

		protected void showList(dynamic response = null)
		{
			Scope.emit("showList", response);
		}

		protected void hideList()
		{
			Scope.emit("hideList");
		}

		//	scope.filteredProperties = (Func<object, object>)
		//		(x =>
		//		{
		//			var retVal = new object();
		//			foreach (var key in Object.keys(x))
		//			{
		//				var val = x[key];
		//				if (!val || (val is Array && val.length == 0))
		//				{
		//					continue;
		//				}
		//				retVal[key] = x[key];
		//			}
		//			return retVal;
		//		});

		public abstract void submit(bool isValid);

		public virtual void delete() { }

		public virtual void cancel()
		{
			showList();
		}

		protected Action<ApiResponse> ReturnToList
		{
			get
			{
				return new Action<ApiResponse>(response =>
				{
					if (response.Success)
					{
						showList(response);
					}
					else
					{
						ApiResponseHelper.handleResponse(response, Scope);
					}
				});
			}
		}
	}
}
