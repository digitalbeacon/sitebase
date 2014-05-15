// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using DigitalBeacon.SiteBase.Model.Content;
using DigitalBeacon.SiteBase.Web.Models;

namespace DigitalBeacon.SiteBase.Models.Content
{
	public class FlexibleContentModel : ListModel<ContentEntryEntity>
	{
		public string CssClass { get; set; }
		public string DateFormat { get; set; }
	}
}
