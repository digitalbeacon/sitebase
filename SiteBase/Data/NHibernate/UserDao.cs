// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Collections.Generic;
using DigitalBeacon.Data.NHibernate;
using DigitalBeacon.SiteBase.Model;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Criterion;
using System.Linq;
using DigitalBeacon.Util;

namespace DigitalBeacon.SiteBase.Data.NHibernate
{
	/// <summary>
	/// Data access object for users
	/// </summary>
	public class UserDao : BaseDao<UserEntity>, IUserDao
	{
		#region IUserDao Members

		public IList<UserEntity> FetchActive(long associationId)
		{
			return HibernateTemplate.ExecuteFind(
				session => (from x in session.Query<UserEntity>()
							where x.Deleted == null && x.Associations.Any(a => a.Id == associationId)
							select x).ToList());
		}

		public UserEntity FetchByUsername(string username)
		{
			return HibernateTemplate.Execute(
				session => (from x in session.Query<UserEntity>()
							where x.Deleted == null && x.Username == username
							select x).SingleOrDefault());
		}

		public UserEntity FetchByUsername(long associationId, string username)
		{
			return HibernateTemplate.Execute(
				session => (from x in session.Query<UserEntity>()
							where x.Deleted == null && x.Username == username && x.Associations.Any(a => a.Id == associationId)
							select x).SingleOrDefault());
		}

		public IList<UserEntity> FetchByRole(long associationId, Role role)
		{
			return HibernateTemplate.ExecuteFind(
				session => (from x in session.Query<UserEntity>()
							where x.Deleted == null
								  && x.Associations.Any(a => a.Id == associationId)
								  && x.Roles.Any(r => r.Role == role)
							select x).ToList());
		}

		public IList<UserEntity> FetchUsers(UserSearchInfo searchInfo)
		{
			return HibernateTemplate.ExecuteFind(
				session =>
					{
						var c = CreateSearchCriteria(session, searchInfo);
						ApplySorts(c, searchInfo);
						if (searchInfo.PageSize > 0 && searchInfo.Page > 0)
						{
							c.SetFirstResult((searchInfo.Page - 1)*searchInfo.PageSize);
							c.SetMaxResults(searchInfo.PageSize);
						}
						return c.List<UserEntity>();
					});
		}

		public int FetchUserCount(UserSearchInfo searchInfo)
		{
			return HibernateTemplate.Execute(
				session => CreateSearchCriteria(session, searchInfo)
							   .SetProjection(Projections.RowCount()).UniqueResult<int>());
		}

		private ICriteria CreateSearchCriteria(ISession session, UserSearchInfo searchInfo)
		{
			var c = session.CreateCriteria(typeof(UserEntity), DefaultAlias);
			ApplyFilters(c, searchInfo);
			c.Add(Restrictions.IsNull(UserEntity.DeletedProperty));
			c.CreateCriteria(UserEntity.AssociationsProperty)
				.Add(Restrictions.Eq(AssociationEntity.IdProperty, searchInfo.AssociationId));
			if (searchInfo.IsAdmin.HasValue)
			{
				var dcAdminRole = DetachedCriteria.For(typeof(UserRoleEntity))
					.SetProjection(Projections.Constant(1))
					.Add(Restrictions.EqProperty(GetIdProperty(UserRoleEntity.UserProperty), GetIdProperty(DefaultAlias)))
					.Add(Restrictions.Eq(UserRoleEntity.RoleProperty, Role.Administrator));
				if (searchInfo.IsAdmin.Value)
				{
					c.Add(Subqueries.Exists(dcAdminRole));
				}
				else
				{
					c.Add(Subqueries.NotExists(dcAdminRole));
				}
			}
			if (searchInfo.SearchText.HasText())
			{
				c.Add(Restrictions.Or(
					Restrictions.Like(UserEntity.EmailProperty, searchInfo.SearchText, MatchMode.Anywhere),
					Restrictions.Or(
						Restrictions.Like(UserEntity.UsernameProperty, searchInfo.SearchText, MatchMode.Anywhere),
						Restrictions.Like(UserEntity.DisplayNameProperty, searchInfo.SearchText, MatchMode.Anywhere))));
			}
			else
			{
				if (searchInfo.Username.HasText())
				{
					c.Add(Restrictions.Like(UserEntity.UsernameProperty, searchInfo.Username, MatchMode.Anywhere));
				}
				if (searchInfo.Email.HasText())
				{
					c.Add(Restrictions.Like(UserEntity.EmailProperty, searchInfo.Email, MatchMode.Anywhere));
				}
				if (searchInfo.LastName.HasText() || searchInfo.FirstName.HasText())
				{
					var c2 = c.CreateCriteria(UserEntity.PersonProperty);
					if (searchInfo.LastName.HasText())
					{
						c2.Add(Restrictions.Like(PersonEntity.LastNameProperty, searchInfo.LastName, MatchMode.Anywhere));
					}
					if (searchInfo.FirstName.HasText())
					{
						c2.Add(Restrictions.Like(PersonEntity.FirstNameProperty, searchInfo.FirstName, MatchMode.Anywhere));
					}
				}
			}
			return c;
		}

		#endregion
	}
}