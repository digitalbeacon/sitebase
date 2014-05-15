// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;

namespace DigitalBeacon.SiteBase.Model.Messaging
{
	[Serializable]
	public class MessageEntity : GeneratedMessageEntity, IComparable<MessageEntity>
	{
		private bool _sendNotificatonOnly = true;
		private bool _expandRoleRecipient;

		/// <summary>
		/// Non-persisted field
		/// </summary>
		public virtual bool SendNotificationOnly
		{
			get { return _sendNotificatonOnly; }
			set { _sendNotificatonOnly = value; }
		}

		/// <summary>
		/// Non-persisted field
		/// </summary>
		public virtual bool ExpandRoleRecipient
		{
			get { return _expandRoleRecipient; }
			set { _expandRoleRecipient = value; }
		}

		/// <summary>
		/// Add a message recipient
		/// </summary>
		/// <param name="recipientType">Type of the recipient.</param>
		/// <param name="recipientId">The recipient id.</param>
		/// <param name="cc">if set to <c>true</c> cc.</param>
		/// <param name="bcc">if set to <c>true</c> bcc.</param>
		public virtual void AddRecipient(EntityType recipientType, long recipientId, bool cc, bool bcc)
		{
			AddRecipient(recipientType, recipientId, null, cc, bcc);
		}

		/// <summary>
		/// Add a message recipient
		/// </summary>
		/// <param name="recipientType">Type of the recipient.</param>
		/// <param name="recipientId">The recipient id.</param>
		/// <param name="recipientName">Name of the recipient.</param>
		/// <param name="cc">if set to <c>true</c> cc.</param>
		/// <param name="bcc">if set to <c>true</c> bcc.</param>
		public virtual void AddRecipient(EntityType recipientType, long recipientId, string recipientName, bool cc, bool bcc)
		{
			var r = new MessageRecipientEntity();
			r.RecipientType = recipientType;
			r.RecipientId = recipientId;
			r.Name = recipientName;
			r.Cc = cc;
			r.Bcc = bcc;
			if (bcc)
			{
				r.Cc = false;
			}
			r.FolderId = (long)MessageFolder.Inbox;
			AddRecipient(r);
		}

		/// <summary>
		/// Add a message recipient
		/// </summary>
		/// <param name="recipient"></param>
		public virtual void AddRecipient(MessageRecipientEntity recipient)
		{
			if (recipient == null)
			{
				throw new ArgumentNullException("recipient");
			}
			if (recipient.RecipientType == default(EntityType) || recipient.RecipientId == default(long))
			{
				throw new ArgumentException("recipient.RecipientType and recipient.RecipientId must be set");
			}
			if (Recipients == null)
			{
				Recipients = new List<MessageRecipientEntity>();
			}
			foreach (var r in Recipients)
			{
				if (r.RecipientType == recipient.RecipientType && r.RecipientId == recipient.RecipientId)
				{
					throw new InvalidOperationException(String.Format("{0} {1} has already been added to the recipient list.", r.RecipientType, r.RecipientId));
				}
			}
			recipient.Message = this;
			Recipients.Add(recipient);
		}

		/// <summary>
		/// Get message recipient by recipient Id. Throws an exception
		/// if the recipient is not found.
		/// </summary>
		/// <param name="recipientType">Type of the recipient.</param>
		/// <param name="recipientId">The recipient id.</param>
		/// <returns></returns>
		public virtual MessageRecipientEntity FindRecipient(EntityType recipientType, long recipientId)
		{
			return FindRecipient(recipientType, recipientId, true);
		}

		/// <summary>
		/// Get message recipient by recipient Id. Optionally throws an exception
		/// if the recipient is not found.
		/// </summary>
		/// <param name="recipientType">Type of the recipient.</param>
		/// <param name="recipientId">The recipient id.</param>
		/// <param name="throwException">if set to <c>true</c> [throw exception].</param>
		/// <returns></returns>
		public virtual MessageRecipientEntity FindRecipient(EntityType recipientType, long recipientId, bool throwException)
		{
			MessageRecipientEntity retVal = null;
			if (Recipients != null)
			{
				foreach (var r in Recipients)
				{
					if (r.RecipientType == recipientType && r.RecipientId == recipientId)
					{
						retVal = r;
						break;
					}
				}
			}
			if (throwException && retVal == null)
			{
				throw new InvalidOperationException(String.Format("Could not find recipient with Type {0} and Id {1}.", recipientType, recipientId));
			}
			return retVal;
		}

		#region IComparable<MessageEntity> Members

		public virtual int CompareTo(MessageEntity other)
		{
			return (DateSent ?? DateTime.MaxValue).CompareTo(other.DateSent ?? DateTime.MaxValue);
		}

		#endregion
	}
}
