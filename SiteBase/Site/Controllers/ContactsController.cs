// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Caching;
using System.Web.Mvc;
using AutoMapper;
using DigitalBeacon.Business;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Business;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Model.Contacts;
using DigitalBeacon.SiteBase.Models;
using DigitalBeacon.SiteBase.Models.Contacts;
using DigitalBeacon.SiteBase.Web;
using DigitalBeacon.SiteBase.Web.Models;
using DigitalBeacon.Util;
using DigitalBeacon.Web;
using DigitalBeacon.Web.Formatters;
using DigitalBeacon.Web.Util;

namespace DigitalBeacon.SiteBase.Controllers
{
	[Authorization(Role.Administrator)]
	public class ContactsController : EntityController<ContactEntity, EditModel>
	{
		private const string ImageMaxWidthPath = "/contacts/image/maxWidth";
		private const string ImageMaxHeightPath = "/contacts/image/maxHeight";
		private const string DeletePath = "/contacts/delete";
		private const string ThumbnailCacheKeyFormat = "contact-thumbnail-{0}";

		private const int DefaultImageMaxWidth = 400;
		private const int DefaultImageMaxHeight = 300;

		private static readonly IFileService FileService = ServiceFactory.Instance.GetService<IFileService>();
		private static readonly IContactService ContactService = ServiceFactory.Instance.GetService<IContactService>();

		private byte[] _photoData;
		private int _photoWidth;
		private int _photoHeight;
		private bool _populateCommentTypes;

		static ContactsController()
		{
			Mapper.CreateMap<ContactEntity, ListItem>();
			Mapper.CreateMap<AddressEntity, ListItem>()
				.ForMember(t => t.Id, o => o.Ignore())
				.ForMember(t => t.MobilePhone, o => o.MapFrom(s => s.MobilePhoneText));
			Mapper.CreateMap<ContactEntity, EditModel>()
				.ForMember(t => t.Photo, o => o.Ignore());
			Mapper.CreateMap<AddressEntity, EditModel>()
				.ForMember(t => t.Id, o => o.Ignore());
			Mapper.CreateMap<EditModel, ContactEntity>()
				.ForMember(t => t.Address, o => o.Ignore())
				.ForMember(t => t.Photo, o => o.Ignore());
			Mapper.CreateMap<EditModel, AddressEntity>()
				.ForMember(t => t.Id, o => o.Ignore());
		}

		public ContactsController()
		{
			MobileModuleName = "contacts";
			ViewBag.Heading = GetListHeading();
			DefaultSort = new[] { new SortItem { Member = ContactEntity.LastNameProperty, SortDirection = ListSortDirection.Ascending } };
		}

		[HttpPost]
		public ActionResult DeletePhoto(long id)
		{
			ActionResult retVal = null;
			ContactService.DeleteContactPhoto(id);
			HttpContext.Cache.Remove(ThumbnailCacheKeyFormat.FormatWith(id));
			if (RenderJson)
			{
				retVal = Json(new ApiResponse { Success = true, Message = GetLocalizedTextWithFormatting("Common.DeletePhoto.Confirmation").ToHtmlString() });
			}
			else if (RenderPartial)
			{
				retVal = RedirectToMessageAction(SingularLabel, "Common.DeletePhoto.Confirmation");
				MessageModel.ReturnUrl = Url.Action(EditActionName, new { renderType = WebConstants.RenderTypePartial });
				MessageModel.ReturnText = ReturnTextSingular;
			}
			else
			{
				AddTransientMessage("Common.DeletePhoto.Confirmation");
			}
			return retVal ?? RedirectToAction(EditActionName);
		}

		public ActionResult Photo(long id)
		{
			var contact = ContactService.GetContact(id);
			if (contact != null && contact.Photo != null)
			{
				return FilesController.GetFileResult(contact.Photo.Id);
				//return new TransferResult(Url.Action("show", "files", new { id = contact.Photo.Id }));
			}
			return new FilePathResult("~/resources/base/images/transparent.gif", "image/gif");
		}

