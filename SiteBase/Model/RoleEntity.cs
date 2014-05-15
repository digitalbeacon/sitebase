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
	public class RoleEntity : BaseEntity, INamedEntity
	{
		#region Private Members
		private long? _associationId;
		private string _name;
		private RoleGroupEntity _roleGroup;
		#endregion
		
		#region Properties Names
			
		public const string AssociationIdProperty = "AssociationId";
		public const string RoleGroupProperty = "RoleGroup";
			
		#endregion
		
		#region String Length Constants
			
		public const int NameMaxLength = 100;
			
		#endregion

		#region Constructor
		/// <summary>
		/// default constructor
		/// </summary>
		public RoleEntity()
		{
			_associationId = null;
			_name = null;
			_roleGroup = null;
		}
		#endregion
		
		#region Public Properties
			
		/// <summary>
		/// AssociationId property
		/// </summary>		
		public virtual long? AssociationId
		{
			get { return _associationId; }
			set { _associationId = value; }
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
		/// RoleGroup property
		/// </summary>		
		public virtual RoleGroupEntity RoleGroup
		{
			get { return _roleGroup; }
			set { _roleGroup = value; }
		}
			
		#endregion
	}
}
