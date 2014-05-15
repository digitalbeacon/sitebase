// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.ComponentModel;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Web.Models;
using DigitalBeacon.Util;
using DigitalBeacon.Web;
using DigitalBeacon.Web.Validation;
using FluentValidation;
using FluentValidation.Attributes;

namespace DigitalBeacon.SiteBase.Models.QueuedEmails
{
	[Validator(typeof(EditModelValidator))]
	public class EditModel : EntityModel
	{
		public const string SendProperty = "Send";
		public const string DequeueProperty = "Dequeue";
		public const string RequeueProperty = "Requeue";

		public string Send { get; set; }
		public string Dequeue { get; set; }
		public string Requeue { get; set; }

		[ReadOnly(true)]
		[LocalizedDisplayName("QueuedEmails.From.Label")]
		public string From { get; set; }

		[ReadOnly(true)]
		[LocalizedDisplayName("QueuedEmails.To.Label")]
		public string To { get; set; }

		[ReadOnly(true)]
		[LocalizedDisplayName("QueuedEmails.Cc.Label")]
		public string Cc { get; set; }

		[ReadOnly(true)]
		[LocalizedDisplayName("QueuedEmails.Bcc.Label")]
		public string Bcc { get; set; }

		[ReadOnly(true)]
		[LocalizedDisplayName("QueuedEmails.Priority.Label")]
		public string Priority { get; set; }

		[ReadOnly(true)]
		[LocalizedDisplayName("QueuedEmails.MessageId.Label")]
		public string MessageId { get; set; }

		[ReadOnly(true)]
		[LocalizedDisplayName("QueuedEmails.TemplateId.Label")]
		public string TemplateId { get; set; }

		[LocalizedDisplayName("QueuedEmails.SendDate.Label")]
		public DateTime SendDate { get; set; }

		[ReadOnly(true)]
		[LocalizedDisplayName("QueuedEmails.DateProcessed.Label")]
		public DateTime? DateProcessed { get; set; }

		[ReadOnly(true)]
		[LocalizedDisplayName("QueuedEmails.DateSent.Label")]
		public DateTime? DateSent { get; set; }

		[StringLength(QueuedEmailEntity.SubjectMaxLength)]
		[LocalizedDisplayName("QueuedEmails.Subject.Label")]
		public string Subject { get; set; }

		[StringLength(QueuedEmailEntity.BodyMaxLength)]
		[LocalizedDisplayName("QueuedEmails.Body.Label")]
		public string Body { get; set; }

		[ReadOnly(true)]
		[LocalizedDisplayName("QueuedEmails.ErrorMessage.Label")]
		public string ErrorMessage { get; set; }

		public bool ContentRequired
		{
			get { return MessageId.IsNullOrBlank() && TemplateId.IsNullOrBlank(); }
		}
	}

	public class EditModelValidator : BaseValidator<EditModel>
	{
		public EditModelValidator()
		{
			RuleFor(x => x.Subject)
				.NotNullOrBlank()
				.When(x => x.ContentRequired)
				.WithLocalizedMessage("Validation.Error.Required", "QueuedEmails.Subject.Label");
			RuleFor(x => x.Subject)
				.Must(x => x.IsNullOrBlank())
				.When(x => !x.ContentRequired)
				.WithLocalizedMessage("QueuedEmails.Error.ContentNotAllowed");
			RuleFor(x => x.Body)
				.NotNullOrBlank()
				.When(x => x.ContentRequired)
				.WithLocalizedMessage("Validation.Error.Required", "QueuedEmails.Body.Label");
			RuleFor(x => x.Body)
				.Must(x => x.IsNullOrBlank())
				.When(x => !x.ContentRequired)
				.WithLocalizedMessage("QueuedEmails.Error.ContentNotAllowed");
		}
	}
}