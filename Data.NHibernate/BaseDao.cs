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
using System.Linq;
using DigitalBeacon.Model;
using DigitalBeacon.Util;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Type;
using Spring.Data.NHibernate.Generic.Support;


namespace DigitalBeacon.Data.NHibernate
{
	/// <summary>
	/// a base class for data access objects using NHibernate persistence
	/// </summary>
	public class BaseDao<T> : HibernateDaoSupport, IDao<T>  where T:class, IBaseEntity, new()
	{
		//private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		protected const string DefaultAlias = "entity";
		protected static readonly string FromClause = "from " + typeof(T).Name + " as x";

		#region IDao<T> Members

		public virtual T Fetch(long id)
		{
			return HibernateTemplate.Get<T>(id);
		}

		public virtual T Fetch(string propertyName, object value)
		{
			return HibernateTemplate.Execute(session => session.CreateCriteria(typeof(T)).Add(Restrictions.Eq(propertyName, value)).UniqueResult<T>());
		}

		public virtual T FetchWithLazyCollections(long id)
		{
			T entity = Fetch(id);
			entity.AreLazyCollectionsInitialized = true;
			return entity;
		}

		public virtual IList<T> FetchAll()
		{
			return HibernateTemplate.LoadAll<T>();
		}

		public virtual IList<T> FetchAll(string sortByPropertyName)
		{
			return FetchAll(sortByPropertyName, ListSortDirection.Ascending);
		}

		public virtual IList<T> FetchAll(string sortByPropertyName, ListSortDirection sortDirection)
		{
			return HibernateTemplate.ExecuteFind(session =>
			{
				var c = session.CreateCriteria(typeof(T));
				if (sortDirection == ListSortDirection.Ascending)
				{
					c.AddOrder(Order.Asc(sortByPropertyName));
				}
				else
				{
					c.AddOrder(Order.Desc(sortByPropertyName));
				}
				return c.List<T>();
			});
		}

		public virtual IList<T> FetchList(string propertyName, object value)
		{
			return HibernateTemplate.ExecuteFind(session => session.CreateCriteria(typeof(T)).Add(Restrictions.Eq(propertyName, value)).List<T>());
		}

		public virtual IList<T> FetchList(string propertyName, object value, string sortByPropertyName, bool asc)
		{
			return HibernateTemplate.ExecuteFind(session =>
			{
				var c = session.CreateCriteria(typeof(T)).Add(Restrictions.Eq(propertyName, value));
				if (asc)
				{
					c.AddOrder(Order.Asc(sortByPropertyName));
				}
				else
				{
					c.AddOrder(Order.Desc(sortByPropertyName));
				}
				return c.List<T>();
			});
		}

		public virtual IList<T> FetchList(SearchInfo<T> searchInfo)
		{
			return HibernateTemplate.ExecuteFind(session =>
			{
				var c = session.CreateCriteria(typeof(T), DefaultAlias);
				ApplyFilters(c, searchInfo);
				ApplySorts(c, searchInfo);
				if (searchInfo.PageSize > 0 && searchInfo.Page > 0)
				{
					c.SetFirstResult((searchInfo.Page - 1)*searchInfo.PageSize);
					c.SetMaxResults(searchInfo.PageSize);
				}
				return c.List<T>();
			});
		}

		public virtual long FetchCount(SearchInfo<T> searchInfo)
		{
			return HibernateTemplate.Execute(session =>
			{
				var c = session.CreateCriteria(typeof(T), DefaultAlias);
				ApplyFilters(c, searchInfo);
				return c.SetProjection(Projections.RowCountInt64()).UniqueResult<long>();
			});
		}

		public virtual T Save(T entity)
		{
			var retVal = HibernateTemplate.SaveOrUpdateCopy(entity) as T;
			if (entity.IsNew)
			{
				HibernateTemplate.Flush();
				if (retVal != null)
				{
					entity.Id = retVal.Id;
				}
			}
			return retVal;
		}

		public virtual void Delete(long id, long version)
		{
			Delete(version < 0 ? Fetch(id) : new T { Id = id, ModificationCounter = version });
		}

		public virtual void Delete(long id)
		{
			Delete(id, -1);
		}

		public virtual void Delete(T entity)
		{
			HibernateTemplate.Delete(entity);
			HibernateTemplate.Flush();
		}

		public virtual void Initialize(T entity, string propertyName)
		{
			HibernateTemplate.Execute(
				delegate(ISession session)
				{
					session.Lock(entity, LockMode.Read);
					var t = entity.GetType();
					var pi = t.GetProperty(propertyName);
					if (pi == null)
					{
						throw new Exception(String.Format("Could not find property [{0}] for type [{1}].", propertyName, entity.GetType().Name));
					}
					var prop = pi.GetValue(entity, null);
					NHibernateUtil.Initialize(prop);
					return entity;
				});
		}

		public virtual void Evict(object entity)
		{
			HibernateTemplate.Evict(entity);
		}

		#endregion

