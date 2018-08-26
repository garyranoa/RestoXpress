using System;
using System.Web;
using System.Web.Routing;
using UHack.Core;
using UHack.Core.Data;
using UHack.Core.Infrastructure;
using UHack.Services.Events;
using UHack.Services.Seo;
using UHack.Web.Framework.Localization;

namespace UHack.Web.Framework.Seo
{
    /// <summary>
    /// Provides properties and methods for defining a SEO friendly route, and for getting information about the route.
    /// </summary>
    public partial class GenericPathRoute : LocalizedRoute
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the System.Web.Routing.Route class, using the specified URL pattern and handler class.
        /// </summary>
        /// <param name="url">The URL pattern for the route.</param>
        /// <param name="routeHandler">The object that processes requests for the route.</param>
        public GenericPathRoute(string url, IRouteHandler routeHandler)
            : base(url, routeHandler)
        {
        }

        /// <summary>
        /// Initializes a new instance of the System.Web.Routing.Route class, using the specified URL pattern, handler class and default parameter values.
        /// </summary>
        /// <param name="url">The URL pattern for the route.</param>
        /// <param name="defaults">The values to use if the URL does not contain all the parameters.</param>
        /// <param name="routeHandler">The object that processes requests for the route.</param>
        public GenericPathRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler)
            : base(url, defaults, routeHandler)
        {
        }

        /// <summary>
        /// Initializes a new instance of the System.Web.Routing.Route class, using the specified URL pattern, handler class, default parameter values and constraints.
        /// </summary>
        /// <param name="url">The URL pattern for the route.</param>
        /// <param name="defaults">The values to use if the URL does not contain all the parameters.</param>
        /// <param name="constraints">A regular expression that specifies valid values for a URL parameter.</param>
        /// <param name="routeHandler">The object that processes requests for the route.</param>
        public GenericPathRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, IRouteHandler routeHandler)
            : base(url, defaults, constraints, routeHandler)
        {
        }

        /// <summary>
        /// Initializes a new instance of the System.Web.Routing.Route class, using the specified URL pattern, handler class, default parameter values, 
        /// constraints,and custom values.
        /// </summary>
        /// <param name="url">The URL pattern for the route.</param>
        /// <param name="defaults">The values to use if the URL does not contain all the parameters.</param>
        /// <param name="constraints">A regular expression that specifies valid values for a URL parameter.</param>
        /// <param name="dataTokens">Custom values that are passed to the route handler, but which are not used to determine whether the route matches a specific URL pattern. The route handler might need these values to process the request.</param>
        /// <param name="routeHandler">The object that processes requests for the route.</param>
        public GenericPathRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens, IRouteHandler routeHandler)
            : base(url, defaults, constraints, dataTokens, routeHandler)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns information about the requested route.
        /// </summary>
        /// <param name="httpContext">An object that encapsulates information about the HTTP request.</param>
        /// <returns>
        /// An object that contains the values from the route definition.
        /// </returns>
        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            RouteData data = base.GetRouteData(httpContext);
            if (data != null && DataSettingsHelper.DatabaseIsInstalled())
            {
                var urlRecordService = EngineContext.Current.Resolve<IUrlRecordService>();
                
                var slug = data.Values["generic_se_name"] as string;

                if (slug == null)
                    slug = data.Values["SeName"] as string;
                //performance optimization.
                //we load a cached verion here. it reduces number of SQL requests for each page load
                var urlRecord = urlRecordService.GetBySlugCached(slug);
                //comment the line above and uncomment the line below in order to disable this performance "workaround"
                //var urlRecord = urlRecordService.GetBySlug(slug);
                if (urlRecord == null)
                {
                    //no URL record found

                    //var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                    //var response = httpContext.Response;
                    //response.Status = "302 Found";
                    //response.RedirectLocation = webHelper.GetStoreLocation(false);
                    //response.End();
                    //return null;

                    if (data.Values["controller"] == "User" && data.Values["action"] == "ResetPasswordRedirect")
                    {
                        data.Values["controller"] = "User";
                        data.Values["action"] = "ResetPasswordRedirect";
                        data.Values["tokenId"] = data.Values["SeName"];
                        return data;
                    }

                    if (data.Values["controller"] == "Business" && data.Values["action"] == "BusinessDealRedemption")
                    {
                        data.Values["controller"] = "Business";
                        data.Values["action"] = "BusinessDealRedemption";
                        data.Values["deal"] = data.Values["Deal"];
                        return data;
                    }

                    if (data.Values["controller"] == "Common" && data.Values["action"] == "ErrorPage")
                    {
                        data.Values["controller"] = "Common";
                        data.Values["action"] = "ErrorPage";
                        return data;
                    }

                    data.Values["controller"] = "Common";
                    data.Values["action"] = "PageNotFound";
                    return data;
                }

                //ensre that URL record is active
                if (!urlRecord.IsActive)
                {
                    //URL record is not active. let's find the latest one
                    var activeSlug = urlRecordService.GetActiveSlug(urlRecord.EntityId, urlRecord.EntityName, urlRecord.LanguageId);
                    if (string.IsNullOrWhiteSpace(activeSlug))
                    {
                        //no active slug found

                        //var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                        //var response = httpContext.Response;
                        //response.Status = "302 Found";
                        //response.RedirectLocation = webHelper.GetStoreLocation(false);
                        //response.End();
                        //return null;

                        data.Values["controller"] = "Common";
                        data.Values["action"] = "PageNotFound";
                        return data;
                    }

                    //the active one is found
                    var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                    var response = httpContext.Response;
                    response.Status = "301 Moved Permanently";
                    response.RedirectLocation = string.Format("{0}{1}", webHelper.GetApplicationLocation(false), activeSlug);
                    response.End();
                    return null;
                }

                //ensure that the slug is the same for the current language
                //otherwise, it can cause some issues when customers choose a new language but a slug stays the same
                var workContext = EngineContext.Current.Resolve<IWorkContext>();
                var slugForCurrentLanguage = SeoExtensions.GetSeName(urlRecord.EntityId, urlRecord.EntityName, workContext.WorkingLanguage.Id);
                if (!String.IsNullOrEmpty(slugForCurrentLanguage) &&
                    !slugForCurrentLanguage.Equals(slug, StringComparison.InvariantCultureIgnoreCase))
                {
                    //we should make not null or "" validation above because some entities does not have SeName for standard (ID=0) language (e.g. news, blog posts)
                    var webHelper = EngineContext.Current.Resolve<IWebHelper>();
                    var response = httpContext.Response;
                    //response.Status = "302 Found";
                    response.Status = "302 Moved Temporarily";
                    response.RedirectLocation = string.Format("{0}{1}", webHelper.GetApplicationLocation(false), slugForCurrentLanguage);
                    response.End();
                    return null;
                }

                string controllerName = "";

                //process URL
                switch (urlRecord.EntityName.ToLowerInvariant())
                {
                    case "business":
                    {
                        controllerName = "Business";
                        if (data.Values["action"] as string == "BusinessLandingRegistration")
                            controllerName = "Public";

                        if (data.Values["action"] as string == "PostCard")
                                controllerName = "Merchant";
                        if (data.Values["action"] as string == "SeoReport")
                                controllerName = "Merchant";
                        if (data.Values["action"] as string == "TVAds")
                                controllerName = "Merchant";

                            data.Values["controller"] = controllerName;
                        data.Values["action"] = data.Values["action"];
                        data.Values["businessid"] = urlRecord.EntityId;
                        data.Values["SeName"] = urlRecord.Slug;
                    }
                    break;
                    case "businesstype":
                    {
                        data.Values["controller"] = "Business";
                        data.Values["action"] = "SearchByBusinessType";
                        data.Values["businessTypeId"] = urlRecord.EntityId;
                        data.Values["SeName"] = urlRecord.Slug;
                    }
                    break;
                    case "businesstypecategory":
                    {
                        data.Values["controller"] = "Business";
                        data.Values["action"] = "SearchByBusinessTypeCategory";
                        data.Values["categoryId"] = urlRecord.EntityId;
                        data.Values["SeName"] = urlRecord.Slug;
                    }
                    break;
                    default:
                        {
                            //no record found

                            //generate an event this way developers could insert their own types
                            EngineContext.Current.Resolve<IEventPublisher>()
                                .Publish(new CustomUrlRecordEntityNameRequested(data, urlRecord));
                        }
                        break;
                }
            }
            return data;
        }

        #endregion
    }
}
