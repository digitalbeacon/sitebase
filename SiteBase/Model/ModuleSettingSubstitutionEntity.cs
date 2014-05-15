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
	public class ModuleSettingSubstitutionEntity : BaseEntity
	{
		#region Private Members
		private ModuleSettingDefinition _moduleSettingDefinition;
		private SubstitutionDefinitionEntity _substitutionDefinition;
		#endregion
		
		#region Properties Names
			
		public const string ModuleSettingDefinitionProperty = "ModuleSettingDefinition";
		public const string SubstitutionDefinitionProperty = "SubstitutionDefinition";
			
		#endregion

		#region Constructor
		/// <summary>
		/// default constructor
		/// </summary>
		public ModuleSettingSubstitutionEntity()
		{
		}
		#endregion
		
		#region Public Properties
			
		/// <summary>
		/// ModuleSettingDefinition property
		/// </summary>		
		public virtual ModuleSettingDefinition ModuleSettingDefinition
		{
			get { return _moduleSettingDefinition; }
			set { _moduleSettingDefinition = value; }
		}
			
		/// <summary>
		/// SubstitutionDefinition property
		/// </summary>		
		public virtual SubstitutionDefinitionEntity SubstitutionDefinition
		{
			get { return _substitutionDefinition; }
			set { _substitutionDefinition = value; }
		}
			
		#endregion
	}
}
