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
	public class ComparisonOperatorEntity : BaseEntity, INamedEntity
	{
		#region Private Members
		private string _name;
		#endregion
		
		#region String Length Constants
			
		public const int NameMaxLength = 50;
			
		#endregion

		#region Constructor
		/// <summary>
		/// default constructor
		/// </summary>
		public ComparisonOperatorEntity()
		{
			_name = null;
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
				if (value != null && value.Length > 50)
				{
					throw new ArgumentOutOfRangeException("Invalid value for Name", value, value.ToString());
				}
				_name = value;
			}
		}
			
		#endregion
	}
}
