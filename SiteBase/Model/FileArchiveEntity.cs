// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using DigitalBeacon.Model;

namespace DigitalBeacon.SiteBase.Model
{
	[Serializable]
	public class FileArchiveEntity : GeneratedFileArchiveEntity
	{
		public const string DisplayNameProperty = "DisplayName";

		public virtual string DisplayName
		{
			get { return String.IsNullOrEmpty(Name) ? Filename : Name; }
		}
	}
}
