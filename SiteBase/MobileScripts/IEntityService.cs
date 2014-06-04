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
		dynamic get(object parameters, Action<dynamic> responseHandler = null);
		void search(object parameters, dynamic response);
		void save(string id, object postData, Action<ApiResponse> responseHandler = null);
		void delete(object parameters, Action<ApiResponse> responseHandler = null);
	}
}
