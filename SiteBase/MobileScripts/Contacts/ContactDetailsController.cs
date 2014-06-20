// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Browser;
using System.Dom;
using System.Html;
using DigitalBeacon;
using DigitalBeacon.SiteBase;
using DigitalBeacon.SiteBase.Mobile;
using jQueryLib;
using ng;
using ng.ui.router;

namespace DigitalBeacon.SiteBase.Mobile.Contacts
{
	public class ContactDetailsController : BaseDetailsController
	{
		private ContactService _contactService;

		protected new ContactDetailsScopeData ScopeData
		{
			get { return (ContactDetailsScopeData)data; }
		}

		public ContactDetailsController(Scope scope, State state, ILocation location, ContactService contactService) : base(scope, state, location)
		{
			_contactService = contactService;
		}

		protected override void init()
		{
			base.init();
			jQuery.extend(data, new ContactDetailsScopeData 
			{ 
				comment = new { },
				isCollapsedCommentEditor = true
			});
			//Scope.watch("data.isCollapsedComments", new Action<object>(arg =>
			//{
			//	console.log("data.isCollapsedComments: " + arg);
			//}));
		}

		protected override void handleResponse(ApiResponse response)
		{
			ScopeData.isOpenPhotoActions = false;
			if (response.Success)
			{
				detailsChanged();
			}
			base.handleResponse(response);
		}

		protected override void load(string id)
		{
			ScopeData.model = _contactService.get(id, createHandler(response =>
			{
				if (response.Id)
				{
					ScopeData.photoUrl = digitalbeacon.resolveUrl("~/contacts/{0}/photo?x={1}".formatWith((object)response.Id, (object)response.PhotoId));
				}
			}));
			getComments(id);
		}

		protected override void submit(string modelName)
		{
			if (modelName == "comment")
			{
				saveComment();
			}
			else
			{
				save();
			}
		}

		protected override void save()
		{
			if (HasFileInput)
			{
				_contactService.saveWithFileData(ScopeData.model.Id, ScopeData.model, Files, SaveHandler);
			}
			else
			{
				_contactService.save(ScopeData.model.Id, ScopeData.model, SaveHandler);
			}
		}

		[ScriptName("$delete")] // work-around for issue with minification
		public override void delete()
		{
			if (ScopeData.model.Id && window.confirm(localization.confirmText))
			{
				detailsChanged();
				_contactService.delete(ScopeData.model.Id, ReturnToList);
			}
		}

		public void deletePhoto()
		{
			if (ScopeData.model.Id && window.confirm(localization.confirmText))
			{
				_contactService.deletePhoto(ScopeData.model.Id, ResponseHandler);
			}
		}

		public void rotatePhotoCounterclockwise()
		{
			if (ScopeData.model.Id && window.confirm(localization.confirmText))
			{
				_contactService.rotatePhotoCounterclockwise(ScopeData.model.Id, ResponseHandler);
			}
		}

		public void rotatePhotoClockwise()
		{
			if (ScopeData.model.Id && window.confirm(localization.confirmText))
			{
				_contactService.rotatePhotoClockwise(ScopeData.model.Id, ResponseHandler);
			}
		}

		private void getComments(string contactId, bool setChanged = false)
		{
			if (RouterState.@is("list.display"))
			{
				_contactService.getComments(contactId,
					createHandler(response =>
					{
						ScopeData.comments = response.Data.Items;
						// need to wait for comments to be bound so the scroll height is avaiable to the collapse directive
						window.setTimeout(new Action(() =>
							{
								ScopeData.isCollapsedCommentEditor = true;
								Scope.apply();
							}), 0);
					}));
			}
			if (setChanged)
			{
				detailsChanged();
			}
		}

		public void newComment()
		{
			ScopeData.comment = new { };
			resetForm("commentEditPanel", "comment");
			ScopeData.isCollapsedCommentEditor = false;
		}

		public void editComment(string commentId)
		{
			ScopeData.comment = _contactService.getComment(commentId,
				new Action<dynamic>(response =>
				{
					resetForm("commentEditPanel", "comment");
					ScopeData.isCollapsedCommentEditor = false;
				}));
		}

		private void saveComment()
		{
			ScopeData.comment.ParentId = ScopeData.model.Id;
			_contactService.saveComment(ScopeData.comment.Id, ScopeData.comment, createHandler(response => getComments(ScopeData.model.Id, true)));
		}

		public void cancelComment()
		{
			ScopeData.isCollapsedCommentEditor = true;
		}

		public void deleteComment(string commentId)
		{
			if (ScopeData.model.Id && commentId && window.confirm(localization.confirmText))
			{
				detailsChanged();
				_contactService.deleteComment(commentId, createHandler(response => getComments(ScopeData.model.Id)));
			}
		}

		[ScriptObjectLiteral]
		public class ContactDetailsScopeData : BaseScopeData
		{
			public string photoUrl;
			public dynamic comment;
			public object[] comments;
			public bool isOpenPhotoActions;
			public bool isCollapsedComments;
			public bool isCollapsedCommentEditor;
		}
	}
}
