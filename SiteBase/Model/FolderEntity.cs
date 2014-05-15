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
	public class FolderEntity : BaseEntity
	{
		#region Private Members
		private FolderType _type;
		private long? _associationId;
		private long? _userId;
		private long? _parentFolderId;
		private string _name;
		private int _displayOrder;
		#endregion
		
		#region Properties Names
			
		public const string TypeProperty = "Type";
		public const string AssociationIdProperty = "AssociationId";
		public const string UserIdProperty = "UserId";
		public const string ParentFolderIdProperty = "ParentFolderId";
			
		#endregion
		
		#region String Length Constants
			
		public const int NameMaxLength = 100;
			
		#endregion

		#region Constructor
		/// <summary>
		/// default constructor
		/// </summary>
		public FolderEntity()
		{
			_associationId = null;
			_userId = null;
			_parentFolderId = null;
			_name = null;
			_displayOrder = 0;
		}
		#endregion
		
		#region Public Properties
			
		/// <summary>
		/// Type property
		/// </summary>		
		public virtual FolderType Type
		{
			get { return _type; }
			set { _type = value; }
		}
			
		/// <summary>
		/// AssociationId property
		/// </summary>		
		public virtual long? AssociationId
		{
			get { return _associationId; }
			set { _associationId = value; }
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
		/// ParentFolderId property
		/// </summary>		
		public virtual long? ParentFolderId
		{
			get { return _parentFolderId; }
			set { _parentFolderId = value; }
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
