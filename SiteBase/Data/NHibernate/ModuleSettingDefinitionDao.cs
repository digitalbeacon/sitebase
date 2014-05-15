// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Collections.Generic;
using NHibernate;
using DigitalBeacon.Data.NHibernate;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.Model;
using NHibernate.Criterion;

namespace DigitalBeacon.SiteBase.Data.NHibernate
{
	/// <summary>
	/// Data access object for module setting definitions
	/// </summary>
	public class ModuleSettingDefinitionDao : BaseDao<ModuleSettingDefinitionEntity>, IModuleSettingDefinitionDao
	{
		#region IModuleSettingDefinitionDao Members

		public IList<ModuleSettingDefinitionEntity> FetchListToDisplayByModuleDefinition(ModuleDefinition moduleDefinition)
		{
			return HibernateTemplate.ExecuteFind<ModuleSettingDefinitionEntity>(
				delegate(ISession session)
				{
					return session.CreateCriteria(typeof(ModuleSettingDefinitionEntity))
						.Add(Expression.Eq(ModuleSettingDefinitionEntity.ModuleDefinitionProperty, moduleDefinition))
						.Add(Expression.Eq(ModuleSettingDefinitionEntity.GlobalProperty, false))
						.Add(Expression.Gt(ModuleSettingDefinitionEntity.DisplayOrderProperty, 0))
						.AddOrder(Order.Asc(ModuleSettingDefinitionEntity.DisplayOrderProperty))
						.AddOrder(Order.Asc(ModuleSettingDefinitionEntity.NameProperty))
						.List<ModuleSettingDefinitionEntity>();
				});
		}

		public IList<ModuleSettingDefinitionEntity> FetchGlobalListToDisplayByModuleDefinition(ModuleDefinition moduleDefinition)
		{
			return HibernateTemplate.ExecuteFind<ModuleSettingDefinitionEntity>(
				delegate(ISession session)
				{
					return session.CreateCriteria(typeof(ModuleSettingDefinitionEntity))
						.Add(Expression.Eq(ModuleSettingDefinitionEntity.ModuleDefinitionProperty, moduleDefinition))
						.Add(Expression.Eq(ModuleSettingDefinitionEntity.GlobalProperty, true))
						.Add(Expression.Gt(ModuleSettingDefinitionEntity.DisplayOrderProperty, 0))
						.AddOrder(Order.Asc(ModuleSettingDefinitionEntity.DisplayOrderProperty))
						.AddOrder(Order.Asc(ModuleSettingDefinitionEntity.NameProperty))
						.List<ModuleSettingDefinitionEntity>();
				});
		}

		public ModuleSettingDefinitionEntity Fetch(ModuleDefinition moduleDefinition, string settingKey)
		{
			return HibernateTemplate.Execute<ModuleSettingDefinitionEntity>(
				delegate(ISession session)
				{
					return session.CreateCriteria(typeof(ModuleSettingDefinitionEntity))
						.Add(Expression.Eq(ModuleSettingDefinitionEntity.ModuleDefinitionProperty, moduleDefinition))
						.Add(Expression.Eq(ModuleSettingDefinitionEntity.KeyProperty, settingKey))
						.UniqueResult<ModuleSettingDefinitionEntity>();

				});
		}

		#endregion
	}
}