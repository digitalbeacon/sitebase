// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using System.Web.Mvc;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Web.Models;
using DigitalBeacon.Util;

namespace DigitalBeacon.SiteBase.Controllers
{
	public abstract class NamedEntityController<TEntity, TModel> : EntityController<TEntity, TModel> where TEntity : class, INamedEntity, new() where TModel : NamedEntityModel, new()
	{
		private const string AssociationIdProperty = "AssociationId";

		protected bool BuiltInEntitiesMutable { get; set; }

		protected NamedEntityController()
		{
			DefaultSort = new[] { new SortItem { Member = BaseEntity.NameProperty } };
		}

		protected bool IsCoded
		{
			get { return typeof(ICodedEntity).IsAssignableFrom(typeof(TEntity)); }
		}

		protected override string GetDescription(TModel model)
		{
			return model.Name;
		}

		protected override void Validate(TEntity entity, TModel model)
		{
			if (!entity.IsNew
				&& !BuiltInEntitiesMutable 
				&& entity.HasProperty<long?>(AssociationIdProperty) 
				&& entity.GetPropertyValue<long?>(AssociationIdProperty) == null)
			{
				ModelState.AddModelError(String.Empty, "Common.Error.ManageBuiltIn.Denied");
				ValidationRedirect = true;
				return;
			}
			if (!IsNameUnique(entity.Id, entity.Name))
			{
				AddPropertyValidationError(BaseEntity.NameProperty, "Error.Name.Duplicate");
			}
			if (IsCoded)
			{
				if (((ICodedEntity)entity).Code.IsNullOrBlank())
				{
					AddPropertyValidationError(BaseEntity.CodeProperty, "Validation.Error.Required", GetLocalizedText("Common.Code.Label"));
				}
				else if (!IsCodeUnique(entity.Id, ((ICodedEntity)entity).Code))
				{
					AddPropertyValidationError(BaseEntity.CodeProperty, "Error.Code.Duplicate");
				}
			}
		}

		protected override void ValidateForDelete(TEntity entity, TModel model)
		{
			if (!BuiltInEntitiesMutable
				&& entity.HasProperty<long?>(AssociationIdProperty)
				&& entity.GetPropertyValue<long?>(AssociationIdProperty) == null)
			{
				ModelState.AddModelError(String.Empty, "Common.Error.ManageBuiltIn.Denied");
				ValidationRedirect = true;
				return;
			}
			base.ValidateForDelete(entity, model);
		}

		protected override IEnumerable<TEntity> GetEntities(SearchInfo<TEntity> searchInfo, ListModelBase model)
		{
			return LookupService.GetEntityList(CurrentAssociationId, searchInfo);
		}

		protected override long GetEntityCount(SearchInfo<TEntity> searchInfo, ListModelBase model)
		{
			return LookupService.GetEntityCount(CurrentAssociationId, searchInfo);
		}

		protected override TEntity GetEntity(long id)
		{
			return LookupService.Get<TEntity>(id);
		}

		protected virtual TEntity GetEntityByName(string name)
		{
			return LookupService.GetByName<TEntity>(CurrentAssociationId, name);
		}

		protected virtual TEntity GetEntityByCode(string code)
		{
			if (!IsCoded)
			{
				throw new InvalidOperationException("{0} is not a coded entity".FormatWith(typeof(TEntity).Name));
			}
			return LookupService.GetByCode<TEntity>(CurrentAssociationId, code);
		}

		protected override TEntity SaveEntity(TEntity entity, TModel model)
		{
			return LookupService.SaveEntity(CurrentAssociationId, entity);
		}

		protected override void DeleteEntity(long id)
		{
			LookupService.DeleteEntity<TEntity>(id);
		}

		protected bool IsNameUnique(long id, string name)
		{
			var entity = GetEntityByName(name);
			return entity == null || entity.Id == id;
		}

		protected bool IsCodeUnique(long id, string code)
		{
			var entity = GetEntityByCode(code);
			return entity == null || entity.Id == id;
		}

		/// <summary>
		/// Validates the name property.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		public ActionResult ValidateName(long id, string name)
		{
			return Json(IsNameUnique(id, name), JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// Validates the code property.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="code">The code.</param>
		/// <returns></returns>
		public ActionResult ValidateCode(long id, string code)
		{
			return Json((!IsCoded || IsCodeUnique(id, code)), JsonRequestBehavior.AllowGet);
		}
	}
}
