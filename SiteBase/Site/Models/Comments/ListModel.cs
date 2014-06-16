// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Reflection;
using System.Web.Script.Serialization;
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

		[ScriptIgnore]
		public virtual bool ShowTextInline { get; set; }

		[ScriptIgnore]
		public virtual bool CanAdd { get; set; }

		[ScriptIgnore]
		public virtual bool CanDelete { get; set; }

		[ScriptIgnore]
		public virtual bool CanUpdate { get; set; }

		[ScriptIgnore]
		public virtual string PanelPrefix { get; set; }

		[ScriptIgnore]
		public virtual Type EntityType { get; set; }

		[ScriptIgnore]
		public virtual PropertyInfo TypePropertyInfo { get; set; }

		[ScriptIgnore]
		public virtual PropertyInfo FlaggedPropertyInfo { get; set; }

		[ScriptIgnore]
		public virtual bool SupportsTypeProperty
		{
			get { return TypePropertyInfo != null; }
		}

		[ScriptIgnore]
		public virtual bool SupportsFlaggedProperty
		{
			get { return FlaggedPropertyInfo != null; }
		}
	}
}