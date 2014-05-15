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
	public class PostalCodeEntity : BaseEntity, ICodedEntity
	{
		#region Private Members
		private string _code;
		private string _city;
		private string _stateCode;
		private string _county;
		#endregion
		
		#region Properties Names
			
		public const string CityProperty = "City";
		public const string StateCodeProperty = "StateCode";
		public const string CountyProperty = "County";
			
		#endregion
		
		#region String Length Constants
			
		public const int CodeMaxLength = 20;
		public const int CityMaxLength = 100;
		public const int StateCodeMaxLength = 20;
		public const int CountyMaxLength = 50;
			
		#endregion

		#region Constructor
		/// <summary>
		/// default constructor
		/// </summary>
		public PostalCodeEntity()
		{
			_code = null;
			_city = null;
			_stateCode = null;
			_county = null;
		}
		#endregion
		
		#region Public Properties
			
		/// <summary>
		/// Code property
		/// </summary>		
		public virtual string Code
		{
			get { return _code; }
			set	
			{
				if (value != null && value.Length > 20)
				{
					throw new ArgumentOutOfRangeException("Invalid value for Code", value, value.ToString());
				}
				_code = value;
			}
		}
			
		/// <summary>
		/// City property
		/// </summary>		
		public virtual string City
		{
			get { return _city; }
			set	
			{
				if (value != null && value.Length > 100)
				{
					throw new ArgumentOutOfRangeException("Invalid value for City", value, value.ToString());
				}
				_city = value;
			}
		}
			
		/// <summary>
		/// StateCode property
		/// </summary>		
		public virtual string StateCode
		{
			get { return _stateCode; }
			set	
			{
				if (value != null && value.Length > 20)
				{
					throw new ArgumentOutOfRangeException("Invalid value for StateCode", value, value.ToString());
				}
				_stateCode = value;
			}
		}
			
		/// <summary>
		/// County property
		/// </summary>		
		public virtual string County
		{
			get { return _county; }
			set	
			{
				if (value != null && value.Length > 50)
				{
					throw new ArgumentOutOfRangeException("Invalid value for County", value, value.ToString());
				}
				_county = value;
			}
		}
			
		#endregion
	}
}
