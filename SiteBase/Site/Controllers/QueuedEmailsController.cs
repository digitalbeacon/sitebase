// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using DigitalBeacon.Business;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Business;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Models.QueuedEmails;
using DigitalBeacon.SiteBase.Web;
using DigitalBeacon.SiteBase.Web.Models;
using DigitalBeacon.Util;
using System.Net.Mail;

namespace DigitalBeacon.SiteBase.Controllers
{
	[Authorization(Role.Administrator,Exclude="ProcessQueue")]
	public class QueuedEmailsController : EntityController<QueuedEmailEntity, EditModel>
	{
		private static readonly IMessageService MsgService = ServiceFactory.Instance.GetService<IMessageService>();

		public QueuedEmailsController()
		{
			EnableNewAction = false;
			DefaultSort = new[] { new SortItem { Member = QueuedEmailEntity.SendDateProperty, SortDirection = ListSortDirection.Descending } };
		}

		[Authorization(RequireLocal=true)]
		public ActionResult ProcessQueue()
		{
			MailService.ProcessEmailQueue();
			return MessageAction(PluralLabel, "QueuedEmails.ProcessQueue.Confirmation");
		}

		[HttpPost]
		public ActionResult Send(EditModel model)
		{
			ActionResult retVal;
			var entity = MailService.GetQueuedEmail(model.Id);
			if (entity.DateProcessed == null)
			{
				entity = MailService.SendQueuedEmail(entity);
			}
			if (RenderPartial)
			{
				if (entity.ErrorMessage.HasText())
				{
					retVal = RedirectToErrorAction(SingularLabel, entity.ErrorMessage);
				}
				else
				{
					retVal = RedirectToMessageAction(SingularLabel, "QueuedEmails.Send.Confirmation");
				}
				MessageModel.ReturnUrl = Url.Action("edit");
				MessageModel.ReturnText = ReturnTextSingular;
			}
			else
			{
				if (entity.ErrorMessage.HasText())
				{
					retVal = RedirectToErrorAction(SingularLabel, entity.ErrorMessage);
					MessageModel.ReturnUrl = Url.Action("edit");
					MessageModel.ReturnText = ReturnTextSingular;
				}
				else
				{
					AddTransientMessage("QueuedEmails.Send.Confirmation");
					retVal = GetIndexAction(model);
				}
			}
			return retVal ?? View(EditView, model);
		}

		[HttpPost]
		public ActionResult Dequeue(EditModel model)
		{
			ActionResult retVal;
			var entity = MailService.GetQueuedEmail(model.Id);
			entity.DateProcessed = DateTime.Now;
			entity.ErrorMessage = GetLocalizedText("QueuedEmails.Dequeue.ErrorMessage", CurrentUsername);
			MailService.SaveQueuedEmail(entity);
			if (RenderPartial)
			{
				retVal = RedirectToMessageAction(SingularLabel, "QueuedEmails.Dequeue.Confirmation");
				MessageModel.ReturnUrl = Url.Action("edit");
				MessageModel.ReturnText = ReturnTextSingular;
			}
			else
			{
				AddTransientMessage("QueuedEmails.Dequeue.Confirmation");
				retVal = GetIndexAction(model);
			}
			return retVal ?? View(EditView, model);
		}

		[HttpPost]
		public ActionResult Requeue(EditModel model)
		{
			ActionResult retVal;
			var entity = MailService.GetQueuedEmail(model.Id);
			MailService.QueueEmail(CurrentAssociationId, entity);
			if (RenderPartial)
			{
				retVal = RedirectToMessageAction(SingularLabel, "QueuedEmails.Requeue.Confirmation");
				MessageModel.ReturnUrl = Url.Action("edit");
				MessageModel.ReturnText = ReturnTextSingular;
			}
			else
			{
				AddTransientMessage("QueuedEmails.Requeue.Confirmation");
				retVal = GetIndexAction(model);
			}
			return retVal ?? View(EditView, model);
		}

