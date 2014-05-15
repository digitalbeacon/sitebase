// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Reflection;
using DigitalBeacon.SiteBase.Model;

namespace DigitalBeacon.SiteBase.Models.Comments
{
	public class ListModel<T> : ListModel
	{
		public ListModel(string commentTypePropertyName, string flaggedPropertyName, bool showTextInline = true)
			: base(typeof(T), commentTypePropertyName, flaggedPropertyName)
		{
			ShowTextInline = showTextInline;
		}
	}

	public class ListModel : DigitalBeacon.SiteBase.Web.Models.ListModel<ListItem>
	{
		public ListModel(Type entityType, string commentTypePropertyName, string flaggedPropertyName)
		{
			EntityType = entityType;
			TypePropertyInfo = entityType.GetProperty(commentTypePropertyName);
			FlaggedPropertyInfo = entityType.GetProperty(flaggedPropertyName);
		}

		public virtual bool ShowTextInline { get; set; }
		public virtual bool CanAdd { get; set; }
		public virtual bool CanDelete { get; set; }
		public virtual bool CanUpdate { get; set; }
		public virtual string PanelPrefix { get; set; }
		public virtual Type EntityType { get; set; }
		public virtual PropertyInfo TypePropertyInfo { get; set; }
		public virtual PropertyInfo FlaggedPropertyInfo { get; set; }

		public virtual bool SupportsTypeProperty
		{
			get { return TypePropertyInfo != null; }
		}

		public virtual bool SupportsFlaggedProperty
		{
			get { return FlaggedPropertyInfo != null; }
		}
	}
}