		protected virtual void Delete(string propertyName, object value, IType type)
		{
			HibernateTemplate.Delete(String.Format("{0} where x.{1} = ?", FromClause, propertyName), value, type);
		}

		protected static string GetPropertyName(string property, string subProperty)
		{
			return String.Format("{0}.{1}", property, subProperty);
		}

		protected static string GetIdProperty(string property)
		{
			return String.Format("{0}.{1}", property, BaseEntity.IdProperty);
		}

		protected virtual ICriteria ApplyFilters(ICriteria c, SearchInfo<T> searchInfo)
		{
			if (searchInfo.Filters != null && searchInfo.Filters.Count > 0)
			{
				var currentGrouping = Int32.MaxValue;
				ICriterion groupedCriteria = null;
				ICriterion currentGroupingExpression = null;
				foreach (var f in searchInfo.Filters.OrderBy(x => x.Grouping))
				{
					ICriterion expression = null;
					//Order order = null;
					switch (f.Operator)
					{
						case ComparisonOperator.Equal:
							expression = Restrictions.Eq(f.Member, f.Value);
							break;
						case ComparisonOperator.NotEqual:
							expression = Restrictions.Not(Restrictions.Eq(f.Member, f.Value));
							break;
						case ComparisonOperator.Null:
							expression = Restrictions.IsNull(f.Member);
							break;
						case ComparisonOperator.NotNull:
							expression = Restrictions.IsNotNull(f.Member);
							break;
						case ComparisonOperator.LessThan:
							expression = Restrictions.Lt(f.Member, f.Value);
							break;
						case ComparisonOperator.LessThanOrEqual:
							expression = Restrictions.Le(f.Member, f.Value);
							break;
						case ComparisonOperator.GreaterThan:
							expression = Restrictions.Gt(f.Member, f.Value);
							break;
						case ComparisonOperator.GreaterThanOrEqual:
							expression = Restrictions.Ge(f.Member, f.Value);
							break;
						case ComparisonOperator.Contains:
							expression = Restrictions.Like(f.Member, (string)f.Value, MatchMode.Anywhere);
							break;
						case ComparisonOperator.StartsWith:
							expression = Restrictions.Like(f.Member, (string)f.Value, MatchMode.Start);
							break;
						case ComparisonOperator.EndsWith:
							expression = Restrictions.Like(f.Member, (string)f.Value, MatchMode.End);
							break;
						case ComparisonOperator.In:
							expression = Restrictions.In(f.Member, (ICollection)f.Value);
							break;
					}
					if (expression == null)
					{
						throw new DataException("Unexpected filter item: {0}", f.ToJson());
					}
					EnsureAliasForMember(c, f.Member);
					if (f.Grouping == 0)
					{
						c.Add(expression);
					}
					else
					{
						if (f.Grouping != currentGrouping)
						{
							if (currentGroupingExpression != null)
							{
								if (currentGrouping > 0)
								{
									groupedCriteria = groupedCriteria == null ? currentGroupingExpression : Restrictions.Or(groupedCriteria, currentGroupingExpression);
								}
								else
								{
									c.Add(currentGroupingExpression);
								}
								currentGroupingExpression = null;
							}
							currentGrouping = f.Grouping;
						}
						currentGroupingExpression = currentGroupingExpression == null ? expression : 
							(currentGrouping > 0 
								? Restrictions.And(currentGroupingExpression, expression)
								: Restrictions.Or(currentGroupingExpression, expression));
					}
				}
				if (currentGroupingExpression != null)
				{
					if (currentGrouping > 0)
					{
						groupedCriteria = groupedCriteria == null ? currentGroupingExpression : Restrictions.Or(groupedCriteria, currentGroupingExpression);
					}
					else
					{
						c.Add(currentGroupingExpression);
					}
				}
				if (groupedCriteria != null)
				{
					c.Add(groupedCriteria);
				}
			}
			return c;
		}

		protected virtual ICriteria ApplySorts(ICriteria c, SearchInfo<T> searchInfo)
		{
			if (searchInfo.Sorts != null && searchInfo.Sorts.Count > 0)
			{
				foreach (var sort in searchInfo.Sorts)
				{
					EnsureAliasForMember(c, sort.Member);
					if (sort.SortDirection == ListSortDirection.Ascending)
					{
						c.AddOrder(Order.Asc(sort.Member));
					}
					else
					{
						c.AddOrder(Order.Desc(sort.Member));
					}
				}
			}
			return c;
		}

		private static void EnsureAliasForMember(ICriteria c, string member)
		{
			if (member.IndexOf('.') <= 0)
			{
				return;
			}
			var parts = member.Split('.');
			if (parts.Length > 3 || (parts.Length == 3 && parts[2] != BaseEntity.IdProperty))
			{
				throw new DataException("Member cannot be more than two level deep.");
			}
			if (c.GetCriteriaByAlias(parts[0]) == null)
			{
				c.CreateAlias(parts[0], parts[0], JoinType.LeftOuterJoin);
			}
		}
	}
}
