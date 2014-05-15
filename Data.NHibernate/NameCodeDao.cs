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
	public class NameCodeDao<T> : NameDao<T>, ICodeDao<T> where T : class, IBaseEntity, INamedEntity, ICodedEntity, new()
	{
		#region ICodeDao<T> Members

		public T FetchByCode(string code)
		{
			return CodeDaoHelper<T>.FetchByCode(HibernateTemplate, code);
		}

		#endregion
	}
}
