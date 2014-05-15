// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Threading;
using AopAlliance.Intercept;

namespace DigitalBeacon.Business.Support
{
	public class CultureAdvice : IMethodInterceptor
	{
		#region Implementation of IMethodInterceptor

		public object Invoke(IMethodInvocation invocation)
		{
			if (ResourceManager.SystemCulture.Name != ResourceManager.ClientCulture.Name)
			{
				Thread.CurrentThread.CurrentCulture = ResourceManager.SystemCulture;
				Thread.CurrentThread.CurrentUICulture = ResourceManager.SystemCulture;
				object retVal;
				try
				{
					retVal = invocation.Proceed();
				}
				finally
				{
					Thread.CurrentThread.CurrentCulture = ResourceManager.ClientCulture;
					Thread.CurrentThread.CurrentUICulture = ResourceManager.ClientCulture;
				}
				return retVal;
			}
			return invocation.Proceed();
		}

		#endregion
	}
}