// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2010-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using DigitalBeacon.Data.NHibernate;
using DigitalBeacon.Model;
using DigitalBeacon.SiteBase.Model.Contacts;
using DigitalBeacon.Util;
using NHibernate;
using NHibernate.Criterion;

namespace DigitalBeacon.SiteBase.Data.Contacts.NHibernate
{
	public class ContactDao : BaseDao<ContactEntity>
	{
		#region BaseDao Overrides

		protected override ICriteria ApplyFilters(ICriteria c, SearchInfo<ContactEntity> searchInfo)
		{
			base.ApplyFilters(c, searchInfo);

			if (!(searchInfo is ContactSearchInfo))
			{
				return c;
			}

			var clientSearch = (ContactSearchInfo)searchInfo;
			if (clientSearch.Inactive.HasValue)
			{
				c.Add(Restrictions.Eq(ContactEntity.EnabledProperty, !clientSearch.Inactive.Value));
			}
			if (clientSearch.ContactId.HasValue)
			{
				c.Add(Restrictions.Eq(ContactEntity.IdProperty, clientSearch.ContactId.Value));
			}
			if (clientSearch.CommentTypeId.HasValue)
			{
				c.Add(Subqueries.Exists(
					DetachedCriteria.For(typeof(ContactCommentEntity))
						.Add(Restrictions.EqProperty(ContactCommentEntity.ContactIdProperty, GetIdProperty(DefaultAlias)))
						.Add(Restrictions.Eq(GetIdProperty(ContactCommentEntity.CommentTypeProperty), clientSearch.CommentTypeId.Value))
						.SetProjection(Projections.Constant(true))));
			}
			if (clientSearch.HasFlaggedComment ?? false)
			{
				c.Add(Subqueries.Exists(
					DetachedCriteria.For(typeof(ContactCommentEntity))
						.CreateAlias(ContactCommentEntity.CommentTypeProperty, ContactCommentEntity.CommentTypeProperty)
						.Add(Restrictions.EqProperty(ContactCommentEntity.ContactIdProperty, GetIdProperty(DefaultAlias)))
						.Add(Restrictions.Eq(GetPropertyName(ContactCommentEntity.CommentTypeProperty, ContactCommentTypeEntity.FlaggedProperty), true))
						.SetProjection(Projections.Constant(true))));
			}
			if (clientSearch.SearchText.HasText())
			{
				var searchTextCriterion = Restrictions.Or(
					Restrictions.Like(ContactEntity.FirstNameProperty, clientSearch.SearchText, MatchMode.Anywhere),
					Restrictions.Like(ContactEntity.LastNameProperty, clientSearch.SearchText, MatchMode.Anywhere));
				// search comments
				searchTextCriterion = Restrictions.Or(
					searchTextCriterion,
					Subqueries.Exists(DetachedCriteria.For(typeof(ContactCommentEntity))
						.Add(Restrictions.EqProperty(ContactCommentEntity.ContactIdProperty, GetIdProperty(DefaultAlias)))
						.Add(Restrictions.Like(ContactCommentEntity.TextProperty, clientSearch.SearchText, MatchMode.Anywhere))
						.SetProjection(Projections.Constant(true))));
				c.Add(searchTextCriterion);
			}

			return c;
		}

		#endregion
	}
}