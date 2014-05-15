// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Reflection;
using System.Text.RegularExpressions;
using Common.Logging;
using DigitalBeacon.Business;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Data;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Model.Messaging;
using DigitalBeacon.Util;

namespace DigitalBeacon.SiteBase.Business.Support
{
	public class MailService : BaseService, IMailService
	{
		#region Private Members

		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public static readonly Regex DisplayNameRegex = new Regex(@"^[A-Za-z0-9\-., ]*$", RegexOptions.Compiled);

		private static readonly IUserDao UserDao = ServiceFactory.Instance.GetService<IUserDao>();
		private static readonly IQueuedEmailDao QueuedEmailDao = ServiceFactory.Instance.GetService<IQueuedEmailDao>();
		private static readonly IModuleService ModuleService = ServiceFactory.Instance.GetService<IModuleService>();

		#endregion

		#region IMailService Members

		public long QueueEmail(long associationId, MessageInfo msg)
		{
			return QueueEmail(associationId, msg, false);
		}

		public long QueueEmail(long associationId, MessageInfo msg, bool isSystemError)
		{
			if (isSystemError)
			{
				QueuedEmailDao.ClearSession();
			}
			var qe = new QueuedEmailEntity
			{
				AssociationId = associationId, 
				Subject = msg.Subject, 
				Body = msg.Body, 
				SendDate = msg.SendDate, 
				Recipients = new List<QueuedEmailRecipientEntity>()
			};
			var r = new QueuedEmailRecipientEntity { QueuedEmail = qe, UserId = msg.UserId };
			var user = DataAdapter.Fetch<UserEntity>(msg.UserId);
			if (user.DisplayName.HasText() && DisplayNameRegex.IsMatch(user.DisplayName))
			{
				r.Email = new MailAddress(user.Email, user.DisplayName).ToString();
			}
			else
			{
				r.Email = new MailAddress(user.Email, user.Email).ToString();
			}
			qe.Recipients.Add(r);
			QueueEmail(associationId, qe);
			return qe.Id;
		}

		public void QueueEmail(long associationId, QueuedEmailEntity qe)
		{
			qe.AssociationId = associationId;
			qe.DateProcessed = null;
			SaveWithAudit(qe);
		}

		public void ProcessEmailQueue()
		{
			var list = QueuedEmailDao.FetchMessagesToSend();
			foreach (var qe in list)
			{
				SendQueuedEmail(qe);
			}
		}

		public QueuedEmailEntity SendQueuedEmail(QueuedEmailEntity qe)
		{
			try
			{
				var recipientList = new List<string>();
				var ccList = new List<string>();
				var bccList = new List<string>();
				foreach (var r in qe.Recipients)
				{
					if (r.Bcc)
					{
						bccList.Add(r.Email);
					}
					else if (r.Cc)
					{
						ccList.Add(r.Email);
					}
					else
					{
						recipientList.Add(r.Email);
					}
				}
				var attachments = new List<Attachment>();
				foreach (var qea in qe.Attachments)
				{
					var attachment = new Attachment(new MemoryStream(qea.Data), qea.ContentType);
					//var contentDisposition = new ContentDisposition();
					if (!String.IsNullOrEmpty(qea.FileName))
					{
						attachment.ContentDisposition.FileName = qea.FileName;
					}
					attachments.Add(attachment);
				}
				var subject = qe.Subject;
				var body = qe.Body;
				if (qe.MessageId.HasValue)
				{
					var msg = DataAdapter.Fetch<MessageEntity>(qe.MessageId.Value);
					subject = msg.Subject;
					body = msg.Content;
				}
				else if (qe.Template.HasValue)
				{
					var template = ModuleService.GetGlobalModuleSetting(qe.Template.Value);
					subject = template.Subject;
					var substitutions = new Dictionary<SubstitutionDefinition, string>();
					if (qe.Recipients.Count == 1 && qe.Recipients[0].UserId.HasValue)
					{
						var user = UserDao.Fetch(qe.Recipients[0].UserId.Value);
						substitutions[SubstitutionDefinition.UserDisplayName] = user.DisplayName;
					}
					if (qe.AssociationId.HasValue)
					{
						substitutions[SubstitutionDefinition.SiteUrl] = ModuleService.GetGlobalModuleSetting(ModuleSettingDefinition.GlobalSiteUrl).Value;
					}
					body = ModuleService.GetModuleSettingValueWithSubstitutions(template, substitutions);
				}
				if (!String.IsNullOrEmpty(subject) && !String.IsNullOrEmpty(body))
				{
					var priority = qe.Priority.HasValue ? (MailPriority)qe.Priority.Value : MailPriority.Normal;
					// get message subject and body
					var errorMessage = SendMail(subject, body, qe.SenderEmail, recipientList, ccList, bccList, attachments, true, priority);
					if (!String.IsNullOrEmpty(errorMessage))
					{
						qe.ErrorMessage = errorMessage;
						Log.Error("The following error occurred while trying to send a queued mail message: " + errorMessage);
					}
					else
					{
						qe.DateSent = DateTime.Now;
						qe.ErrorMessage = null;
					}
				}
				else
				{
					qe.ErrorMessage = "Subject or Body was empty.";
					Log.Error("The following error occurred while trying to send a queued mail message: " + qe.ErrorMessage);
				}
			}
			catch (Exception ex)
			{
				qe.ErrorMessage = ex.Message;
				Log.Error("The following error occurred while trying to send a queued mail message: " + ex.Message, ex);
			}
			finally
			{
				qe.DateProcessed = DateTime.Now;
			}
			return SaveWithAudit(qe);
		}

