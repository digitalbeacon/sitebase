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
	public class AssociationEntity : BaseEntity, INamedEntity
	{
		#region Private Members
		private string _key;
		private string _name;
		private long _addressId;
		#endregion
		
		#region Properties Names
			
		public const string KeyProperty = "Key";
		public const string AddressIdProperty = "AddressId";
			
		#endregion
		
		#region String Length Constants
			
		public const int KeyMaxLength = 100;
		public const int NameMaxLength = 200;
			
		#endregion

		#region Constructor
		/// <summary>
		/// default constructor
		/// </summary>
		public AssociationEntity()
		{
			_key = null;
			_name = null;
			_addressId = 0;
		}
		#endregion
		
		#region Public Properties
			
		/// <summary>
		/// Key property
		/// </summary>		
		public virtual string Key
		{
			get { return _key; }
			set	
			{
				if (value != null && value.Length > 100)
				{
					throw new ArgumentOutOfRangeException("Invalid value for Key", value, value.ToString());
				}
				_key = value;
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
				if (value != null && value.Length > 200)
				{
					throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
				}
				_name = value;
			}
		}
			
		/// <summary>
		/// AddressId property
		/// </summary>		
		public virtual long AddressId
		{
			get { return _addressId; }
			set { _addressId = value; }
		}
			
		#endregion
	}
}
