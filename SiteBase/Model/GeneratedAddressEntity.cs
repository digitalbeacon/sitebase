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
	public class GeneratedAddressEntity : BaseEntity
	{
		#region Private Members
		private string _line1;
		private string _line2;
		private string _city;
		private State? _state;
		private Country? _country;
		private string _postalCode;
		private string _county;
		private PhoneType? _defaultPhone;
		private string _homePhone;
		private string _workPhone;
		private string _workPhoneExt;
		private string _mobilePhone;
		private string _fax;
		private string _email;
		#endregion
		
		#region Properties Names
			
		public const string Line1Property = "Line1";
		public const string Line2Property = "Line2";
		public const string CityProperty = "City";
		public const string StateProperty = "State";
		public const string CountryProperty = "Country";
		public const string PostalCodeProperty = "PostalCode";
		public const string CountyProperty = "County";
		public const string DefaultPhoneProperty = "DefaultPhone";
		public const string HomePhoneProperty = "HomePhone";
		public const string WorkPhoneProperty = "WorkPhone";
		public const string WorkPhoneExtProperty = "WorkPhoneExt";
		public const string MobilePhoneProperty = "MobilePhone";
		public const string FaxProperty = "Fax";
		public const string EmailProperty = "Email";
			
		#endregion
		
		#region String Length Constants
			
		public const int Line1MaxLength = 200;
		public const int Line2MaxLength = 200;
		public const int CityMaxLength = 100;
		public const int PostalCodeMaxLength = 20;
		public const int CountyMaxLength = 50;
		public const int HomePhoneMaxLength = 20;
		public const int WorkPhoneMaxLength = 20;
		public const int WorkPhoneExtMaxLength = 10;
		public const int MobilePhoneMaxLength = 20;
		public const int FaxMaxLength = 20;
		public const int EmailMaxLength = 100;
			
		#endregion

		#region Constructor
		/// <summary>
		/// default constructor
		/// </summary>
		public GeneratedAddressEntity()
		{
			_line1 = null;
			_line2 = null;
			_city = null;
			_state = null;
			_country = null;
			_postalCode = null;
			_county = null;
			_defaultPhone = null;
			_homePhone = null;
			_workPhone = null;
			_workPhoneExt = null;
			_mobilePhone = null;
			_fax = null;
			_email = null;
		}
		#endregion
		
		#region Public Properties
			
		/// <summary>
		/// Line1 property
		/// </summary>		
		public virtual string Line1
		{
			get { return _line1; }
			set	
			{
				if (value != null && value.Length > 200)
				{
					throw new ArgumentOutOfRangeException("Invalid value for Line1", value, value.ToString());
				}
				_line1 = value;
			}
		}
			
		/// <summary>
		/// Line2 property
		/// </summary>		
		public virtual string Line2
		{
			get { return _line2; }
			set	
			{
				if (value != null && value.Length > 200)
				{
					throw new ArgumentOutOfRangeException("Invalid value for Line2", value, value.ToString());
				}
				_line2 = value;
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
		/// State property
		/// </summary>		
		public virtual State? State
		{
			get { return _state; }
			set { _state = value; }
		}
			
		/// <summary>
		/// Country property
		/// </summary>		
		public virtual Country? Country
		{
			get { return _country; }
			set { _country = value; }
		}
			
		/// <summary>
		/// PostalCode property
		/// </summary>		
		public virtual string PostalCode
		{
			get { return _postalCode; }
			set	
			{
				if (value != null && value.Length > 20)
				{
					throw new ArgumentOutOfRangeException("Invalid value for PostalCode", value, value.ToString());
				}
				_postalCode = value;
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
			
		/// <summary>
		/// DefaultPhone property
		/// </summary>		
		public virtual PhoneType? DefaultPhone
		{
			get { return _defaultPhone; }
			set { _defaultPhone = value; }
		}
			
		/// <summary>
		/// HomePhone property
		/// </summary>		
		public virtual string HomePhone
		{
			get { return _homePhone; }
			set	
			{
				if (value != null && value.Length > 20)
				{
					throw new ArgumentOutOfRangeException("Invalid value for HomePhone", value, value.ToString());
				}
				_homePhone = value;
			}
		}
			
		/// <summary>
		/// WorkPhone property
		/// </summary>		
		public virtual string WorkPhone
		{
			get { return _workPhone; }
			set	
			{
				if (value != null && value.Length > 20)
				{
					throw new ArgumentOutOfRangeException("Invalid value for WorkPhone", value, value.ToString());
				}
				_workPhone = value;
			}
		}
			
		/// <summary>
		/// WorkPhoneExt property
		/// </summary>		
		public virtual string WorkPhoneExt
		{
			get { return _workPhoneExt; }
			set	
			{
				if (value != null && value.Length > 10)
				{
					throw new ArgumentOutOfRangeException("Invalid value for WorkPhoneExt", value, value.ToString());
				}
				_workPhoneExt = value;
			}
		}
			
		/// <summary>
		/// MobilePhone property
		/// </summary>		
		public virtual string MobilePhone
		{
			get { return _mobilePhone; }
			set	
			{
				if (value != null && value.Length > 20)
				{
					throw new ArgumentOutOfRangeException("Invalid value for MobilePhone", value, value.ToString());
				}
				_mobilePhone = value;
			}
		}
			
		/// <summary>
		/// Fax property
		/// </summary>		
		public virtual string Fax
		{
			get { return _fax; }
			set	
			{
				if (value != null && value.Length > 20)
				{
					throw new ArgumentOutOfRangeException("Invalid value for Fax", value, value.ToString());
				}
				_fax = value;
			}
		}
			
		/// <summary>
		/// Email property
		/// </summary>		
		public virtual string Email
		{
			get { return _email; }
			set	
			{
				if (value != null && value.Length > 100)
				{
					throw new ArgumentOutOfRangeException("Invalid value for Email", value, value.ToString());
				}
				_email = value;
			}
		}
			
		#endregion
	}
}
