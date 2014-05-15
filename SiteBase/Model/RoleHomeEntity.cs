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
	public class RoleHomeEntity : BaseEntity
	{
		#region Private Members
		private long _associationId;
		private EntityType _entityType;
		private long _entityId;
		private string _url;
		private bool _redirect;
		private int _displayOrder;
		#endregion
		
		#region Properties Names
			
		public const string AssociationIdProperty = "AssociationId";
		public const string EntityTypeProperty = "EntityType";
		public const string EntityIdProperty = "EntityId";
		public const string UrlProperty = "Url";
		public const string RedirectProperty = "Redirect";
			
		#endregion
		
		#region String Length Constants
			
		public const int UrlMaxLength = 100;
			
		#endregion

		#region Constructor
		/// <summary>
		/// default constructor
		/// </summary>
		public RoleHomeEntity()
		{
			_associationId = 0;
			_entityId = 0;
			_url = null;
			_redirect = false; 
			_displayOrder = 0;
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
		/// Url property
		/// </summary>		
		public virtual string Url
		{
			get { return _url; }
			set	
			{
				if (value != null && value.Length > 100)
				{
					throw new ArgumentOutOfRangeException("Invalid value for Url", value, value.ToString());
				}
				_url = value;
			}
		}
			
		/// <summary>
		/// Redirect property
		/// </summary>		
		public virtual bool Redirect
		{
			get { return _redirect; }
			set { _redirect = value; }
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
