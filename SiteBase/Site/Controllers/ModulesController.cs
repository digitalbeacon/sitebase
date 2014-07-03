// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DigitalBeacon.Business;
using DigitalBeacon.SiteBase.Business;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Models.Modules;
using DigitalBeacon.SiteBase.Web;
using DigitalBeacon.SiteBase.Web.Models;
using DigitalBeacon.Util;
using Spark;

namespace DigitalBeacon.SiteBase.Controllers
{
	[Precompile("Modules/Edit")]
	[Precompile("Modules/Editor")]
	[Precompile("Modules/List")]
	[Authorization(Role.Administrator)]
	public class ModulesController : SiteBaseController
	{
		public const string ListView = "Modules/List";
		public const string EditView = "Modules/Edit";

		#region Common

		/// <summary>
		/// Validates the name of the module.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		public ActionResult ValidateName(long id, string name)
		{
			var m = ModuleService.GetModuleInstance(CurrentAssociationId, name);
			return Json(m == null || m.Id == id, JsonRequestBehavior.AllowGet);
		}

		#endregion

		#region List

		/// <summary>
		/// Displays the list of modules
		/// </summary>
		/// <returns></returns>
		public ActionResult Index()
		{
			var model = new ListModel<ModuleDef> { Items = ModuleService.GetModuleDefinitionsToDisplay().Select(x => ConstructModuleDef(x)) };
			return View(ListView, AddTransientMessages(model));
		}

		private ModuleDef ConstructModuleDef(ModuleDefinitionEntity x)
		{
			return new ModuleDef
			{
				Id = x.Id,
				Name = x.Name,
				Instances = ModuleService.GetModulesByDefinition(CurrentAssociationId, (ModuleDefinition)x.Id)
			};
		}

		#endregion

		#region Edit

		/// <summary>
		/// Displays the edit form
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns></returns>
		public ActionResult Edit(long id)
		{
			var module = ModuleService.GetModuleInstance(id);
			var model = new EditModel
							{
				Id = module.Id,
				Name = module.Name,
				Url = module.Url,
				Settings = new List<ModuleSetting>()
			};
			foreach (var def in ModuleService.GetModuleSettingDefinitionsToDisplay(module.ModuleDefinition)
				.Where(x => x.ModuleSettingType != ModuleSettingType.Custom))
			{
				model.Settings.Add(ConstructSetting(def, ModuleService.GetModuleSetting(model.Id, (ModuleSettingDefinition)def.Id)));
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
			ActionResult retVal = null;
			var moduleId = Convert.ToInt64(form[WebConstants.IdKey]);
			if (form[WebConstants.CancelKey].HasText())
			{
				return RedirectToAction(ListActionName, new { id = String.Empty });
			}
			var module = ModuleService.GetModuleInstance(moduleId);
			var model = new EditModel
			{
				Id = module.Id,
				Name = module.Name,
				Url = module.Url,
				Definitions = ModuleService.GetModuleSettingDefinitionsToDisplay(module.ModuleDefinition).ToDictionary(x => x.Id)
			};
			TryUpdateModel(model);
			if (ModelState.IsValid)
			{
				try
				{
					//using (TransactionScope tx = new TransactionScope())
					//{
						if (model.Settings != null)
						{
							var saveList = new List<ModuleSettingEntity>();
							foreach (var info in model.Settings)
							{
								var entity = ModuleService.GetModuleSetting(model.Id, (ModuleSettingDefinition)info.DefinitionId);
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
						if (model.Name != module.Name || model.Url != module.Url)
						{
							module.Name = model.Name;
							module.Url = model.Url ?? "~/content/basic/" + model.Name.ToCamelCase();
							ModuleService.SaveModule(module);
						}
					//	tx.Complete();
					//}
					AddTransientMessage("Modules.Edit.Confirmation");
					retVal = RedirectToAction(EditActionName);
				}
				catch (ServiceValidationException ex)
				{
					if (!ex.ValidationMessages.Contains(ModuleConstants.ErrorModuleDuplicate))
					{
						throw;
					}
					AddError(model, "Modules.Error.DuplicateName");
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
			var value = info.Value;
			if (info.Value.IsNullOrBlank())
			{
				value = null;
			}
			else if (info.Type == ModuleSettingType.LongText || info.Type == ModuleSettingType.MessageTemplate)
			{
				value = Server.HtmlDecode(info.Value);
			}
			if (entity.Value != value)
			{
				entity.Value = value;
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

		#region Delete

		public ActionResult Delete(long id)
		{
			var model = new EntityModel 
			{ 
				Id = id,
				Description = ModuleService.GetModuleInstance(id).Name
			};
			return View(DeleteViewName, model);
		}

		[HttpDelete]
		public ActionResult Delete(EntityModel model)
		{
			ActionResult retVal;
			if (model.Cancel.HasText())
			{
				return RedirectToAction(ListActionName, new { id = String.Empty });
			}
			var module = ModuleService.GetModuleInstance(model.Id);
			ModuleService.DeleteModule(model.Id);
			AddTransientMessage("Modules.Delete.Confirmation", module.Name);
			if (RenderPartial)
			{
				retVal = RedirectToAction(EditActionName, "moduleDefinitions", new { id = (long)module.ModuleDefinition });
			}
			else
			{
				retVal = RedirectToAction(ListActionName, "modules", new { id = String.Empty });
			}
			return retVal ?? View(DeleteViewName, model);
		}

		#endregion
	}
}
