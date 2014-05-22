// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.ComponentModel;
using System.Reflection;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Web.Models;
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

		public Type EntityType { get; set; }
		public PropertyInfo TypePropertyInfo { get; set; }

		public bool SupportsTypeProperty
		{
			get { return TypePropertyInfo != null; }
		}

		public virtual bool CommentTypeRequired { get; set; }

		public virtual bool CommentTextRequired { get; set; }

		public virtual long ParentId { get; set; }

		[ReadOnly(true)]
		public virtual string PanelPrefix { get; set; }

		[ReadOnly(true)]
		public virtual bool CanDelete { get; set; }

		[ReadOnly(true)]
		public virtual DateTime? Date { get; set; }

		[ReadOnly(true)]
		public virtual string Text { get; set; }

		[LocalizedDisplayName("Comments.Text.Label")]
		public virtual string CommentText
		{
			get { return Text; }
			set { Text = value; }
		}
		
		[LocalizedDisplayName("Common.Date.Label")]
		public virtual DateTime? CommentDate
		{
			get { return Date; }
			set { Date = value; }
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