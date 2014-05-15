// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.ComponentModel;
using DigitalBeacon.Business;

namespace DigitalBeacon.Web
{
	public class LocalizedDisplayNameAttribute : DisplayNameAttribute
	{
		private readonly string _resourceKey;

		public LocalizedDisplayNameAttribute(string resourceKey)
		{
			_resourceKey = resourceKey;
		}

		public override string DisplayName
		{
			get { return ResourceManager.Instance.GetString(_resourceKey); }
		}
	}
}

