// ---------------------------------------------------------------------- //
//                                                                        //
//                       Copyright (c) 2007-2014                          //
//                         Digital Beacon, LLC                            //
//                                                                        //
// ---------------------------------------------------------------------- //

using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Common.Logging;
using DigitalBeacon.Business;
using DigitalBeacon.SiteBase.Business.Support;
using DigitalBeacon.SiteBase.Data;
using DigitalBeacon.SiteBase.Model;
using DigitalBeacon.SiteBase.Model.Contacts;
using DigitalBeacon.SiteBase.SqlUpdate;
using DigitalBeacon.SiteBase.Web;
using DigitalBeacon.Util;
using DigitalBeacon.Web;
using DigitalBeacon.Web.Validation;
using FluentValidation.Attributes;
using FluentValidation.Mvc;
using Spark;
using Spark.Web.Mvc;

namespace DigitalBeacon.SiteBase
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class Application : HttpApplication
	{
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		protected virtual void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			RegisterRoutes(RouteTable.Routes);
			RegisterViewEngines();
			RegisterModelMetadataProviders();
			RegisterModelValidatorProviders();
			RegisterCustomModelBinders();
			RegisterCustomValidators();
			RegisterDatabaseResourceManager();
			RegisterLookupAdminTypes();
			UpgradeDatabase();
			CreateDefaultUsers();
		}

		public override void Init()
		{
			base.Init();
			EndRequest += HandleForbiddenResponse;
		}

		#region Protected Methods

		protected virtual void HandleForbiddenResponse(object sender, EventArgs e)
		{
			if (Context.Response.StatusCode == 403)
			{
				var context = HttpContext.Current;
				context.Server.TransferRequest("~/default/unavailable", true, context.Request.HttpMethod, context.Request.Headers, false);
			}
		}

		protected static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			const string renderTypeRegex = "^(|json|template|partial|partialwrapped)$";
			const string notRenderTypeRegex = "^(?!(json|template|partial|partialwrapped)$).*$";

			routes.MapRoute(
				"DependencyUpdate",
				"{parentController}/{parentId}/{controller}/{id}",
				new { action = "update" },
				new { parentId = @"\d+", id = @"\d+", httpVerb = new HttpVerbConstraint(HttpVerbs.Put) }
			);

			routes.MapRoute(
				"DependencyDelete",
				"{parentController}/{parentId}/{controller}/{id}",
				new { action = "delete" },
				new { parentId = @"\d+", id = @"\d+", httpVerb = new HttpVerbConstraint(HttpVerbs.Delete) }
			);
			/*
						routes.MapRoute(
							"DependencyCreate",
							"{parentController}/{parentId}/{controller}",
							new { action = "Create" },
							new { parentId = @"\d+", httpVerb = new HttpVerbConstraint(HttpVerbs.Post) }
						);
			*/
			routes.MapRoute(
				"DependencyEntityPaging",
				"{parentController}/{parentId}/{controller}/{action}/page/{page}",
				new { action = "index" },
				new { parentId = @"\d+", page = @"\d+", httpVerb = new HttpVerbConstraint(HttpVerbs.Get) }
			);

			routes.MapRoute(
				"DependencyIdPaging",
				"{parentController}/{parentId}/{controller}/{action}/{id}/page/{page}",
				new { action = "index" },
				new { parentId = @"\d+", page = @"\d+", httpVerb = new HttpVerbConstraint(HttpVerbs.Get) }
			);

			routes.MapRoute(
				"DependencyActionPaging",
				"{parentController}/{parentId}/{controller}/{action}/page/{page}",
				new { action = "index" },
				new { parentId = @"\d+", page = @"\d+", httpVerb = new HttpVerbConstraint(HttpVerbs.Get) }
			);

			routes.MapRoute(
				"DependencyEntity",
				"{parentController}/{parentId}/{controller}/{id}/{action}",
				new { action = "show" },
				new { parentId = @"\d+", id = @"\d+", action = notRenderTypeRegex }
			);

			routes.MapRoute(
				"Dependency",
				"{parentController}/{parentId}/{controller}/{action}/{renderType}",
				new { renderType = UrlParameter.Optional },
				new { parentId = @"\d+", action = notRenderTypeRegex, renderType = renderTypeRegex }
			);

			//routes.MapRoute(
			//	"Dependency",
			//	"{parentController}/{parentId}/{controller}/{renderType}",
			//	new { action = "index", renderType = UrlParameter.Optional },
			//	new { parentId = @"\d+", controller = notRenderTypeRegex, renderType = renderTypeRegex }
			//);

			routes.MapRoute(
				"Update",
				"{controller}/{id}/{renderType}",
				new { action = "update", renderType = UrlParameter.Optional },
				new { id = @"\d+", renderType = renderTypeRegex, httpVerb = new HttpVerbConstraint(HttpVerbs.Put) }
			);

			routes.MapRoute(
				"Delete",
				"{controller}/{id}/{renderType}",
				new { action = "delete", renderType = UrlParameter.Optional },
				new { id = @"\d+", renderType = renderTypeRegex, httpVerb = new HttpVerbConstraint(HttpVerbs.Delete) }
			);

			routes.MapRoute(
				"Create",
				"{controller}/{renderType}",
				new { action = "create", renderType = UrlParameter.Optional },
				new { controller = notRenderTypeRegex, renderType = renderTypeRegex, httpVerb = new HttpVerbConstraint(HttpVerbs.Post) }
			);

			routes.MapRoute(
				"EntityPaging",
				"{controller}/{id}/{action}/page/{page}",
				new { action = "show" },
				new { id = @"\d+", page = @"\d+" }
			);

			routes.MapRoute(
				"IdPaging",
				"{controller}/{action}/{id}/page/{page}",
				new { action = "index" },
				new { page = @"\d+", httpVerb = new HttpVerbConstraint(HttpVerbs.Get) }
			);

			routes.MapRoute(
				"ActionPaging",
				"{controller}/{action}/page/{page}",
				new { action = "index" },
				new { page = @"\d+", httpVerb = new HttpVerbConstraint(HttpVerbs.Get) }
			);

			routes.MapRoute(
				"EntityAction",
				"{controller}/{id}/{action}/{renderType}",
				new { renderType = UrlParameter.Optional },
				new { id = @"\d+", action = notRenderTypeRegex, renderType = renderTypeRegex }
			);

			routes.MapRoute(
				"Entity",
				"{controller}/{id}/{renderType}",
				new { action = "show", renderType = UrlParameter.Optional },
				new { id = @"\d+", renderType = renderTypeRegex }
			);

			routes.MapRoute(
				"Detail",
				"{controller}/{action}/{id}/{renderType}",
				new { renderType = UrlParameter.Optional },
				new { id = notRenderTypeRegex, renderType = renderTypeRegex }
			);

			routes.MapRoute(
				"Action",
				"{controller}/{action}/{renderType}",
				new { renderType = UrlParameter.Optional },
				new { action = notRenderTypeRegex, renderType = renderTypeRegex }
			);

			routes.MapRoute(
				"Controller",
				"{controller}/{renderType}",
				new { action = "index", renderType = UrlParameter.Optional },
				new { controller = notRenderTypeRegex, renderType = renderTypeRegex }
			);

			routes.MapRoute(
				"DefaultEntity",
				"{controller}/{id}/{action}",
				new { id = @"\d+", action = "show" }
			);

			routes.MapRoute(
				"Default",
				"{controller}/{action}/{id}",
				new { controller = "home", action = "index", id = UrlParameter.Optional }
			);

			//RouteDebug.RouteDebugger.RewriteRoutesForTesting(routes);
		}

		protected void RegisterViewEngines()
		{
			var spark = new SparkViewFactory();
			ViewEngines.Engines.Add(spark);
			var precompileViewsAssembly = ConfigurationManager.AppSettings[WebConstants.PrecompileViewsKey];
			if (precompileViewsAssembly.HasText())
			{
				if (!precompileViewsAssembly.ToLowerInvariant().EndsWith(".dll"))
				{
					precompileViewsAssembly = precompileViewsAssembly + ".dll";
				}
				var targetPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"bin\{0}".FormatWith(precompileViewsAssembly));
				if (File.Exists(targetPath))
				{
					Log.Info("View precompilation file [{0}] already exists. Skipping view precompilation.".FormatWith(targetPath));
				}
				else
				{
					var assembly = GetType().BaseType.Assembly;
					Log.Info("Precompiling views...");
					Log.Info("source: " + assembly.GetName().Name);
					var batch = new SparkBatchDescriptor(targetPath);
					Log.Info("target: " + batch.OutputAssembly);
					batch.FromAssembly(assembly);
					if (GetType().BaseType.BaseType.Assembly == MethodBase.GetCurrentMethod().DeclaringType.Assembly)
					{
						// add views from SiteBase assembly
						batch.FromAssembly(GetType().BaseType.BaseType.Assembly);
					}
					spark.Precompile(batch);
				}
			}
			var precompiledViewsAssemblyNames = ConfigurationManager.AppSettings[WebConstants.PrecompiledViewsKey];
			if (precompiledViewsAssemblyNames.HasText() || precompileViewsAssembly.HasText())
			{
				if (precompiledViewsAssemblyNames.IsNullOrBlank())
				{
					precompiledViewsAssemblyNames = precompileViewsAssembly;
				}
				var assemblyNames = precompiledViewsAssemblyNames.Split(',');
				foreach (var assemblyName in assemblyNames)
				{
					var name = assemblyName.Trim();
					if (name.ToLowerInvariant().EndsWith(".dll"))
					{
						name = name.Substring(0, name.Length - 4);
					}
					Log.Info("Loading precompiled views: " + name);
					spark.Engine.LoadBatchCompilation(Assembly.Load(name));
				}
			}
		}

		protected static void RegisterModelMetadataProviders()
		{
			ModelMetadataProviders.Current = new DigitalBeacon.Web.FluentValidationModelMetadataProvider(new AttributedValidatorFactory());
		}

		protected static void RegisterCustomModelBinders()
		{
			ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder(false));
			ModelBinders.Binders.Add(typeof(decimal?), new DecimalModelBinder(true));
		}

		protected static void RegisterCustomValidators()
		{
			DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(RequiredAttribute), typeof(RequiredAttributeAdapter));
			DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(StringLengthAttribute), typeof(StringLengthAttributeAdapter));
			DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(RegularExpressionAttribute), typeof(RegularExpressionAttributeAdapter));
			DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(RangeAttribute), typeof(RangeAttributeAdapter));
			DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(EmailAttribute), typeof(RegularExpressionAttributeAdapter));
			DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(GuidAttribute), typeof(RegularExpressionAttributeAdapter));
			DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(ManagedRegularExpressionAttribute), typeof(RegularExpressionAttributeAdapter));
		}

		protected static void RegisterModelValidatorProviders()
		{
			foreach (var provider in ModelValidatorProviders.Providers)
			{
				if (provider is ClientDataTypeModelValidatorProvider)
				{
					ModelValidatorProviders.Providers.Remove(provider);
					break;
				}
			}
			ModelValidatorProviders.Providers.Add(new CustomClientDataTypeModelValidatorProvider());
			ModelValidatorProviders.Providers.Add(new FluentValidationModelValidatorProvider(new AttributedValidatorFactory())
			{
				AddImplicitRequiredValidator = false
			});
		}

		private void RegisterDatabaseResourceManager()
		{
			if (ConfigurationManager.AppSettings[WebConstants.UseDatabaseResourcesKey].ToBoolean() ?? false)
			{
				ResourceManager.Instance.UseDatabaseResources = true;
			}
			if (ResourceManager.Instance.UseDatabaseResources)
			{
				RegisterLocalizedEntityTypes();
			}
		}

		protected virtual void RegisterLocalizedEntityTypes()
		{
			ResourceManager.RegisterLocalizedEntity<CountryEntity>();
			ResourceManager.RegisterLocalizedEntity<GenderEntity>();
			ResourceManager.RegisterLocalizedEntity<LanguageEntity>();
			ResourceManager.RegisterLocalizedEntity<NavigationItemEntity>(NavigationItemEntity.TextProperty);
			ResourceManager.RegisterLocalizedEntity<SecurityQuestionEntity>(SecurityQuestionEntity.TextProperty);
		}

		protected virtual void RegisterLookupAdminTypes()
		{
			LookupAdminService.RegisterAcceptedType<PostalCodeEntity>();
			//LookupAdminService.RegisterAcceptedType<ContactCommentTypeEntity>();
			LookupAdminService.RegisterAcceptedType<ContactCommentEntity>();
		}

		protected static void UpgradeDatabase()
		{
			var sqlUpdateConnection = ConfigurationManager.AppSettings[SqlUpdater.ConnectionKey];
			if (!String.IsNullOrEmpty(sqlUpdateConnection))
			{
				var sqlUpdater = new SqlUpdater();
				sqlUpdater.Run();
			}
		}

		protected static void CreateDefaultUsers()
		{
			var usernames = ConfigurationManager.AppSettings[WebConstants.CreateDefaultUsersKey].DefaultTo(string.Empty).Split(',').Where(x => x.HasText());
			if (usernames.Any())
			{
				const string password = "Password1";
				const string question = "What was the name of your first pet?";
				var userDao = ServiceFactory.Instance.GetService<IUserDao>();
				foreach (var username in usernames)
				{
					var user = userDao.FetchByUsername(username);
					if (user != null && Membership.GetUser(user.Username) == null)
					{
						MembershipCreateStatus status;
						Membership.CreateUser(user.Username, password, user.Email, question, Membership.GeneratePassword(10, 1), true, out status);
					}
				}
			}
		}

		#endregion
	}
}