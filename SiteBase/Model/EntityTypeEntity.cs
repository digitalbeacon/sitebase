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
	public class EntityTypeEntity : BaseEntity, INamedEntity
	{
		#region Private Members
		private string _name;
		private string _type;
		#endregion
		
		#region Properties Names
			
		public const string TypeProperty = "Type";
			
		#endregion
		
		#region String Length Constants
			
		public const int NameMaxLength = 100;
		public const int TypeMaxLength = 100;
			
		#endregion

		#region Constructor
		/// <summary>
		/// default constructor
		/// </summary>
		public EntityTypeEntity()
		{
			_name = null;
			_type = null;
		}
		#endregion
		
		#region Public Properties
			
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
			
		#endregion
	}
}
