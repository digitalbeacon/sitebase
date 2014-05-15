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
	[Serializable]
	public class PermissionEntity : GeneratedPermissionEntity
	{
		#region Public Properties
			
		/// <summary>
		/// Access property
		/// </summary>		
		public virtual bool Access
		{
			get { return (Mask & (int)Permission.Access) > 0 || (Mask & (int)Permission.Admin) > 0; }
		}

		/// <summary>
		/// Create property
		/// </summary>		
		public virtual bool Create
		{
			get { return (Mask & (int)Permission.Create) > 0 || (Mask & (int)Permission.Admin) > 0; }
		}

		/// <summary>
		/// Update property
		/// </summary>		
		public virtual bool Update
		{
			get { return (Mask & (int)Permission.Update) > 0 || (Mask & (int)Permission.Admin) > 0; }
		}

		/// <summary>
		/// Delete property
		/// </summary>		
		public virtual bool Delete
		{
			get { return (Mask & (int)Permission.Delete) > 0 || (Mask & (int)Permission.Admin) > 0; }
		}

		/// <summary>
		/// Admin property
		/// </summary>		
		public virtual bool Admin
		{
			get { return (Mask & (int)Permission.Admin) > 0; }
		}
			
		#endregion
	}
}
