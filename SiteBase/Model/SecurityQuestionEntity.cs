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
	public class SecurityQuestionEntity : BaseEntity
	{
		#region Private Members
		private string _text;
		private int _displayOrder;
		#endregion
		
		#region Properties Names
			
		public const string TextProperty = "Text";
			
		#endregion
		
		#region String Length Constants
			
		public const int TextMaxLength = 255;
			
		#endregion

		#region Constructor
		/// <summary>
		/// default constructor
		/// </summary>
		public SecurityQuestionEntity()
		{
			_text = null;
			_displayOrder = 0;
		}
		#endregion
		
		#region Public Properties
			
		/// <summary>
		/// Text property
		/// </summary>		
		public virtual string Text
		{
			get { return _text; }
			set	
			{
				if (value != null && value.Length > 255)
				{
					throw new ArgumentOutOfRangeException("Invalid value for Text", value, value.ToString());
				}
				_text = value;
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
			
		#endregion
	}
}
