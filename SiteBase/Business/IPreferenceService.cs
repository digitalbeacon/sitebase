// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using DigitalBeacon.SiteBase.Model;

namespace DigitalBeacon.SiteBase.Business
{
	public interface IPreferenceService
	{
		#region Preferences

		/// <summary>
		/// Get preference
		/// </summary>
		/// <param name="associationId">The association id.</param>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		PreferenceEntity GetPreference(long associationId, string key);

		/// <summary>
		/// Save preference
		/// </summary>
		/// <param name="pref"></param>
		void SavePreference(PreferenceEntity pref);

		/// <summary>
		/// Save preference
		/// </summary>
		/// <param name="associationId">The association id.</param>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		void SetPreference(long associationId, string key, string value);

		/// <summary>
		/// Delete preference
		/// </summary>
		/// <param name="associationId">The association id.</param>
		/// <param name="key">The key.</param>
		void DeletePreference(long associationId, string key);

		#endregion

		#region User Preferences

		/// <summary>
		/// Get preference
		/// </summary>
		/// <param name="associationId"></param>
		/// <param name="userId"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		PreferenceEntity GetUserPreference(long associationId, long userId, string key);

		/// <summary>
		/// Save user preference
		/// </summary>
		/// <param name="pref"></param>
		void SaveUserPreference(PreferenceEntity pref);

		/// <summary>
		/// Save user preference
		/// </summary>
		/// <param name="associationId"></param>
		/// <param name="userId"></param>
		/// <param name="key"></param>
		/// <param name="value"></param>
		void SetUserPreference(long associationId, long userId, string key, string value);

		/// <summary>
		/// Delete user preference
		/// </summary>
		/// <param name="associationId"></param>
		/// <param name="userId"></param>
		/// <param name="key"></param>
		void DeleteUserPreference(long associationId, long userId, string key);

		#endregion

		#region Other

		/// <summary>
		/// Gets the size of the list page.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="associationId">The association id.</param>
		/// <param name="user">The user.</param>
		/// <returns></returns>
		int GetListPageSize(string key, long associationId, UserEntity user);

		#endregion
	}
}
