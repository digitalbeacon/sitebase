// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

namespace DigitalBeacon.Model
{
	public enum ComparisonOperator
	{
		Equal = 1,
		NotEqual = 2,
		Null = 3,
		NotNull = 4,
		LessThan = 5,
		LessThanOrEqual = 6,
		GreaterThan = 7,
		GreaterThanOrEqual = 8,
		Contains = 9,
		StartsWith = 10,
		EndsWith = 11,
		In = 12,
		SortAscending = 101,
		SortDescending = 102
	}
}
