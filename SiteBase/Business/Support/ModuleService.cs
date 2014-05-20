// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DigitalBeacon.Business;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Data;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.Util;

namespace DigitalBeacon.SiteBase.Business.Support
{
	/// <summary>
	/// Implementation of IModuleService
	/// </summary>
	public class ModuleService : BaseService, IModuleService
	{
		#region Private Members

		private static readonly IModuleDao ModuleDao = ServiceFactory.Instance.GetService<IModuleDao>();
		private static readonly IModuleSettingDao ModuleSettingDao = ServiceFactory.Instance.GetService<IModuleSettingDao>();
		private static readonly IModuleDefinitionDao ModuleDefinitionDao = ServiceFactory.Instance.GetService<IModuleDefinitionDao>();
		private static readonly IModuleSettingDefinitionDao ModuleSettingDefinitionDao = ServiceFactory.Instance.GetService<IModuleSettingDefinitionDao>();
		private static readonly IModuleSettingSubstitutionDao MsgTemplateSubstDao = ServiceFactory.Instance.GetService<IModuleSettingSubstitutionDao>();
		private static readonly INavigationItemDao NavigationItemDao = ServiceFactory.Instance.GetService<INavigationItemDao>();
		private static readonly IPermissionService PermissionService = ServiceFactory.Instance.GetService<IPermissionService>();
		private static readonly ILocalizationService LocalizationService = ServiceFactory.Instance.GetService<ILocalizationService>();

		#endregion

		#region IModuleService Members

		public ModuleEntity RegisterModule(long associationId, ModuleDefinition moduleDefinition, string url)
		{
			return RegisterModule(associationId, moduleDefinition, url, String.Empty);
		}

		public ModuleEntity RegisterModule(long associationId, ModuleDefinition moduleDefinition, string url, string name)
		{
			var module = new ModuleEntity();
			if (String.IsNullOrEmpty(name))
			{
				name = Guid.NewGuid().ToString();
			}
			module.AssociationId = associationId;
			module.ModuleDefinition = moduleDefinition;
			module.Name = name;
			module.Url = url;
			module.DefaultInstance = ModuleDao.FetchDefaultInstance(associationId, moduleDefinition) == null;
			return ModuleDao.Save(module);
		}

		public ModuleEntity GetDefaultInstance(long associationId, ModuleDefinition moduleDefinition)
		{
			return ModuleDao.FetchDefaultInstance(associationId, moduleDefinition);
		}

		public ModuleEntity GetModuleInstance(long moduleInstanceId)
		{
			return ModuleDao.Fetch(moduleInstanceId);
		}

		public ModuleEntity GetModuleInstance(long associationId, string name)
		{
			return ModuleDao.FetchInstance(associationId, name);
		}

		public ModuleEntity GetModuleInstance(long associationId, ModuleDefinition moduleDefinition, string name)
		{
			return ModuleDao.FetchInstance(associationId, moduleDefinition, name);
		}

		public ModuleEntity SaveModule(ModuleEntity module)
		{
			ValidateModule(module);
			return SaveWithAudit(module);
		}

		public void DeleteModule(long id)
		{
			foreach (var s in ModuleSettingDao.FetchList(ModuleSettingEntity.ModuleIdProperty, id))
			{
				DeleteWithAudit(s);
			}
			DeleteWithAudit<ModuleEntity>(id);
		}

		public IList<ModuleEntity> GetModulesByDefinition(long associationId, ModuleDefinition moduleDefinition)
		{
			return ModuleDao.FetchByModuleDefinition(associationId, moduleDefinition);
		}

		public ModuleDefinitionEntity GetModuleDefinition(long id)
		{
			return ModuleDefinitionDao.Fetch(id);
		}

		public IList<ModuleDefinitionEntity> GetModuleDefinitionsToDisplay()
		{
			return ModuleDefinitionDao.FetchListToDisplay();
		}

		public IList<ModuleSettingDefinitionEntity> GetModuleSettingDefinitionsToDisplay(ModuleDefinition moduleDefinition)
		{
			return ModuleSettingDefinitionDao.FetchListToDisplayByModuleDefinition(moduleDefinition);
		}

