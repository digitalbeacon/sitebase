// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2010-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Web.Mvc;
using AutoMapper;
using DigitalBeacon.Business;
using DigitalBeacon.SiteBase.Business;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Model.Contacts;
using DigitalBeacon.SiteBase.Models.Comments;
using DigitalBeacon.SiteBase.Web;
using DigitalBeacon.Util;

namespace DigitalBeacon.SiteBase.Controllers
{
	[Authorization(Role.Administrator)]
	public class ContactCommentsController : CommentsController<ContactCommentEntity>
	{
		private const string CreatePath = "/contactComments/create";
		private const string DeletePath = "/contactComments/delete";
		private const string UpdatePath = "/contactComments/update";

		private static readonly IContactService ContactService = ServiceFactory.Instance.GetService<IContactService>();

		static ContactCommentsController()
		{
			Mapper.CreateMap<ContactCommentEntity, ListItem>();
			Mapper.CreateMap<ContactCommentEntity, EditModel>()
				.ForMember(t => t.CommentType, o => o.Ignore());
			Mapper.CreateMap<EditModel, ContactCommentEntity>()
				.ForMember(t => t.CommentType, o => o.Ignore());
		}

		public ContactCommentsController()
		{
			BaseName = "Comments";
			CheckCustomResources = true;
		}

		#region CommentsController

		protected override string GetParentName(long parentId)
		{
			return ContactService.GetContact(parentId).DisplayName;
		}

		protected override string ParentIdProperty
		{
			get { return ContactCommentEntity.ContactIdProperty; }
		}

		protected override bool CanAdd
		{
			get { return PermissionService.HasPermissionToSitePath(CurrentUser, CreatePath); }
		}

		protected override bool CanDelete
		{
			get { return PermissionService.HasPermissionToSitePath(CurrentUser, DeletePath); }
		}

		protected override bool CanUpdate
		{
			get { return PermissionService.HasPermissionToSitePath(CurrentUser, UpdatePath); }
		}

		protected override bool ShowTextInline
		{
			get { return false; }
		}

		protected override bool CommentTextRequired
		{
			get { return false; }
		}

		#endregion

		#region EntityController

		protected override EditModel PopulateSelectLists(EditModel model)
		{
			base.PopulateSelectLists(model);
			AddSelectList(model, model.PropertyName(x => x.CommentType), LookupService.GetNameList<ContactCommentTypeEntity>());
			return model;
		}

		protected override ContactCommentEntity ConstructEntity(EditModel model)
		{
			var entity = base.ConstructEntity(model);
			entity.CommentType = LookupService.Get<ContactCommentTypeEntity>(model.CommentType.ToInt64().Value);
			return entity;
		}

		protected override EditModel ConstructModel(ContactCommentEntity entity)
		{
			var model = base.ConstructModel(entity);
			model.CommentType = entity.CommentType.IfNotNull(x => x.Id.ToSafeString());
			return model;
		}

		[HttpPost]
		public override ActionResult Create(FormCollection form)
		{
			var retVal = base.Create(form);
			if (!CanUpdate && MessageModel != null)
			{
				MessageModel.ReturnUrl = null;
			}
			return retVal;
		}

		#endregion
	}
}