// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;

namespace DigitalBeacon.SiteBase.Mobile
{
	[ScriptIgnoreNamespace]
	public abstract class BaseController
	{
		private dynamic _scope;

		public dynamic[] alerts;

		protected dynamic Scope
		{
			get { return _scope ?? this; }
			set { _scope = value; }
		}

		public void closeAlert(int index)
		{
			alerts.splice(index, 1);
		}

		public abstract void init();
	}
}
