// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Collections.Generic;
using System.Dynamic;
using System.Web.Mvc;
using DigitalBeacon.SiteBase.Web.Models;

namespace DigitalBeacon.SiteBase.Models
{
	public class FormRowOptions
	{
		public string Property { get; set; }
		public string SubProperty { get; set; }
		public string Label { get; set; }
		public bool? Required { get; set; }
		public bool? InputReadOnly { get; set; }
		public string InputType { get; set; }
		public string CustomText { get; set; }
		public bool IncludeEmptyOption { get; set; }
		public string HelpText { get; set; }
		public string Validation { get; set; }
		public string[] Validations { get; set; }
		public bool GenerateRowId { get; set; }
		public string RowId { get; set; }
		public string RowClass { get; set; }
		public string InputClass { get; set; }
		public string RowStyle { get; set; }
		public string Mask { get; set; }
		public bool Focus { get; set; }
		public int? MaxLength { get; set; }
		public string AutoCompleteUrl { get; set; }
		public bool IsEntityModel { get; set; }
		public bool EnableSave { get; set; }
		public bool EnableDelete { get; set; }
		public bool EnableCancel { get; set; }
		public bool EnableBulkCreate { get; set; }
		public bool EnableCaptcha { get; set; }
		public int InputSize { get; set; }
		public BaseViewModel Model { get; set; }
		public string FormPanelId { get; set; }
		public string SingularLabel { get; set; }
		public string PluralLabel { get; set; }
		public ViewDataDictionary ViewData { get; set; }
		public string RenderContent { get; set; }
	}
}