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
	public class CountryEntity : BaseEntity, INamedEntity, ICodedEntity
	{
		#region Private Members
		private string _code;
		private string _name;
		private int _displayOrder;
		#endregion
		
		#region String Length Constants
			
		public const int CodeMaxLength = 20;
		public const int NameMaxLength = 100;
			
		#endregion

		#region Constructor
		/// <summary>
		/// default constructor
		/// </summary>
		public CountryEntity()
		{
			_code = null;
			_name = null;
			_displayOrder = 0;
		}
		#endregion
		
		#region Public Properties
			
		/// <summary>
		/// Code property
		/// </summary>		
		public virtual string Code
		{
			get { return _code; }
			set	
			{
				if (value != null && value.Length > 20)
				{
					throw new ArgumentOutOfRangeException("Invalid value for Code", value, value.ToString());
				}
				_code = value;
			}
		}
			
		/// <summary>
		/// Name property
		/// </summary>		
		public virtual string Name
		{
			get { return _name; }
			set	
			{
				if (value != null && value.Length > 100)
				{
					throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
				}
				_name = value;
			}
		}
			
		/// <summary>
		/// DisplayOrder property
		/// </summary>		
		public virtual int DisplayOrder
		{
			get { return _displayOrder; }
			set { _displayOrder = value; }
		}
			
		#endregion
	}
}