		public IList<QueuedEmailEntity> GetQueuedEmails(QueuedEmailSearchInfo searchInfo)
		{
			return DataAdapter.FetchList(PrepareSearchInfo(searchInfo));
		}

		public long GetQueuedEmailCount(QueuedEmailSearchInfo searchInfo)
		{
			return DataAdapter.FetchCount(PrepareSearchInfo(searchInfo));
		}

		public QueuedEmailEntity GetQueuedEmail(long id)
		{
			return DataAdapter.Fetch<QueuedEmailEntity>(id);
		}

		public QueuedEmailEntity SaveQueuedEmail(QueuedEmailEntity entity)
		{
			return SaveWithAudit(entity);
		}

		public void DeleteQueuedEmail(long id)
		{
			DeleteWithAudit<QueuedEmailEntity>(id);
		}

		#endregion

		private static string SendMail(string subject, string body, string sender, IEnumerable<string> recipients, IEnumerable<string> ccRecipients, IEnumerable<string> bccRecipients, IEnumerable<Attachment> attachments, bool isBodyHtml, MailPriority priority)
		{
			var retVal = String.Empty;
			subject.Guard("subject");
			body.Guard("body");
			var msg = new MailMessage();
			if (recipients != null)
			{
				foreach (var recipient in recipients)
				{
					msg.To.Add(recipient);
				}
			}
			if (ccRecipients != null)
			{
				foreach (var recipient in ccRecipients)
				{
					msg.CC.Add(recipient);
				}
			}
			if (bccRecipients != null)
			{
				foreach (var recipient in bccRecipients)
				{
					msg.Bcc.Add(recipient);
				}
			}
			if (msg.To.Count == 0 && msg.CC.Count == 0 && msg.Bcc.Count == 0)
			{
				throw new ServiceException("Message must contain at least one recipient.");
			}
			if (attachments != null)
			{
				foreach (var a in attachments)
				{
					msg.Attachments.Add(a);
				}
			}
			msg.Priority = priority;
			msg.IsBodyHtml = isBodyHtml;
			if (!String.IsNullOrEmpty(sender))
			{
				msg.From = new MailAddress(sender);
			}
			msg.Subject = subject;
			msg.Body = body;
			var smtpClient = new SmtpClient();
			try
			{
				smtpClient.Send(msg);
			}
			catch (Exception ex)
			{
				retVal = ex.Message;
			}
			finally
			{
				msg.Dispose();
			}
			return retVal;
		}

		private static QueuedEmailSearchInfo PrepareSearchInfo(QueuedEmailSearchInfo searchInfo)
		{
			if (searchInfo.ApplyDefaultFilters)
			{
				searchInfo.AddFilter(x => x.AssociationId, searchInfo.AssociationId);
				if (searchInfo.Sent.HasValue)
				{
					searchInfo.AddFilter(x => x.DateSent, searchInfo.Sent.Value ? ComparisonOperator.NotNull : ComparisonOperator.Null);
				}
				if (searchInfo.HasError.HasValue)
				{
					searchInfo.AddFilter(x => x.ErrorMessage, searchInfo.HasError.Value ? ComparisonOperator.NotNull : ComparisonOperator.Null);
				}
				if (searchInfo.SearchText.HasText())
				{
					searchInfo.AddFilter(x => x.Subject, ComparisonOperator.Contains, searchInfo.SearchText).Grouping = 1;
					searchInfo.AddFilter(x => x.Body, ComparisonOperator.Contains, searchInfo.SearchText).Grouping = 2;
					searchInfo.AddFilter(x => x.Recipients[0].Email, ComparisonOperator.Contains, searchInfo.SearchText).Grouping = 3;
					if (searchInfo.SearchText.IsDate())
					{
						var minDate = searchInfo.MinDateForSearchText.Value;
						var maxDate = searchInfo.MaxDateForSearchText.Value;
						searchInfo.AddFilter(x => x.SendDate, ComparisonOperator.GreaterThanOrEqual, minDate).Grouping = 4;
						searchInfo.AddFilter(x => x.SendDate, ComparisonOperator.LessThanOrEqual, maxDate).Grouping = 4;
						searchInfo.AddFilter(x => x.DateProcessed, ComparisonOperator.GreaterThanOrEqual, minDate).Grouping = 5;
						searchInfo.AddFilter(x => x.DateProcessed, ComparisonOperator.LessThanOrEqual, maxDate).Grouping = 5;
						searchInfo.AddFilter(x => x.DateSent, ComparisonOperator.GreaterThanOrEqual, minDate).Grouping = 6;
						searchInfo.AddFilter(x => x.DateSent, ComparisonOperator.LessThanOrEqual, maxDate).Grouping = 6;
					}
				}
				searchInfo.ApplyDefaultFilters = false;
			}
			return searchInfo;
		}
	}
}
