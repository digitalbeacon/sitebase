// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using DigitalBeacon.Business;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Business;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Model.Content;
using DigitalBeacon.SiteBase.Models.Content;
using DigitalBeacon.SiteBase.Web;
using DigitalBeacon.SiteBase.Web.Models;
using DigitalBeacon.Util;
using Spark;

namespace DigitalBeacon.SiteBase.Controllers
{
	[Precompile("Content/FlexibleContent")]
	[Authorization(Role.Administrator)]
	public class ContentController : SiteBaseController
	{
		private const string DefaultContentModuleName = "Default";
		private const string DefaultContentGroupName = "Default";

		private static readonly IContentService ContentService = ServiceFactory.Instance.GetService<IContentService>();

		public ContentController()
		{
			SuppressViewNameTranslation = true;
		}

		#region Basic

		/// <summary>
		/// Displays basic content
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns></returns>
		public ActionResult Basic(string id)
		{
			if (id.IsNullOrBlank())
			{
				id = DefaultContentModuleName;
			}

			ModuleEntity module = null;
			if (id.IndexOf(".") == -1 && ResourceManager.ClientCulture.Name != ResourceManager.SystemCulture.Name)
			{
				module = ModuleService.GetModuleInstance(CurrentAssociationId, "{0}.{1}".FormatWith(id, ResourceManager.ClientCulture.Name)); 
			}
			if (module == null)
			{
				module = ModuleService.GetModuleInstance(CurrentAssociationId, id);
			}
			if (module == null)
			{
				var user = CurrentUser;
				if (user != null && (user.SuperUser || HasRole(Role.Administrator)))
				{
					module = ModuleService.RegisterModule(CurrentAssociationId, ModuleDefinition.BasicContent, Request.RawUrl.Replace(Request.ApplicationPath, "~"), id);
				}
			}

			if (module == null || module.ModuleDefinition != ModuleDefinition.BasicContent)
			{
				throw new BaseException("Could not locate content module instance with name [{0}].", id);
			}

			ModuleId = module.Id;

			var ms = ModuleService.GetModuleSetting(ModuleId.Value, ModuleSettingDefinition.ContentMain);

			var model = AddTransientMessages(new BaseViewModel(ms.Subject));
			model.Content = ms.Value;

			return View("Message", model);
		}

		#endregion

		#region Flexible

		/// <summary>
		/// Displays flexible content
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="page">The page.</param>
		/// <returns></returns>
		public ActionResult Flexible(string id, int? pageSize, int? page)
		{
			if (id.IsNullOrBlank())
			{
				id = DefaultContentGroupName;
			}
			ContentGroupEntity group = null;
			if (id.IndexOf(".") == -1 && ResourceManager.ClientCulture.Name != ResourceManager.SystemCulture.Name)
			{
				group = ContentService.GetContentGroup(CurrentAssociationId, "{0}.{1}".FormatWith(id, ResourceManager.ClientCulture.Name));
			}
			if (group == null)
			{
				group = ContentService.GetContentGroup(CurrentAssociationId, id);
			}
			if (group == null)
			{
				var module = ModuleService.GetModuleInstance(CurrentAssociationId, id);
				var user = IdentityService.GetCurrentUser();
				if (module == null && user != null && (user.SuperUser || HasRole(Role.Administrator)))
				{
					group = new ContentGroupEntity
					{
						AssociationId = CurrentAssociationId,
						Name = id,
						Title = id,
						DisplayOrder = 1,
						ContentGroupType = ContentService.GetContentGroupType(ContentGroupType.Default)
					};
					group = ContentService.SaveContentGroup(group);
					ModuleService.RegisterModule(CurrentAssociationId, ModuleDefinition.FlexibleContent, Request.RawUrl, id);
				}
			}

			if (group == null)
			{
				throw new BaseException("Could not locate content group for [{0}].", id);
			}

			if (ResourceManager.Instance.UseDatabaseResources)
			{
				group = LocalizationService.LocalizeProperty(group, ContentGroupEntity.TitleProperty);
			}

			var model = new FlexibleContentModel
			{
				Heading = group.Title,
				CssClass = group.ContentGroupType.CssClass,
				DateFormat = group.ContentGroupType.DateFormat,
				Page = page.HasValue ? page.Value : 1,
				PageSize = pageSize.HasValue ? pageSize.Value :
					PreferenceService.GetListPageSize(ContentGroupsController.EntriesPageSizeKey.FormatWith(group.Id), CurrentAssociationId, CurrentUser)
			};
			var searchInfo = new SearchInfo<ContentEntryEntity> { PageSize = model.PageSize, Page = model.Page, ApplyDefaultSorting = false };
			searchInfo.AddSort(x => x.DisplayOrder);
			searchInfo.AddSort(x => x.ContentDate, ListSortDirection.Descending);
			model.Items = LocalizationService.Localize(ContentService.GetEntries(group.Id, true, searchInfo), ContentEntryEntity.TitleProperty);
			model.TotalCount = model.Items.Count();
			if (model.Page > 1 || model.TotalCount == model.PageSize)
			{
				model.TotalCount = ContentService.GetEntryCount(group.Id, true, new SearchInfo<ContentEntryEntity>());
			}

			if (IsMobile)
			{
				return View("Content/Mobile/FlexibleContent", AddTransientMessages(model));
			}

			return View("Content/FlexibleContent", AddTransientMessages(model));
		}

		#endregion
	}
}
