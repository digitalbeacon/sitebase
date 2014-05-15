// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Globalization;
using DigitalBeacon.Business;
using DigitalBeacon.SiteBase.Business;
using DigitalBeacon.Util;

namespace DigitalBeacon.SiteBase.Web
{
	public class DatabaseResourceManager : System.Resources.ResourceManager
	{
		private static readonly ILocalizationService LocalizationService = ServiceFactory.Instance.GetService<ILocalizationService>();

		public override object GetObject(string name)
		{
			return GetObject(name, CultureInfo.CurrentUICulture);
		}

		public override object GetObject(string name, CultureInfo culture)
		{
			var resource = LocalizationService.GetResource(culture, name);
			if (resource != null && resource.Value.HasText())
			{
				return resource.Value;
			}
			return null;
		}

		//public override ResourceSet GetResourceSet(CultureInfo culture, bool createIfNotExists, bool tryParents)
		//{
		//	ResourceSet set = null;
		//	if (culture == CultureInfo.InvariantCulture)
		//	{
		//		set = new ResourceSet(this._resourceProvider.ResourceReader);
		//	}
		//	return set;
		//}
	}

}
