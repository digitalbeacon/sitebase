// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Linq;
using System.Collections.Generic;
using NHibernate.Criterion;
using NHibernate.Linq;
using DigitalBeacon.Data.NHibernate;
using DigitalBeacon.SiteBase.Model;

namespace DigitalBeacon.SiteBase.Data.NHibernate
{
	public class ResourceDao : BaseDao<ResourceEntity>, IResourceDao
	{
		#region IResourceDao Members

		public IList<string> FetchUniqueTypes()
		{
			return HibernateTemplate.ExecuteFind(session => 
				(from x in session.Query<ResourceEntity>() 
				 orderby x.Type 
				 select x.Type).Distinct().ToList());
		}

		#endregion
	}
}