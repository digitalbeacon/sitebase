// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Model.Contacts;
using DigitalBeacon.SiteBase.Models.Contacts;
using DigitalBeacon.SiteBase.Web;
using DigitalBeacon.SiteBase.Web.Models;

namespace DigitalBeacon.SiteBase.Controllers
{
	[Authorization(Role.Administrator)]
	public class ContactCommentTypesController : NamedEntityController<ContactCommentTypeEntity, CommentTypeEditModel>
	{
		static ContactCommentTypesController()
		{
			Mapper.CreateMap<ContactCommentTypeEntity, CommentTypeListItem>();
			Mapper.CreateMap<ContactCommentTypeEntity, CommentTypeEditModel>();
			Mapper.CreateMap<CommentTypeEditModel, ContactCommentTypeEntity>();
		}

		#region Overrides of EntityController

		protected override string EditView
		{
			get { return @"..\Contacts\CommentTypeEdit"; }
		}

		protected override string DisplayView
		{
			get { return @"..\Contacts\CommentTypeEdit"; }
		}

		protected override string ListView

		{
			get { return @"..\Contacts\CommentTypeList"; }
		}

		#endregion

		#region Overrides of NamedEntityController

		protected override ListModelBase ConstructListModel()
		{
			return new ListModel<CommentTypeListItem>();
		}

		protected override IEnumerable ConstructGridItems(IEnumerable<ContactCommentTypeEntity> source, ListModelBase model)
		{
			return source.Select(x => Mapper.Map<CommentTypeListItem>(x));
		}

		protected override CommentTypeEditModel ConstructModel(ContactCommentTypeEntity entity)
		{
			var model = Mapper.Map<CommentTypeEditModel>(entity);
			model.Inactive = entity.DisplayOrder == 0;
			return model;
		}

		protected override ContactCommentTypeEntity ConstructEntity(CommentTypeEditModel model)
		{
			var entity = base.ConstructEntity(model);
			Mapper.Map(model, entity);
			if (model.DisplayOrder == null)
			{
				entity.DisplayOrder = 1;
			}
			return entity;
		}

		#endregion
	}
}