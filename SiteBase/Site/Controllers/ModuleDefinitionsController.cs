// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using System.Linq;
//using System.Transactions;
using System.Web.Mvc;
using DigitalBeacon.Business;
using DigitalBeacon.SiteBase.Business;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Models.Modules;
using DigitalBeacon.SiteBase.Web;
using DigitalBeacon.SiteBase.Web.Models;
using DigitalBeacon.Util;

namespace DigitalBeacon.SiteBase.Controllers
{
	[Authorization(Role.Administrator)]
	public class ModuleDefinitionsController : SiteBaseController
	{
		public const string EditView = "Modules/Edit";

		#region Edit

		/// <summary>
		/// Displays the edit form
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns></returns>
		public ActionResult Edit(long id)
		{
			var moduleDefinition = ModuleService.GetModuleDefinition(id);
			var model = new EditModel
							{
				Id = moduleDefinition.Id,
				Global = true,
				AllowMultiple = moduleDefinition.AllowMultiple,
				Name = moduleDefinition.Name,
				Settings = new List<ModuleSetting>()
			};
			foreach (var def in ModuleService.GetGlobalModuleSettingDefinitionsToDisplay((ModuleDefinition)model.Id))
			{
				if (def.ModuleSettingType == ModuleSettingType.Custom)
				{
					model.CustomSettings.Add(new CustomSetting { Name = def.Name, Path = def.CustomEditor });
				}
				else
				{
					model.Settings.Add(ConstructSetting(def, ModuleService.GetGlobalModuleSetting((ModuleSettingDefinition)def.Id)));
				}
			}
			return View(EditView, AddTransientMessages(model));
		}

		public ActionResult Editor()
		{
			return View("Modules/Editor");
		}

		[HttpPut]
		public ActionResult Update(FormCollection form)
		{
			if (form[WebConstants.CancelKey].HasText())
			{
				return RedirectToAction(ListActionName, new { id = String.Empty });
			}

			ActionResult retVal = null;
			var moduleDefinition = ModuleService.GetModuleDefinition(Convert.ToInt64(form[WebConstants.IdKey]));
			var model = new EditModel
							{
				Id = moduleDefinition.Id,
				Global = true,
				AllowMultiple = moduleDefinition.AllowMultiple,
				Name = moduleDefinition.Name,
				Definitions = ModuleService.GetGlobalModuleSettingDefinitionsToDisplay((ModuleDefinition)moduleDefinition.Id).ToDictionary(x => x.Id)
			};
			TryUpdateModel(model);
			if (ModelState.IsValid)
			{
				if (model.Settings != null)
				{
					var saveList = new List<ModuleSettingEntity>();
					foreach (var info in model.Settings)
					{
						var entity = ModuleService.GetGlobalModuleSetting((ModuleSettingDefinition)info.DefinitionId);
						if (PopulateSettingEntity(entity, info))
						{
							saveList.Add(entity);
						}
					}
					if (saveList.Count > 0)
					{
						ModuleService.SaveModuleSettings(saveList);
					}
				}
				AddTransientMessage("ModuleDefinitions.Edit.Confirmation");
				retVal = RedirectToAction(EditActionName);
			}
			else
			{
				foreach (var def in model.Definitions.Values)
				{
					if (def.ModuleSettingType == ModuleSettingType.Custom)
					{
						model.CustomSettings.Add(new CustomSetting
						{
							Name = def.Name,
							Path = def.CustomEditor
						});
					}
				}
			}
			return retVal ?? View(EditView, model);
		}

		private static ModuleSetting ConstructSetting(ModuleSettingDefinitionEntity def, ModuleSettingEntity setting)
		{
			var retVal = new ModuleSetting
			{
				Default = true,
				DefinitionId = def.Id,
				TypeId = (long)def.ModuleSettingType,
				Name = def.Name,
				Required = def.Required,
				Min = def.MinValue,
				Max = def.MaxValue
			};
			if (setting != null)
			{
				retVal.Default = false;
				retVal.Id = setting.Id;
				retVal.Value = setting.Value;
				retVal.Subject = setting.Subject;
			}
			return retVal;
		}

