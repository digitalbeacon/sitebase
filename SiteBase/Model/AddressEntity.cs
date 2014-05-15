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
using DigitalBeacon.Web.Formatters;
using DigitalBeacon.Util;

namespace DigitalBeacon.SiteBase.Model
{
	[Serializable]
	public class AddressEntity : GeneratedAddressEntity
	{
		private static readonly PhoneNumberFormatter PhoneFormatter = new PhoneNumberFormatter();
		
		private bool _parsePhoneNumbers = true;

		public virtual bool ParsePhoneNumbers
		{
			get { return _parsePhoneNumbers; }
			set { _parsePhoneNumbers = value; }
		}

		public virtual string DefaultPhoneText
		{
			get
			{
				string retVal = String.Empty;
				if (DefaultPhone != null)
				{
					if (DefaultPhone.Value == PhoneType.Home && !String.IsNullOrEmpty(HomePhone))
					{
						retVal = HomePhone;
					}
					else if (DefaultPhone.Value == PhoneType.Work && !String.IsNullOrEmpty(WorkPhone))
					{
						retVal = WorkPhone;
					}
					else if (DefaultPhone.Value == PhoneType.Mobile && !String.IsNullOrEmpty(MobilePhone))
					{
						retVal = MobilePhone;
					}
				}
				if (String.IsNullOrEmpty(retVal))
				{
					if (!String.IsNullOrEmpty(HomePhone))
					{
						retVal = HomePhone;
					}
					else if (!String.IsNullOrEmpty(MobilePhone))
					{
						retVal = MobilePhone;
					}
					else if (!String.IsNullOrEmpty(WorkPhone))
					{
						retVal = WorkPhone;
					}
				}
				return retVal;
			}
		}

		public virtual string HomePhoneText
		{
			get { return PhoneFormatter.Format(HomePhone); }
		}

		public virtual string WorkPhoneText
		{
			get { return PhoneFormatter.Format(WorkPhone); }
		}

		public virtual string MobilePhoneText
		{
			get { return PhoneFormatter.Format(MobilePhone); }
		}

		public virtual string FaxText
		{
			get { return PhoneFormatter.Format(Fax); }
		}

		public override string HomePhone
		{
			get { return base.HomePhone; }
			set { base.HomePhone = _parsePhoneNumbers ? (value.HasText() ? PhoneFormatter.Parse(value) : null) : value; }
		}

		public override string WorkPhone
		{
			get { return base.WorkPhone; }
			set { base.WorkPhone = _parsePhoneNumbers ? (value.HasText() ? PhoneFormatter.Parse(value) : null) : value; }
		}

		public override string MobilePhone
		{
			get { return base.MobilePhone; }
			set { base.MobilePhone = _parsePhoneNumbers ? (value.HasText() ? PhoneFormatter.Parse(value) : null) : value; }
		}

		public override string Fax
		{
			get { return base.Fax; }
			set { base.Fax = _parsePhoneNumbers ? (value.HasText() ? PhoneFormatter.Parse(value) : null) : value; }
		}

	}
}
