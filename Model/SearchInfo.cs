// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using DigitalBeacon.Util;
using System.Linq.Expressions;

namespace DigitalBeacon.Model
{
	/// <summary>
	/// A struct used to aggregate search parameters
	/// </summary>
	public class SearchInfo<T> where T : IBaseEntity
	{
		private bool _applyDefaultFilters = true;
		private bool _applyDefaultSorting = true;
		private bool _localize = true;

		/// <summary>
		/// Gets or sets the size of the page.
		/// </summary>
		/// <value>The size of the page.</value>
		public int PageSize { get; set; }

		/// <summary>
		/// Gets or sets the page.
		/// </summary>
		/// <value>The page.</value>
		public int Page { get; set; }

		/// <summary>
		/// Gets or sets the filters.
		/// </summary>
		/// <value>The filters.</value>
		public IList<FilterItem> Filters { get; set; }

		/// <summary>
		/// Gets or sets the sorts.
		/// </summary>
		/// <value>The sorts.</value>
		public IList<SortItem> Sorts { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether default filters should be applied.
		/// </summary>
		/// <value><c>true</c> if default filters should be applied; otherwise, <c>false</c>.</value>
		public bool ApplyDefaultFilters
		{
			get { return _applyDefaultFilters; }
			set { _applyDefaultFilters = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether default sorting should be applied.
		/// </summary>
		/// <value><c>true</c> if default sorting should be applied; otherwise, <c>false</c>.</value>
		public bool ApplyDefaultSorting
		{
			get { return _applyDefaultSorting; }
			set { _applyDefaultSorting = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether returned entities should be localized.
		/// </summary>
		/// <value><c>true</c> if returned entities should be localized; otherwise, <c>false</c>.</value>
		public bool Localize
		{
			get { return _localize; }
			set { _localize = value; }
		}

		#region Common Search Properties

		/// <summary>
		/// Gets or sets the search text.
		/// </summary>
		/// <value>The search text.</value>
		public string SearchText { get; set; }

		/// <summary>
		/// Gets or sets the association id.
		/// </summary>
		/// <value>The association id.</value>
		public long AssociationId { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether null associations should be matched.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if null associations should be matched; otherwise, <c>false</c>.
		/// </value>
		public bool MatchNullAssociations { get; set; }

		/// <summary>
		/// Gets or sets the parent id.
		/// </summary>
		/// <value>The parent id.</value>
		public long? ParentId { get; set; }

		#endregion

		/// <summary>
		/// Adds the predicate.
		/// </summary>
		/// <param name="member">The member.</param>
		/// <param name="op">The op.</param>
		/// <returns></returns>
		public FilterItem AddFilter(string member, ComparisonOperator op)
		{
			return AddFilter(member, op, null);
		}

		/// <summary>
		/// Gets the min date for search text.
		/// </summary>
		/// <value>The min date for search text.</value>
		public DateTime? MinDateForSearchText
		{
			get { return SearchText.ToDate(); } 
		}

		/// <summary>
		/// Gets the max date for search text.
		/// </summary>
		/// <value>The max date for search text.</value>
		public DateTime? MaxDateForSearchText
		{
			get
			{
				var minDate = SearchText.ToDate();
				if (minDate.HasValue)
				{
					if (minDate.Value.Hour == 0 && minDate.Value.Minute == 0 && minDate.Value.Second == 0)
					{
						return minDate.Value.AddDays(1).AddMilliseconds(-1);
					}
					if (minDate.Value.Minute == 0 && minDate.Value.Second == 0)
					{
						return minDate.Value.AddHours(1).AddMilliseconds(-1);
					}
					if (minDate.Value.Second == 0)
					{
						return minDate.Value.AddMinutes(1).AddMilliseconds(-1);
					}
					return minDate.Value.AddSeconds(1).AddMilliseconds(-1);
				}
				return null;
			}
		}

		/// <summary>
		/// Adds the predicate.
		/// </summary>
		/// <typeparam name="TProperty">The type of the property.</typeparam>
		/// <param name="memberExpression">The member expression.</param>
		/// <param name="op">The op.</param>
		/// <returns></returns>
		public FilterItem AddFilter<TProperty>(Expression<Func<T, TProperty>> memberExpression, ComparisonOperator op)
		{
			return AddFilter(GetMemberFromExpression(memberExpression), op, null, false);
		}

		/// <summary>
		/// Adds the predicate.
		/// </summary>
		/// <param name="member">The member.</param>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public FilterItem AddFilter(string member, object value)
		{
			return AddFilter(member, ComparisonOperator.Equal, value);
		}

		/// <summary>
		/// Adds the predicate.
		/// </summary>
		/// <param name="memberExpression">The memberExpression.</param>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public FilterItem AddFilter<TProperty>(Expression<Func<T, TProperty>> memberExpression, object value)
		{
			return AddFilter(GetMemberFromExpression(memberExpression), ComparisonOperator.Equal, value, false);
		}

		/// <summary>
		/// Adds the predicate.
		/// </summary>
		/// <param name="member">The member.</param>
		/// <param name="op">The op.</param>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public FilterItem AddFilter(string member, ComparisonOperator op, object value)
		{
			return AddFilter(member, op, value, true);
		}

		/// <summary>
		/// Adds the predicate.
		/// </summary>
		/// <param name="memberExpression">The memberExpression.</param>
		/// <param name="op">The op.</param>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public FilterItem AddFilter<TProperty>(Expression<Func<T, TProperty>> memberExpression, ComparisonOperator op, object value)
		{
			return AddFilter(GetMemberFromExpression(memberExpression), op, value, false);
		}

		/// <summary>
		/// Gets the filter.
		/// </summary>
		/// <param name="member">The member.</param>
		/// <returns></returns>
		public FilterItem GetFilter(string member)
		{
			FilterItem retVal = null;
			if (Filters != null && Filters.Count > 0)
			{
				retVal = Filters.Where(x => x.Member == member).FirstOrDefault();
			}
			return retVal;
		}

		/// <summary>
		/// Gets the filter.
		/// </summary>
		/// <param name="memberExpression">The member expression.</param>
		/// <returns></returns>
		public FilterItem GetFilter<TProperty>(Expression<Func<T, TProperty>> memberExpression)
		{
			return GetFilter(GetMemberFromExpression(memberExpression));
		}

		/// <summary>
		/// Adds the sort.
		/// </summary>
		/// <param name="member">The member.</param>
		public SortItem AddSort(string member)
		{
			return AddSort(member, ListSortDirection.Ascending, true);
		}

		/// <summary>
		/// Adds the sort.
		/// </summary>
		/// <param name="member">The member.</param>
		/// <param name="sortDirection">The sort direction.</param>
		public SortItem AddSort(string member, ListSortDirection sortDirection)
		{
			return AddSort(member, sortDirection, true);
		}

		/// <summary>
		/// Adds the sort.
		/// </summary>
		/// <typeparam name="TProperty">The type of the property.</typeparam>
		/// <param name="memberExpression">The member expression.</param>
		/// <returns></returns>
		public SortItem AddSort<TProperty>(Expression<Func<T, TProperty>> memberExpression)
		{
			return AddSort(GetMemberFromExpression(memberExpression), ListSortDirection.Ascending, false);
		}

		/// <summary>
		/// Adds the sort.
		/// </summary>
		/// <param name="memberExpression">The member expression.</param>
		/// <param name="sortDirection">The sort direction.</param>
		/// <returns></returns>
		public SortItem AddSort<TProperty>(Expression<Func<T, TProperty>> memberExpression, ListSortDirection sortDirection)
		{
			return AddSort(GetMemberFromExpression(memberExpression), sortDirection, false);
		}

		/// <summary>
		/// Gets the sort.
		/// </summary>
		/// <param name="member">The member.</param>
		/// <returns></returns>
		public SortItem GetSort(string member)
		{
			SortItem retVal = null;
			if (Sorts != null && Sorts.Count > 0)
			{
				retVal = Sorts.Where(x => x.Member == member).FirstOrDefault();
			}
			return retVal;
		}

		/// <summary>
		/// Gets the filter.
		/// </summary>
		/// <param name="memberExpression">The member expression.</param>
		/// <returns></returns>
		public SortItem GetSort<TProperty>(Expression<Func<T, TProperty>> memberExpression)
		{
			return GetSort(GetMemberFromExpression(memberExpression));
		}


		private FilterItem AddFilter(string member, ComparisonOperator op, object value, bool validateMember)
		{
			if (Filters == null)
			{
				Filters = new List<FilterItem>();
			}
			if (validateMember)
			{
				ValidateMember(member);
			}
			var item = new FilterItem { Member = member, Operator = op, Value = value };
			Filters.Add(item);
			return item;
		}

		private SortItem AddSort(string member, ListSortDirection sortDirection, bool validateMember)
		{
			if (Sorts == null)
			{
				Sorts = new List<SortItem>();
			}
			if (validateMember)
			{
				ValidateMember(member);
			}
			var item = new SortItem { Member = member, SortDirection = sortDirection };
			Sorts.Add(item);
			return item;
		}

		private static string GetMemberFromExpression<TProperty>(Expression<Func<T, TProperty>> memberExpression)
		{
			var x0 = memberExpression.Body;
			if (x0 is UnaryExpression)
			{
				x0 = ((UnaryExpression)x0).Operand;
			}
			if (x0 is MemberExpression)
			{
				var x1 = (MemberExpression)x0;
				if (x1.Expression is ParameterExpression)
				{
					return x1.Member.Name;
				}
				if (x1.Expression is MemberExpression)
				{
					var x2 = (MemberExpression)x1.Expression;
					if (!(x2.Expression is ParameterExpression))
					{
						throw new ArgumentException("Member expression does not support acessing members more than two levels deep.");
					}
					return "{0}.{1}".FormatWith(x2.Member.Name, x1.Member.Name);
				}
				if (x1.Expression is MethodCallExpression)
				{
					var x2 = (MethodCallExpression)x1.Expression;
					if (x2.Method.Name == "get_Item" && x2.Object is MemberExpression)
					{
						var x3 = (MemberExpression)x2.Object;
						if (!(x3.Expression is ParameterExpression))
						{
							throw new ArgumentException("Member expression does not support acessing members more than two levels deep.");
						}
						return "{0}.{1}".FormatWith(x3.Member.Name, x1.Member.Name);
					}
				}
			}
			throw new ArgumentException("Could not resolve member from memberExpression.");
		}

		private static void ValidateMember(string member)
		{
			member.Guard("member");
			if (member.IndexOf('.') < 0 && typeof(T).GetProperty(member) == null)
			{
				throw new ArgumentException(String.Format("{0} is not a valid property for {1}.", member, typeof(T).Name));
			}
			if (member.IndexOf('.') > 0)
			{
				var parts = member.Split('.');
				if (parts.Length > 3 || (parts.Length == 3 && parts[2] != BaseEntity.IdProperty))
				{
					throw new ArgumentException("Member property does not support acessing members more than two levels deep.");
				}
				var type = typeof(T);
				foreach (var part in parts)
				{
					var prop = type.GetProperty(part);
					if (prop == null)
					{
						var genericArgs = type.GetGenericArguments();
						if (genericArgs.Length == 1)
						{
							prop = genericArgs[0].GetProperty(part);
						}
					}
					if (prop == null)
					{
						throw new ArgumentException(String.Format("{0} is not a valid property for {1}.", member, typeof(T).Name));
					}
					type = prop.PropertyType;
				}
			}
		}
	}
}