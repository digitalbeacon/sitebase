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
	public class QueuedEmailRecipientEntity : BaseEntity
	{
		#region Private Members
		private QueuedEmailEntity _queuedEmail;
		private long? _userId;
		private long? _personId;
		private string _email;
		private bool _cc;
		private bool _bcc;
		#endregion
		
		#region Properties Names
			
		public const string QueuedEmailProperty = "QueuedEmail";
		public const string UserIdProperty = "UserId";
		public const string PersonIdProperty = "PersonId";
		public const string EmailProperty = "Email";
		public const string CcProperty = "Cc";
		public const string BccProperty = "Bcc";
			
		#endregion
		
		#region String Length Constants
			
		public const int EmailMaxLength = 200;
			
		#endregion

		#region Constructor
		/// <summary>
		/// default constructor
		/// </summary>
		public QueuedEmailRecipientEntity()
		{
			_userId = null;
			_personId = null;
			_email = null;
			_cc = false; 
			_bcc = false; 
		}
		#endregion
		
		#region Public Properties
			
		/// <summary>
		/// QueuedEmail property
		/// </summary>		
		public virtual QueuedEmailEntity QueuedEmail
		{
			get { return _queuedEmail; }
			set { _queuedEmail = value; }
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
		/// PersonId property
		/// </summary>		
		public virtual long? PersonId
		{
			get { return _personId; }
			set { _personId = value; }
		}
			
		/// <summary>
		/// Email property
		/// </summary>		
		public virtual string Email
		{
			get { return _email; }
			set	
			{
				if (value != null && value.Length > 200)
				{
					throw new ArgumentOutOfRangeException("Invalid value for Email", value, value.ToString());
				}
				_email = value;
			}
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
			
		#endregion
	}
}