		public ActionResult Thumbnail(long id, int size = 90)
		{
			var cacheKey = ThumbnailCacheKeyFormat.FormatWith(id);
			var t = HttpContext.Cache.Get(cacheKey) as Tuple<byte[], string, int>;
			if (t == null || t.Item3 != size)
			{
				var contact = ContactService.GetContact(id);
				if (contact != null && contact.Photo != null)
				{
					if (size <= 0)
					{
						size = 90;
					}
					var file = FileService.GetFile(contact.Photo.Id);
					using (var photo = Image.FromStream(new MemoryStream(file.FileData.Data)))
					using (var thumbnail = ImageUtil.CreateThumbnail(photo, size))
					{
						t = Tuple.Create(ImageUtil.GetBytes(thumbnail), file.ContentType, size);
					}
					HttpContext.Cache.Insert(cacheKey, t);
				}
				else
				{
					HttpContext.Cache.Insert(cacheKey, Tuple.Create<byte[], string, int>(null, null, size));
				}
			}
			if (t != null && t.Item1 != null && t.Item2.HasText())
			{
				return new FileStreamResult(new MemoryStream(t.Item1), t.Item2);
			}
			return new FilePathResult("~/resources/base/images/transparent.gif", "image/gif");
		}

		public ActionResult Comments(long id)
		{
			return new TransferResult(Url.Action("index", "contactComments", new { parentId = id, renderType = RenderType }));
		}

		[HttpPost]
		public ActionResult RotatePhotoCounterclockwise(long id)
		{
			return RotatePhoto(id, RotateDirection.Counterclockwise);
		}

		[HttpPost]
		public ActionResult RotatePhotoClockwise(long id)
		{
			return RotatePhoto(id, RotateDirection.Clockwise);
		}

		private ActionResult RotatePhoto(long id, RotateDirection direction)
		{
			var contact = ContactService.GetContact(id);
			if (contact.Photo != null)
			{
				var height = contact.PhotoHeight;
				contact.PhotoHeight = contact.PhotoWidth;
				contact.PhotoWidth = height;

				contact.Photo = new FileEntity
				{
					AssociationId = CurrentAssociationId,
					DataChanged = true,
					ContentType = "image/jpeg",
					DataCompressed = false,
					Filename = Guid.NewGuid() + ".jpg",
					FileData = new FileDataEntity { Data = ImageUtil.Rotate(contact.Photo.FileData.Data, direction) }
				};

				ContactService.SaveContact(contact);
				HttpContext.Cache.Remove(ThumbnailCacheKeyFormat.FormatWith(id));
			}
			if (RenderJson)
			{
				return Json(new ApiResponse { Success = true });
			}
			return RedirectToAction(EditActionName);
		}

		#region EntityController

		protected override string NewView
		{
			get { return NewViewName; }
		}

		protected override string DisplayView
		{
			get { return RenderTemplate ? DisplayViewName : EditViewName; }
		}

		protected override ListModelBase ConstructListModel()
		{
			var model = new ListModel();

			// semantics of model.Inactive value are non-standard
			model.Inactive = GetParamAsString(model.PropertyName(x => x.Inactive)).ToBoolean();
			model.ListItems[model.PropertyName(x => x.Inactive)] = new[] { 
				new SelectListItem { Value = String.Empty, Text = GetLocalizedText("Common.Enabled.Active.Label") },
				new SelectListItem { Value = Boolean.TrueString, Text = GetLocalizedText("Common.Enabled.Inactive.Label") },
				new SelectListItem { Value = Boolean.FalseString, Text = GetLocalizedText("Common.Enabled.Both.Label") }
			};

			model.CommentTypeId = GetParamAsString(model.PropertyName(x => x.CommentTypeId)).ToInt64();
			AddSelectList(model, model.PropertyName(x => x.CommentTypeId), 
				LookupService.GetNameList<ContactCommentTypeEntity>());

			if (RenderTemplate)
			{
				model.ListItems[WebConstants.AvailableSortFieldsKey] = new[] { 
					new SelectListItem { Value = ContactEntity.LastNameProperty, Text = GetLocalizedText("Common.LastName.Label") },
					new SelectListItem { Value = ContactEntity.FirstNameProperty, Text = GetLocalizedText("Common.FirstName.Label") }
				};
			}

			model.CanDelete = PermissionService.HasPermissionToSitePath(CurrentUser, DeletePath);
			return model;
		}

