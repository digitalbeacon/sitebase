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
	/// Data access object for modules
	/// </summary>
	public class ModuleDao : BaseDao<ModuleEntity>, IModuleDao
	{
		#region IModuleDao Members

		public IList<ModuleEntity> FetchByModuleDefinition(long associationId, ModuleDefinition moduleDefinition)
		{
			return HibernateTemplate.ExecuteFind<ModuleEntity>(
				delegate(ISession session)
				{
					return session.CreateCriteria(typeof(ModuleEntity))
						.Add(Expression.Eq(ModuleEntity.AssociationIdProperty, associationId))
						.Add(Expression.Eq(ModuleEntity.ModuleDefinitionProperty, moduleDefinition))
						.List<ModuleEntity>();
				});
		}

		public ModuleEntity FetchDefaultInstance(long associationId, ModuleDefinition moduleDefinition)
		{
			return HibernateTemplate.Execute<ModuleEntity>(
				delegate(ISession session)
				{
					return session.CreateCriteria(typeof(ModuleEntity))
						.Add(Expression.Eq(ModuleEntity.AssociationIdProperty, associationId))
						.Add(Expression.Eq(ModuleEntity.ModuleDefinitionProperty, moduleDefinition))
						.Add(Expression.Eq(ModuleEntity.DefaultInstanceProperty, true))
						.UniqueResult<ModuleEntity>();
				});
		}

		public ModuleEntity FetchInstance(long associationId, string name)
		{
			return HibernateTemplate.Execute<ModuleEntity>(
				delegate(ISession session)
				{
					return session.CreateCriteria(typeof(ModuleEntity))
						.Add(Expression.Eq(ModuleEntity.AssociationIdProperty, associationId))
						.Add(Expression.Eq(ModuleEntity.NameProperty, name))
						.UniqueResult<ModuleEntity>();
				});
		}

		public ModuleEntity FetchInstance(long associationId, ModuleDefinition moduleDefinition, string name)
		{
			return HibernateTemplate.Execute<ModuleEntity>(
				delegate(ISession session)
				{
					return session.CreateCriteria(typeof(ModuleEntity))
						.Add(Expression.Eq(ModuleEntity.AssociationIdProperty, associationId))
						.Add(Expression.Eq(ModuleEntity.ModuleDefinitionProperty, moduleDefinition))
						.Add(Expression.Eq(ModuleEntity.NameProperty, name))
						.UniqueResult<ModuleEntity>();
				});
		}

		#endregion
	}
}