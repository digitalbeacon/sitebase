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
	/// Data access object for module settings
	/// </summary>
	public class ModuleSettingDao : BaseDao<ModuleSettingEntity>, IModuleSettingDao
	{
		private const string FetchBySettingKeyHql =
			"select x from ModuleSettingEntity as x, ModuleSettingDefinitionEntity as y where x.ModuleSettingDefinition = y.Id and x.ModuleId = ? and y.Key = ?";
		private const string FetchGlobalBySettingKeyHql =
			"select x from ModuleSettingEntity as x, ModuleSettingDefinitionEntity as y where x.ModuleSettingDefinition = y.Id and x.ModuleId is null and y.Key = ?";

		#region IModuleSettingDao Members

		public ModuleSettingEntity Fetch(long? moduleId, ModuleSettingDefinition moduleSettingDefinition)
		{
			return HibernateTemplate.Execute<ModuleSettingEntity>(
				delegate(ISession session)
					{
						ICriteria c = session.CreateCriteria(typeof(ModuleSettingEntity))
							.Add(Expression.Eq(ModuleSettingEntity.ModuleSettingDefinitionProperty, moduleSettingDefinition));
						if (moduleId == null)
						{
							c.Add(Expression.IsNull(ModuleSettingEntity.ModuleIdProperty));
						}
						else
						{
							c.Add(Expression.Eq(ModuleSettingEntity.ModuleIdProperty, moduleId));
						}
						return c.UniqueResult<ModuleSettingEntity>();
					});
		}

		public ModuleSettingEntity Fetch(long? moduleId, string settingDefKey)
		{
			return HibernateTemplate.Execute<ModuleSettingEntity>(
				delegate(ISession session)
				{
					if (moduleId == null)
					{
						IQuery query = session.CreateQuery(FetchGlobalBySettingKeyHql);
						query.SetString(0, settingDefKey);
						return query.UniqueResult<ModuleSettingEntity>();
					}
					else
					{
						IQuery query = session.CreateQuery(FetchBySettingKeyHql);
						query.SetInt64(0, moduleId.Value);
						query.SetString(1, settingDefKey);
						return query.UniqueResult<ModuleSettingEntity>();
					}
				});
		}

		#endregion

	}
}