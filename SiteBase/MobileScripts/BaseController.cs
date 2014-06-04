// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using System.Html;
using jQueryLib;
using ng;

namespace DigitalBeacon.SiteBase.Mobile
{
	public abstract class BaseController
	{
		private Scope _scope;
		private dynamic _routerState;
		private ILocation _location;

		public Alert[] alerts = new Alert[0];
		public dynamic model = new Dictionary<object>();
		public dynamic data = new Dictionary<object>();

		protected Scope Scope
		{
			get { return ((dynamic)_scope) ?? this; }
			set { _scope = value; }
		}

		protected dynamic RouterState
		{
			get { return _routerState; }
			set { _routerState = value; }
		}

		protected dynamic RouterParams
		{
			get { return _routerState ? _routerState.@params : new Dictionary<object>(); }
		}

		protected dynamic Location
		{
			get { return _location; }
			set { _location = value; }
		}

		protected Action<ApiResponse> DefaultHandler
		{
			get { return new Action<ApiResponse>(response => ApiResponseHelper.handleResponse(response, Scope)); }
		}

		protected Action<ApiResponse> ResponseHandler
		{
			get { return new Action<ApiResponse>(response => handleResponse(response)); }
		}

		public virtual void init()
		{
		}

		public bool hasAlert(string key)
		{
			if (!alerts || alerts.length == 0)
			{
				return false;
			}
			for (var i = 0; i < alerts.length; i++)
			{
				if (alerts[i].key == key)
				{
					return true;
				}
			}
			return false;
		}

		public void clearAlerts(string key = null)
		{
			if (alerts && alerts.length > 0)
			{
				if (key)
				{
					for (var i = alerts.length -1; i >= 0; i--)
					{
						if (alerts[i].key == key)
						{
							closeAlert(i);
						}
					}
				}
				else
				{
					alerts.length = 0;
				}
			}
		}

		public void closeAlert(int index)
		{
			alerts.splice(index, 1);
		}

		public void fileChanged(dynamic fileInput)
		{
			if (!fileInput)
			{
				return;
			}
			var files = fileInput.files;
			if (fileInput.files && fileInput.files.length)
			{
				data.fileInput = fileInput;
			}
			else
			{
				data.fileInput = null;
			}
		}

		public static void extend(object target, object obj)
		{
			((BaseController)jQuery.extend(target, obj)).init();
		}

		protected virtual void handleResponse(ApiResponse response)
		{
		}
	}
}
