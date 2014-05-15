// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using DigitalBeacon.Model;

namespace DigitalBeacon.SiteBase.Model
{
	/// <summary>
	/// A struct used to aggregate search parameters
	/// </summary>
	public class QueuedEmailSearchInfo : SearchInfo<QueuedEmailEntity>
	{
		public bool? Sent { get; set; }
		public bool? HasError { get; set; }
	}
}