// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Collections.Generic;

namespace DigitalBeacon.SiteBase.Model
{
	[Serializable]
	public class UserEntity : GeneratedUserEntity
	{
		private string _password;
		private string _securityQuestion;
		private string _securityAnswer;
		private bool _approved;
		private bool _lockedOut;

		#region Properties Names

		public const string PasswordProperty = "Password";
		public const string SecurityQuestionProperty = "SecurityQuestion";
		public const string SecurityAnswerProperty = "SecurityAnswer";
		public const string IsApprovedProperty = "IsApproved";
		public const string IsLockedOutProperty = "IsLockedOut";

		#endregion

		public virtual string Password
		{
			get { return _password; }
			set { _password = value; }
		}

		public virtual string SecurityQuestion
		{
			get { return _securityQuestion; }
			set { _securityQuestion = value; }
		}

		public virtual string SecurityAnswer
		{
			get { return _securityAnswer; }
			set { _securityAnswer = value; }
		}

		public virtual bool Approved
		{
			get { return _approved; }
			set { _approved = value; }
		}

		public virtual bool LockedOut
		{
			get { return _lockedOut; }
			set { _lockedOut = value; }
		}

		public virtual bool HasRole(long associationId, Role role)
		{
			var retVal = false;
			if (Roles != null && Roles.Count > 0)
			{
				foreach (var userRole in Roles)
				{
					if (userRole.AssociationId == associationId && userRole.Role == role)
					{
						retVal = true;
					}
				}
			}
			return retVal;
		}

		public virtual void AddRole(long associationId, Role role)
		{
			if (HasRole(associationId, role))
			{
				return;
			}
			var userRole = new UserRoleEntity { User = this, AssociationId = associationId, Role = role };
			if (Roles == null)
			{
				Roles = new List<UserRoleEntity>();
			}
			Roles.Add(userRole);
		}

		public virtual void RemoveRole(long associationId, Role role)
		{
			if (Roles == null || Roles.Count == 0)
			{
				return;
			}
			for (var i = 0; i < Roles.Count; i++)
			{
				if (Roles[i].AssociationId == associationId && Roles[i].Role == role)
				{
					Roles.RemoveAt(i);
					break;
				}
			}
		}
	}
}
