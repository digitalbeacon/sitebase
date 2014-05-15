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
	public class GeneratedPreferenceEntity : BaseEntity
	{
		#region Private Members
		private long _associationId;
		private string _key;
		private long? _userId;
		private string _value;
		#endregion
		
		#region Properties Names
			
		public const string AssociationIdProperty = "AssociationId";
		public const string KeyProperty = "Key";
		public const string UserIdProperty = "UserId";
		public const string ValueProperty = "Value";
			
		#endregion
		
		#region String Length Constants
			
		public const int KeyMaxLength = 100;
		public const int ValueMaxLength = 1073741823;
			
		#endregion

		#region Constructor
		/// <summary>
		/// default constructor
		/// </summary>
		public GeneratedPreferenceEntity()
		{
			_associationId = 0;
			_key = null;
			_userId = null;
			_value = null;
		}
		#endregion
		
		#region Public Properties
			
		/// <summary>
		/// AssociationId property
		/// </summary>		
		public virtual long AssociationId
		{
			get { return _associationId; }
			set { _associationId = value; }
		}
			
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
		/// UserId property
		/// </summary>		
		public virtual long? UserId
		{
			get { return _userId; }
			set { _userId = value; }
		}
			
		/// <summary>
		/// Value property
		/// </summary>		
		public virtual string Value
		{
			get { return _value; }
			set	
			{
				if (value != null && value.Length > 1073741823)
				{
					throw new ArgumentOutOfRangeException("Invalid value for Value", value, value.ToString());
				}
				_value = value;
			}
		}
			
		#endregion
	}
}
