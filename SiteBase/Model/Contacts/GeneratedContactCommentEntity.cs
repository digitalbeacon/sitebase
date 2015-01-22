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

namespace DigitalBeacon.SiteBase.Model.Contacts
{
	/// <summary>
	///	Generated with MyGeneration using the Entity template.
	/// </summary>
	[Serializable]
	public class GeneratedContactCommentEntity : BaseEntity
	{
		#region Private Members
		private long _contactId;
		private ContactCommentTypeEntity _commentType;
		private string _text;
		private DateTime _date;
		#endregion
		
		#region Properties Names
			
		public const string ContactIdProperty = "ContactId";
		public const string CommentTypeProperty = "CommentType";
		public const string TextProperty = "Text";
		public const string DateProperty = "Date";
			
		#endregion
		
		#region String Length Constants
			
		public const int TextMaxLength = 1073741823;
			
		#endregion

		#region Constructor
		/// <summary>
		/// default constructor
		/// </summary>
		public GeneratedContactCommentEntity()
		{
			_contactId = 0;
			_text = null;
			_date = DateTime.MinValue;
		}
		#endregion
		
		#region Public Properties
			
		/// <summary>
		/// ContactId property
		/// </summary>		
		public virtual long ContactId
		{
			get { return _contactId; }
			set { _contactId = value; }
		}
			
		/// <summary>
		/// CommentType property
		/// </summary>		
		public virtual ContactCommentTypeEntity CommentType
		{
			get { return _commentType; }
			set { _commentType = value; }
		}
			
		/// <summary>
		/// Text property
		/// </summary>		
		public virtual string Text
		{
			get { return _text; }
			set	
			{
				if (value != null && value.Length > 1073741823)
				{
					throw new ArgumentOutOfRangeException("Invalid value for Text", value, value.ToString());
				}
				_text = value;
			}
		}
			
		/// <summary>
		/// Date property
		/// </summary>		
		public virtual DateTime Date
		{
			get { return _date; }
			set { _date = value; }
		}
			
		#endregion
	}
}
