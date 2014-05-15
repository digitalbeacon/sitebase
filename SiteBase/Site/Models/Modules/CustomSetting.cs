// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using DigitalBeacon.SiteBase.Model.Content;
using DigitalBeacon.SiteBase.Web.Models;

namespace DigitalBeacon.SiteBase.Models.Modules
{
	public class CustomSetting
	{
		public string Name { get; set; }
		public string Controller { get; set; }
		public string Action { get; set; }

		public string Path
		{
			set
			{
				string[] parts = value.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
				Controller = parts[0];
				Action = parts[1];
			}
		}
	}
}
