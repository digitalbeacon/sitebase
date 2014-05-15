// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using Iesi.Collections.Generic;
using DigitalBeacon.Model;

namespace DigitalBeacon.SiteBase.Model
{
	/// <summary>
	///	Generated with MyGeneration using the Entity template.
	/// </summary>
	[Serializable]
	public class FileDataEntity : BaseEntity
	{
		#region Private Members
		private byte[] _data;
		#endregion
		
		#region Properties Names
			
		public const string DataProperty = "Data";
			
		#endregion
		
		#region String Length Constants
			
		public const int DataMaxLength = 2147483647;
			
		#endregion

		#region Constructor
		/// <summary>
		/// default constructor
		/// </summary>
		public FileDataEntity()
		{ 
			_data = new byte[]{};
		}
		#endregion
		
		#region Public Properties
			
		/// <summary>
		/// Data property
		/// </summary>		
		public virtual byte[] Data
		{
			get { return _data; }
			set { _data = value; }
		}
			
		#endregion
	}
}
