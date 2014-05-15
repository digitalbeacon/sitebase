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
	public class ResourceEntity : BaseEntity
	{
		#region Private Members
		private LanguageEntity _language;
		private string _type;
		private string _key;
		private string _property;
		private string _value;
		private FileEntity _file;
		#endregion
		
		#region Properties Names
			
		public const string LanguageProperty = "Language";
		public const string TypeProperty = "Type";
		public const string KeyProperty = "Key";
		public const string PropertyProperty = "Property";
		public const string ValueProperty = "Value";
		public const string FileProperty = "File";
			
		#endregion
		
		#region String Length Constants
			
		public const int TypeMaxLength = 100;
		public const int KeyMaxLength = 100;
		public const int PropertyMaxLength = 100;
		public const int ValueMaxLength = 1073741823;
			
		#endregion

		#region Constructor
		/// <summary>
		/// default constructor
		/// </summary>
		public ResourceEntity()
		{
			_type = null;
			_key = null;
			_property = null;
			_value = null;
			_file = null;
		}
		#endregion
		
		#region Public Properties
			
		/// <summary>
		/// Language property
		/// </summary>		
		public virtual LanguageEntity Language
		{
			get { return _language; }
			set { _language = value; }
		}
			
		/// <summary>
		/// Type property
		/// </summary>		
		public virtual string Type
		{
			get { return _type; }
			set	
			{
				if (value != null && value.Length > 100)
				{
					throw new ArgumentOutOfRangeException("Invalid value for Type", value, value.ToString());
				}
				_type = value;
			}
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
		/// Property property
		/// </summary>		
		public virtual string Property
		{
			get { return _property; }
			set	
			{
				if (value != null && value.Length > 100)
				{
					throw new ArgumentOutOfRangeException("Invalid value for Property", value, value.ToString());
				}
				_property = value;
			}
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
			
		/// <summary>
		/// File property
		/// </summary>		
		public virtual FileEntity File
		{
			get { return _file; }
			set { _file = value; }
		}
			
		#endregion
	}
}
