// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;

namespace DigitalBeacon.SiteBase.Mobile
{
	public interface IEntityService
	{
		dynamic get(string id, Action<dynamic> responseHandler = null);
		void search(object parameters, dynamic response);
		void save(string id, object postData, Action<ApiResponse> responseHandler = null);
		void delete(string id, Action<ApiResponse> responseHandler = null);
	}
}
