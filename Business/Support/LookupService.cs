// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Collections.Generic;
using DigitalBeacon.Data;
using DigitalBeacon.Model;

namespace DigitalBeacon.Business.Support
{
	public class LookupService : ILookupService
	{
		#region Private Members

		private static readonly IDataAdapter DataAdapter = ServiceFactory.Instance.GetService<IDataAdapter>();

		#endregion

		#region ILookupService Members

		public T Get<T>(long id) where T : class, IBaseEntity, new()
		{
			return DataAdapter.Fetch<T>(id);
		}

		public IList<INamedEntity> GetNameList<T>() where T : class, INamedEntity, new()
		{
			return DataAdapter.FetchNameList<T>();
		}

		public string GetName<T>(long id) where T : class, INamedEntity, new()
		{
			return DataAdapter.Fetch<T>(id).Name;
		}

		public T GetByName<T>(string name) where T : class, INamedEntity, new()
		{
			return DataAdapter.FetchByName<T>(name);
		}

		public string GetCode<T>(long id) where T : class, ICodedEntity, new()
		{
			return DataAdapter.Fetch<T>(id).Code;
		}

		public T GetByCode<T>(string code) where T : class, ICodedEntity, new()
		{
			return DataAdapter.FetchByCode<T>(code);
		}

		#endregion
	}
}