		private bool PopulateSettingEntity(ModuleSettingEntity entity, ModuleSetting info)
		{
			var retVal = false;
			entity.ModuleSettingDefinition = (ModuleSettingDefinition)info.DefinitionId;
			if (info.Value.IsNullOrBlank())
			{
				info.Value = null;
			}
			else if (info.Type == ModuleSettingType.LongText || info.Type == ModuleSettingType.MessageTemplate)
			{
				info.Value = Server.HtmlDecode(info.Value);
			}
			if (entity.Value != info.Value)
			{
				entity.Value = info.Value;
				retVal = true;
			}
			if (info.Subject.IsNullOrBlank())
			{
				info.Subject = null;
			}
			if (entity.Subject != info.Subject)
			{
				entity.Subject = info.Subject;
				retVal = true;
			}
			return retVal;
		}

		#endregion

		#region New/Create

		/// <summary>
		/// Displays the create instance form
		/// </summary>
		/// <returns></returns>
		public ActionResult NewInstance(long id)
		{
			var moduleDefinition = ModuleService.GetModuleDefinition(id);
			var model = new EditModel
							{
				Heading = GetLocalizedText("ModuleDefinitions.NewInstance.Heading", moduleDefinition.Name),
				Id = id,
				CreateInstance = true,
				Settings = new List<ModuleSetting>()
			};
			foreach (var def in ModuleService.GetModuleSettingDefinitionsToDisplay((ModuleDefinition)id))
			{
				model.Settings.Add(ConstructSetting(def, null));
			}
			return View(EditView, model);
		}

		/// <summary>
		/// Handles the create submission
		/// </summary>
		/// <param name="form">The form.</param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult CreateInstance(FormCollection form)
		{
			if (form[WebConstants.CancelKey].HasText())
			{
				return RedirectToAction(EditActionName);
			}

			ActionResult retVal = null;
			var moduleDefinition = ModuleService.GetModuleDefinition(Convert.ToInt64(form[WebConstants.IdKey]));
			var model = new EditModel
			{
				Heading = GetLocalizedText("ModuleDefinitions.NewInstance.Heading", moduleDefinition.Name),
				Id = moduleDefinition.Id,
				CreateInstance = true,
				Definitions = ModuleService.GetModuleSettingDefinitionsToDisplay((ModuleDefinition)moduleDefinition.Id).ToDictionary(x => x.Id)
			};
			TryUpdateModel(model);
			if (ModelState.IsValid)
			{
				var module = new ModuleEntity
				{
					AssociationId = CurrentAssociationId,
					ModuleDefinition = (ModuleDefinition)moduleDefinition.Id,
					Name = model.Name,
					Url = model.Url ?? "~/content/basic/" + model.Name.ToCamelCase()
				};
				var saveList = new List<ModuleSettingEntity>();
				if (model.Settings == null)
				{
					model.Settings = new List<ModuleSetting>();
				}
				else
				{
					foreach (var info in model.Settings)
					{
						ModuleSettingDefinitionEntity def = model.Definitions[info.DefinitionId];
						var entity = new ModuleSettingEntity
						{
							ModuleSettingDefinition = (ModuleSettingDefinition)def.Id,
							Value = def.DefaultValue,
							Subject = def.DefaultSubject
						};
						if (PopulateSettingEntity(entity, info))
						{
							saveList.Add(entity);
						}
					}
				}
				try
				{
					//using (var tx = new TransactionScope())
					//{
						module = ModuleService.SaveModule(module);
						if (saveList.Count > 0)
						{
							foreach (var s in saveList)
							{
								s.ModuleId = module.Id;
							}
							ModuleService.SaveModuleSettings(saveList);
						}
					//	tx.Complete();
					//}
					AddTransientMessage("ModuleDefinitions.CreateInstance.Confirmation", module.Name, moduleDefinition.Name);
					retVal = RedirectToAction(EditActionName, "modules", new { id = module.Id });
				}
				catch (ServiceValidationException ex)
				{
					foreach (var x in ex.ValidationMessages)
					{
						if (x == ModuleConstants.ErrorModuleDuplicate)
						{
							AddError(model, "Modules.Error.DuplicateName");
						}
						else if (x == ModuleConstants.ErrorModuleInstanceNotAllowed)
						{
							AddError(model, x);
						}
						else
						{
							throw;
						}
					}
				}
			}
			return retVal ?? View(EditView, model);
		}

		#endregion
	}
}
