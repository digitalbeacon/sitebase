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

namespace DigitalBeacon.SiteBase.Model.Content
{
	/// <summary>
	///	Generated with MyGeneration using the Entity template.
	/// </summary>
	[Serializable]
	public class GeneratedContentEntryEntity : BaseEntity
	{
		#region Private Members
		private DateTime _lastModificationDate;
		private ContentGroupEntity _contentGroup;
		private DateTime? _contentDate;
		private int _displayOrder;
		private string _title;
		private string _body;
		#endregion
		
		#region Properties Names
			
		public const string LastModificationDateProperty = "LastModificationDate";
		public const string ContentGroupProperty = "ContentGroup";
		public const string ContentDateProperty = "ContentDate";
		public const string TitleProperty = "Title";
		public const string BodyProperty = "Body";
			
		#endregion
		
		#region String Length Constants
			
		public const int TitleMaxLength = 1073741823;
		public const int BodyMaxLength = 1073741823;
			
		#endregion

		#region Constructor
		/// <summary>
		/// default constructor
		/// </summary>
		public GeneratedContentEntryEntity()
		{
			_lastModificationDate = DateTime.MinValue;
			_contentDate = null;
			_displayOrder = 0;
			_title = null;
			_body = null;
		}
		#endregion
		
		#region Public Properties
			
		/// <summary>
		/// LastModificationDate property
		/// </summary>		
		public virtual DateTime LastModificationDate
		{
			get { return _lastModificationDate; }
			set { _lastModificationDate = value; }
		}
			
		/// <summary>
		/// ContentGroup property
		/// </summary>		
		public virtual ContentGroupEntity ContentGroup
		{
			get { return _contentGroup; }
			set { _contentGroup = value; }
		}
			
		/// <summary>
		/// ContentDate property
		/// </summary>		
		public virtual DateTime? ContentDate
		{
			get { return _contentDate; }
			set { _contentDate = value; }
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
		/// Title property
		/// </summary>		
		public virtual string Title
		{
			get { return _title; }
			set	
			{
				if (value != null && value.Length > 1073741823)
				{
					throw new ArgumentOutOfRangeException("Invalid value for Title", value, value.ToString());
				}
				_title = value;
			}
		}
			
		/// <summary>
		/// Body property
		/// </summary>		
		public virtual string Body
		{
			get { return _body; }
			set	
			{
				if (value != null && value.Length > 1073741823)
				{
					throw new ArgumentOutOfRangeException("Invalid value for Body", value, value.ToString());
				}
				_body = value;
			}
		}
			
		#endregion
	}
}