		public IList<ModuleSettingDefinitionEntity> GetGlobalModuleSettingDefinitionsToDisplay(ModuleDefinition moduleDefinition)
		{
			return ModuleSettingDefinitionDao.FetchGlobalListToDisplayByModuleDefinition(moduleDefinition);
		}

		public ModuleSettingEntity GetModuleSetting(long moduleId, ModuleSettingDefinition moduleSettingDefinition)
		{
			return GetModuleSetting((long?)moduleId, moduleSettingDefinition);
		}

		private static ModuleSettingEntity GetModuleSetting(long? moduleId, ModuleSettingDefinition moduleSettingDefinition)
		{
			var s = ModuleSettingDao.Fetch(moduleId, moduleSettingDefinition);
			if (s == null)
			{
				var def = ModuleSettingDefinitionDao.Fetch((long)moduleSettingDefinition);
				if (def.Global && moduleId != null)
				{
					moduleId = null;
					s = ModuleSettingDao.Fetch(null, (ModuleSettingDefinition)def.Id);
				}
				if (s == null && def.ModuleSettingType != ModuleSettingType.Custom)
				{
					ModuleEntity m = null;
					if (moduleId != null)
					{
						m = ModuleDao.Fetch(moduleId.Value);
					}
					if (moduleId == null || (m != null && m.ModuleDefinition == def.ModuleDefinition))
					{
						s = new ModuleSettingEntity
								{
									ModuleId = moduleId, 
									ModuleSettingDefinition = (ModuleSettingDefinition)def.Id, 
									Value = def.DefaultValue, 
									Subject = def.DefaultSubject
								};
					}
				}
			}
			return s;
		}

		public ModuleSettingEntity GetModuleSetting(long moduleId, string settingDefKey)
		{
			var s = ModuleSettingDao.Fetch(moduleId, settingDefKey);
			if (s == null)
			{
				var m = ModuleDao.Fetch(moduleId);
				if (m != null)
				{
					var def = ModuleSettingDefinitionDao.Fetch(m.ModuleDefinition, settingDefKey);
					if (def == null)
					{
						return null;
					}
					if (def.Global)
					{
						s = ModuleSettingDao.Fetch(null, (ModuleSettingDefinition)def.Id);
					}
					if (s == null && def.ModuleSettingType != ModuleSettingType.Custom)
					{
						s = new ModuleSettingEntity
								{
									ModuleId = moduleId, 
									ModuleSettingDefinition = (ModuleSettingDefinition)def.Id, 
									Value = def.DefaultValue, 
									Subject = def.DefaultSubject
								};
					}
				}
			}
			return s;
		}

		public ModuleSettingEntity GetGlobalModuleSetting(ModuleSettingDefinition moduleSettingDefinition)
		{
			var retVal = GetModuleSetting(null, moduleSettingDefinition);
			if (retVal == null)
			{
				throw new ServiceException("Could not find module setting with key [{0}].", moduleSettingDefinition);
			}
			return retVal;
		}

		public ModuleSettingEntity GetGlobalModuleSetting(ModuleSettingDefinition moduleSettingDefinition, Language? language)
		{
			var moduleSetting = GetGlobalModuleSetting(moduleSettingDefinition);
			if (language.HasValue)
			{
				var languageEntity = DataAdapter.Fetch<LanguageEntity>((long)language.Value);
				if (languageEntity.Code != ResourceManager.SystemCulture.Name)
				{
					moduleSetting = LocalizeModuleSetting(moduleSetting, languageEntity);
				}
			}
			return moduleSetting;
		}

		public ModuleSettingEntity GetGlobalModuleSetting(ModuleDefinition moduleDef, string settingDefKey)
		{
			var moduleSettingDef = ModuleSettingDefinitionDao.Fetch(moduleDef, settingDefKey);
			return moduleSettingDef != null ? GetGlobalModuleSetting((ModuleSettingDefinition)moduleSettingDef.Id) : null;
		}

		public ModuleSettingEntity GetGlobalSetting(string settingDefKey)
		{
			return GetGlobalModuleSetting(ModuleDefinition.Site, settingDefKey);
		}

