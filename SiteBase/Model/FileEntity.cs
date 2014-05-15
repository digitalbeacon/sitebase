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
	public class FileEntity : GeneratedFileEntity
	{
		public const string DisplayNameProperty = "DisplayName";

		private bool _dataChanged;

		public virtual string DisplayName
		{
			get { return String.IsNullOrEmpty(Name) ? Filename : Name; }
		}

		public virtual bool DataChanged
		{
			get { return _dataChanged; }
			set { _dataChanged = value; }
		}
	}
}
