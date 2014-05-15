// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using DigitalBeacon.Data.NHibernate;
using DigitalBeacon.SiteBase.Model;

namespace DigitalBeacon.SiteBase.Data.NHibernate
{
	/// <summary>
	/// Data access object for user preferences
	/// </summary>
	public class PreferenceDao : BaseDao<PreferenceEntity>, IPreferenceDao
	{
		#region IPreferenceDao Members

		public PreferenceEntity Fetch(long associationId, string key, long? userId)
		{
			return HibernateTemplate.Execute<PreferenceEntity>(
				delegate(ISession session)
				{
					var query = from x in session.Query<PreferenceEntity>()
						   where x.AssociationId == associationId
							  && x.Key == key
						   select x;
					if (userId.HasValue)
					{
						return query.Where(x => x.UserId == userId.Value || x.UserId == null)
							.OrderByDescending(x => x.UserId)
							.FirstOrDefault();
					}
					else
					{
						return query.Where(x => x.UserId == null).SingleOrDefault();
					}
				});
		}

		#endregion
	}
}