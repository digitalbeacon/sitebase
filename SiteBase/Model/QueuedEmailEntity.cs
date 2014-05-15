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
using DigitalBeacon.SiteBase.Model.Messaging;

namespace DigitalBeacon.SiteBase.Model
{
	/// <summary>
	///	Generated with MyGeneration using the Entity template.
	/// </summary>
	[Serializable]
	public class QueuedEmailEntity : BaseEntity
	{
		#region Private Members
		private long? _associationId;
		private long? _messageId;
		private ModuleSettingDefinition? _template;
		private int? _priority;
		private string _senderEmail;
		private string _subject;
		private string _body;
		private DateTime _sendDate;
		private DateTime? _dateProcessed;
		private DateTime? _dateSent;
		private string _errorMessage;
		private IList<QueuedEmailRecipientEntity> _recipients;
		private IList<QueuedEmailAttachmentEntity> _attachments;
		#endregion
		
		#region Properties Names
			
		public const string AssociationIdProperty = "AssociationId";
		public const string MessageIdProperty = "MessageId";
		public const string TemplateProperty = "Template";
		public const string PriorityProperty = "Priority";
		public const string SenderEmailProperty = "SenderEmail";
		public const string SubjectProperty = "Subject";
		public const string BodyProperty = "Body";
		public const string SendDateProperty = "SendDate";
		public const string DateProcessedProperty = "DateProcessed";
		public const string DateSentProperty = "DateSent";
		public const string ErrorMessageProperty = "ErrorMessage";
		public const string RecipientsProperty = "Recipients";
		public const string AttachmentsProperty = "Attachments";
			
		#endregion
		
		#region String Length Constants
			
		public const int SenderEmailMaxLength = 200;
		public const int SubjectMaxLength = 1073741823;
		public const int BodyMaxLength = 1073741823;
		public const int ErrorMessageMaxLength = 1073741823;
			
		#endregion

		#region Constructor
		/// <summary>
		/// default constructor
		/// </summary>
		public QueuedEmailEntity()
		{
			_associationId = null;
			_messageId = null;
			_template = null;
			_priority = null;
			_senderEmail = null;
			_subject = null;
			_body = null;
			_sendDate = DateTime.MinValue;
			_dateProcessed = null;
			_dateSent = null;
			_errorMessage = null;
		}
		#endregion
		
		#region Public Properties
			
		/// <summary>
		/// AssociationId property
		/// </summary>		
		public virtual long? AssociationId
		{
			get { return _associationId; }
			set { _associationId = value; }
		}
			
		/// <summary>
		/// MessageId property
		/// </summary>		
		public virtual long? MessageId
		{
			get { return _messageId; }
			set { _messageId = value; }
		}
			
		/// <summary>
		/// Template property
		/// </summary>		
		public virtual ModuleSettingDefinition? Template
		{
			get { return _template; }
			set { _template = value; }
		}
			
		/// <summary>
		/// Priority property
		/// </summary>		
		public virtual int? Priority
		{
			get { return _priority; }
			set { _priority = value; }
		}
			
		/// <summary>
		/// SenderEmail property
		/// </summary>		
		public virtual string SenderEmail
		{
			get { return _senderEmail; }
			set	
			{
				if (value != null && value.Length > 200)
				{
					throw new ArgumentOutOfRangeException("Invalid value for SenderEmail", value, value.ToString());
				}
				_senderEmail = value;
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
			
		/// <summary>
		/// SendDate property
		/// </summary>		
		public virtual DateTime SendDate
		{
			get { return _sendDate; }
			set { _sendDate = value; }
		}
			
		/// <summary>
		/// DateProcessed property
		/// </summary>		
		public virtual DateTime? DateProcessed
		{
			get { return _dateProcessed; }
			set { _dateProcessed = value; }
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
		/// ErrorMessage property
		/// </summary>		
		public virtual string ErrorMessage
		{
			get { return _errorMessage; }
			set	
			{
				if (value != null && value.Length > 1073741823)
				{
					throw new ArgumentOutOfRangeException("Invalid value for ErrorMessage", value, value.ToString());
				}
				_errorMessage = value;
			}
		}
			
		/// <summary>
		/// Recipients collection
		/// </summary>		
		public virtual IList<QueuedEmailRecipientEntity> Recipients
		{
			get { return _recipients; }
			set { _recipients = value; }
		}
		
		/// <summary>
		/// Attachments collection
		/// </summary>		
		public virtual IList<QueuedEmailAttachmentEntity> Attachments
		{
			get { return _attachments; }
			set { _attachments = value; }
		}
		
		#endregion
	}
}
