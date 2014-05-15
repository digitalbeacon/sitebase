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

namespace DigitalBeacon.SiteBase.Model.Messaging
{
	/// <summary>
	///	Generated with MyGeneration using the Entity template.
	/// </summary>
	[Serializable]
	public class MessageTemplateEntity : BaseEntity
	{
		#region Private Members
		private long _associationId;
		private string _name;
		private string _subject;
		private string _content;
		private DateTime _dateCreated;
		#endregion
		
		#region Properties Names
			
		public const string AssociationIdProperty = "AssociationId";
		public const string SubjectProperty = "Subject";
		public const string ContentProperty = "Content";
		public const string DateCreatedProperty = "DateCreated";
			
		#endregion
		
		#region String Length Constants
			
		public const int NameMaxLength = 100;
		public const int SubjectMaxLength = 200;
		public const int ContentMaxLength = 1073741823;
			
		#endregion

		#region Constructor
		/// <summary>
		/// default constructor
		/// </summary>
		public MessageTemplateEntity()
		{
			_associationId = 0;
			_name = null;
			_subject = null;
			_content = null;
			_dateCreated = DateTime.MinValue;
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
		/// Subject property
		/// </summary>		
		public virtual string Subject
		{
			get { return _subject; }
			set	
			{
				if (value != null && value.Length > 200)
				{
					throw new ArgumentOutOfRangeException("Invalid value for Subject", value, value.ToString());
				}
				_subject = value;
			}
		}
			
		/// <summary>
		/// Content property
		/// </summary>		
		public virtual string Content
		{
			get { return _content; }
			set	
			{
				if (value != null && value.Length > 1073741823)
				{
					throw new ArgumentOutOfRangeException("Invalid value for Content", value, value.ToString());
				}
				_content = value;
			}
		}
			
		/// <summary>
		/// DateCreated property
		/// </summary>		
		public virtual DateTime DateCreated
		{
			get { return _dateCreated; }
			set { _dateCreated = value; }
		}
			
		#endregion
	}
}
