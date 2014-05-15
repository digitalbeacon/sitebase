// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Web.UI.WebControls;
using DigitalBeacon.Util;

namespace DigitalBeacon.Web.Controls
{
	public class DateTextBox : TextBox
	{
		private const string DefaultFormat = "M/d/yyyy";

		public string Format { get; set; }

		public DateTextBox()
		{
			Format = DefaultFormat;
		}

		public DateTime? Value
		{
			get { return Text.ToDate(Format); }
			set { Text = value.HasValue ? value.Value.ToString(Format) : String.Empty; }
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			Page.ClientScript.RegisterClientScriptBlock(GetType(), GetType().Name, String.Format("<script type=\"text/javascript\" src=\"{0}\"></script>",
				Page.ClientScript.GetWebResourceUrl(GetType(), String.Format("{0}.DateTextBox.js", GetType().Namespace))));
			Page.ClientScript.RegisterStartupScript(GetType(), String.Format("{0}_js", ClientID), String.Format("<script>var {0} = new DbeTextBox('{1}','{2}');</script>", ClientID, ClientID, Format));
		}
	}
}
