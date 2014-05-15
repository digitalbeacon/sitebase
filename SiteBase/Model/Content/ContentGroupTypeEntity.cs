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

namespace DigitalBeacon.SiteBase.Model.Content
{
	/// <summary>
	///	Generated with MyGeneration using the Entity template.
	/// </summary>
	[Serializable]
	public class ContentGroupTypeEntity : BaseEntity, INamedEntity
	{
		#region Private Members
		private string _name;
		private string _cssClass;
		private string _dateFormat;
		private int? _pageSize;
		#endregion
		
		#region Properties Names
			
		public const string CssClassProperty = "CssClass";
		public const string DateFormatProperty = "DateFormat";
		public const string PageSizeProperty = "PageSize";
			
		#endregion
		
		#region String Length Constants
			
		public const int NameMaxLength = 100;
		public const int CssClassMaxLength = 100;
		public const int DateFormatMaxLength = 50;
			
		#endregion

		#region Constructor
		/// <summary>
		/// default constructor
		/// </summary>
		public ContentGroupTypeEntity()
		{
			_name = null;
			_cssClass = null;
			_dateFormat = null;
			_pageSize = null;
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
		/// CssClass property
		/// </summary>		
		public virtual string CssClass
		{
			get { return _cssClass; }
			set	
			{
				if (value != null && value.Length > 100)
				{
					throw new ArgumentOutOfRangeException("Invalid value for CssClass", value, value.ToString());
				}
				_cssClass = value;
			}
		}
			
		/// <summary>
		/// DateFormat property
		/// </summary>		
		public virtual string DateFormat
		{
			get { return _dateFormat; }
			set	
			{
				if (value != null && value.Length > 50)
				{
					throw new ArgumentOutOfRangeException("Invalid value for DateFormat", value, value.ToString());
				}
				_dateFormat = value;
			}
		}
			
		/// <summary>
		/// PageSize property
		/// </summary>		
		public virtual int? PageSize
		{
			get { return _pageSize; }
			set { _pageSize = value; }
		}
			
		#endregion
	}
}
