// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Reflection;
using System.Linq.Expressions;

namespace DigitalBeacon.Util
{
	public static class LambdaExtensions
	{
		public static MemberInfo GetMember<T, TProperty>(this Expression<Func<T, TProperty>> expression)
		{
			var expression2 = RemoveUnary(expression.Body);
			return expression2 == null ? null : expression2.Member;
		}

		public static MemberInfo GetMember(this LambdaExpression expression)
		{
			var expression2 = RemoveUnary(expression.Body);
			return expression2 == null ? null : expression2.Member;
		}

		private static MemberExpression RemoveUnary(Expression toUnwrap)
		{
			if (toUnwrap is UnaryExpression)
			{
				return (((UnaryExpression)toUnwrap).Operand as MemberExpression);
			}
			return (toUnwrap as MemberExpression);
		}
	}
}
