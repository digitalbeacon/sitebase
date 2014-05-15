// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

namespace DigitalBeacon.SiteBase.Web.Models
{
	public class EntityModel : BaseViewModel
	{
		public const string BulkCreateProperty = "BulkCreate";
		public const string SequencerProperty = "Sequencer";

		public string Cancel { get; set; }
		public string Delete { get; set; }

		public bool BulkCreate { get; set; }

		public string Sequencer { get; set; }

		public long Id { get; set; }

		public string Description { get; set; }
		public string SingularLabel { get; set; }
		public string PluralLabel { get; set; }
 
		public bool IsNew
		{
			get { return Id == 0; }
		}
	}
}