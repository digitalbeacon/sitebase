// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using DigitalBeacon.Data.NHibernate;
using DigitalBeacon.SiteBase.Model;

namespace DigitalBeacon.SiteBase.Data.NHibernate
{
	/// <summary>
	/// Data access object for module definitions
	/// </summary>
	public class ModuleDefinitionDao : NameDao<ModuleDefinitionEntity>, IModuleDefinitionDao
	{
		#region IModuleDefinitionDao Members

		public IList<ModuleDefinitionEntity> FetchListToDisplay()
		{
			return HibernateTemplate.ExecuteFind<ModuleDefinitionEntity>(
				delegate(ISession session)
				{
					return session.CreateCriteria(typeof(ModuleDefinitionEntity))
						.Add(Expression.Gt(ModuleDefinitionEntity.DisplayOrderProperty, 0))
						.AddOrder(Order.Asc(ModuleDefinitionEntity.DisplayOrderProperty))
						.List<ModuleDefinitionEntity>();
				});
		}

		#endregion
	}
}