		public ModuleSettingEntity SaveModuleSetting(ModuleSettingEntity moduleSetting)
		{
			var sd = ModuleSettingDefinitionDao.Fetch((long)moduleSetting.ModuleSettingDefinition);
			if (sd == null)
			{
				throw new ServiceException("Could not find module setting definition: " + moduleSetting.ModuleSettingDefinition);
			}
			if (sd.Global && moduleSetting.ModuleId != null)
			{
				throw new ServiceException("ModuleId should not be set for global module settings.");
			}
			if (moduleSetting.ModuleId != null)
			{
				var m = ModuleDao.Fetch(moduleSetting.ModuleId.Value);
				if (m == null)
				{
					throw new ServiceException("Could not find module with Id: " + moduleSetting.ModuleId);
				}
				if (m.ModuleDefinition != sd.ModuleDefinition)
				{
					throw new ServiceException("ModuleDefinition mismatch. ModuleDefinition for module = {0}. ModuleDefintion for setting definition = {1}.", m.ModuleDefinition, sd.ModuleDefinition);
				}
			}
			if (sd.ModuleSettingType == ModuleSettingType.Number
				|| sd.ModuleSettingType == ModuleSettingType.Integer
				|| sd.ModuleSettingType == ModuleSettingType.Currency
				|| sd.ModuleSettingType == ModuleSettingType.Date)
			{
				if (sd.MinValue != null && moduleSetting.ValueAsDouble < sd.MinValue.Value)
				{
					throw new ServiceException("ModuleSetting value must be greater than or equal to {0}.", sd.MinValue.Value);
				}
				if (sd.MaxValue != null && moduleSetting.ValueAsDouble > sd.MaxValue.Value)
				{
					throw new ServiceException("ModuleSetting value must be less than or equal to {0}.", sd.MaxValue.Value);
				}
			}
			return SaveWithAudit(moduleSetting);
		}

		public void SaveModuleSettings(IEnumerable<ModuleSettingEntity> moduleSettings)
		{
			foreach (var setting in moduleSettings)
			{
				SaveModuleSetting(setting);
			}
		}

		public ModuleSettingDefinitionEntity GetModuleSettingDefinition(ModuleSettingDefinition moduleSettingDefinition)
		{
			return ModuleSettingDefinitionDao.Fetch((long)moduleSettingDefinition);
		}

		public string GetModuleSettingValueWithSubstitutions(ModuleSettingEntity moduleSetting, IDictionary<SubstitutionDefinition, string> substitutions)
		{
			var substitutionsWithStringKeys = new Dictionary<string, string>();
			var undefinedKeys = ValidateSubstitutions(moduleSetting.ModuleSettingDefinition, substitutions, substitutionsWithStringKeys);
			if (undefinedKeys.Count > 0)
			{
				throw new ServiceException("One or more substitution keys were not satisfied for the message template: {0}.", TextUtil.ToJson(undefinedKeys));
			}
			return TextUtil.SubstituteReplacements(moduleSetting.Value, substitutionsWithStringKeys);
		}

		public bool ValidateSubstitutions(ModuleSettingDefinition moduleSettingDefinition, string message, string enclosingSubstitutionTag)
		{
			throw new NotImplementedException();
		}

		public void CreateResourceSet(long languageId)
		{
			var language = DataAdapter.Fetch<LanguageEntity>(languageId);
			var type = ResourceManager.GetTypeKey<ModuleSettingEntity>();
			var resources = LocalizationService.GetResources(CultureInfo.GetCultureInfo(language.Code), type);
			var search = new SearchInfo<ModuleSettingDefinitionEntity> { ApplyDefaultFilters = false, ApplyDefaultSorting = false };
			search.AddFilter(x => x.Localizable, true);
			var entities = DataAdapter.FetchList(search);
			foreach (var e in entities)
			{
				var resource = resources.Where(x => x.Key.EqualsIgnoreCase(e.Key)
					&& x.Property == ModuleSettingEntity.SubjectProperty).SingleOrDefault();
				if (resource == null)
				{
					LocalizationService.SaveResource(new ResourceEntity
					{
						Language = language,
						Type = type,
						Key = e.Key,
						Property = ModuleSettingEntity.SubjectProperty,
						Value = String.Empty
					});
				}
				resource = resources.Where(x => x.Key.EqualsIgnoreCase(e.Key)
					&& x.Property == ModuleSettingEntity.ValueProperty).SingleOrDefault();
				if (resource == null)
				{
					LocalizationService.SaveResource(new ResourceEntity
					{
						Language = language,
						Type = type,
						Key = e.Key,
						Property = ModuleSettingEntity.ValueProperty,
						Value = String.Empty
					});
				}
			}
		}

