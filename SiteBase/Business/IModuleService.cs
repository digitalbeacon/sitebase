// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Collections.Generic;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Model;

namespace DigitalBeacon.SiteBase.Business
{
	/// <summary>
	/// interface for business logic pertaining to modules and such
	/// </summary>
	public interface IModuleService
	{
		/// <summary>
		/// Register a new module
		/// </summary>
		/// <param name="associationId"></param>
		/// <param name="moduleDefinition"></param>
		/// <param name="url"></param>
		ModuleEntity RegisterModule(long associationId, ModuleDefinition moduleDefinition, string url);

		/// <summary>
		/// Register a new module
		/// </summary>
		/// <param name="associationId"></param>
		/// <param name="moduleDefinition"></param>
		/// <param name="url"></param>
		/// <param name="name"></param>
		ModuleEntity RegisterModule(long associationId, ModuleDefinition moduleDefinition, string url, string name);

		/// <summary>
		/// Get the default instance of a particular module type
		/// </summary>
		/// <param name="associationId"></param>
		/// <param name="moduleDefinition"></param>
		/// <returns></returns>
		ModuleEntity GetDefaultInstance(long associationId, ModuleDefinition moduleDefinition);

		/// <summary>
		/// Gets the module instance by Id
		/// </summary>
		/// <param name="moduleInstanceId">The module instance id.</param>
		/// <returns></returns>
		ModuleEntity GetModuleInstance(long moduleInstanceId);

		/// <summary>
		/// Gets the module instance by name for the specified association.
		/// </summary>
		/// <param name="associationId">The association id.</param>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		ModuleEntity GetModuleInstance(long associationId, string name);

		/// <summary>
		/// Get the module instance for the given module def and name
		/// </summary>
		/// <param name="associationId"></param>
		/// <param name="moduleDefinition"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		ModuleEntity GetModuleInstance(long associationId, ModuleDefinition moduleDefinition, string name);

		/// <summary>
		/// Saves the module.
		/// </summary>
		/// <param name="module">The module.</param>
		/// <returns></returns>
		ModuleEntity SaveModule(ModuleEntity module);

		/// <summary>
		/// Deletes the module.
		/// </summary>
		/// <param name="id">The id.</param>
		void DeleteModule(long id);

		/// <summary>
		/// Get all modules for by module definition
		/// </summary>
		/// <param name="associationId"></param>
		/// <param name="moduleDefinition"></param>
		/// <returns></returns>
		IList<ModuleEntity> GetModulesByDefinition(long associationId, ModuleDefinition moduleDefinition);

		/// <summary>
		/// Gets the module definition.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns></returns>
		ModuleDefinitionEntity GetModuleDefinition(long id);

		/// <summary>
		/// Gets a list of module definitions to display
		/// </summary>
		/// <returns></returns>
		IList<ModuleDefinitionEntity> GetModuleDefinitionsToDisplay();

		/// <summary>
		/// Gets all instance module setting definitions to display by module definition
		/// </summary>
		/// <param name="moduleDefinition"></param>
		/// <returns></returns>
		IList<ModuleSettingDefinitionEntity> GetModuleSettingDefinitionsToDisplay(ModuleDefinition moduleDefinition);

		/// <summary>
		/// Gets all global module setting definitions to display by module definition
		/// </summary>
		/// <param name="moduleDefinition"></param>
		/// <returns></returns>
		IList<ModuleSettingDefinitionEntity> GetGlobalModuleSettingDefinitionsToDisplay(ModuleDefinition moduleDefinition);

		/// <summary>
		/// Get module setting defintion by Id
		/// </summary>
		/// <param name="moduleSettingDefinition"></param>
		/// <returns></returns>
		ModuleSettingDefinitionEntity GetModuleSettingDefinition(ModuleSettingDefinition moduleSettingDefinition);

		/// <summary>
		/// Get module setting by module setting defintion
		/// </summary>
		/// <param name="moduleId"></param>
		/// <param name="moduleSettingDefinition"></param>
		/// <returns></returns>
		ModuleSettingEntity GetModuleSetting(long moduleId, ModuleSettingDefinition moduleSettingDefinition);

		/// <summary>
		/// Get module setting by module setting defintion key
		/// </summary>
		/// <param name="moduleId"></param>
		/// <param name="settingDefKey"></param>
		/// <returns></returns>
		ModuleSettingEntity GetModuleSetting(long moduleId, string settingDefKey);

