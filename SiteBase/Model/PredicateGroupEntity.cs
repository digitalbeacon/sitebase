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
	public class PredicateGroupEntity : BaseEntity
	{
		#region Private Members
		private EntityType _type;
		private long? _associationId;
		private long? _userId;
		private string _name;
		private int _displayOrder;
		private IList<PredicateEntity> _predicates;
		#endregion
		
		#region Properties Names
			
		public const string TypeProperty = "Type";
		public const string AssociationIdProperty = "AssociationId";
		public const string UserIdProperty = "UserId";
		public const string PredicatesProperty = "Predicates";
			
		#endregion
		
		#region String Length Constants
			
		public const int NameMaxLength = 50;
			
		#endregion

		#region Constructor
		/// <summary>
		/// default constructor
		/// </summary>
		public PredicateGroupEntity()
		{
			_associationId = null;
			_userId = null;
			_name = null;
			_displayOrder = 0;
		}
		#endregion
		
		#region Public Properties
			
		/// <summary>
		/// Type property
		/// </summary>		
		public virtual EntityType Type
		{
			get { return _type; }
			set { _type = value; }
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
		/// UserId property
		/// </summary>		
		public virtual long? UserId
		{
			get { return _userId; }
			set { _userId = value; }
		}
			
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
			
		/// <summary>
		/// DisplayOrder property
		/// </summary>		
		public virtual int DisplayOrder
		{
			get { return _displayOrder; }
			set { _displayOrder = value; }
		}
			
		/// <summary>
		/// Predicates collection
		/// </summary>		
		public virtual IList<PredicateEntity> Predicates
		{
			get { return _predicates; }
			set { _predicates = value; }
		}
		
		#endregion
	}
}
