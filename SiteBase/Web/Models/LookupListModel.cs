﻿// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using DigitalBeacon.Model;

namespace DigitalBeacon.SiteBase.Web.Models
{
	public class LookupListModel<T> : LookupListModel where T : INamedEntity
	{
		public LookupListModel() : base(typeof(T))
		{
			EntityType = typeof(T);
		}
	}

	public class LookupListModel : ListModel<LookupInfo>
	{
		public Type EntityType { get; set; }
		public bool IsCoded { get; set; }
		public bool UseDisplayOrder { get; set; }
		public bool SupportsInactive { get; set; }

		public LookupListModel(Type entityType)
		{
			if (!typeof(INamedEntity).IsAssignableFrom(entityType))
			{
				throw new ArgumentException("entityType must be an instance of INamedEntity");
			}
			if (typeof(ICodedEntity).IsAssignableFrom(entityType))
			{
				IsCoded = true;
			}
			if (entityType.GetProperty(BaseEntity.DisplayOrderProperty) != null)
			{
				UseDisplayOrder = true;
			}
			EntityType = entityType;
		}

		public string BaseName
		{
			get { return EntityType.Name.Replace("Entity", String.Empty); }
		}
	}
}
