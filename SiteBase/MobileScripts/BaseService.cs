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
	public abstract class BaseService
	{
		[ScriptExternal]
		public extern dynamic get(object parameters, Action<dynamic> reponseHandler = null);

		[ScriptExternal]
		public extern void search(object parameters, dynamic response);

		[ScriptExternal]
		public extern void save(object parameters, Action<ApiResponse> responseHandler = null);

		[ScriptExternal]
		public extern void update(object parameters, Action<ApiResponse> responseHandler = null);
	}
}
