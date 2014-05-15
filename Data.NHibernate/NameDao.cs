// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using DigitalBeacon.Model;
using NHibernate;
using NHibernate.Criterion;

namespace DigitalBeacon.Data.NHibernate
{
	/// <summary>
	/// Base class for data access objects for named entities using NHibernate persistence
	/// </summary>
	public class NameDao<T> : BaseDao<T>, INameDao<T> where T:class, IBaseEntity, INamedEntity, new()
	{
		#region static variables

		protected const string SelectName = "select x.Id, x.ModificationCounter, x.Name ";
		protected const string WhereClauseByName = " where x.Name = ?";

		#endregion

		#region INameDao implementation

		public T FetchByName(string name)
		{
			T retVal = default(T);
			IList<object[]> list = HibernateTemplate.Find<object[]>(SelectName + FromClause + WhereClauseByName, name);
			if (list != null && list.Count > 0)
			{
				retVal = CreateNamedEntity(list[0]);
			}
			return retVal;
		}

		public IList<INamedEntity> FetchNameList()
		{
			return HibernateTemplate.ExecuteFind<INamedEntity>(
				delegate(ISession session)
				{
					IList<INamedEntity> retVal = null;
					ICriteria c = session.CreateCriteria(typeof(T));
					c.SetProjection(
						Projections.ProjectionList()
							.Add(Projections.Property(BaseEntity.IdProperty))
							.Add(Projections.Property(BaseEntity.VersionProperty))
							.Add(Projections.Property(BaseEntity.NameProperty)));
					if (typeof(T).GetProperty(BaseEntity.DisplayOrderProperty) != null)
					{
						c.Add(Restrictions.Gt(BaseEntity.DisplayOrderProperty, 0));
						c.AddOrder(Order.Asc(BaseEntity.DisplayOrderProperty));
					}
					c.AddOrder(Order.Asc(BaseEntity.NameProperty));
					IList<object[]> list = c.List<object[]>();
					if (list != null)
					{
						retVal = CreateNamedEntityList(list);
					}
					return retVal;
				});
		}

		private static T CreateNamedEntity(object[] result)
		{
			T entity = default(T);
			if (result != null)
			{
				entity = new T();
				entity.Id = Int64.Parse(result[0].ToString());
				entity.ModificationCounter = Int64.Parse(result[1].ToString());
				entity.Name = result[2] as string;
			}
			return entity;
		}

		private static List<INamedEntity> CreateNamedEntityList(ICollection<object[]> resultList)
		{
			List<INamedEntity> retVal = new List<INamedEntity>(resultList.Count);
			foreach (object[] result in resultList)
			{
				retVal.Add(CreateNamedEntity(result));
			}
			return retVal;
		}

		#endregion
	}
}
