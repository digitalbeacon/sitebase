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
	public class GeneratedFileArchiveEntity : BaseEntity, IArchivedEntity
	{
		#region Private Members
		private long _refId;
		private DateTime _created;
		private DateTime _archived;
		private string _filename;
		private string _name;
		private string _contentType;
		private bool _dataCompressed;
		private FileDataEntity _fileData;
		private int _cachedSize;
		private DateTime _lastModified;
		#endregion
		
		#region Properties Names
			
		public const string RefIdProperty = "RefId";
		public const string CreatedProperty = "Created";
		public const string ArchivedProperty = "Archived";
		public const string FilenameProperty = "Filename";
		public const string ContentTypeProperty = "ContentType";
		public const string DataCompressedProperty = "DataCompressed";
		public const string FileDataProperty = "FileData";
		public const string CachedSizeProperty = "CachedSize";
		public const string LastModifiedProperty = "LastModified";
			
		#endregion
		
		#region String Length Constants
			
		public const int FilenameMaxLength = 250;
		public const int NameMaxLength = 100;
		public const int ContentTypeMaxLength = 100;
			
		#endregion

		#region Constructor
		/// <summary>
		/// default constructor
		/// </summary>
		public GeneratedFileArchiveEntity()
		{
			_refId = 0;
			_created = DateTime.Now;
			_archived = DateTime.MinValue;
			_filename = null;
			_name = null;
			_contentType = null;
			_dataCompressed = false; 
			_cachedSize = 0;
			_lastModified = DateTime.MinValue;
		}
		#endregion
		
		#region Public Properties
			
		/// <summary>
		/// RefId property
		/// </summary>		
		public virtual long RefId
		{
			get { return _refId; }
			set { _refId = value; }
		}
			
		/// <summary>
		/// Created property
		/// </summary>		
		public virtual DateTime Created
		{
			get { return _created; }
			set { _created = value; }
		}
			
		/// <summary>
		/// Archived property
		/// </summary>		
		public virtual DateTime Archived
		{
			get { return _archived; }
			set { _archived = value; }
		}
			
		/// <summary>
		/// Filename property
		/// </summary>		
		public virtual string Filename
		{
			get { return _filename; }
			set	
			{
				if (value != null && value.Length > 250)
				{
					throw new ArgumentOutOfRangeException("Invalid value for Filename", value, value.ToString());
				}
				_filename = value;
			}
		}
			
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
		/// ContentType property
		/// </summary>		
		public virtual string ContentType
		{
			get { return _contentType; }
			set	
			{
				if (value != null && value.Length > 100)
				{
					throw new ArgumentOutOfRangeException("Invalid value for ContentType", value, value.ToString());
				}
				_contentType = value;
			}
		}
			
		/// <summary>
		/// DataCompressed property
		/// </summary>		
		public virtual bool DataCompressed
		{
			get { return _dataCompressed; }
			set { _dataCompressed = value; }
		}
			
		/// <summary>
		/// FileData property
		/// </summary>		
		public virtual FileDataEntity FileData
		{
			get { return _fileData; }
			set { _fileData = value; }
		}
			
		/// <summary>
		/// CachedSize property
		/// </summary>		
		public virtual int CachedSize
		{
			get { return _cachedSize; }
			set { _cachedSize = value; }
		}
			
		/// <summary>
		/// LastModified property
		/// </summary>		
		public virtual DateTime LastModified
		{
			get { return _lastModified; }
			set { _lastModified = value; }
		}
			
		#endregion
	}
}
