// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Html;
using jQueryLib;
using ng;
using ng.ui.router;

namespace DigitalBeacon.SiteBase.Mobile
{
	public abstract class BaseController
	{
		private Scope _scope;
		private dynamic _routerState;
		private ILocation _location;
		public dynamic data;

		protected BaseScopeData ScopeData
		{
			get { return (BaseScopeData)data; }
		}

		protected Scope Scope
		{
			get { return ((dynamic)_scope) ?? this; }
			set { _scope = value; }
		}

		protected State RouterState
		{
			get { return _routerState; }
			set { _routerState = value; }
		}

		protected dynamic RouterParams
		{
			get { return _routerState ? _routerState.@params : new { }; }
		}

		protected dynamic Location
		{
			get { return _location; }
			set { _location = value; }
		}

		protected Action<ApiResponse> DefaultHandler
		{
			get { return new Action<ApiResponse>(response => ControllerHelper.handleResponse(response, Scope)); }
		}

		protected Action<ApiResponse> ResponseHandler
		{
			get { return new Action<ApiResponse>(response => handleResponse(response)); }
		}

		public static void initScope(object target, object obj)
		{
			((BaseController)jQuery.extend(target, obj)).init();
		}

		protected virtual void init()
		{
			data = new BaseScopeData { model = new { } };
		}

		public virtual void submitForm(string modelName, bool isValid)
		{
			ScopeData[modelName].submitted = true;
			if (!isValid)
			{
				window.scrollTo(0, 0);
				return;
			}
			submit(modelName);
		}

		protected void resetForm(string formName, string modelName = "model")
		{
			((Form)Scope[formName]).setPristine();
			if (modelName)
			{
				ScopeData[modelName].submitted = false;
			}
		}

		protected virtual void submit(string modelName)
		{
		}

		public bool hasAlert(string key)
		{
			if (!ScopeData.alerts || ScopeData.alerts.length == 0)
			{
				return false;
			}
			for (var i = 0; i < ScopeData.alerts.length; i++)
			{
				if (ScopeData.alerts[i].key == key)
				{
					return true;
				}
			}
			return false;
		}

		public void clearAlerts(string key = null)
		{
			if (ScopeData.alerts && ScopeData.alerts.length > 0)
			{
				if (key)
				{
					for (var i = ScopeData.alerts.length -1; i >= 0; i--)
					{
						if (ScopeData.alerts[i].key == key)
						{
							closeAlert(i);
						}
					}
				}
				else
				{
					ScopeData.alerts.length = 0;
				}
			}
			if (!ScopeData.alerts)
			{
				ScopeData.alerts = new Alert[0];
			}
		}

		public void closeAlert(int index)
		{
			ScopeData.alerts.splice(index, 1);
		}

		public void setAlert(string msg, bool isError = true)
		{
			clearAlerts();
			ScopeData.alerts.push(new Alert { msg = msg, type = isError ? "danger" : "success" });
		}

		public void toggle(dynamic evt, string dataKey)
		{
			evt.preventDefault();
			evt.stopPropagation();
			ScopeData[dataKey] = ScopeData[dataKey] ? false : true;
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
				ScopeData.fileInput = fileInput;
			}
			else
			{
				ScopeData.fileInput = null;
			}
		}

		protected bool HasFileInput
		{
			get { return ScopeData.fileInput; }
		}

		protected object[] Files
		{
			get { return ScopeData.fileInput.files; }
		}

		protected virtual void handleResponse(ApiResponse response)
		{
			ControllerHelper.handleResponse(response, Scope);
		}

		protected dynamic getStateData(string stateName)
		{
			var state = RouterState.get(stateName);
			if (state)
			{
				if (!Utils.isDefined(state.data))
				{
					state.data = new { };
				}
				return state.data;
			}
			return null;
		}

		public void go(string stateName, object args = null)
		{
			RouterState.go(stateName, args);
		}

		protected Action<dynamic> createHandler(Action<dynamic> handler)
		{
			return new Action<dynamic>(response =>
			{
				if (response.Success)
				{
					handler(response);
				}
				else
				{
					ControllerHelper.handleResponse(response, Scope);
				}
			});
		}
	}
}
