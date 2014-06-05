// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.ComponentModel;
using System.Web.Script.Serialization;
namespace DigitalBeacon.SiteBase.Web.Models
{
	public class EntityModel : BaseViewModel
	{
		public const string BulkCreateProperty = "BulkCreate";
		public const string SequencerProperty = "Sequencer";

		[ReadOnly(true)]
		[ScriptIgnore]
		public string Cancel { get; set; }

		[ReadOnly(true)]
		[ScriptIgnore]
		public string Delete { get; set; }

		[ReadOnly(true)]
		[ScriptIgnore]
		public bool BulkCreate { get; set; }

		[ReadOnly(true)]
		[ScriptIgnore]
		public string Sequencer { get; set; }

		[ReadOnly(true)]
		[ScriptIgnore]
		public string Description { get; set; }

		[ReadOnly(true)]
		[ScriptIgnore]
		public string SingularLabel { get; set; }

		[ReadOnly(true)]
		[ScriptIgnore]
		public string PluralLabel { get; set; }

		public long Id { get; set; }

		public bool IsNew
		{
			get { return Id == 0; }
		}
	}
}