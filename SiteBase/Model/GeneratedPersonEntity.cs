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
	public class GeneratedPersonEntity : BaseEntity, IVersionedEntity
	{
		#region Private Members
		private DateTime _created;
		private DateTime? _deleted;
		private string _firstName;
		private string _middleName;
		private string _lastName;
		private string _title;
		private string _suffix;
		private Gender? _gender;
		private DateTime? _dateOfBirth;
		private AddressEntity _address;
		private string _encryptedSsn;
		private string _ssn4;
		private long? _raceId;
		#endregion
		
		#region Properties Names
			
		public const string CreatedProperty = "Created";
		public const string DeletedProperty = "Deleted";
		public const string FirstNameProperty = "FirstName";
		public const string MiddleNameProperty = "MiddleName";
		public const string LastNameProperty = "LastName";
		public const string TitleProperty = "Title";
		public const string SuffixProperty = "Suffix";
		public const string GenderProperty = "Gender";
		public const string DateOfBirthProperty = "DateOfBirth";
		public const string AddressProperty = "Address";
		public const string EncryptedSsnProperty = "EncryptedSsn";
		public const string Ssn4Property = "Ssn4";
		public const string RaceIdProperty = "RaceId";
			
		#endregion
		
		#region String Length Constants
			
		public const int FirstNameMaxLength = 100;
		public const int MiddleNameMaxLength = 100;
		public const int LastNameMaxLength = 100;
		public const int TitleMaxLength = 100;
		public const int SuffixMaxLength = 100;
		public const int EncryptedSsnMaxLength = 100;
		public const int Ssn4MaxLength = 4;
			
		#endregion

		#region Constructor
		/// <summary>
		/// default constructor
		/// </summary>
		public GeneratedPersonEntity()
		{
			_created = DateTime.Now;
			_deleted = null;
			_firstName = null;
			_middleName = null;
			_lastName = null;
			_title = null;
			_suffix = null;
			_gender = null;
			_dateOfBirth = null;
			_address = null;
			_encryptedSsn = null;
			_ssn4 = null;
			_raceId = null;
		}
		#endregion
		
		#region Public Properties
			
		/// <summary>
		/// Created property
		/// </summary>		
		public virtual DateTime Created
		{
			get { return _created; }
			set { _created = value; }
		}
			
		/// <summary>
		/// Deleted property
		/// </summary>		
		public virtual DateTime? Deleted
		{
			get { return _deleted; }
			set { _deleted = value; }
		}
			
		/// <summary>
		/// FirstName property
		/// </summary>		
		public virtual string FirstName
		{
			get { return _firstName; }
			set	
			{
				if (value != null && value.Length > 100)
				{
					throw new ArgumentOutOfRangeException("Invalid value for FirstName", value, value.ToString());
				}
				_firstName = value;
			}
		}
			
		/// <summary>
		/// MiddleName property
		/// </summary>		
		public virtual string MiddleName
		{
			get { return _middleName; }
			set	
			{
				if (value != null && value.Length > 100)
				{
					throw new ArgumentOutOfRangeException("Invalid value for MiddleName", value, value.ToString());
				}
				_middleName = value;
			}
		}
			
		/// <summary>
		/// LastName property
		/// </summary>		
		public virtual string LastName
		{
			get { return _lastName; }
			set	
			{
				if (value != null && value.Length > 100)
				{
					throw new ArgumentOutOfRangeException("Invalid value for LastName", value, value.ToString());
				}
				_lastName = value;
			}
		}
			
		/// <summary>
		/// Title property
		/// </summary>		
		public virtual string Title
		{
			get { return _title; }
			set	
			{
				if (value != null && value.Length > 100)
				{
					throw new ArgumentOutOfRangeException("Invalid value for Title", value, value.ToString());
				}
				_title = value;
			}
		}
			
		/// <summary>
		/// Suffix property
		/// </summary>		
		public virtual string Suffix
		{
			get { return _suffix; }
			set	
			{
				if (value != null && value.Length > 100)
				{
					throw new ArgumentOutOfRangeException("Invalid value for Suffix", value, value.ToString());
				}
				_suffix = value;
			}
		}
			
		/// <summary>
		/// Gender property
		/// </summary>		
		public virtual Gender? Gender
		{
			get { return _gender; }
			set { _gender = value; }
		}
			
		/// <summary>
		/// DateOfBirth property
		/// </summary>		
		public virtual DateTime? DateOfBirth
		{
			get { return _dateOfBirth; }
			set { _dateOfBirth = value; }
		}
			
		/// <summary>
		/// Address property
		/// </summary>		
		public virtual AddressEntity Address
		{
			get { return _address; }
			set { _address = value; }
		}
			
		/// <summary>
		/// EncryptedSsn property
		/// </summary>		
		public virtual string EncryptedSsn
		{
			get { return _encryptedSsn; }
			set	
			{
				if (value != null && value.Length > 100)
				{
					throw new ArgumentOutOfRangeException("Invalid value for EncryptedSsn", value, value.ToString());
				}
				_encryptedSsn = value;
			}
		}
			
		/// <summary>
		/// Ssn4 property
		/// </summary>		
		public virtual string Ssn4
		{
			get { return _ssn4; }
			set	
			{
				if (value != null && value.Length > 4)
				{
					throw new ArgumentOutOfRangeException("Invalid value for Ssn4", value, value.ToString());
				}
				_ssn4 = value;
			}
		}
			
		/// <summary>
		/// RaceId property
		/// </summary>		
		public virtual long? RaceId
		{
			get { return _raceId; }
			set { _raceId = value; }
		}
			
		#endregion
	}
}
