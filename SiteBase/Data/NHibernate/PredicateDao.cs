// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DigitalBeacon.Data.NHibernate;
using DigitalBeacon.SiteBase.Model;
using NHibernate;
using NHibernate.Criterion;
using DigitalBeacon.Model;

namespace DigitalBeacon.SiteBase.Data.NHibernate
{
	public class PredicateDao : BaseDao<PredicateEntity>
	{
		#region Public Methods

		public static ICriteria AddPredicates(IList<PredicateEntity> predicates, ICriteria criteria)
		{
			int currentGrouping = -1;
			ICriterion currentGroupingExpression = null;
			foreach (PredicateEntity p in predicates.OrderBy(x => x.Grouping))
			{
				ICriterion expression = null;
				Order order = null;
				switch ((ComparisonOperator)p.OperatorId)
				{
					case ComparisonOperator.Equal:
						expression = Expression.Eq(p.Field, p.Value);
						break;
					case ComparisonOperator.NotEqual:
						expression = Expression.Not(Expression.Eq(p.Field, p.Value));
						break;
					case ComparisonOperator.Null:
						expression = Expression.IsNull(p.Field);
						break;
					case ComparisonOperator.NotNull:
						expression = Expression.IsNotNull(p.Field);
						break;
					case ComparisonOperator.LessThan:
						expression = Expression.Lt(p.Field, p.Value);
						break;
					case ComparisonOperator.LessThanOrEqual:
						expression = Expression.Le(p.Field, p.Value);
						break;
					case ComparisonOperator.GreaterThan:
						expression = Expression.Gt(p.Field, p.Value);
						break;
					case ComparisonOperator.GreaterThanOrEqual:
						expression = Expression.Ge(p.Field, p.Value);
						break;
					case ComparisonOperator.Contains:
						expression = Expression.Like(p.Field, (string)p.Value, MatchMode.Anywhere);
						break;
					case ComparisonOperator.StartsWith:
						expression = Expression.Like(p.Field, (string)p.Value, MatchMode.Start);
						break;
					case ComparisonOperator.EndsWith:
						expression = Expression.Like(p.Field, (string)p.Value, MatchMode.End);
						break;
					case ComparisonOperator.In:
						expression = Expression.In(p.Field, (ICollection)p.Value);
						break;
					case ComparisonOperator.SortAscending:
						order = Order.Asc(p.Field);
						break;
					case ComparisonOperator.SortDescending:
						order = Order.Desc(p.Field);
						break;
				}
				if (p.Grouping == 0)
				{
					criteria.Add(expression);
				}
				else
				{
					if (p.Grouping != currentGrouping)
					{
						if (currentGrouping > -1 && currentGroupingExpression != null)
						{
							criteria.Add(currentGroupingExpression);
							currentGroupingExpression = null;
						}
						currentGrouping = p.Grouping;
					}
					if (currentGroupingExpression == null)
					{
						currentGroupingExpression = expression;
					}
					else
					{
						currentGroupingExpression = Expression.Or(currentGroupingExpression, expression);
					}
				}
				if (order != null)
				{
					criteria.AddOrder(order);
				}
			}
			if (currentGroupingExpression != null)
			{
				criteria.Add(currentGroupingExpression);
			}
			return criteria;
		}

		#endregion
	}
}