		#region EntityController

		[HttpPut]
		public override ActionResult Update(FormCollection form)
		{
			if (form[EditModel.SendProperty].HasText())
			{
				return Send(ConstructUpdateModel(form));
			}
			if (form[EditModel.DequeueProperty].HasText())
			{
				return Dequeue(ConstructUpdateModel(form));
			}
			if (form[EditModel.RequeueProperty].HasText())
			{
				return Requeue(ConstructUpdateModel(form));
			}
			return base.Update(form);
		}

		protected override ListModelBase ConstructListModel()
		{
			var model = new ListModel
			{
				Sent = GetParamAsString(ListModel.SentProperty),
				HasError = GetParamAsString(ListModel.HasErrorProperty)
			};
			model.ListItems[ListModel.SentProperty] = new[]
			{
				new SelectListItem { Text = GetLocalizedText("QueuedEmails.Unsent.Label"), Value = Boolean.FalseString },
				new SelectListItem { Text = GetLocalizedText("QueuedEmails.Sent.Label"), Value = Boolean.TrueString }
			};
			model.ListItems[ListModel.HasErrorProperty] = new[]
			{
				new SelectListItem { Text = GetLocalizedText("QueuedEmails.HasError.Label"), Value = Boolean.TrueString },
				new SelectListItem { Text = GetLocalizedText("QueuedEmails.NoError.Label"), Value = Boolean.FalseString }
			};
			return model;
		}

		protected override EditModel ConstructCreateModel()
		{
			return new EditModel
					   {
						   SendDate = DateTime.Now
					   };
		}

		protected override EditModel PopulateModelForValidation(EditModel model, FormCollection form)
		{
			var id = Convert.ToInt64(form[BaseEntity.IdProperty]);
			if (id > 0)
			{
				var entity = GetEntity(id);
				if (entity.Template.HasValue)
				{
					model.TemplateId = ((long)entity.Template.Value).ToString();
				}
				model.MessageId = entity.MessageId.ToStringSafe();
			}
			return model;
		}

		protected override string GetDescription(EditModel model)
		{
			return model.Subject;
		}

		protected override SearchInfo<QueuedEmailEntity> ConstructSearchInfo()
		{
			return new QueuedEmailSearchInfo();
		}

		protected override SearchInfo<QueuedEmailEntity> PrepareSearchInfo(SearchInfo<QueuedEmailEntity> searchInfo, ListModelBase model)
		{
			var search = (QueuedEmailSearchInfo)searchInfo;
			var listModel = (ListModel)model;
			search.Sent = listModel.Sent.HasText() ? listModel.Sent.EqualsIgnoreCase(Boolean.TrueString) : (bool?)null;
			search.HasError = listModel.HasError.HasText() ? listModel.HasError.EqualsIgnoreCase(Boolean.TrueString) : (bool?)null;
			return base.PrepareSearchInfo(searchInfo, model);
		}

		protected override IEnumerable<QueuedEmailEntity> GetEntities(SearchInfo<QueuedEmailEntity> searchInfo, ListModelBase model)
		{
			return MailService.GetQueuedEmails((QueuedEmailSearchInfo)searchInfo);
		}

		protected override long GetEntityCount(SearchInfo<QueuedEmailEntity> searchInfo, ListModelBase model)
		{
			return MailService.GetQueuedEmailCount((QueuedEmailSearchInfo)searchInfo);
		}

		protected override QueuedEmailEntity GetEntity(long id)
		{
			return MailService.GetQueuedEmail(id);
		}

		protected override QueuedEmailEntity SaveEntity(QueuedEmailEntity entity, EditModel model)
		{
			entity.AssociationId = CurrentAssociationId;
			return MailService.SaveQueuedEmail(entity);
		}

		protected override void DeleteEntity(long id)
		{
			MailService.DeleteQueuedEmail(id);
		}

