// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Models.PostalCodes;
using DigitalBeacon.SiteBase.Web;
using DigitalBeacon.SiteBase.Web.Models;
using DigitalBeacon.Util;

namespace DigitalBeacon.SiteBase.Controllers
{
	[Authorization(Role.Administrator)]
	public class PostalCodesController : EntityController<PostalCodeEntity, EditModel>
	{
		static PostalCodesController()
		{
			Mapper.CreateMap<PostalCodeEntity, ListItem>();
			Mapper.CreateMap<PostalCodeEntity, EditModel>();
			Mapper.CreateMap<EditModel, PostalCodeEntity>();
		}

		public ActionResult Json(long id)
		{
			var item = LookupService.GetByCode<PostalCodeEntity>(id.ToString()).IfNotNull(x => Mapper.Map<ListItem>(x));
			if (item != null && item.StateCode.HasText())
			{
				item.StateId = LookupService.GetByCode<StateEntity>(item.StateCode).IfNotNull(x => (long?)x.Id);
			}
			return Json(item, JsonRequestBehavior.AllowGet);
		}

		protected override EditModel ConstructModel(PostalCodeEntity entity)
		{
			var model = Mapper.Map<EditModel>(entity);
			if (entity.StateCode.HasText())
			{
				model.StateId = LookupService.GetByCode<StateEntity>(entity.StateCode).IfNotNull(x => (long?)x.Id);
			}
			return model;
		}

		protected override ListModelBase ConstructListModel()
		{
			return new ListModel<ListItem>();
		}

		protected override IEnumerable ConstructGridItems(IEnumerable<PostalCodeEntity> source, ListModelBase model)
		{
			return source.Select(x => Mapper.Map<ListItem>(x));
		}

		protected override void DeleteEntity(long id)
		{
			LookupService.DeleteEntity<PostalCodeEntity>(id);
		}

		protected override string GetDescription(EditModel model)
		{
			return model.Code;
		}

		protected override EditModel PopulateSelectLists(EditModel model)
		{
			AddSelectList(model, model.PropertyName(x => x.StateId), LookupService.GetNameList<StateEntity>());
			return model;
		}

		protected override IEnumerable<PostalCodeEntity> GetEntities(SearchInfo<PostalCodeEntity> searchInfo, ListModelBase model)
		{
			return LookupService.GetEntityList(searchInfo);
		}

		protected override PostalCodeEntity GetEntity(long id)
		{
			return LookupService.Get<PostalCodeEntity>(id);
		}

		protected override long GetEntityCount(SearchInfo<PostalCodeEntity> searchInfo, ListModelBase model)
		{
			return LookupService.GetEntityCount(searchInfo);
		}

		protected override PostalCodeEntity ConstructEntity(EditModel model)
		{
			var entity = base.ConstructEntity(model);
			Mapper.Map(model, entity);
			if (model.StateId.HasValue)
			{
				entity.StateCode = LookupService.GetCode<StateEntity>(model.StateId.Value);
			}
			return entity;
		}

		protected override PostalCodeEntity SaveEntity(PostalCodeEntity entity, EditModel model)
		{
			return LookupService.SaveEntity(entity);
		}
	}
}