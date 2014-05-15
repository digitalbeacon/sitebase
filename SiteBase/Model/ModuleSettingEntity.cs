// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using DigitalBeacon.Util;

namespace DigitalBeacon.SiteBase.Model
{
	[Serializable]
	public class ModuleSettingEntity : GeneratedModuleSettingEntity
	{
		/// <summary>
		/// Get value as an Int64
		/// </summary>
		public virtual long? ValueAsInt64
		{
			get
			{
				long? retVal = null;
				if (Value.HasText())
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
		/// Get value as an Double
		/// </summary>
		public virtual double? ValueAsDouble
		{
			get
			{
				double? retVal = null;
				if (Value.HasText())
				{
					double doubleVal;
					if (Double.TryParse(Value, out doubleVal))
					{
						retVal = doubleVal;
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
				if (Value.HasText())
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
				if (Value.HasText())
				{
					retVal = Boolean.TrueString.EqualsIgnoreCase(Value);
				}
				return retVal;
			}
		}
	}
}
