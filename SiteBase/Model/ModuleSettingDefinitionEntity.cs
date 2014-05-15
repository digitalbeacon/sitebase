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
	public class ModuleSettingDefinitionEntity : BaseEntity
	{
		#region Private Members
		private ModuleDefinition _moduleDefinition;
		private ModuleSettingType _moduleSettingType;
		private string _key;
		private string _name;
		private string _introducedInVersion;
		private string _customEditor;
		private string _defaultValue;
		private string _defaultSubject;
		private double? _minValue;
		private double? _maxValue;
		private bool _required;
		private bool _global;
		private int _displayOrder;
		private bool _localizable;
		#endregion
		
		#region Properties Names
			
		public const string ModuleDefinitionProperty = "ModuleDefinition";
		public const string ModuleSettingTypeProperty = "ModuleSettingType";
		public const string KeyProperty = "Key";
		public const string IntroducedInVersionProperty = "IntroducedInVersion";
		public const string CustomEditorProperty = "CustomEditor";
		public const string DefaultValueProperty = "DefaultValue";
		public const string DefaultSubjectProperty = "DefaultSubject";
		public const string MinValueProperty = "MinValue";
		public const string MaxValueProperty = "MaxValue";
		public const string RequiredProperty = "Required";
		public const string GlobalProperty = "Global";
		public const string LocalizableProperty = "Localizable";
			
		#endregion
		
		#region String Length Constants
			
		public const int KeyMaxLength = 100;
		public const int NameMaxLength = 100;
		public const int IntroducedInVersionMaxLength = 20;
		public const int CustomEditorMaxLength = 200;
		public const int DefaultValueMaxLength = 1073741823;
		public const int DefaultSubjectMaxLength = 1073741823;
			
		#endregion

		#region Constructor
		/// <summary>
		/// default constructor
		/// </summary>
		public ModuleSettingDefinitionEntity()
		{
			_key = null;
			_name = null;
			_introducedInVersion = null;
			_customEditor = null;
			_defaultValue = null;
			_defaultSubject = null;
			_minValue = null;
			_maxValue = null;
			_required = false; 
			_global = false; 
			_displayOrder = 0;
			_localizable = false; 
		}
		#endregion
		
		#region Public Properties
			
		/// <summary>
		/// ModuleDefinition property
		/// </summary>		
		public virtual ModuleDefinition ModuleDefinition
		{
			get { return _moduleDefinition; }
			set { _moduleDefinition = value; }
		}
			
		/// <summary>
		/// ModuleSettingType property
		/// </summary>		
		public virtual ModuleSettingType ModuleSettingType
		{
			get { return _moduleSettingType; }
			set { _moduleSettingType = value; }
		}
			
		/// <summary>
		/// Key property
		/// </summary>		
		public virtual string Key
		{
			get { return _key; }
			set	
			{
				if (value != null && value.Length > 100)
				{
					throw new ArgumentOutOfRangeException("Invalid value for Key", value, value.ToString());
				}
				_key = value;
			}
		}
			
		/// <summary>
		/// Name property
		/// </summary>		
		public virtual string Name
		{
			get { return _name; }
			set	
			{
				if (value != null && value.Length > 100)
				{
					throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
				}
				_name = value;
			}
		}
			
		/// <summary>
		/// IntroducedInVersion property
		/// </summary>		
		public virtual string IntroducedInVersion
		{
			get { return _introducedInVersion; }
			set	
			{
				if (value != null && value.Length > 20)
				{
					throw new ArgumentOutOfRangeException("Invalid value for IntroducedInVersion", value, value.ToString());
				}
				_introducedInVersion = value;
			}
		}
			
		/// <summary>
		/// CustomEditor property
		/// </summary>		
		public virtual string CustomEditor
		{
			get { return _customEditor; }
			set	
			{
				if (value != null && value.Length > 200)
				{
					throw new ArgumentOutOfRangeException("Invalid value for CustomEditor", value, value.ToString());
				}
				_customEditor = value;
			}
		}
			
		/// <summary>
		/// DefaultValue property
		/// </summary>		
		public virtual string DefaultValue
		{
			get { return _defaultValue; }
			set	
			{
				if (value != null && value.Length > 1073741823)
				{
					throw new ArgumentOutOfRangeException("Invalid value for DefaultValue", value, value.ToString());
				}
				_defaultValue = value;
			}
		}
			
		/// <summary>
		/// DefaultSubject property
		/// </summary>		
		public virtual string DefaultSubject
		{
			get { return _defaultSubject; }
			set	
			{
				if (value != null && value.Length > 1073741823)
				{
					throw new ArgumentOutOfRangeException("Invalid value for DefaultSubject", value, value.ToString());
				}
				_defaultSubject = value;
			}
		}
			
		/// <summary>
		/// MinValue property
		/// </summary>		
		public virtual double? MinValue
		{
			get { return _minValue; }
			set { _minValue = value; }
		}
			
		/// <summary>
		/// MaxValue property
		/// </summary>		
		public virtual double? MaxValue
		{
			get { return _maxValue; }
			set { _maxValue = value; }
		}
			
		/// <summary>
		/// Required property
		/// </summary>		
		public virtual bool Required
		{
			get { return _required; }
			set { _required = value; }
		}
			
		/// <summary>
		/// Global property
		/// </summary>		
		public virtual bool Global
		{
			get { return _global; }
			set { _global = value; }
		}
			
		/// <summary>
		/// DisplayOrder property
		/// </summary>		
		public virtual int DisplayOrder
		{
			get { return _displayOrder; }
			set { _displayOrder = value; }
		}
			
		/// <summary>
		/// Localizable property
		/// </summary>		
		public virtual bool Localizable
		{
			get { return _localizable; }
			set { _localizable = value; }
		}
			
		#endregion
	}
}
