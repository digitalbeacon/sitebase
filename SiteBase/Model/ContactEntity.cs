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
	public class ContactEntity : PersonEntity
	{
		#region Private Members
		private bool _enabled;
		private long? _userId;
		private long _associationId;
		private Relationship? _relationship;
		private FileEntity _photo;
		private int? _photoWidth;
		private int? _photoHeight;
		#endregion
		
		#region Properties Names
			
		public const string EnabledProperty = "Enabled";
		public const string UserIdProperty = "UserId";
		public const string AssociationIdProperty = "AssociationId";
		public const string RelationshipProperty = "Relationship";
		public const string PhotoProperty = "Photo";
		public const string PhotoWidthProperty = "PhotoWidth";
		public const string PhotoHeightProperty = "PhotoHeight";
			
		#endregion

		#region Constructor
		/// <summary>
		/// default constructor
		/// </summary>
		public ContactEntity()
		{
			_enabled = false; 
			_userId = null;
			_associationId = 0;
			_relationship = null;
			_photo = null;
			_photoWidth = null;
			_photoHeight = null;
		}
		#endregion
		
		#region Public Properties
			
		/// <summary>
		/// Enabled property
		/// </summary>		
		public virtual bool Enabled
		{
			get { return _enabled; }
			set { _enabled = value; }
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
		/// AssociationId property
		/// </summary>		
		public virtual long AssociationId
		{
			get { return _associationId; }
			set { _associationId = value; }
		}
			
		/// <summary>
		/// Relationship property
		/// </summary>		
		public virtual Relationship? Relationship
		{
			get { return _relationship; }
			set { _relationship = value; }
		}
			
		/// <summary>
		/// Photo property
		/// </summary>		
		public virtual FileEntity Photo
		{
			get { return _photo; }
			set { _photo = value; }
		}
			
		/// <summary>
		/// PhotoWidth property
		/// </summary>		
		public virtual int? PhotoWidth
		{
			get { return _photoWidth; }
			set { _photoWidth = value; }
		}
			
		/// <summary>
		/// PhotoHeight property
		/// </summary>		
		public virtual int? PhotoHeight
		{
			get { return _photoHeight; }
			set { _photoHeight = value; }
		}
			
		#endregion
	}
}
