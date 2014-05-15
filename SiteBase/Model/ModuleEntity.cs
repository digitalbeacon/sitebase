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
	public class ModuleEntity : BaseEntity
	{
		#region Private Members
		private long _associationId;
		private ModuleDefinition _moduleDefinition;
		private string _name;
		private string _url;
		private bool _defaultInstance;
		#endregion
		
		#region Properties Names
			
		public const string AssociationIdProperty = "AssociationId";
		public const string ModuleDefinitionProperty = "ModuleDefinition";
		public const string UrlProperty = "Url";
		public const string DefaultInstanceProperty = "DefaultInstance";
			
		#endregion
		
		#region String Length Constants
			
		public const int NameMaxLength = 100;
		public const int UrlMaxLength = 250;
			
		#endregion

		#region Constructor
		/// <summary>
		/// default constructor
		/// </summary>
		public ModuleEntity()
		{
			_associationId = 0;
			_name = null;
			_url = null;
			_defaultInstance = false; 
		}
		#endregion
		
		#region Public Properties
			
		/// <summary>
		/// AssociationId property
		/// </summary>		
		public virtual long AssociationId
		{
			get { return _associationId; }
			set { _associationId = value; }
		}
			
		/// <summary>
		/// ModuleDefinition property
		/// </summary>		
		public virtual ModuleDefinition ModuleDefinition
		{
			get { return _moduleDefinition; }
			set { _moduleDefinition = value; }
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
		/// Url property
		/// </summary>		
		public virtual string Url
		{
			get { return _url; }
			set	
			{
				if (value != null && value.Length > 250)
				{
					throw new ArgumentOutOfRangeException("Invalid value for Url", value, value.ToString());
				}
				_url = value;
			}
		}
			
		/// <summary>
		/// DefaultInstance property
		/// </summary>		
		public virtual bool DefaultInstance
		{
			get { return _defaultInstance; }
			set { _defaultInstance = value; }
		}
			
		#endregion
	}
}