		protected override EditModel PopulateSelectLists(EditModel model)
		{
			AddSelectList(model, model.PropertyName(m => m.GenderId), LookupService.GetNameList<GenderEntity>().Reverse());
			AddSelectList(model, ContactEntity.RaceIdProperty, LookupService.GetNameList<RaceEntity>());
			AddSelectList(model, model.PropertyName(m => m.StateId), LookupService.GetNameList<StateEntity>());
			AddSelectList(model, model.PropertyName(m => m.DefaultPhoneId), LookupService.GetNameList<PhoneTypeEntity>());

			if (_populateCommentTypes || RenderTemplate)
			{
				AddSelectList(model, model.PropertyName(x => x.CommentType), LookupService.GetNameList<ContactCommentTypeEntity>());
			}

			return model;
		}

		protected override string GetSortMember(string member)
		{
			if (member == "Age")
			{
				return ContactEntity.DateOfBirthProperty;
			}
			if (member == "Inactive")
			{
				return ContactEntity.EnabledProperty;
			}
			return base.GetSortMember(member);
		}

		protected override ListSortDirection GetSortDirection(string member, ListSortDirection sortDirection)
		{
			if (member == "Age" || member == "Flagged")
			{
				return sortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;
			}
			return base.GetSortDirection(member, sortDirection);
		}

		protected override string GetDescription(EditModel model)
		{
			return "{0} {1}".FormatWith(model.FirstName, model.LastName);
		}

		protected override SearchInfo<ContactEntity> ConstructSearchInfo()
		{
			return new ContactSearchInfo();
		}

		protected override SearchInfo<ContactEntity> PrepareSearchInfo(SearchInfo<ContactEntity> searchInfo, ListModelBase model)
		{
			var listModel = (ListModel)model;
			var clientSearch = (ContactSearchInfo)searchInfo;
			// semantics of model.Inactive value are non-standard
			if (listModel.Inactive == null)
			{
				clientSearch.Inactive = false;
			}
			else
			{
				clientSearch.Inactive = listModel.Inactive.Value ? true : (bool?)null;
			}
			clientSearch.CommentTypeId = listModel.CommentTypeId;
			return base.PrepareSearchInfo(searchInfo, listModel);
		}

		protected override IEnumerable<ContactEntity> GetEntities(SearchInfo<ContactEntity> searchInfo, ListModelBase model)
		{
			return ContactService.GetContacts((ContactSearchInfo)searchInfo);
		}

		protected override long GetEntityCount(SearchInfo<ContactEntity> searchInfo, ListModelBase model)
		{
			return ContactService.GetContactCount((ContactSearchInfo)searchInfo);
		}

		protected override ContactEntity GetEntity(long id)
		{
			var entity = ContactService.GetContact(id);
			if (entity.Photo != null)
			{
				entity.Photo.DataChanged = false;
			}
			return entity;
		}

		protected override EditModel ConstructCreateModel()
		{
			_populateCommentTypes = true;
			var model = new CreateModel
			{ 
				Date = DateTime.Now,
				DefaultPhoneId = (long)PhoneType.Home
			};
			return model;
		}

		protected override EditModel ConstructCreateModel(FormCollection form)
		{
			var model = (CreateModel)PopulateModelForValidation(new CreateModel(), form);
			TryUpdateModel(model);
			return model;
		}

