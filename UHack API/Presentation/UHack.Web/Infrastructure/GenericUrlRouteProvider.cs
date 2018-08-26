using System.Web.Routing;
using UHack.Web.Framework.Localization;
using UHack.Web.Framework.Mvc.Routes;
using UHack.Web.Framework.Seo;

namespace UHack.Web.Infrastructure
{
    public partial class GenericUrlRouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {

  
        }

        public int Priority
        {
            get
            {
                //it should be the last route
                //we do not set it to -int.MaxValue so it could be overridden (if required)
                return -1000000;
            }
        }
    }
}