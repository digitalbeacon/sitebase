// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using DigitalBeacon.SiteBase.Web.Models;

namespace DigitalBeacon.SiteBase.Models.Localization
{
	public class ListModel : ListModel<ListItem>
	{
		public long? Language { get; set; }
		public string Type { get; set; }
		public string DefaultLanguageName { get; set; }
		public string LanguageName { get; set; }
	}
}