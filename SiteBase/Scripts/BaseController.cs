// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Dom;
using System.Html;
using DigitalBeacon.SiteBase;

namespace DigitalBeacon.SiteBase
{
	[ScriptIgnoreNamespace]
	public abstract class BaseController
	{
		private dynamic _scope;

		public BaseController(dynamic scope)
		{
			_scope = scope;
		}

		public void closeAlert(int index)
		{
			_scope.alerts.splice(index, 1);
		}
	}
}
