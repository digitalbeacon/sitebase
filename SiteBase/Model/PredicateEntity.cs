// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using Iesi.Collections.Generic;
using DigitalBeacon.Model;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace DigitalBeacon.SiteBase.Model
{
	[Serializable]
	public class PredicateEntity : GeneratedPredicateEntity
	{
		private static readonly Dictionary<Type, XmlSerializer> _serializerMap = new Dictionary<Type, XmlSerializer>();

		static PredicateEntity()
		{
			RegisterSerializer(typeof(string));
			RegisterSerializer(typeof(long));
			RegisterSerializer(typeof(int));
			RegisterSerializer(typeof(double));
			RegisterSerializer(typeof(bool));
			RegisterSerializer(typeof(DateTime));
			RegisterSerializer(typeof(decimal));
		}

		#region Public Properties

		public PredicateEntity() : base() {}

		public PredicateEntity(string field, object value) : base()
		{
			Field = field;
			OperatorId = (long)ComparisonOperator.Equal;
			Value = value;
		}

		public PredicateEntity(string field, ComparisonOperator op, object value) : base()
		{
			Field = field;
			OperatorId = (long)op;
			Value = value;
		}

		public PredicateEntity(string field, ComparisonOperator op, object value, int? grouping) : base()
		{
			Field = field;
			OperatorId = (long)op;
			Value = value;
			Grouping = grouping.HasValue ? grouping.Value : 0;
		}

		public static void RegisterSerializer(Type type)
		{
			if (!_serializerMap.ContainsKey(type))
			{
				_serializerMap[type] = new XmlSerializer(type);
			}
		}

		public virtual object Value
		{
			get
			{
				object retVal = null;
				if (!String.IsNullOrEmpty(SerializedValue))
				{
					XmlReader reader = XmlReader.Create(new StringReader(SerializedValue));
					foreach (XmlSerializer serializer in _serializerMap.Values)
					{
						if (serializer.CanDeserialize(reader))
						{
							retVal = serializer.Deserialize(reader);
							break;
						}
					}
				}
				return retVal;
			}

			set
			{
				StringWriter writer = null;
				if (value != null)
				{
					foreach (Type t in _serializerMap.Keys)
					{
						if (value.GetType() == t || t.IsAssignableFrom(value.GetType()))
						{
							writer = new StringWriter();
							_serializerMap[t].Serialize(writer, value);
						}
					}
				}
				SerializedValue = writer != null ? writer.ToString() : null;
			}
		}

		#endregion
	}
}
