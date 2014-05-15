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
	public class GeneratedPermissionEntity : BaseEntity
	{
		#region Private Members
		private string _key1;
		private long? _key2;
		private string _key3;
		private EntityType _entityType;
		private long _entityId;
		private int _mask;
		#endregion
		
		#region Properties Names
			
		public const string Key1Property = "Key1";
		public const string Key2Property = "Key2";
		public const string Key3Property = "Key3";
		public const string EntityTypeProperty = "EntityType";
		public const string EntityIdProperty = "EntityId";
		public const string MaskProperty = "Mask";
			
		#endregion
		
		#region String Length Constants
			
		public const int Key1MaxLength = 100;
		public const int Key3MaxLength = 100;
			
		#endregion

		#region Constructor
		/// <summary>
		/// default constructor
		/// </summary>
		public GeneratedPermissionEntity()
		{
			_key1 = null;
			_key2 = null;
			_key3 = null;
			_entityId = 0;
			_mask = 0;
		}
		#endregion
		
		#region Public Properties
			
		/// <summary>
		/// Key1 property
		/// </summary>		
		public virtual string Key1
		{
			get { return _key1; }
			set	
			{
				if (value != null && value.Length > 100)
				{
					throw new ArgumentOutOfRangeException("Invalid value for Key1", value, value.ToString());
				}
				_key1 = value;
			}
		}
			
		/// <summary>
		/// Key2 property
		/// </summary>		
		public virtual long? Key2
		{
			get { return _key2; }
			set { _key2 = value; }
		}
			
		/// <summary>
		/// Key3 property
		/// </summary>		
		public virtual string Key3
		{
			get { return _key3; }
			set	
			{
				if (value != null && value.Length > 100)
				{
					throw new ArgumentOutOfRangeException("Invalid value for Key3", value, value.ToString());
				}
				_key3 = value;
			}
		}
			
		/// <summary>
		/// EntityType property
		/// </summary>		
		public virtual EntityType EntityType
		{
			get { return _entityType; }
			set { _entityType = value; }
		}
			
		/// <summary>
		/// EntityId property
		/// </summary>		
		public virtual long EntityId
		{
			get { return _entityId; }
			set { _entityId = value; }
		}
			
		/// <summary>
		/// Mask property
		/// </summary>		
		public virtual int Mask
		{
			get { return _mask; }
			set { _mask = value; }
		}
			
		#endregion
	}
}
