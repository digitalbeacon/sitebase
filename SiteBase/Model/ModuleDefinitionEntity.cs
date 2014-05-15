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
	public class ModuleDefinitionEntity : BaseEntity, INamedEntity
	{
		#region Private Members
		private string _name;
		private string _versionNumber;
		private int _displayOrder;
		private bool _allowMultiple;
		#endregion
		
		#region Properties Names
			
		public const string VersionNumberProperty = "VersionNumber";
		public const string AllowMultipleProperty = "AllowMultiple";
			
		#endregion
		
		#region String Length Constants
			
		public const int NameMaxLength = 100;
		public const int VersionNumberMaxLength = 20;
			
		#endregion

		#region Constructor
		/// <summary>
		/// default constructor
		/// </summary>
		public ModuleDefinitionEntity()
		{
			_name = null;
			_versionNumber = null;
			_displayOrder = 0;
			_allowMultiple = false; 
		}
		#endregion
		
		#region Public Properties
			
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
		/// VersionNumber property
		/// </summary>		
		public virtual string VersionNumber
		{
			get { return _versionNumber; }
			set	
			{
				if (value != null && value.Length > 20)
				{
					throw new ArgumentOutOfRangeException("Invalid value for VersionNumber", value, value.ToString());
				}
				_versionNumber = value;
			}
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
		/// AllowMultiple property
		/// </summary>		
		public virtual bool AllowMultiple
		{
			get { return _allowMultiple; }
			set { _allowMultiple = value; }
		}
			
		#endregion
	}
}
