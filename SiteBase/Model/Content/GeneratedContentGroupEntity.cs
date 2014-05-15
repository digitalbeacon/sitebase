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
using DigitalBeacon.SiteBase.Model;

namespace DigitalBeacon.SiteBase.Model.Content
{
	/// <summary>
	///	Generated with MyGeneration using the Entity template.
	/// </summary>
	[Serializable]
	public class GeneratedContentGroupEntity : BaseEntity, INamedEntity
	{
		#region Private Members
		private long _associationId;
		private string _name;
		private string _title;
		private ContentGroupTypeEntity _contentGroupType;
		private int _displayOrder;
		private IList<ContentEntryEntity> _contentEntries;
		#endregion
		
		#region Properties Names
			
		public const string AssociationIdProperty = "AssociationId";
		public const string TitleProperty = "Title";
		public const string ContentGroupTypeProperty = "ContentGroupType";
		public const string ContentEntriesProperty = "ContentEntries";
			
		#endregion
		
		#region String Length Constants
			
		public const int NameMaxLength = 100;
		public const int TitleMaxLength = 200;
			
		#endregion

		#region Constructor
		/// <summary>
		/// default constructor
		/// </summary>
		public GeneratedContentGroupEntity()
		{
			_associationId = 0;
			_name = null;
			_title = null;
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
		/// Title property
		/// </summary>		
		public virtual string Title
		{
			get { return _title; }
			set	
			{
				if (value != null && value.Length > 200)
				{
					throw new ArgumentOutOfRangeException("Invalid value for Title", value, value.ToString());
				}
				_title = value;
			}
		}
			
		/// <summary>
		/// ContentGroupType property
		/// </summary>		
		public virtual ContentGroupTypeEntity ContentGroupType
		{
			get { return _contentGroupType; }
			set { _contentGroupType = value; }
		}
			
		/// <summary>
		/// DisplayOrder property
		/// </summary>		
		public virtual int DisplayOrder
		{
			get { return _displayOrder; }
			set { _displayOrder = value; }
		}
			
		/// <summary>
		/// ContentEntries collection
		/// </summary>		
		public virtual IList<ContentEntryEntity> ContentEntries
		{
			get { return _contentEntries; }
			set { _contentEntries = value; }
		}
		
		#endregion
	}
}
