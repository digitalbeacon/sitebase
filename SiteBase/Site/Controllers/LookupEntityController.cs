// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Web;
using DigitalBeacon.SiteBase.Web.Models;
using DigitalBeacon.Util;

namespace DigitalBeacon.SiteBase.Controllers
{
	[Authorization(Role.Administrator)]
	public class LookupEntityController<T> : NamedEntityController<T, LookupEntityModel<T>> where T : class, INamedEntity, new()
	{
		protected bool UseDisplayOrderForInactive { get; set; }
		public bool ShowDisabledCheckBox { get; set; }

		public LookupEntityController()
		{
			UseDisplayOrderForInactive = true;
			if (ConstructModel(new T()).UseDisplayOrder)
			{
				DefaultSort = new[] 
				{ 
					new SortItem { Member = BaseEntity.DisplayOrderProperty }, 
					new SortItem { Member = BaseEntity.NameProperty } 
				};
			}
			else
			{
				DefaultSort = new[] 
				{ 
					new SortItem { Member = BaseEntity.NameProperty } 
				};
			}
		}

		#region EntityController

		protected override string ListView
		{
			get { return "LookupList"; }
		}

		protected override string EditView
		{
			get { return "LookupEdit"; }
		}

		protected override string DisplayView
		{
			get { return "LookupEdit"; }
		}

		protected override ListModelBase ConstructListModel()
		{
			var model = new LookupListModel<T>();
			model.SupportsInactive = UseDisplayOrderForInactive;
			return model;
		}

		protected override sealed LookupEntityModel<T> ConstructModel(T entity)
		{
			var model = new LookupEntityModel<T>
			{
				Id = entity.Id,
				Name = entity.Name
			};
			if (model.IsCoded)
			{
				model.Code = ((ICodedEntity)entity).Code;
			}
			if (model.UseDisplayOrder)
			{
				model.DisplayOrder = (int?)model.DisplayOrderPropertyInfo.GetValue(entity, null);
			}
			if (model.SupportsComments)
			{
				model.Comment = (string)model.CommentPropertyInfo.GetValue(entity, null);
			}
			model.Inactive = model.DisplayOrder.HasValue && model.DisplayOrder == 0;
			model.SupportsInactive = model.UseDisplayOrder && UseDisplayOrderForInactive;
			return model;
		}

		protected override T ConstructEntity(LookupEntityModel<T> model)
		{
			var entity = model.Id == 0 ? new T() : LookupService.Get<T>(model.Id);
			entity.Name = model.Name;
			if (model.IsCoded)
			{
				((ICodedEntity)entity).Code = model.Code;
			}
			if (model.UseDisplayOrder)
			{
				model.DisplayOrderPropertyInfo.SetValue(entity, model.DisplayOrder ?? 1, null);
			}
			if (model.SupportsComments)
			{
				model.CommentPropertyInfo.SetValue(entity, model.Comment, null);
			}
			return entity;
		}

		protected override IEnumerable ConstructGridItems(IEnumerable<T> source, ListModelBase listModel)
		{
			var model = ConstructModel(new T());
			return source.Select(entity => new LookupInfo { 
				Id = entity.Id, 
				Name = entity.Name, 
				Code = model.IsCoded ? ((ICodedEntity)entity).Code : null,
				DisplayOrder = model.UseDisplayOrder ? (int?)model.DisplayOrderPropertyInfo.GetValue(entity, null) : (int?)null
			});
		}

		#endregion
	}
}
