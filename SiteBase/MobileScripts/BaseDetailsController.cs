// ---------------------------------------------------------------------- //
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
using ng;
using ng.ui.router;

namespace DigitalBeacon.SiteBase.Mobile
{
	public abstract class BaseDetailsController : BaseController
	{
		protected BaseDetailsController(Scope scope, State state, ILocation location)
		{
			Scope = scope;
			RouterState = state;
			Location = location;
		}

		protected override void init()
		{
			base.init();
			if (RouterParams.id)
			{
				load(RouterParams.id);
			}
			if (RouterState.current.data && RouterState.current.data.alerts)
			{
				ScopeData.alerts = RouterState.current.data.alerts;
				RouterState.current.data.alerts = null;
			}
			show();
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

		protected void hide(dynamic response = null)
		{
			Scope.emit("hideDetails", response);
		}

		protected void show()
		{
			Scope.emit("showDetails");
		}

		protected void detailsChanged()
		{
			Scope.emit("detailsChanged");
		}

		protected override void submit(string modelName)
		{
			save();
		}

		protected abstract void load(string id);

		protected abstract void save();

		public virtual void remove() { } // work-around for issue with minification, delete is a reserved word

		public virtual void cancel()
		{
			if (RouterState.@is("list.edit"))
			{
				RouterState.go("list.display", new { id = ScopeData.model.Id });
			}
			else
			{
				hide();
			}
		}

		public virtual void previous()
		{
			Scope.emit("showPrevious", ScopeData.model.Id);
		}

		public virtual void next()
		{
			Scope.emit("showNext", ScopeData.model.Id);
		}

		protected override void handleResponse(ApiResponse response)
		{
			ControllerHelper.handleResponse(response, Scope);
			if (response.Success)
			{
				load(ScopeData.model.Id);
			}
		}

		protected virtual void onSaveSuccess(object response)
		{
			detailsChanged();
			ScopeData.fileInput = null;
			getStateData("list.display").alerts = ControllerHelper.getAlerts((dynamic)response);
			RouterState.go("list.display", new { id = ScopeData.model.Id ?? ((dynamic)response).Data });
		}

		protected Action<ApiResponse> ReturnToList
		{
			get { return createHandler(response => hide(response)); }
		}

		protected Action<ApiResponse> SaveHandler
		{
			get { return createHandler(response => onSaveSuccess(response)); }
		}
	}
}
