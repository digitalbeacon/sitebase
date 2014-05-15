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
	public class MessageRecipientEntity : BaseEntity
	{
		#region Private Members
		private MessageEntity _message;
		private EntityType _recipientType;
		private long _recipientId;
		private string _name;
		private long _folderId;
		private bool _cc;
		private bool _bcc;
		private DateTime? _dateAvailable;
		private DateTime? _dateFirstRead;
		private DateTime? _dateReplied;
		private bool _flagged;
		private bool _read;
		#endregion
		
		#region Properties Names
			
		public const string MessageProperty = "Message";
		public const string RecipientTypeProperty = "RecipientType";
		public const string RecipientIdProperty = "RecipientId";
		public const string FolderIdProperty = "FolderId";
		public const string CcProperty = "Cc";
		public const string BccProperty = "Bcc";
		public const string DateAvailableProperty = "DateAvailable";
		public const string DateFirstReadProperty = "DateFirstRead";
		public const string DateRepliedProperty = "DateReplied";
		public const string FlaggedProperty = "Flagged";
		public const string ReadProperty = "Read";
			
		#endregion
		
		#region String Length Constants
			
		public const int NameMaxLength = 200;
			
		#endregion

		#region Constructor
		/// <summary>
		/// default constructor
		/// </summary>
		public MessageRecipientEntity()
		{
			_recipientId = 0;
			_name = null;
			_folderId = 0;
			_cc = false; 
			_bcc = false; 
			_dateAvailable = null;
			_dateFirstRead = null;
			_dateReplied = null;
			_flagged = false; 
			_read = false; 
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
		/// RecipientType property
		/// </summary>		
		public virtual EntityType RecipientType
		{
			get { return _recipientType; }
			set { _recipientType = value; }
		}
			
		/// <summary>
		/// RecipientId property
		/// </summary>		
		public virtual long RecipientId
		{
			get { return _recipientId; }
			set { _recipientId = value; }
		}
			
		/// <summary>
		/// Name property
		/// </summary>		
		public virtual string Name
		{
			get { return _name; }
			set	
			{
				if (value != null && value.Length > 200)
				{
					throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
				}
				_name = value;
			}
		}
			
		/// <summary>
		/// FolderId property
		/// </summary>		
		public virtual long FolderId
		{
			get { return _folderId; }
			set { _folderId = value; }
		}
			
		/// <summary>
		/// Cc property
		/// </summary>		
		public virtual bool Cc
		{
			get { return _cc; }
			set { _cc = value; }
		}
			
		/// <summary>
		/// Bcc property
		/// </summary>		
		public virtual bool Bcc
		{
			get { return _bcc; }
			set { _bcc = value; }
		}
			
		/// <summary>
		/// DateAvailable property
		/// </summary>		
		public virtual DateTime? DateAvailable
		{
			get { return _dateAvailable; }
			set { _dateAvailable = value; }
		}
			
		/// <summary>
		/// DateFirstRead property
		/// </summary>		
		public virtual DateTime? DateFirstRead
		{
			get { return _dateFirstRead; }
			set { _dateFirstRead = value; }
		}
			
		/// <summary>
		/// DateReplied property
		/// </summary>		
		public virtual DateTime? DateReplied
		{
			get { return _dateReplied; }
			set { _dateReplied = value; }
		}
			
		/// <summary>
		/// Flagged property
		/// </summary>		
		public virtual bool Flagged
		{
			get { return _flagged; }
			set { _flagged = value; }
		}
			
		/// <summary>
		/// Read property
		/// </summary>		
		public virtual bool Read
		{
			get { return _read; }
			set { _read = value; }
		}
			
		#endregion
	}
}
