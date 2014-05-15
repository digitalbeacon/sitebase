// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using DigitalBeacon.Data;
using DigitalBeacon.SiteBase.Model;
using System.Collections.Generic;

namespace DigitalBeacon.SiteBase.Data
{
	/// <summary>
	/// The interface for accessing data associated with users
	/// </summary>
	public interface IUserDao : IDao<UserEntity>
	{
		/// <summary>
		/// Fetch list of active users
		/// </summary>
		/// <param name="associationId"></param>
		/// <returns></returns>
		IList<UserEntity> FetchActive(long associationId);

		/// <summary>
		/// Retrieve a UserEntity with given username
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		UserEntity FetchByUsername(string username);

		/// <summary>
		/// Retrieve a UserEntity with given username for given association
		/// </summary>
		/// <param name="associationId"></param>
		/// <param name="username"></param>
		/// <returns></returns>
		UserEntity FetchByUsername(long associationId, string username);

		/// <summary>
		/// Get users by role
		/// </summary>
		/// <param name="associationId"></param>
		/// <param name="role"></param>
		/// <returns></returns>
		IList<UserEntity> FetchByRole(long associationId, Role role);

		/// <summary>
		/// Fetches users that match search parameters
		/// </summary>
		/// <param name="searchInfo">The search info.</param>
		/// <returns></returns>
		IList<UserEntity> FetchUsers(UserSearchInfo searchInfo);

		/// <summary>
		/// Fetches count for matched records
		/// </summary>
		/// <param name="searchInfo">The search info.</param>
		/// <returns></returns>
		int FetchUserCount(UserSearchInfo searchInfo);
	}
}
