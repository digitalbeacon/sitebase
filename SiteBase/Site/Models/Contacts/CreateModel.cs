// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using DigitalBeacon.Web;

namespace DigitalBeacon.SiteBase.Models.Contacts
{
	public class CreateModel : EditModel
	{
		[LocalizedDisplayName("Comments.Date.Label")]
		public override string CommentDate 
		{
			get { return base.CommentDate; }
			set { base.CommentDate = value; }
		}

		[LocalizedDisplayName("Comments.Type.Label")]
		public override string CommentType
		{
			get { return base.CommentType; }
			set { base.CommentType = value; }
		}
	}
}