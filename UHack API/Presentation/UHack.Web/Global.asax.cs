using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using FluentValidation.Mvc;
using UHack.Core;
using UHack.Core.Data;
using UHack.Core.Domain;
///using UHack.Core.Domain.Common;
using UHack.Core.Infrastructure;
using UHack.Services.Logging;
using UHack.Services.Tasks;
using UHack.Web.Controllers;
using UHack.Web.Framework;
using UHack.Web.Framework.Mvc;
using UHack.Web.Framework.Mvc.Routes;
using StackExchange.Profiling;
using System.Web.Helpers;
using System.Web.Optimization;
using StackExchange.Profiling.EntityFramework6;

using Autofac;
using Autofac.Integration.SignalR;
using Microsoft.AspNet.SignalR;
using System.Reflection;
using StackExchange.Profiling.Mvc;

namespace UHack.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {  
        public static void RegisterRoutes(RouteCollection routes)
        {
            //routes.IgnoreRoute("favicon.ico");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //register custom routes (plugins, etc)
            var routePublisher = EngineContext.Current.Resolve<IRoutePublisher>();
            routePublisher.RegisterRoutes(routes);

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "UHack.Web.Controllers" }
            );
        }

        protected void Application_Start()
        {
            //GlobalFilters.Filters.Add(new ProfilingActionFilter());
            //MiniProfilerEF6.Initialize();

            AreaRegistration.RegisterAllAreas();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalConfiguration.Configure(WebApiConfig.Register);

            //initialize engine context
            EngineContext.Initialize(false);

            bool databaseInstalled = DataSettingsHelper.DatabaseIsInstalled();
            //bool databaseInstalled = true;
            if (databaseInstalled) 
            {
                //remove all view engines
                //ViewEngines.Engines.Clear();
                //except the themeable razor view engine we use
                //http://www.siddharthpandey.net/speed-up-asp-net-mvc-view-rendering-process/
                //IViewEngine razorEngine = new RazorViewEngine() { FileExtensions = new string[] { "cshtml" } };
                //ViewEngines.Engines.Add(razorEngine);
                
            }

            AntiForgeryConfig.SuppressXFrameOptionsHeader = true;

            //Add some functionality on top of the default ModelMetadataProvider
            //ModelMetadataProviders.Current = new NopMetadataProvider();

            //Registering some regular mvc stuff
            //AreaRegistration.RegisterAllAreas();
            RegisterRoutes(RouteTable.Routes);

            //fluent validation
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
            ModelValidatorProviders.Providers.Add(new FluentValidationModelValidatorProvider(new AppValidatorFactory()));

            //start scheduled tasks
            if (databaseInstalled)
            {
                TaskManager.Instance.Initialize();
                TaskManager.Instance.Start();
            }

            //log application start
            if (databaseInstalled)
            {
                try
                {
                    //log
                    var logger = EngineContext.Current.Resolve<ILogger>();
                    logger.Information("Application started", null, null);
                }
                catch (Exception)
                {
                    //don't throw new exception if occurs
                }
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            Response.Cache.SetValidUntilExpires(false);
            Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            Response.Cache.SetNoStore();

            //miniprofiler
            MiniProfiler.Start();
            if (Request.IsLocal)
            {
                
            }
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            //miniprofiler
            MiniProfiler.Stop();
          
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            var exception = Server.GetLastError();

            //log error
            LogException(exception);

            //process 404 HTTP errors
            var httpException = exception as HttpException;
            if (httpException != null && httpException.GetHttpCode() == 404)
            {
                var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                if (!webHelper.IsStaticResource(this.Request))
                {
                    Response.Clear();
                    Server.ClearError();
                    Response.TrySkipIisCustomErrors = true;

      
                }
            }
        }

        protected void LogException(Exception exc)
        {
            if (exc == null)
                return;

            //ignore 404 HTTP errors
            var httpException = exc as HttpException;
            if (httpException != null && httpException.GetHttpCode() == 404)
                return;

            try
            {
                //log
                var logger = EngineContext.Current.Resolve<ILogger>();
                var workContext = EngineContext.Current.Resolve<IWorkContext>();
                if (workContext != null && workContext.CurrentUser != null)
                    logger.Fatal(exc.Message, exc, workContext.CurrentUser);
                else
                    logger.Fatal(exc.Message, exc);
            }
            catch (Exception)
            {
                //don't throw new exception if occurs
            }
        }
    }
}