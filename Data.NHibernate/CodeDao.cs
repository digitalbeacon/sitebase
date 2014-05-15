// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using DigitalBeacon.Model;

namespace DigitalBeacon.Data.NHibernate
{
	/// <summary>
	/// Base class for data access objects for named entities using NHibernate persistence
	/// </summary>
	public class CodeDao<T> : BaseDao<T>, ICodeDao<T> where T:class, IBaseEntity, ICodedEntity, new()
	{
		#region ICodeDao<T> Members

		public T FetchByCode(string code)
		{
			return CodeDaoHelper<T>.FetchByCode(HibernateTemplate, code);
		}

		#endregion
	}
}
