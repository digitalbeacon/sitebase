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
	/// Encapsulates a single data filter expression
	/// </summary>
	public class FilterItem
	{
		/// <summary>
		/// Gets or sets the member.
		/// </summary>
		/// <value>The member.</value>
		public string Member { get; set; }

		/// <summary>
		/// Gets or sets the operator.
		/// </summary>
		/// <value>The operator.</value>
		public ComparisonOperator Operator { get; set; }

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>The value.</value>
		public object Value { get; set; }

		/// <summary>
		/// Gets or sets the grouping.
		/// </summary>
		/// <value>The grouping.</value>
		public int Grouping { get; set; }
	}
}
