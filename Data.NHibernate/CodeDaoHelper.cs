// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;
using DigitalBeacon.Model;
using Spring.Data.NHibernate.Generic;

namespace DigitalBeacon.Data.NHibernate
{
	/// <summary>
	/// Helper class for data access objects for coded entities using NHibernate persistence
	/// </summary>
	public class CodeDaoHelper<T> where T:class, ICodedEntity, new()
	{
		#region static variables

		protected const string FromClause = "from {0} as x";
		protected const string WhereClauseByCode = " where x.Code = :code";
		protected const string CodeParam = "code";

		#endregion

		public static T FetchByCode(HibernateTemplate hibernateTemplate, string code)
		{
			T retVal = default(T);
			IList<T> list = hibernateTemplate.FindByNamedParam<T>(String.Format(FromClause, typeof(T).Name) + WhereClauseByCode, CodeParam, code);
			if (list != null && list.Count > 0)
			{
				retVal = list[0];
			}
			return retVal;
		}

	}
}
