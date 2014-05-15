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
	public class AuditLogEntity : BaseEntity
	{
		#region Private Members
		private long? _associationId;
		private DateTime _created;
		private AuditActionEntity _action;
		private UserEntity _user;
		private long? _refId;
		private string _entityType;
		private string _details;
		#endregion
		
		#region Properties Names
			
		public const string AssociationIdProperty = "AssociationId";
		public const string CreatedProperty = "Created";
		public const string ActionProperty = "Action";
		public const string UserProperty = "User";
		public const string RefIdProperty = "RefId";
		public const string EntityTypeProperty = "EntityType";
		public const string DetailsProperty = "Details";
			
		#endregion
		
		#region String Length Constants
			
		public const int EntityTypeMaxLength = 200;
		public const int DetailsMaxLength = 1073741823;
			
		#endregion

		#region Constructor
		/// <summary>
		/// default constructor
		/// </summary>
		public AuditLogEntity()
		{
			_associationId = null;
			_created = DateTime.Now;
			_user = null;
			_refId = null;
			_entityType = null;
			_details = null;
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
		/// Created property
		/// </summary>		
		public virtual DateTime Created
		{
			get { return _created; }
			set { _created = value; }
		}
			
		/// <summary>
		/// Action property
		/// </summary>		
		public virtual AuditActionEntity Action
		{
			get { return _action; }
			set { _action = value; }
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
		/// RefId property
		/// </summary>		
		public virtual long? RefId
		{
			get { return _refId; }
			set { _refId = value; }
		}
			
		/// <summary>
		/// EntityType property
		/// </summary>		
		public virtual string EntityType
		{
			get { return _entityType; }
			set	
			{
				if (value != null && value.Length > 200)
				{
					throw new ArgumentOutOfRangeException("Invalid value for EntityType", value, value.ToString());
				}
				_entityType = value;
			}
		}
			
		/// <summary>
		/// Details property
		/// </summary>		
		public virtual string Details
		{
			get { return _details; }
			set	
			{
				if (value != null && value.Length > 1073741823)
				{
					throw new ArgumentOutOfRangeException("Invalid value for Details", value, value.ToString());
				}
				_details = value;
			}
		}
			
		#endregion
	}
}
