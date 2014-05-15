// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.ComponentModel;

namespace DigitalBeacon.Model
{
	/// <summary>
	/// Encapsulates a single data sort expression
	/// </summary>
	public class SortItem
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SortItem"/> class.
		/// </summary>
		public SortItem()
		{
			SortDirection = ListSortDirection.Ascending;
		}

		/// <summary>
		/// Gets or sets the member.
		/// </summary>
		/// <value>The member.</value>
		public string Member { get; set; }

		/// <summary>
		/// Gets or sets the sort direction.
		/// </summary>
		/// <value>The sort direction.</value>
		public ListSortDirection SortDirection { get; set; }
	}
}
