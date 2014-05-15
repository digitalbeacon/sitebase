// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;

namespace DigitalBeacon.Model
{
	[Serializable]
	public class LocalizedEntity
	{
		public Type Type { get; set; }
		public string LocalizedProperty { get; set; }

		protected LocalizedEntity()
		{
		}

		public static LocalizedEntity Create<T>() where T : INamedEntity
		{
			return new LocalizedEntity { Type = typeof(T), LocalizedProperty = BaseEntity.NameProperty };
		}

		public static LocalizedEntity Create<T>(string property) where T : IBaseEntity
		{
			return new LocalizedEntity { Type = typeof(T), LocalizedProperty = property };
		}
	}
}
