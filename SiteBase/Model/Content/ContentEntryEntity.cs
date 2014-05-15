// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using DigitalBeacon.Model;

namespace DigitalBeacon.SiteBase.Model.Content
{
	[Serializable]
	public class ContentEntryEntity : GeneratedContentEntryEntity, IComparable<ContentEntryEntity>
	{
		public virtual bool CalculateDisplayOrder { get; set; }

		#region IComparable<ContentEntryEntity> Members

		public virtual int CompareTo(ContentEntryEntity other)
		{
			int retVal = 0;
			if (DisplayOrder == 0 && other.DisplayOrder > 0)
			{
				retVal = 1;
			
			}
			else if (other.DisplayOrder == 0 && DisplayOrder > 0)
			{
				retVal = -1;
			}
			else
			{
				retVal = DisplayOrder.CompareTo(other.DisplayOrder);
				if (retVal == 0)
				{
					if (ContentDate.HasValue)
					{
						retVal = ContentDate.Value.CompareTo(other.ContentDate.HasValue ? other.ContentDate.Value : DateTime.MinValue);
					}
					else if (other.ContentDate.HasValue)
					{
						retVal = -(other.ContentDate.Value.CompareTo(DateTime.MinValue));
					}
				}
			}
			return retVal;
		}

		#endregion
	}
}
