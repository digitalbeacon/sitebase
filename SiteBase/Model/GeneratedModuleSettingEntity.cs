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
	public class GeneratedModuleSettingEntity : BaseEntity
	{
		#region Private Members
		private long? _moduleId;
		private ModuleSettingDefinition _moduleSettingDefinition;
		private string _value;
		private string _subject;
		#endregion
		
		#region Properties Names
			
		public const string ModuleIdProperty = "ModuleId";
		public const string ModuleSettingDefinitionProperty = "ModuleSettingDefinition";
		public const string ValueProperty = "Value";
		public const string SubjectProperty = "Subject";
			
		#endregion
		
		#region String Length Constants
			
		public const int ValueMaxLength = 1073741823;
		public const int SubjectMaxLength = 1073741823;
			
		#endregion

		#region Constructor
		/// <summary>
		/// default constructor
		/// </summary>
		public GeneratedModuleSettingEntity()
		{
			_moduleId = null;
			_value = null;
			_subject = null;
		}
		#endregion
		
		#region Public Properties
			
		/// <summary>
		/// ModuleId property
		/// </summary>		
		public virtual long? ModuleId
		{
			get { return _moduleId; }
			set { _moduleId = value; }
		}
			
		/// <summary>
		/// ModuleSettingDefinition property
		/// </summary>		
		public virtual ModuleSettingDefinition ModuleSettingDefinition
		{
			get { return _moduleSettingDefinition; }
			set { _moduleSettingDefinition = value; }
		}
			
		/// <summary>
		/// Value property
		/// </summary>		
		public virtual string Value
		{
			get { return _value; }
			set	
			{
				if (value != null && value.Length > 1073741823)
				{
					throw new ArgumentOutOfRangeException("Invalid value for Value", value, value.ToString());
				}
				_value = value;
			}
		}
			
		/// <summary>
		/// Subject property
		/// </summary>		
		public virtual string Subject
		{
			get { return _subject; }
			set	
			{
				if (value != null && value.Length > 1073741823)
				{
					throw new ArgumentOutOfRangeException("Invalid value for Subject", value, value.ToString());
				}
				_subject = value;
			}
		}
			
		#endregion
	}
}
