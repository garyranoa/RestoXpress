using System.Web.Mvc;
using System.Web.Routing;
using UHack.Web.Framework.Localization;
using UHack.Web.Framework.Mvc.Routes;

namespace UHack.Web.Infrastructure
{
    public partial class RouteProvider : IRouteProvider
    {

        public void RegisterRoutes(RouteCollection routes)
        {

            //home page
            routes.MapLocalizedRoute("HomePage", "", new { controller = "Home", action = "Index" }, new[] { "UHack.Web.Controllers" });

            #region Common Controller

            routes.MapLocalizedRoute("ErrorPage", "page-error", new { controller = "Common", action = "ErrorPage" }, new[] { "UHack.Web.Controllers" });
            routes.MapLocalizedRoute("AsyncUpload", "async-upload", new { controller = "Common", action = "AsyncUpload" }, new[] { "UHack.Web.Controllers" });
            routes.MapLocalizedRoute("AsyncUpload_", "async-upload_", new { controller = "Common", action = "AsyncUpload_" }, new[] { "UHack.Web.Controllers" });
            
            routes.MapLocalizedRoute("PageNotFound", "page-not-found", new { controller = "Common", action = "PageNotFound" }, new[] { "UHack.Web.Controllers" });
            routes.MapLocalizedRoute("ClearCache", "cache-clear", new { controller = "Common", action = "ClearCache" }, new[] { "UHack.Web.Controllers" });
            routes.MapLocalizedRoute("ClearCacheByKey", "cache-clear-key", new { controller = "Common", action = "ClearCacheByKey" }, new[] { "UHack.Web.Controllers" });
            routes.MapLocalizedRoute("RestartApplication", "application-restart", new { controller = "Common", action = "RestartApplication" }, new[] { "UHack.Web.Controllers" });
           

            routes.MapLocalizedRoute("KeepAlive", "keep-alive", new { controller = "KeepAlive", action = "Index" }, new[] { "UHack.Web.Controllers" });

            #endregion


        }

        public int Priority
        {
            get
            {
                return 0;
            }
        }
    }
}