		/// <summary>
		/// Get global module setting by module setting defintion
		/// </summary>
		/// <param name="moduleSettingDefinition"></param>
		/// <returns></returns>
		ModuleSettingEntity GetGlobalModuleSetting(ModuleSettingDefinition moduleSettingDefinition);

		/// <summary>
		/// Get localized global module setting by module setting defintion
		/// </summary>
		/// <param name="moduleSettingDefinition">The module setting definition.</param>
		/// <param name="language">The language.</param>
		/// <returns></returns>
		ModuleSettingEntity GetGlobalModuleSetting(ModuleSettingDefinition moduleSettingDefinition, Language? language);

		/// <summary>
		/// Get global module setting by module defintion and setting definition key
		/// </summary>
		/// <param name="moduleDef"></param>
		/// <param name="settingDefKey"></param>
		/// <returns></returns>
		ModuleSettingEntity GetGlobalModuleSetting(ModuleDefinition moduleDef, string settingDefKey);

		/// <summary>
		/// Get global setting by setting defintion key
		/// </summary>
		/// <param name="settingDefKey"></param>
		/// <returns></returns>
		ModuleSettingEntity GetGlobalSetting(string settingDefKey);

		/// <summary>
		/// Save the given module setting
		/// </summary>
		/// <param name="moduleSetting"></param>
		ModuleSettingEntity SaveModuleSetting(ModuleSettingEntity moduleSetting);

		/// <summary>
		/// Saves the collection of module settings.
		/// </summary>
		/// <param name="moduleSettings">The module settings.</param>
		void SaveModuleSettings(IEnumerable<ModuleSettingEntity> moduleSettings);

		/// <summary>
		/// Get module setting value with substitution replacements
		/// </summary>
		/// <param name="moduleSetting"></param>
		/// <param name="substitutions"></param>
		/// <returns></returns>
		string GetModuleSettingValueWithSubstitutions(ModuleSettingEntity moduleSetting, IDictionary<SubstitutionDefinition, string> substitutions);

		/// <summary>
		/// Validate module setting value against defined substitutions
		/// </summary>
		/// <param name="moduleSettingDefinition"></param>
		/// <param name="message"></param>
		/// <param name="enclosingSubstitutionTag"></param>
		/// <returns></returns>
		bool ValidateSubstitutions(ModuleSettingDefinition moduleSettingDefinition, string message, string enclosingSubstitutionTag);

		/// <summary>
		/// Creates the resource set for module settings.
		/// </summary>
		/// <param name="languageId">The language id.</param>
		void CreateResourceSet(long languageId);

		/// <summary>
		/// Localizes the module setting.
		/// </summary>
		/// <param name="moduleSetting">The module setting.</param>
		/// <param name="language">The language.</param>
		/// <returns></returns>
		ModuleSettingEntity LocalizeModuleSetting(ModuleSettingEntity moduleSetting, LanguageEntity language);

		#region Navigation Items

		/// <summary>
		/// Gets the navigation item.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns></returns>
		NavigationItemEntity GetNavigationItem(long id);

		/// <summary>
		/// Gets the navigation items.
		/// </summary>
		/// <param name="searchInfo">The search info.</param>
		/// <returns></returns>
		IList<NavigationItemEntity> GetNavigationItems(SearchInfo<NavigationItemEntity> searchInfo);

		/// <summary>
		/// Gets the navigation item count.
		/// </summary>
		/// <param name="searchInfo">The search info.</param>
		/// <returns></returns>
		long GetNavigationItemCount(SearchInfo<NavigationItemEntity> searchInfo);

		/// <summary>
		/// Saves the navigation item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns></returns>
		NavigationItemEntity SaveNavigationItem(NavigationItemEntity item);

		/// <summary>
		/// Deletes the navigation item.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="deleteOrphanedPermissions">if set to <c>true</c> [delete orphaned permissions].</param>
		void DeleteNavigationItem(long id, bool deleteOrphanedPermissions);

		/// <summary>
		/// Gets the parent candidates for the specified navigation item.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="associationId">The association id.</param>
		/// <param name="navigationId">The navigation id.</param>
		/// <returns></returns>
		IEnumerable<NavigationItemEntity> GetParentCandidatesForNavigationItem(long id, long associationId, long navigationId);

		/// <summary>
		/// Gets navigation items.
		/// </summary>
		/// <param name="associationId">The association id.</param>
		/// <param name="nav">The navigation type.</param>
		/// <param name="userId">The user id.</param>
		/// <returns></returns>
		IList<NavigationItemEntity> GetNavigationItems(long associationId, Navigation nav, long? userId);

		#endregion
	}
}
