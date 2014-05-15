// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using DigitalBeacon.Util;
using DigitalBeacon.Web.Formatters;

namespace DigitalBeacon.SiteBase.Model
{
	[Serializable]
	public class PersonEntity : GeneratedPersonEntity
	{
		public const string DisplayNameProperty = "DisplayName";
		public const string DisplayNameFormatKey = "Common.Person.DisplayName.Format";
		public const string LastFourSsnProperty = "LastFourSsn";
		public const int SsnMaxLength = 11;

		private const string EncryptionKey = "7084A5A6-0C83-440a-8B6A-62B60EB3AE1F";

		private static readonly SsnFormatter SsnFormatter = new SsnFormatter();

		private string _ssn;

		/// <summary>
		/// DisplayName property
		/// </summary>
		public virtual string DisplayName
		{
			get
			{
				return String.Format("{0}{1} {2}{3}", !String.IsNullOrEmpty(Title) ? Title + " " : String.Empty, FirstName, LastName, !String.IsNullOrEmpty(Suffix) ? ", " + Suffix : String.Empty);
			}
		}

		/// <summary>
		/// Ssn property
		/// </summary>
		public virtual string Ssn
		{
			get
			{
				if (_ssn.IsNullOrBlank() && EncryptedSsn.HasText())
				{
					_ssn = new EncryptionUtil().DecryptData(EncryptionKey, EncryptedSsn);
				}
				return _ssn;
			}

			set
			{
				_ssn = value.HasText() ? SsnFormatter.Parse(value) : null;
				EncryptedSsn = _ssn.HasText() ? new EncryptionUtil().EncryptData(EncryptionKey, _ssn) : null;
			}
		}

		/// <summary>
		/// LastFourSsn property
		/// </summary>		
		public virtual string LastFourSsn
		{
			get
			{
				if (Ssn.HasText() && Ssn.Length == 9)
				{
					return Ssn.Substring(5);
				}
				return String.Empty;
			}
		}

		#region Suffixes

		private static readonly Dictionary<string, string> Suffixes = new Dictionary<string, string>
		{
			{ " jr", "Jr." },
			{ " Jr", "Jr." },
			{ " JR", "Jr." },
			{ ",jr", "Jr." },
			{ ",Jr", "Jr." },
			{ ",JR", "Jr." },
			{ " sr", "Sr." },
			{ " Sr", "Sr." },
			{ " SR", "Sr." },
			{ ",sr", "Sr." },
			{ ",Sr", "Sr." },
			{ ",SR", "Sr." },
			{ "III", "III" },
			{ "II", "II" },
			{ "IV", "IV" },
		};

		#endregion

		/// <summary>
		/// Populates the FirstName, LastName, MiddleName and Suffix properties from a combined name string.
		/// Currently supported format is LastName, FirstName MiddleInitial
		/// </summary>
		/// <param name="name">The combined name.</param>
		/// <param name="defaultLastName">Default last name if name only has one part.</param>
		/// <returns>true if name is parsed without warnings; false otherwise</returns>
		public virtual bool ParseName(string name, string defaultLastName = null)
		{
			bool retVal = true;
			if (name.IsNullOrBlank())
			{
				retVal = false;
			}
			else
			{
				name = name.Replace(".", ",");
				// strip suffixes
				foreach (var s in Suffixes.Keys)
				{
					if (name.Contains(s))
					{
						Suffix = Suffixes[s];
						name = name.Replace(s, String.Empty);
					}
				}
				// split string
				var parts = name.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
				if (parts.Length == 2)
				{
					LastName = parts[0];
					FirstName = parts[1];
				}
				else if (parts.Length == 1)
				{
					if (defaultLastName.HasText())
					{
						FirstName = parts[0];
						LastName = defaultLastName;
					}
					else
					{
						retVal = false;
					}
				}
				else if (parts.Length >= 3)
				{
					LastName = parts[0];
					FirstName = parts[1];
					if (parts.Length == 3 && parts[1].Length == 1 && parts[2].Length > 1)
					{
						FirstName = parts[2];
						MiddleName = parts[1];
					}
					else if (parts[2].Length == 1)
					{
						MiddleName = parts[2];
					}
					else
					{
						for (int i = 2; i < parts.Length; i++)
						{
							FirstName += " " + parts[i];
						}
					}
				}
				else
				{
					retVal = false;
				}
			}
			return retVal;
		}
	}
}
