// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using DigitalBeacon.Data;
using DigitalBeacon.SiteBase.Model;

namespace DigitalBeacon.SiteBase.Data
{
	/// <summary>
	/// The interface for accessing data associated with preferences
	/// </summary>
	public interface IPreferenceDao : IDao<PreferenceEntity>
	{
		/// <summary>
		/// Get specified user preference
		/// </summary>
		/// <param name="associationId">The association id.</param>
		/// <param name="key">The key.</param>
		/// <param name="userId">The user id.</param>
		/// <returns></returns>
		PreferenceEntity Fetch(long associationId, string key, long? userId);
	}
}
