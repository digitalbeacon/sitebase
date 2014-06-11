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

namespace DigitalBeacon.SiteBase.Mobile
{
	public abstract class BaseDetailsController : BaseController
	{
		protected BaseDetailsController(Scope scope, object state, ILocation location)
		{
			Scope = scope;
			RouterState = state;
			Location = location;
		}

		public override void init()
		{
			base.init();
			if (RouterParams.id)
			{
				load(RouterParams.id);
			}
			if (RouterState.current.data && RouterState.current.data.alerts)
			{
				alerts = RouterState.current.data.alerts;
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

		protected abstract void load(int id);

		protected abstract void save();

		public virtual void delete() { }

		public virtual void cancel()
		{
			hide();
		}

		public override void submit()
		{
			save();
		}

		protected override void handleResponse(ApiResponse response)
		{
			ControllerHelper.handleResponse(response, Scope);
			if (response.Success)
			{
				load(model.Id);
			}
		}

		protected Action<ApiResponse> ReturnToList
		{
			get
			{
				return new Action<ApiResponse>(response =>
				{
					if (response.Success)
					{
						hide(response);
					}
					else
					{
						ControllerHelper.handleResponse(response, Scope);
					}
				});
			}
		}

		protected Action<ApiResponse> SaveHandler
		{
			get
			{
				return new Action<ApiResponse>(response =>
				{
					var responseHandled = false;
					if (response.Success)
					{
						detailsChanged();
						data.fileInput = null;
						if (model.Id)
						{
							load(model.Id);
						}
						else if (response.Id)
						{
							responseHandled = true;
							getStateData("list.edit").alerts = ControllerHelper.getAlerts(response);
							RouterState.go("list.edit", new { id = response.Id });
						}
					}
					if (!responseHandled)
					{
						ControllerHelper.handleResponse(response, Scope);
					}
				});
			}
		}
	}
}
