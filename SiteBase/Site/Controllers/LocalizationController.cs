// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Routing;
using DigitalBeacon.Business;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Models.Localization;
using DigitalBeacon.SiteBase.Web;
using DigitalBeacon.SiteBase.Web.Models;
using DigitalBeacon.Util;

namespace DigitalBeacon.SiteBase.Controllers
{
	[Authorization(Role.Administrator)]
	public class LocalizationController : EntityController<ResourceEntity, EditModel>
	{
		private static readonly IDictionary<string, string> ResourceTypes = new Dictionary<string, string>();
		private static readonly string ModuleSettingKey = ResourceManager.GetTypeKey<ModuleSettingEntity>();
		private static readonly Regex NewlineRegex = new Regex(@"\r\n", RegexOptions.Compiled | RegexOptions.Multiline);
		private static readonly Regex WhitespaceRegex = new Regex(@">\s+", RegexOptions.Compiled);
		private IDictionary<string, LocalizedEntity> _entityTypes;

		public LocalizationController()
		{
			DefaultSort = new[] { new SortItem { Member = ResourceEntity.KeyProperty } };
			if (ResourceTypes.Count == 0)
			{
				foreach (var x in LocalizationService.GetResourceSetNames())
				{
					var i = x.LastIndexOf('.');
					ResourceTypes[x] = i >= 0 ? x.Substring(x.LastIndexOf('.') + 1) : x;
				}
			}
		}

		[HttpPost]
		public ActionResult SaveResources(ListModel model)
		{
			if (model.Items != null && model.Items.Count() > 0)
			{
				var resources = new Dictionary<long, string>();
				foreach (var item in model.Items)
				{
					resources[item.Id] = Server.HtmlDecode(item.Value);
				}
				LocalizationService.SaveResources(resources);
			}
			return Json(GetLocalizedText("Common.Update.Confirmation", String.Empty, PluralLabel));
		}

		[HttpPost]
		public ActionResult ClearCache()
		{
			ResourceManager.Instance.ClearCache();
			return Json(GetLocalizedText("Localization.ClearCache.Confirmation"));
		}

		[HttpPost]
		public ActionResult CreateSet(long language, string type)
		{
			if (type == ModuleSettingKey)
			{
				ModuleService.CreateResourceSet(language);
			}
			else if (ResourceTypes.ContainsKey(type))
			{
				LocalizationService.CreateResourceSet(language, type);
			}
			else if (EntityTypes.ContainsKey(type))
			{
				var typeInfo = EntityTypes[type];
				LocalizationService.GetType().GetMethod("CreateEntityResourceSet").MakeGenericMethod(typeInfo.Type)
					.Invoke(LocalizationService, new object[] { language, typeInfo.LocalizedProperty });
			}
			else
			{
				throw new ArgumentException("Type {0} not supported for this operation.".FormatWith(type));
			}
			return Json(GetLocalizedText("Localization.CreateSet.Confirmation"));
		}

		[HttpPost]
		public ActionResult DeleteEmptyValues(long language, string type)
		{
			if (type == ModuleSettingKey)
			{
				LocalizationService.DeleteEntityResourceSet<ModuleSettingEntity>(language, true);
			}
			else if (ResourceTypes.ContainsKey(type))
			{
				LocalizationService.DeleteResourceSet(language, type, true);
			}
			else if (EntityTypes.ContainsKey(type))
			{
				var typeInfo = EntityTypes[type];
				LocalizationService.GetType().GetMethod("DeleteEntityResourceSet").MakeGenericMethod(typeInfo.Type)
					.Invoke(LocalizationService, new object[] { language, true });
			}
			else
			{
				throw new ArgumentException("Type {0} not supported for this operation.".FormatWith(type));
			}
			return Json(GetLocalizedText("Localization.DeleteEmptyValues.Confirmation"));
		}

		#region EntityController

		protected override string GetDescription(EditModel model)
		{
			return String.Empty;
		}

		protected override string GetListHeading()
		{
			return GetLocalizedText("{0}.Heading".FormatWith(BaseName));
		}

		protected override ListModelBase ConstructListModel()
		{
			var model = new ListModel
			{
				Language = GetParamAsString(ResourceEntity.LanguageProperty).ToInt64(),
				Type = GetParamAsString(ResourceEntity.TypeProperty)
			};
			var languages = LookupService.GetNameList<LanguageEntity>();
			var systemLanguage = LookupService.GetByCode<LanguageEntity>(ResourceManager.SystemCulture.Name);
			model.DefaultLanguageName = systemLanguage != null ? systemLanguage.Name : ResourceManager.SystemCulture.DisplayName;
			model.LanguageName = model.Language.HasValue ? 
				languages.Where(x => x.Id == model.Language.Value).Select(x => x.Name).Single() : 
				GetLocalizedText("Common.Value.Label");
			AddSelectList(model, ResourceEntity.LanguageProperty, languages);
			AddTypeSelectList(model);
			if (model.Language == null && languages.Count > 0)
			{
				model.Language = languages[0].Id;
			}
			if (model.Type == null && model.ListItems[ResourceEntity.TypeProperty].Count() > 0)
			{
				model.Type = model.ListItems[ResourceEntity.TypeProperty].First().Value;
			}
			return model;
		}

		protected override SearchInfo<ResourceEntity> PrepareSearchInfo(SearchInfo<ResourceEntity> searchInfo, ListModelBase model)
		{
			var listModel = (ListModel)model;
			if (listModel.Language.HasValue)
			{
				searchInfo.AddFilter(x => x.Language.Id, listModel.Language.Value);
			}
			if (listModel.Type.HasText())
			{
				if (ResourceTypes.ContainsKey(listModel.Type))
				{
					searchInfo.AddFilter(x => x.Type, ComparisonOperator.Null);
					searchInfo.AddFilter(x => x.Property, listModel.Type);
				}
				else
				{
					searchInfo.AddFilter(x => x.Type, listModel.Type);
				}
			}
			return base.PrepareSearchInfo(searchInfo, model);
		}