		public ModuleSettingEntity LocalizeModuleSetting(ModuleSettingEntity moduleSetting, LanguageEntity language)
		{
			moduleSetting.Guard("moduleSetting");
			language.Guard("language");
			if (ResourceManager.Instance.UseDatabaseResources)
			{
				DataAdapter.Evict<ModuleSettingEntity>(moduleSetting);
				var def = GetModuleSettingDefinition(moduleSetting.ModuleSettingDefinition);
				var culture = CultureInfo.GetCultureInfo(language.Code);
				//moduleSetting = LocalizationService.Localize(culture, moduleSetting, null);
				var resources = LocalizationService.GetResources(culture, ResourceManager.GetTypeKey<ModuleSettingEntity>(), def.Key, null);
				foreach (var resource in resources)
				{
					if (resource.Value.IsNullOrBlank())
					{
						continue;
					}
					if (resource.Property == ModuleSettingEntity.SubjectProperty)
					{
						moduleSetting.Subject = resource.Value;
					}
					else if (resource.Property == ModuleSettingEntity.ValueProperty)
					{
						moduleSetting.Value = resource.Value;
					}
				}
				//var resources = LocalizationService.GetResources(culture, type, def.Key);
				//if (resource != null && resource.Value.HasText())
				//{
				//	moduleSetting.Subject = resource.Value;
				//}
				//resource = LocalizationService.GetResource(culture, null, "{0}.Content".FormatWith(def.Key));
				//if (resource != null && resource.Value.HasText())
				//{
				//	moduleSetting.Value = resource.Value;
				//}
			}
			return moduleSetting;
		}

		public NavigationItemEntity GetNavigationItem(long id)
		{
			return NavigationItemDao.Fetch(id);
		}

		public IList<NavigationItemEntity> GetNavigationItems(SearchInfo<NavigationItemEntity> searchInfo)
		{
			return NavigationItemDao.FetchList(PrepareSearch(searchInfo));
		}

		public long GetNavigationItemCount(SearchInfo<NavigationItemEntity> searchInfo)
		{
			return NavigationItemDao.FetchCount(PrepareSearch(searchInfo));
		}

		public NavigationItemEntity SaveNavigationItem(NavigationItemEntity item)
		{
			if (item.IsNew)
			{
				return SaveWithAudit(item);
			}
			NavigationItemDao.Evict(item);
			var existing = GetNavigationItem(item.Id);
			if (item.Parent != null && item.Navigation.Id != item.Parent.Navigation.Id)
			{
				throw new ServiceException("Can not change navigation placement for child item {0}.", item.Id);
			}
			if (item.Parent == null && item.Navigation.Id != existing.Navigation.Id)
			{
				foreach (var c in NavigationItemDao.FetchChildren(item.Id))
				{
					c.Navigation = item.Navigation;
					SaveWithAudit(c);
				}
			}
			if (!item.Url.EqualsIgnoreCase(existing.Url))
			{
				var search = new SearchInfo<NavigationItemEntity> { AssociationId = item.AssociationId ?? 0, MatchNullAssociations = true };
				search.AddFilter(x => x.Url, existing.Url);
				if (NavigationItemDao.FetchCount(PrepareSearch(search)) <= 1)
				{
					foreach (var p in PermissionService.GetPermissions(SiteBaseConstants.SitePathKey, null, existing.Url, null, true))
					{
						p.Key3 = item.Url;
						SaveWithAudit(p);
					}
				}
			}
			return SaveWithAudit(item);
		}

