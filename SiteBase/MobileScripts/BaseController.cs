// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using jQueryLib;
using ng;

namespace DigitalBeacon.SiteBase.Mobile
{
	public abstract class BaseController
	{
		private Scope _scope;
		private dynamic _state;
		private ILocation _location;

		public dynamic[] alerts;
		public dynamic formData = new Dictionary<object>();

		protected Scope Scope
		{
			get { return ((dynamic)_scope) ?? this; }
			set { _scope = value; }
		}

		protected dynamic State
		{
			get { return _state; }
			set { _state = value; }
		}

		protected dynamic StateParams
		{
			get { return _state ? _state.@params : new Dictionary<object>(); }
		}

		protected dynamic Location
		{
			get { return _location; }
			set { _location = value; }
		}

		public virtual void init()
		{
		}

		public void clearAlerts()
		{
			if (alerts && alerts.length > 0)
			{
				alerts.length = 0;
			}
		}

		public void closeAlert(int index)
		{
			alerts.splice(index, 1);
		}

		public static void extend(object target, object obj)
		{
			((BaseController)jQuery.extend(target, obj)).init();
		}
	}
}
