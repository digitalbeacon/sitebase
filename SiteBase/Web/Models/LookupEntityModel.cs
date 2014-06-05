// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using DigitalBeacon.Web;
using DigitalBeacon.Web.Validation;
using DigitalBeacon.Model;
using System.Reflection;
using System.ComponentModel;

namespace DigitalBeacon.SiteBase.Web.Models
{
	public class LookupEntityModel<T> : LookupEntityModel where T : INamedEntity
	{
		public LookupEntityModel() : base(typeof(T))
		{
			EntityType = typeof(T);
		}
	}

	public class LookupEntityModel : NamedEntityModel
	{
		public LookupEntityModel(Type entityType)
		{
			if (!typeof(INamedEntity).IsAssignableFrom(entityType))
			{
				throw new ArgumentException("EntityType must be an instance of INamedEntity");
			}
			if (typeof(ICodedEntity).IsAssignableFrom(entityType))
			{
				IsCoded = true;
			}
			DisplayOrderPropertyInfo = entityType.GetProperty(BaseEntity.DisplayOrderProperty);
			CommentPropertyInfo = entityType.GetProperty(BaseEntity.CommentProperty);
			EntityType = entityType;
		}

		[ReadOnly(true)]
		public Type EntityType { get; set; }
		
		[ReadOnly(true)]
		public bool IsCoded { get; set; }
		
		[ReadOnly(true)]
		public PropertyInfo DisplayOrderPropertyInfo { get; set; }

		[ReadOnly(true)]
		public PropertyInfo CommentPropertyInfo { get; set; }

		[ReadOnly(true)]
		public bool SupportsInactive { get; set; }

		public bool UseDisplayOrder 
		{
			get { return DisplayOrderPropertyInfo != null; }
		}

		public bool SupportsComments
		{
			get { return CommentPropertyInfo != null; }
		}

		public string BaseName
		{
			get { return EntityType.Name.Replace("Entity", String.Empty); }
		}

		[StringLength(100)]
		[LocalizedDisplayName("Common.Code.Label")]
		public virtual string Code { get; set; }

		[LocalizedDisplayName("Common.DisplayOrder.Label")]
		public virtual int? DisplayOrder { get; set; }

		[LocalizedDisplayName("Common.Comment.Label")]
		public virtual string Comment { get; set; }

		[LocalizedDisplayName("Common.Inactive.Label")]
		public virtual bool? Inactive { get; set; }
	}
}