		public void DeleteNavigationItem(long id, bool deleteOrphanedPermissions)
		{
			var item = GetNavigationItem(id);
			var search = new SearchInfo<NavigationItemEntity> { AssociationId = item.AssociationId ?? 0, MatchNullAssociations = true };
			search.AddFilter(x => x.Url, item.Url);
			foreach (var p in PermissionService.GetPermissions(typeof(NavigationItemEntity).Name, id, null, null, true))
			{
				DeleteWithAudit(p);
			}
			if (deleteOrphanedPermissions)
			{
				if (NavigationItemDao.FetchCount(PrepareSearch(search)) <= 1)
				{
					foreach (var p in PermissionService.GetPermissions(SiteBaseConstants.SitePathKey, null, item.Url, null, true))
					{
						DeleteWithAudit(p);
					}
				}
			}
			DeleteWithAudit(item);
		}

		public IEnumerable<NavigationItemEntity> GetParentCandidatesForNavigationItem(long id, long associationId, long navigationId)
		{
			return NavigationItemDao.FetchParentCandidates(id, associationId, navigationId);
		}

		public IList<NavigationItemEntity> GetNavigationItems(long associationId, Navigation nav, long? userId)
		{
			return LocalizeRegistered(NavigationItemDao.FetchItems(associationId, nav, userId));
		}

		#endregion

		#region Private Helpers

		/// <summary>
		/// Validates the module.
		/// </summary>
		/// <param name="module">The module.</param>
		private static void ValidateModule(ModuleEntity module)
		{
			var m = ModuleDao.Fetch(ModuleEntity.NameProperty, module.Name);
			if (m != null && m.Id != module.Id)
			{
				throw new ServiceValidationException(ModuleConstants.ErrorModuleDuplicate);
			}
			var d = ModuleDefinitionDao.Fetch((long)module.ModuleDefinition);
			if (!d.AllowMultiple)
			{
				throw new ServiceValidationException(ModuleConstants.ErrorModuleInstanceNotAllowed);
			}
		}

		/// <summary>
		/// Validates runtime substitution keys against defined substitutions
		/// </summary>
		/// <param name="moduleSettingDefinition">The module setting definition.</param>
		/// <param name="substitutions">The substitutions.</param>
		/// <param name="substitutionsWithStringKeys">The substitutions with string keys.</param>
		/// <returns>list of undefined substitution keys</returns>
		private static IList<string> ValidateSubstitutions(ModuleSettingDefinition moduleSettingDefinition, IDictionary<SubstitutionDefinition, string> substitutions, IDictionary<string, string> substitutionsWithStringKeys)
		{
			var retVal = new List<string>();
			var definedSubstitutions = MsgTemplateSubstDao.FetchByModuleSettingDefinition(moduleSettingDefinition);
			foreach (var mts in definedSubstitutions)
			{
				var found = false;
				foreach (var key in substitutions.Keys)
				{
					if (mts.SubstitutionDefinition.Id == (long)key)
					{
						substitutionsWithStringKeys[mts.SubstitutionDefinition.Name] = substitutions[key];
						found = true;
						break;
					}
				}
				if (!found)
				{
					retVal.Add(mts.SubstitutionDefinition.Name);
				}
			}
			return retVal;
		}

		private static SearchInfo<NavigationItemEntity> PrepareSearch(SearchInfo<NavigationItemEntity> searchInfo)
		{
			if (searchInfo.ApplyDefaultFilters)
			{
				if (searchInfo.AssociationId > 0)
				{
					searchInfo.AddFilter(x => x.AssociationId, searchInfo.AssociationId).Grouping = -1;
					if (searchInfo.MatchNullAssociations)
					{
						searchInfo.AddFilter(x => x.AssociationId, ComparisonOperator.Null).Grouping = -1;
					}
				}
				if (searchInfo.SearchText.HasText())
				{
					searchInfo.AddFilter(x => x.Text, ComparisonOperator.Contains, searchInfo.SearchText).Grouping = 1;
					searchInfo.AddFilter(x => x.Url, ComparisonOperator.Contains, searchInfo.SearchText).Grouping = 2;
				}
				searchInfo.ApplyDefaultFilters = false;
			}
			return searchInfo;
		}

		#endregion
	}
}
