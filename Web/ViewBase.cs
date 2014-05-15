// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System.Collections.Generic;
using MvcContrib.FluentHtml;
using MvcContrib.FluentHtml.Behaviors;
using Spark.Web.Mvc;
using System.Web.Mvc;

namespace DigitalBeacon.Web
{
	/// <summary>
	/// The strongly-typed base type for views for Spark/FluentHtml compatibility
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class ViewBase<T> : SparkView<T>, IViewModelContainer<T> where T : class
	{
		private readonly List<IBehaviorMarker> _behaviors = new List<IBehaviorMarker>();

		/// <summary>
		/// Initializes a new instance of the <see cref="SparkModelViewPage&lt;T&gt;"/> class.
		/// </summary>
		protected ViewBase()
		{
			_behaviors.Add(new ValidationBehavior(() => ViewData.ModelState));
			//add any other desired behaviors here
		}

		/// <summary>
		/// Gets or sets the HTML name prefix.
		/// </summary>
		/// <value>The HTML name prefix.</value>
		public string HtmlNamePrefix { get; set; }

		/// <summary>
		/// Gets the view model.
		/// </summary>
		/// <value>The view model.</value>
		public T ViewModel
		{
			get { return Model; }
		}

		/// <summary>
		/// The collection of <see cref="T:MvcContrib.FluentHtml.Behaviors.IBehaviorMarker"/> objects.
		/// </summary>
		/// <value></value>
		public IEnumerable<IBehaviorMarker> Behaviors
		{
			get { return _behaviors; }
		}
	}

	/// <summary>
	/// The base type for views for Spark/FluentHtml compatibility
	/// </summary>
	public abstract class ViewBase : ViewBase<object> { }
}