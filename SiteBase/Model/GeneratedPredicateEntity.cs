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

namespace DigitalBeacon.SiteBase.Model
{
	/// <summary>
	///	Generated with MyGeneration using the Entity template.
	/// </summary>
	[Serializable]
	public class GeneratedPredicateEntity : BaseEntity
	{
		#region Private Members
		private PredicateGroupEntity _group;
		private string _field;
		private long _operatorId;
		private string _serializedValue;
		private int _grouping;
		#endregion
		
		#region Properties Names
			
		public const string GroupProperty = "Group";
		public const string FieldProperty = "Field";
		public const string OperatorIdProperty = "OperatorId";
		public const string SerializedValueProperty = "SerializedValue";
		public const string GroupingProperty = "Grouping";
			
		#endregion
		
		#region String Length Constants
			
		public const int FieldMaxLength = 50;
		public const int SerializedValueMaxLength = 1073741823;
			
		#endregion

		#region Constructor
		/// <summary>
		/// default constructor
		/// </summary>
		public GeneratedPredicateEntity()
		{
			_group = null;
			_field = null;
			_operatorId = 0;
			_serializedValue = null;
			_grouping = 0;
		}
		#endregion
		
		#region Public Properties
			
		/// <summary>
		/// Group property
		/// </summary>		
		public virtual PredicateGroupEntity Group
		{
			get { return _group; }
			set { _group = value; }
		}
			
		/// <summary>
		/// Field property
		/// </summary>		
		public virtual string Field
		{
			get { return _field; }
			set	
			{
				if (value != null && value.Length > 50)
				{
					throw new ArgumentOutOfRangeException("Invalid value for Field", value, value.ToString());
				}
				_field = value;
			}
		}
			
		/// <summary>
		/// OperatorId property
		/// </summary>		
		public virtual long OperatorId
		{
			get { return _operatorId; }
			set { _operatorId = value; }
		}
			
		/// <summary>
		/// SerializedValue property
		/// </summary>		
		public virtual string SerializedValue
		{
			get { return _serializedValue; }
			set	
			{
				if (value != null && value.Length > 1073741823)
				{
					throw new ArgumentOutOfRangeException("Invalid value for SerializedValue", value, value.ToString());
				}
				_serializedValue = value;
			}
		}
			
		/// <summary>
		/// Grouping property
		/// </summary>		
		public virtual int Grouping
		{
			get { return _grouping; }
			set { _grouping = value; }
		}
			
		#endregion
	}
}
