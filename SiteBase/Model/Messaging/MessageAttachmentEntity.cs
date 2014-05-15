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

namespace DigitalBeacon.SiteBase.Model.Messaging
{
	/// <summary>
	///	Generated with MyGeneration using the Entity template.
	/// </summary>
	[Serializable]
	public class MessageAttachmentEntity : BaseEntity
	{
		#region Private Members
		private MessageEntity _message;
		private DateTime _dateCreated;
		private string _fileName;
		private string _contentType;
		private byte[] _data;
		#endregion
		
		#region Properties Names
			
		public const string MessageProperty = "Message";
		public const string DateCreatedProperty = "DateCreated";
		public const string FileNameProperty = "FileName";
		public const string ContentTypeProperty = "ContentType";
		public const string DataProperty = "Data";
			
		#endregion
		
		#region String Length Constants
			
		public const int FileNameMaxLength = 100;
		public const int ContentTypeMaxLength = 50;
		public const int DataMaxLength = 2147483647;
			
		#endregion

		#region Constructor
		/// <summary>
		/// default constructor
		/// </summary>
		public MessageAttachmentEntity()
		{
			_message = null;
			_dateCreated = DateTime.MinValue;
			_fileName = null;
			_contentType = null; 
			_data = new byte[]{};
		}
		#endregion
		
		#region Public Properties
			
		/// <summary>
		/// Message property
		/// </summary>		
		public virtual MessageEntity Message
		{
			get { return _message; }
			set { _message = value; }
		}
			
		/// <summary>
		/// DateCreated property
		/// </summary>		
		public virtual DateTime DateCreated
		{
			get { return _dateCreated; }
			set { _dateCreated = value; }
		}
			
		/// <summary>
		/// FileName property
		/// </summary>		
		public virtual string FileName
		{
			get { return _fileName; }
			set	
			{
				if (value != null && value.Length > 100)
				{
					throw new ArgumentOutOfRangeException("Invalid value for FileName", value, value.ToString());
				}
				_fileName = value;
			}
		}
			
		/// <summary>
		/// ContentType property
		/// </summary>		
		public virtual string ContentType
		{
			get { return _contentType; }
			set	
			{
				if (value != null && value.Length > 50)
				{
					throw new ArgumentOutOfRangeException("Invalid value for ContentType", value, value.ToString());
				}
				_contentType = value;
			}
		}
			
		/// <summary>
		/// Data property
		/// </summary>		
		public virtual byte[] Data
		{
			get { return _data; }
			set { _data = value; }
		}
			
		#endregion
	}
}
