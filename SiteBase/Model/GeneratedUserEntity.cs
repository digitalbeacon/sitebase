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
	public class GeneratedUserEntity : BaseEntity
	{
		#region Private Members
		private DateTime? _deleted;
		private string _username;
		private string _displayName;
		private string _email;
		private bool _superUser;
		private PersonEntity _person;
		private Language? _language;
		private IList<AssociationEntity> _associations;
		private IList<UserRoleEntity> _roles;
		#endregion
		
		#region Properties Names
			
		public const string DeletedProperty = "Deleted";
		public const string UsernameProperty = "Username";
		public const string DisplayNameProperty = "DisplayName";
		public const string EmailProperty = "Email";
		public const string SuperUserProperty = "SuperUser";
		public const string PersonProperty = "Person";
		public const string LanguageProperty = "Language";
		public const string AssociationsProperty = "Associations";
		public const string RolesProperty = "Roles";
			
		#endregion
		
		#region String Length Constants
			
		public const int UsernameMaxLength = 100;
		public const int DisplayNameMaxLength = 100;
		public const int EmailMaxLength = 100;
			
		#endregion

		#region Constructor
		/// <summary>
		/// default constructor
		/// </summary>
		public GeneratedUserEntity()
		{
			_deleted = null;
			_username = null;
			_displayName = null;
			_email = null;
			_superUser = false; 
			_language = null;
		}
		#endregion
		
		#region Public Properties
			
		/// <summary>
		/// Deleted property
		/// </summary>		
		public virtual DateTime? Deleted
		{
			get { return _deleted; }
			set { _deleted = value; }
		}
			
		/// <summary>
		/// Username property
		/// </summary>		
		public virtual string Username
		{
			get { return _username; }
			set	
			{
				if (value != null && value.Length > 100)
				{
					throw new ArgumentOutOfRangeException("Invalid value for Username", value, value.ToString());
				}
				_username = value;
			}
		}
			
		/// <summary>
		/// DisplayName property
		/// </summary>		
		public virtual string DisplayName
		{
			get { return _displayName; }
			set	
			{
				if (value != null && value.Length > 100)
				{
					throw new ArgumentOutOfRangeException("Invalid value for DisplayName", value, value.ToString());
				}
				_displayName = value;
			}
		}
			
		/// <summary>
		/// Email property
		/// </summary>		
		public virtual string Email
		{
			get { return _email; }
			set	
			{
				if (value != null && value.Length > 100)
				{
					throw new ArgumentOutOfRangeException("Invalid value for Email", value, value.ToString());
				}
				_email = value;
			}
		}
			
		/// <summary>
		/// SuperUser property
		/// </summary>		
		public virtual bool SuperUser
		{
			get { return _superUser; }
			set { _superUser = value; }
		}
			
		/// <summary>
		/// Person property
		/// </summary>		
		public virtual PersonEntity Person
		{
			get { return _person; }
			set { _person = value; }
		}
			
		/// <summary>
		/// Language property
		/// </summary>		
		public virtual Language? Language
		{
			get { return _language; }
			set { _language = value; }
		}
			
		/// <summary>
		/// Associations collection
		/// </summary>		
		public virtual IList<AssociationEntity> Associations
		{
			get { return _associations; }
			set { _associations = value; }
		}
		
		/// <summary>
		/// Roles collection
		/// </summary>		
		public virtual IList<UserRoleEntity> Roles
		{
			get { return _roles; }
			set { _roles = value; }
		}
		
		#endregion
	}
}
