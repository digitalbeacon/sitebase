// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace DigitalBeacon.SiteBase.Web.Models
{
	public class BaseViewModel
	{
		private List<string> _messages;
		private List<string> _errors;
		private Dictionary<string, IEnumerable<SelectListItem>> _listItems;

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseViewModel"/> class.
		/// </summary>
		public BaseViewModel() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseViewModel"/> class.
		/// </summary>
		/// <param name="heading">The heading.</param>
		public BaseViewModel(string heading)
		{
			Heading = heading;
		}

		/// <summary>
		/// Gets or sets the heading.
		/// </summary>
		/// <value>The heading.</value>
		[ReadOnly(true)]
		public string Heading { get; set; }

		/// <summary>
		/// Gets or sets the content.
		/// </summary>
		/// <value>The content.</value>
		[ReadOnly(true)]
		public string Content { get; set; }

		/// <summary>
		/// Gets or sets the return URL.
		/// </summary>
		/// <value>The return URL.</value>
		[ReadOnly(true)]
		public string ReturnUrl { get; set; }

		/// <summary>
		/// Gets or sets the return text.
		/// </summary>
		/// <value>The return text.</value>
		[ReadOnly(true)]
		public string ReturnText { get; set; }

		/// <summary>
		/// Gets or sets the additional return URL.
		/// </summary>
		/// <value>The return URL.</value>
		[ReadOnly(true)]
		public string AdditionalReturnUrl { get; set; }

		/// <summary>
		/// Gets or sets the additional return text.
		/// </summary>
		/// <value>The return text.</value>
		[ReadOnly(true)]
		public string AdditionalReturnText { get; set; }

		/// <summary>
		/// Gets the messages.
		/// </summary>
		/// <value>The messages.</value>
		[ReadOnly(true)]
		public List<string> Messages 
		{
			get
			{
				if (_messages == null)
				{
					_messages = new List<string>();
				}
				return _messages;
			}
		}

		/// <summary>
		/// Gets the errors.
		/// </summary>
		/// <value>The errors.</value>
		[ReadOnly(true)]
		public List<string> Errors
		{
			get
			{
				if (_errors == null)
				{
					_errors = new List<string>();
				}
				return _errors;
			}
		}

		/// <summary>
		/// Gets the list items.
		/// </summary>
		/// <value>The list items.</value>
		[ReadOnly(true)]
		public Dictionary<string, IEnumerable<SelectListItem>> ListItems 
		{
			get
			{
				if (_listItems == null)
				{
					_listItems = new Dictionary<string, IEnumerable<SelectListItem>>();
				}
				return _listItems;
			}
		}
	}
}