		protected override EditModel ConstructUpdateModel(long id)
		{
			var model = base.ConstructUpdateModel(id);
			if (model.Inactive ?? false)
			{
				AddError(model, "Contacts.Inactive.Warning");
			}
			return model;
		}

		protected override void Validate(ContactEntity entity, EditModel model)
		{
			base.Validate(entity, model);
			try
			{
				int maxWidth = PreferenceService.GetPreference(CurrentAssociationId, ImageMaxWidthPath).IfNotNull(x => x.Value.ToInt32()) ?? DefaultImageMaxWidth;
				int maxHeight = PreferenceService.GetPreference(CurrentAssociationId, ImageMaxHeightPath).IfNotNull(x => x.Value.ToInt32()) ?? DefaultImageMaxHeight;
				_photoData = ImageUtil.GetPhotoData(Request, out _photoWidth, out _photoHeight, maxWidth, maxHeight);
			}
			catch (Exception)
			{
				AddPropertyValidationError(m => m.Photo, "Common.Error.Photo.Invalid");
			}
			if (!ModelState.IsValid && entity.Photo != null)
			{
				model.PhotoId = entity.Photo.Id;
			}
		}

		protected override ContactEntity SaveEntity(ContactEntity entity, EditModel model)
		{
			if (_photoData != null)
			{
				entity.Photo = new FileEntity
				{
					AssociationId = CurrentAssociationId,
					DataChanged = true,
					ContentType = "image/jpeg",
					DataCompressed = false,
					Filename = Guid.NewGuid() + ".jpg",
					FileData = new FileDataEntity { Data = _photoData }
				};
				entity.PhotoWidth = _photoWidth;
				entity.PhotoHeight = _photoHeight;
				if (entity.Id > 0)
				{
					HttpContext.Cache.Remove(ThumbnailCacheKeyFormat.FormatWith(entity.Id));
				}
			}
			if (entity.AssociationId <= 0)
			{
				entity.AssociationId = CurrentAssociationId;
			}
			var contact = ContactService.SaveContact(entity);
			if (model.CommentType.ToInt64().HasValue)
			{
				LookupService.SaveEntity(new ContactCommentEntity
				{
					ContactId = contact.Id,
					Date = model.Date.HasValue ? model.Date.Value : DateTime.Now,
					CommentType = LookupService.Get<ContactCommentTypeEntity>(model.CommentType.ToInt64().Value),
					Text = model.CommentText
				});
			}
			return contact;
		}

		protected override void DeleteEntity(long id)
		{
			ContactService.DeleteContact(id);
		}

		protected override EditModel ConstructModel(ContactEntity entity)
		{
			var model = Mapper.Map<EditModel>(entity);
			Mapper.Map(entity.Address, model);
			model.Age = entity.DateOfBirth.HasValue ? entity.DateOfBirth.Value.Age() : (int?)null;
			model.CanDelete = PermissionService.HasPermissionToSitePath(CurrentUser, DeletePath);
			return model;
		}

		protected override ContactEntity ConstructEntity(EditModel model)
		{
			var entity = model.Id == 0 ? new ContactEntity() : GetEntity(model.Id);
			if (entity.Address == null)
			{
				entity.Address = new AddressEntity();
			}
			Mapper.Map(model, entity);
			Mapper.Map(model, entity.Address);
			return entity;
		}

		protected override IEnumerable ConstructGridItems(IEnumerable<ContactEntity> source, ListModelBase model)
		{
			return source.Select(entity =>
			{
				var item = Mapper.Map<ListItem>(entity);
				Mapper.Map(entity.Address ?? new AddressEntity(), item);
				var flaggedCommentSearch = new SearchInfo<ContactCommentEntity>();
				flaggedCommentSearch.AddFilter(x => x.ContactId, entity.Id);
				flaggedCommentSearch.AddFilter(x => x.CommentType.Flagged, true);
				item.HasFlaggedComment = LookupService.GetEntityCount(flaggedCommentSearch) > 0;
				return item;
			});
		}

		#endregion
	}
}
