// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;

using DigitalBeacon.Util;

namespace DigitalBeacon.Model
{
	/// <summary>
	///	This is the base class for all business objects
	/// </summary>
	[Serializable]
	public abstract class BaseEntity : IBaseEntity
	{
		#region Public Members

		public const string IdProperty = "Id";
		public const string VersionProperty = "ModificationCounter";
		public const string NameProperty = "Name";
		public const string CodeProperty = "Code";
		public const string DisplayOrderProperty = "DisplayOrder";
		public const string CommentProperty = "Comment";

		#endregion

		#region Private Members
		private long _id;
		private long _modificationCounter;
		private bool _lazyCollectionsInitialized;
		#endregion

		#region Constuctor

		/// <summary>
		/// default constructor
		/// </summary>
		protected BaseEntity()
		{
			_id = 0;
			_modificationCounter = -1;
			_lazyCollectionsInitialized = false;
		}

		#endregion

		#region Equals, HashCode and ToJson overrides

		/// <summary>
		/// local implementation of Equals based on unique value members
		/// </summary>
		public override bool Equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			if (obj == null || obj.GetType() != GetType())
			{
				return false;
			}
			var castObj = obj as BaseEntity;
			return (castObj != null && _id == castObj.Id);
		}

		/// <summary>
		/// local implementation of GetHashCode based on unique value members
		/// </summary>
		public override int GetHashCode()
		{
			return 1539 * _id.GetHashCode();
		}

		/// <summary>
		/// ToJson implementation
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return this.ToJson(false, false);
		}

		#endregion

		#region IBaseEntity Members

		public virtual long Id
		{
			get { return _id; }
			set { _id = value; }
		}

		public virtual long ModificationCounter
		{
			get { return _modificationCounter; }
			set { _modificationCounter = value; }
		}

		public virtual bool AreLazyCollectionsInitialized
		{
			get { return _lazyCollectionsInitialized; }
			set { _lazyCollectionsInitialized = value; }
		}

		public virtual bool IsNew
		{
			get { return Id <= 0; }
		}

		#endregion
	}
}
