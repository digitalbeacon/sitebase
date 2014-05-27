// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using ng;

namespace DigitalBeacon.SiteBase.Mobile
{
	[ScriptIgnoreNamespace]
	public abstract class BaseController
	{
		private dynamic _scope;
		private dynamic _routeParams;
		private ILocation _location;

		public dynamic[] alerts;
		public Dictionary<object> formData = new Dictionary<object>();

		protected dynamic Scope
		{
			get { return _scope ?? this; }
			set { _scope = value; }
		}

		protected dynamic RouteParams
		{
			get { return _routeParams; }
			set { _routeParams = value; }
		}

		protected dynamic Location
		{
			get { return _location; }
			set { _location = value; }
		}

		public void closeAlert(int index)
		{
			alerts.splice(index, 1);
		}

		public abstract void init();
	}
}
