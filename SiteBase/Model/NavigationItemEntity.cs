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
	public class NavigationItemEntity : BaseEntity
	{
		#region Private Members
		private bool _enabled;
		private long? _associationId;
		private NavigationEntity _navigation;
		private NavigationItemEntity _parent;
		private int _displayOrder;
		private string _text;
		private int? _moduleId;
		private string _url;
		private string _imageUrl;
		#endregion
		
		#region Properties Names
			
		public const string EnabledProperty = "Enabled";
		public const string AssociationIdProperty = "AssociationId";
		public const string NavigationProperty = "Navigation";
		public const string ParentProperty = "Parent";
		public const string TextProperty = "Text";
		public const string ModuleIdProperty = "ModuleId";
		public const string UrlProperty = "Url";
		public const string ImageUrlProperty = "ImageUrl";
			
		#endregion
		
		#region String Length Constants
			
		public const int TextMaxLength = 100;
		public const int UrlMaxLength = 100;
		public const int ImageUrlMaxLength = 100;
			
		#endregion

		#region Constructor
		/// <summary>
		/// default constructor
		/// </summary>
		public NavigationItemEntity()
		{
			_enabled = false; 
			_associationId = null;
			_parent = null;
			_displayOrder = 0;
			_text = null;
			_moduleId = null;
			_url = null;
			_imageUrl = null;
		}
		#endregion
		
		#region Public Properties
			
		/// <summary>
		/// Enabled property
		/// </summary>		
		public virtual bool Enabled
		{
			get { return _enabled; }
			set { _enabled = value; }
		}
			
		/// <summary>
		/// AssociationId property
		/// </summary>		
		public virtual long? AssociationId
		{
			get { return _associationId; }
			set { _associationId = value; }
		}
			
		/// <summary>
		/// Navigation property
		/// </summary>		
		public virtual NavigationEntity Navigation
		{
			get { return _navigation; }
			set { _navigation = value; }
		}
			
		/// <summary>
		/// Parent property
		/// </summary>		
		public virtual NavigationItemEntity Parent
		{
			get { return _parent; }
			set { _parent = value; }
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
		/// Text property
		/// </summary>		
		public virtual string Text
		{
			get { return _text; }
			set	
			{
				if (value != null && value.Length > 100)
				{
					throw new ArgumentOutOfRangeException("Invalid value for Text", value, value.ToString());
				}
				_text = value;
			}
		}
			
		/// <summary>
		/// ModuleId property
		/// </summary>		
		public virtual int? ModuleId
		{
			get { return _moduleId; }
			set { _moduleId = value; }
		}
			
		/// <summary>
		/// Url property
		/// </summary>		
		public virtual string Url
		{
			get { return _url; }
			set	
			{
				if (value != null && value.Length > 100)
				{
					throw new ArgumentOutOfRangeException("Invalid value for Url", value, value.ToString());
				}
				_url = value;
			}
		}
			
		/// <summary>
		/// ImageUrl property
		/// </summary>		
		public virtual string ImageUrl
		{
			get { return _imageUrl; }
			set	
			{
				if (value != null && value.Length > 100)
				{
					throw new ArgumentOutOfRangeException("Invalid value for ImageUrl", value, value.ToString());
				}
				_imageUrl = value;
			}
		}
			
		#endregion
	}
}