		protected override EditModel ConstructModel(QueuedEmailEntity entity)
		{
			var model = new EditModel
			{
				Id = entity.Id,
				TemplateId = entity.Template.HasValue? ((long)entity.Template.Value).ToString() : null,
				MessageId = entity.MessageId.ToStringSafe(),
				SendDate = entity.SendDate,
				Subject = entity.Subject,
				Body = entity.Body,
				DateProcessed = entity.DateProcessed,
				DateSent = entity.DateSent,
				ErrorMessage = entity.ErrorMessage,
				From = (entity.SenderEmail.HasText() ? entity.SenderEmail : new MailMessage().From.ToString()).ToStringSafe().Replace("\"", String.Empty),
				To = ConstructRecipientString(entity.Recipients, false, false, false),
				Cc = ConstructRecipientString(entity.Recipients, true, false, false),
				Bcc = ConstructRecipientString(entity.Recipients, false, true, false),
				Priority = entity.Priority.HasValue ? ((MailPriority)entity.Priority.Value).ToString() : null
			};
			if (entity.Template.HasValue)
			{
				var template = ModuleService.GetGlobalModuleSetting(entity.Template.Value);
				model.Subject = template.Subject;
				model.Body = template.Value;
				//var substitutions = new Dictionary<SubstitutionDefinition, string>();
				//if (entity.Recipients.Count == 1 && entity.Recipients[0].UserId.HasValue)
				//{
				//	var user = IdentityService.GetUser(entity.Recipients[0].UserId.Value);
				//	substitutions[SubstitutionDefinition.UserDisplayName] = user.DisplayName;
				//}
				//if (entity.AssociationId.HasValue)
				//{
				//	substitutions[SubstitutionDefinition.SiteUrl] = ModuleService.GetGlobalModuleSetting(ModuleSettingDefinition.GlobalSiteUrl).Value;
				//}
				//model.Body = ModuleService.GetModuleSettingValueWithSubstitutions(template, substitutions);
			}
			else if (entity.MessageId.HasValue)
			{
				var msg = MsgService.GetMessage(entity.MessageId.Value);
				model.Subject = msg.Subject;
				model.Body = msg.Content;
			}
			return model;
		}

		protected override QueuedEmailEntity ConstructEntity(EditModel model)
		{
			var entity = model.Id == 0 ? new QueuedEmailEntity { Id = model.Id } : GetEntity(model.Id);
			entity.SendDate = model.SendDate;
			entity.Subject = model.Subject.HasText() ? model.Subject : null;
			entity.Body = model.Body.HasText() ? Server.HtmlDecode(model.Body) : null;
			entity.DateProcessed = model.DateProcessed;
			entity.DateSent = model.DateSent;
			return entity;
		}

		protected override IEnumerable ConstructGridItems(IEnumerable<QueuedEmailEntity> source, ListModelBase model)
		{
			return source.Select(entity => new ListItem
			{
				Id = entity.Id,
				To = ConstructRecipientString(entity.Recipients, false, false, true),
				SendDate = entity.SendDate,
				Subject = entity.Subject,
				DateProcessed = entity.DateProcessed,
				DateSent = entity.DateSent,
				Error = entity.ErrorMessage
			});
		}

		#endregion

		#region Private Methods

		private static string ConstructRecipientString(IEnumerable<QueuedEmailRecipientEntity> recipients, bool cc, bool bcc, bool nameOnly)
		{
			if (cc)
			{
				recipients = recipients.Where(x => x.Cc);
			}
			else if (bcc)
			{
				recipients = recipients.Where(x => x.Bcc);
			}
			else
			{
				recipients = recipients.Where(x => !x.Cc && !x.Bcc);
			}
			return String.Join(", ", recipients.Select(x => nameOnly ? x.Email.Substring(0, x.Email.IndexOf('<')) : x.Email).ToArray()).ToStringSafe().Replace("\"", String.Empty);
		}

		#endregion
	}
}
