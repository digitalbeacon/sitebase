// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;

namespace DigitalBeacon.SiteBase.Model
{
	[Serializable]
	public class PreferenceEntity : GeneratedPreferenceEntity
	{
		/// <summary>
		/// Get value as an Int64
		/// </summary>
		public virtual long? ValueAsInt64
		{
			get
			{
				long? retVal = null;
				if (!String.IsNullOrEmpty(Value))
				{
					long longVal;
					if (Int64.TryParse(Value, out longVal))
					{
						retVal = longVal;
					}
				}
				return retVal;
			}
		}

		/// <summary>
		/// Get value as a DateTime
		/// </summary>
		public virtual DateTime? ValueAsDateTime
		{
			get
			{
				DateTime? retVal = null;
				if (!String.IsNullOrEmpty(Value))
				{
					long longVal;
					if (Int64.TryParse(Value, out longVal))
					{
						retVal = new DateTime(longVal);
					}
				}
				return retVal;
			}
		}

		/// <summary>
		/// Get value as a Boolean
		/// </summary>
		public virtual bool? ValueAsBoolean
		{
			get
			{
				bool? retVal = null;
				if (!String.IsNullOrEmpty(Value))
				{
					retVal = (Value.ToLower() == "true" || Value == "1") ? true : false;
				}
				return retVal;
			}
		}
	}
}
