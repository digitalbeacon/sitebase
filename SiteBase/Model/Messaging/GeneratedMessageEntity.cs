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
	public class GeneratedMessageEntity : BaseEntity
	{
		#region Private Members
		private long _associationId;
		private long _senderId;
		private Role? _senderRole;
		private string _senderName;
		private string _subject;
		private string _content;
		private DateTime? _dateSent;
		private DateTime? _dateExpires;
		private long? _relatedId;
		private long? _replyToId;
		private bool _flagged;
		private bool _replyDisabled;
		private MessageImportance _importance;
		private bool _email;
		private MessageType _type;
		private IList<MessageRecipientEntity> _recipients;
		private IList<MessageAttachmentEntity> _attachments;
		#endregion
		
		#region Properties Names
			
		public const string AssociationIdProperty = "AssociationId";
		public const string SenderIdProperty = "SenderId";
		public const string SenderRoleProperty = "SenderRole";
		public const string SenderNameProperty = "SenderName";
		public const string SubjectProperty = "Subject";
		public const string ContentProperty = "Content";
		public const string DateSentProperty = "DateSent";
		public const string DateExpiresProperty = "DateExpires";
		public const string RelatedIdProperty = "RelatedId";
		public const string ReplyToIdProperty = "ReplyToId";
		public const string FlaggedProperty = "Flagged";
		public const string ReplyDisabledProperty = "ReplyDisabled";
		public const string ImportanceProperty = "Importance";
		public const string EmailProperty = "Email";
		public const string TypeProperty = "Type";
		public const string RecipientsProperty = "Recipients";
		public const string AttachmentsProperty = "Attachments";
			
		#endregion
		
		#region String Length Constants
			
		public const int SenderNameMaxLength = 200;
		public const int SubjectMaxLength = 1073741823;
		public const int ContentMaxLength = 1073741823;
			
		#endregion

		#region Constructor
		/// <summary>
		/// default constructor
		/// </summary>
		public GeneratedMessageEntity()
		{
			_associationId = 0;
			_senderId = 0;
			_senderRole = null;
			_senderName = null;
			_subject = null;
			_content = null;
			_dateSent = null;
			_dateExpires = null;
			_relatedId = null;
			_replyToId = null;
			_flagged = false; 
			_replyDisabled = false; 
			_email = false; 
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
		/// SenderId property
		/// </summary>		
		public virtual long SenderId
		{
			get { return _senderId; }
			set { _senderId = value; }
		}
			
		/// <summary>
		/// SenderRole property
		/// </summary>		
		public virtual Role? SenderRole
		{
			get { return _senderRole; }
			set { _senderRole = value; }
		}
			
		/// <summary>
		/// SenderName property
		/// </summary>		
		public virtual string SenderName
		{
			get { return _senderName; }
			set	
			{
				if (value != null && value.Length > 200)
				{
					throw new ArgumentOutOfRangeException("Invalid value for SenderName", value, value.ToString());
				}
				_senderName = value;
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
				if (value != null && value.Length > 1073741823)
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
		/// DateSent property
		/// </summary>		
		public virtual DateTime? DateSent
		{
			get { return _dateSent; }
			set { _dateSent = value; }
		}
			
		/// <summary>
		/// DateExpires property
		/// </summary>		
		public virtual DateTime? DateExpires
		{
			get { return _dateExpires; }
			set { _dateExpires = value; }
		}
			
		/// <summary>
		/// RelatedId property
		/// </summary>		
		public virtual long? RelatedId
		{
			get { return _relatedId; }
			set { _relatedId = value; }
		}
			
		/// <summary>
		/// ReplyToId property
		/// </summary>		
		public virtual long? ReplyToId
		{
			get { return _replyToId; }
			set { _replyToId = value; }
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
		/// ReplyDisabled property
		/// </summary>		
		public virtual bool ReplyDisabled
		{
			get { return _replyDisabled; }
			set { _replyDisabled = value; }
		}
			
		/// <summary>
		/// Importance property
		/// </summary>		
		public virtual MessageImportance Importance
		{
			get { return _importance; }
			set { _importance = value; }
		}
			
		/// <summary>
		/// Email property
		/// </summary>		
		public virtual bool Email
		{
			get { return _email; }
			set { _email = value; }
		}
			
		/// <summary>
		/// Type property
		/// </summary>		
		public virtual MessageType Type
		{
			get { return _type; }
			set { _type = value; }
		}
			
		/// <summary>
		/// Recipients collection
		/// </summary>		
		public virtual IList<MessageRecipientEntity> Recipients
		{
			get { return _recipients; }
			set { _recipients = value; }
		}
		
		/// <summary>
		/// Attachments collection
		/// </summary>		
		public virtual IList<MessageAttachmentEntity> Attachments
		{
			get { return _attachments; }
			set { _attachments = value; }
		}
		
		#endregion
	}
}