		protected override IEnumerable ConstructGridItems(IEnumerable<ResourceEntity> source, ListModelBase model)
		{
			return source.Select(x => new ListItem
			{
				Id = x.Id,
				Language = x.Language != null ? x.Language.Name : String.Empty,
				Type = x.Type,
				Key = x.Key,
				Property = x.Type == ModuleSettingKey ? x.Property : String.Empty,
				Default = x.Type.IsNullOrBlank() ? ResourceManager.Instance.GetString(ResourceManager.SystemCulture, x.Key) : String.Empty,
				Value = x.Value
			});
		}

		protected override EditModel PopulateSelectLists(EditModel model)
		{
			if (model.ListItems.Count == 0)
			{
				AddSelectList(model, ResourceEntity.LanguageProperty, LookupService.GetNameList<LanguageEntity>());
				AddTypeSelectList(model);
			}
			return model;
		}

		protected override EditModel ConstructModel(ResourceEntity entity)
		{
			return new EditModel
			{
				Id = entity.Id,
				Language = entity.Language != null ? entity.Language.Id : (long?)null,
				Type = entity.Type,
				Key = entity.Key,
				Property = entity.Property,
				Value = entity.Value
			};
		}

		protected override EditModel ConstructCreateModel()
		{
			var model = new EditModel
			{
				Language = GetParamAsString(ResourceEntity.LanguageProperty).ToInt64(),
				Property = GetParamAsString(ResourceEntity.PropertyProperty)
			};
			var type = GetParamAsString(ResourceEntity.TypeProperty);
			if (type.HasText())
			{
				if (ResourceTypes.ContainsKey(type))
				{
					model.Property = type;
				}
				else
				{
					model.Type = type;
				}
			}
			return model;
		}

		protected override RouteValueDictionary ConstructRouteValuesForBulkCreate()
		{
			var routeValues = base.ConstructRouteValuesForBulkCreate();
			if (Entity != null && Entity.Language != null)
			{
				routeValues[ResourceEntity.LanguageProperty] = Entity.Language.Id;
				if (Entity.Type.HasText())
				{
					routeValues[ResourceEntity.TypeProperty] = Entity.Type;
				}
				else
				{
					routeValues[ResourceEntity.PropertyProperty] = Entity.Property;
				}
			}
			return routeValues;
		}

		protected override ResourceEntity ConstructEntity(EditModel model)
		{
			var entity = base.ConstructEntity(model);
			if (model.IsNew)
			{
				entity.Language = model.Language.HasValue ? LookupService.Get<LanguageEntity>(model.Language.Value) : null;
				entity.Type = model.Type;
				entity.Key = model.Key;
				entity.Property = model.Property;
			}
			entity.Value = WhitespaceRegex.Replace(NewlineRegex.Replace(Server.HtmlDecode(model.Value.DefaultTo(String.Empty)), String.Empty), ">");
			return entity;
		}

		protected override void Validate(ResourceEntity entity, EditModel model)
		{
			if (entity.IsNew)
			{
				var search = new SearchInfo<ResourceEntity> { ApplyDefaultFilters = false };
				search.AddFilter(x => x.Language.Id, entity.Language.Id);
				search.AddFilter(x => x.Key, entity.Key);
				if (entity.Property.HasText())
				{
					search.AddFilter(x => x.Property, entity.Property);
				}
				else
				{
					search.AddFilter(x => x.Property, ComparisonOperator.Null);
				}
				if (LocalizationService.GetResourceCount(search) > 0)
				{
					AddPropertyValidationError(String.Empty, "Localization.Error.DuplicateResource");
				}
			}
		}

		protected override IEnumerable<ResourceEntity> GetEntities(SearchInfo<ResourceEntity> searchInfo, ListModelBase model)
		{
			return LocalizationService.GetResources(searchInfo);
		}

		protected override long GetEntityCount(SearchInfo<ResourceEntity> searchInfo, ListModelBase model)
		{
			return LocalizationService.GetResourceCount(searchInfo);
		}

		protected override ResourceEntity GetEntity(long id)
		{
			return LocalizationService.GetResource(id);
		}

		protected override ResourceEntity SaveEntity(ResourceEntity entity, EditModel model)
		{
			return LocalizationService.SaveResource(entity);
		}

		protected override void DeleteEntity(long id)
		{
			LocalizationService.DeleteResource(id);
		}

		#endregion

		#region Private Methods

		private IDictionary<string, LocalizedEntity> EntityTypes
		{
			get
			{
				if (_entityTypes == null)
				{
					_entityTypes = new Dictionary<string, LocalizedEntity>();
					foreach (var x in ResourceManager.Instance.LocalizedEntityTypes)
					{
						_entityTypes[ResourceManager.GetTypeKey(x.Type)] = x;
					}
				}
				return _entityTypes;
			}
		}

		private void AddTypeSelectList(BaseViewModel model)
		{
			model.ListItems[ResourceEntity.TypeProperty] =
				ResourceTypes.Select(x => new SelectListItem { Value = x.Key, Text = x.Value })
					.Union(new[] { new SelectListItem { Value = ModuleSettingKey, Text = "Module" } })
					.Union(EntityTypes.Keys.OrderBy(x => x).Select(x => new SelectListItem { Value = x, Text = TextUtil.SeparatePascalCaseText(x, " ") }));
		}

		#endregion
	}
}
