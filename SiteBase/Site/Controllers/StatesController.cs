// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Models.States;
using DigitalBeacon.SiteBase.Web;
using DigitalBeacon.SiteBase.Web.Models;
using DigitalBeacon.Util;

namespace DigitalBeacon.SiteBase.Controllers
{
	[Authorization(Role.Administrator,Exclude="List")]
	public class StatesController : EntityController<StateEntity, EditModel>
	{

		public StatesController()
		{
			DefaultSort = new[] { new SortItem { Member = StateEntity.NameProperty } };
		}

		/// <summary>
		/// Get states for the specified country
		/// </summary>
		/// <param name="country">The country.</param>
		/// <returns></returns>
		public ActionResult List(long country)
		{
			var search = new SearchInfo<StateEntity> { ApplyDefaultFilters = false };
			search.AddFilter(x => x.Country.Id, country);
			return Json(LookupService.GetEntityList(CurrentAssociationId, search)
							.Select(x => new { x.Id, x.Name }), JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// Validates the name property.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="country">The country.</param>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		public ActionResult ValidateName(long id, long country, string name)
		{
			return Json(IsNameUnique(id, country, name), JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// Validates the code property.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="country">The country.</param>
		/// <param name="code">The code.</param>
		/// <returns></returns>
		public ActionResult ValidateCode(long id, long country, string code)
		{
			return Json(IsCodeUnique(id, country, code), JsonRequestBehavior.AllowGet);
		}

		#region EntityController

		protected override string GetDescription(EditModel model)
		{
			return model.Name;
		}

		protected override ListModelBase ConstructListModel()
		{
			var model = new ListModel { Country = GetParamAsString(StateEntity.CountryProperty).ToInt64() ?? (long)Country.UnitedStates };
			AddSelectList(model, StateEntity.CountryProperty,
						  LookupService.GetNameList<CountryEntity>());
			return model;
		}

		protected override SearchInfo<StateEntity> PrepareSearchInfo(SearchInfo<StateEntity> searchInfo, ListModelBase model)
		{
			base.PrepareSearchInfo(searchInfo, model);
			searchInfo.ApplyDefaultFilters = false;
			searchInfo.AddFilter(x => x.Country.Id, ((ListModel)model).Country);
			return searchInfo;
		}

		protected override EditModel PopulateSelectLists(EditModel model)
		{
			AddSelectList(model, StateEntity.CountryProperty,
						  LookupService.GetNameList<CountryEntity>());
			return model;
		}

		protected override IEnumerable<StateEntity> GetEntities(SearchInfo<StateEntity> searchInfo, ListModelBase model)
		{
			return LookupService.GetEntityList(CurrentAssociationId, searchInfo);
		}

		protected override long GetEntityCount(SearchInfo<StateEntity> searchInfo, ListModelBase model)
		{
			return LookupService.GetEntityCount(CurrentAssociationId, searchInfo);
		}

		protected override StateEntity GetEntity(long id)
		{
			return LookupService.Get<StateEntity>(id);
		}

		protected override EditModel ConstructModel(StateEntity entity)
		{
			return new EditModel
					   {
						   Id = entity.Id,
						   Name = entity.Name,
						   Code = entity.Code,
						   Country = entity.Country != null ? entity.Country.Id : (long?)null
					   };
		}

		protected override EditModel ConstructCreateModel()
		{
			return new EditModel { Country = GetParamAsString(StateEntity.CountryProperty).ToInt64() };
		}

		protected override RouteValueDictionary ConstructRouteValuesForBulkCreate()
		{
			var routeValues = base.ConstructRouteValuesForBulkCreate();
			if (Entity != null && Entity.Country != null)
			{
				routeValues[StateEntity.CountryProperty] = Entity.Country.Id;
			}
			return routeValues;
		}

		protected override void Validate(StateEntity entity, EditModel model)
		{
			if (!IsNameUnique(entity.Id, entity.Country != null ? entity.Country.Id : 0, entity.Name))
			{
				AddPropertyValidationError(BaseEntity.NameProperty, "Error.Name.Duplicate");
			}
			if (!IsCodeUnique(entity.Id, entity.Country != null ? entity.Country.Id : 0, entity.Code))
			{
				AddPropertyValidationError(BaseEntity.NameProperty, "Error.Code.Duplicate");
			}
			base.Validate(entity, model);
		}

		protected override StateEntity SaveEntity(StateEntity entity, EditModel model)
		{
			return LookupService.SaveEntity(CurrentAssociationId, entity);
		}

		protected override void DeleteEntity(long id)
		{
			LookupService.DeleteEntity<StateEntity>(id);
		}

		protected override StateEntity ConstructEntity(EditModel model)
		{
			var entity = model.IsNew ? new StateEntity() : GetEntity(model.Id);
			entity.Name = model.Name;
			entity.Code = model.Code;
			entity.Country = model.Country.HasValue ? LookupService.Get<CountryEntity>(model.Country.Value) : null;
			return entity;
		}

		#endregion

		#region Private Methods

		private bool IsNameUnique(long id, long country, string name)
		{
			var search = new SearchInfo<StateEntity>();
			search.AddFilter(x => x.Country.Id, country);
			search.AddFilter(x => x.Name, name);
			var list = LookupService.GetEntityList(CurrentAssociationId, search);
			return list.Count == 0 || list[0].Id == id;
		}

		private bool IsCodeUnique(long id, long country, string code)
		{
			var search = new SearchInfo<StateEntity>();
			search.AddFilter(x => x.Country.Id, country);
			search.AddFilter(x => x.Code, code);
			var list = LookupService.GetEntityList(CurrentAssociationId, search);
			return list.Count == 0 || list[0].Id == id;
		}

		#endregion
	}
}