// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.Web;

namespace DigitalBeacon.SiteBase.Models
{
	public class User
	{
		public User(UserEntity e)
		{
			Id = e.Id;
			Username = e.Username;
			DisplayName = e.DisplayName;
			Email = e.Email;
			FirstName = e.Person.FirstName;
			LastName = e.Person.LastName;
			Enabled = e.Approved;
		}

		public long Id { get; set; }

		[LocalizedDisplayName("Common.Username.Label")]
		public string Username { get; set; }
		
		[LocalizedDisplayName("Common.DisplayName.Label")]
		public string DisplayName { get; set; }

		[LocalizedDisplayName("Common.Email.Label")]
		public string Email { get; set; }

		[LocalizedDisplayName("Common.FirstName.Label")]
		public string FirstName { get; set; }

		[LocalizedDisplayName("Common.LastName.Label")]
		public string LastName { get; set; }

		[LocalizedDisplayName("Common.Enabled.Label")]
		public bool Enabled { get; set; }
	}
}
