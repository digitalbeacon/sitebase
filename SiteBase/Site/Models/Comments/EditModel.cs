// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.ComponentModel;
using System.Reflection;
using System.Web.Script.Serialization;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Web;
using DigitalBeacon.SiteBase.Web.Models;
using DigitalBeacon.Util;
using DigitalBeacon.Web;
using DigitalBeacon.Web.Validation;
using FluentValidation;
using FluentValidation.Attributes;

namespace DigitalBeacon.SiteBase.Models.Comments
{
	[Validator(typeof(EditModelValidator))]
	public class EditModel<T> : EditModel where T : IBaseEntity
	{
		public EditModel(string commentTypePropertyName) : base(typeof(T), commentTypePropertyName)
		{
		}
	}

	[Validator(typeof(EditModelValidator))]
	public class EditModel : EntityModel
	{
		public EditModel()
		{
		}

		public EditModel(Type entityType, string commentTypePropertyName)
		{
			EntityType = entityType;
			TypePropertyInfo = entityType.GetProperty(commentTypePropertyName);
		}

		[ScriptIgnore]
		public Type EntityType { get; set; }

		[ScriptIgnore]
		public PropertyInfo TypePropertyInfo { get; set; }

		[ScriptIgnore]
		public bool SupportsTypeProperty
		{
			get { return TypePropertyInfo != null; }
		}

		[ReadOnly(true)]
		[ScriptIgnore]
		public virtual bool CommentTypeRequired { get; set; }

		[ReadOnly(true)]
		[ScriptIgnore]
		public virtual bool CommentTextRequired { get; set; }

		public virtual long ParentId { get; set; }

		[ReadOnly(true)]
		[ScriptIgnore]
		public virtual string PanelPrefix { get; set; }

		[ReadOnly(true)]
		[ScriptIgnore]
		public virtual bool CanDelete { get; set; }

		[ReadOnly(true)]
		[ScriptIgnore]
		public virtual DateTime? Date { get; set; }

		[ReadOnly(true)]
		[ScriptIgnore]
		public virtual string Text { get; set; }

		[LocalizedDisplayName("Comments.Text.Label")]
		public virtual string CommentText
		{
			get { return Text; }
			set { Text = value; }
		}
		
		[LocalizedDisplayName("Common.Date.Label")]
		public string CommentDate
		{
			get { return Date.HasValue ? Date.Value.ToString(WebConstants.DefaultDateTimeFormat) : string.Empty; }
			set { Date = value.ToDate(); }
		}

		[LocalizedDisplayName("Common.Type.Label")]
		public virtual string CommentType { get; set; }
	}

	public class EditModelValidator : BaseValidator<EditModel>
	{
		public EditModelValidator()
		{
			RuleFor(x => x.CommentText)
				.NotNullOrBlank()
				.When(x => x.CommentTextRequired)
				.WithLocalizedMessage("Validation.Error.Required", "Comments.Text.Label");
			RuleFor(x => x.CommentType)
				.NotNullOrBlank()
				.When(x => x.CommentTypeRequired)
				.WithLocalizedMessage("Validation.Error.Required", "Common.Type.Label");
		}
	}
}