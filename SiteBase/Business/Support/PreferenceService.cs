// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Reflection;
using Common.Logging;
using DigitalBeacon.Business;
using DigitalBeacon.SiteBase.Data;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.Util;

namespace DigitalBeacon.SiteBase.Business.Support
{
	public class PreferenceService : BaseService, IPreferenceService
	{
		#region Private Members

		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private readonly IPreferenceDao _preferenceDao = ServiceFactory.Instance.GetService<IPreferenceDao>();
		private readonly IModuleService _moduleService = ServiceFactory.Instance.GetService<IModuleService>();

		#endregion

		#region IPreferenceService Members

		public PreferenceEntity GetPreference(long associationId, string key)
		{
			return _preferenceDao.Fetch(associationId, key, null);
		}

		public void SavePreference(PreferenceEntity pref)
		{
			if (pref.AssociationId == 0 || pref.Key.IsNullOrBlank() || pref.Value.IsNullOrBlank())
			{
				throw new ServiceException("All fields except UserId are required: {0}", pref.ToString());
			}
			if (pref.UserId.HasValue)
			{
				throw new ServiceException("UserId must be null: {0}", pref.ToString());
			}
			if (pref.IsNew)
			{
				PreferenceEntity existingPref = GetPreference(pref.AssociationId, pref.Key);
				if (existingPref != null)
				{
					pref.Id = existingPref.Id;
					pref.ModificationCounter = existingPref.ModificationCounter;
				}
			}
			SaveWithAudit(pref);
		}

		public void SetPreference(long associationId, string key, string value)
		{
			SavePreference(new PreferenceEntity
			{
				AssociationId = associationId,
				Key = key,
				Value = value
			});
		}

		public void DeletePreference(long associationId, string key)
		{
			PreferenceEntity pref = GetPreference(associationId, key);
			if (pref != null)
			{
				DeleteWithAudit(pref);
			}
		}

		public PreferenceEntity GetUserPreference(long associationId, long userId, string key)
		{
			return _preferenceDao.Fetch(associationId, key, userId);
		}

		public void SaveUserPreference(PreferenceEntity pref)
		{
			if (pref.AssociationId == 0 || !pref.UserId.HasValue || String.IsNullOrEmpty(pref.Key) || String.IsNullOrEmpty(pref.Value))
			{
				throw new ServiceException("All fields are required: {0}", pref.ToString());
			}
			if (pref.IsNew)
			{
				PreferenceEntity existingPref = GetUserPreference(pref.AssociationId, pref.UserId.Value, pref.Key);
				if (existingPref != null)
				{
					pref.Id = existingPref.Id;
					pref.ModificationCounter = existingPref.ModificationCounter;
				}
			}
			SaveWithAudit(pref);
		}

		public void SetUserPreference(long associationId, long userId, string key, string value)
		{
			SavePreference(new PreferenceEntity
			{
				AssociationId = associationId,
				Key = key,
				UserId = userId,
				Value = value
			});
		}

		public void DeleteUserPreference(long associationId, long userId, string key)
		{
			PreferenceEntity pref = GetUserPreference(associationId, userId, key);
			if (pref != null)
			{
				DeleteWithAudit(pref);
			}
		}

		public int GetListPageSize(string key, long associationId, UserEntity user)
		{
			PreferenceEntity pref;
			if (user != null)
			{
				pref = GetUserPreference(associationId, user.Id, key);
			}
			else
			{
				pref = GetPreference(associationId, key);
			}
			if (pref != null)
			{
				return (int)pref.ValueAsInt64.Value;
			}
			else
			{
				return (int)_moduleService.GetGlobalModuleSetting(ModuleSettingDefinition.GlobalListPageSize).ValueAsInt64.Value;
			}
		}

		#endregion
	}
}
