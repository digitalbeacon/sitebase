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
	public class UserRoleEntity : BaseEntity
	{
		#region Private Members
		private long _associationId;
		private UserEntity _user;
		private Role _role;
		#endregion
		
		#region Properties Names
			
		public const string AssociationIdProperty = "AssociationId";
		public const string UserProperty = "User";
		public const string RoleProperty = "Role";
			
		#endregion

		#region Constructor
		/// <summary>
		/// default constructor
		/// </summary>
		public UserRoleEntity()
		{
			_associationId = 0;
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
		/// User property
		/// </summary>		
		public virtual UserEntity User
		{
			get { return _user; }
			set { _user = value; }
		}
			
		/// <summary>
		/// Role property
		/// </summary>		
		public virtual Role Role
		{
			get { return _role; }
			set { _role = value; }
		}
			
		#endregion
	}
}
