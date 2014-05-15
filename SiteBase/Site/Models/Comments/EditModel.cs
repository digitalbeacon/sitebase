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

namespace DigitalBeacon.SiteBase.Models.Comments
{
	public class EditModel<T> : EditModel where T : IBaseEntity
	{
		public EditModel(string commentTypePropertyName) : base(typeof(T), commentTypePropertyName)
		{
		}
	}

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

		public virtual long ParentId { get; set; }

		[ReadOnly(true)]
		public virtual string PanelPrefix { get; set; }

		[ReadOnly(true)]
		public virtual bool CanDelete { get; set; }

		[ReadOnly(true)]
		public virtual DateTime? Date { get; set; }

		[ReadOnly(true)]
		public virtual string Text { get; set; }

		[Required]
		[LocalizedDisplayName("Comments.Text.Label")]
		public virtual string CommentText
		{
			get { return Text; }
			set { Text = value; }
		}
		
		[Required]
		[LocalizedDisplayName("Common.Date.Label")]
		public virtual DateTime? CommentDate
		{
			get { return Date; }
			set { Date = value; }
		}

		[LocalizedDisplayName("Common.Type.Label")]
		public virtual string CommentType { get; set; }
	}
}