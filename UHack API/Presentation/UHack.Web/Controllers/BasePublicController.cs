using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using UHack.Core.Infrastructure;
using UHack.Web.Framework;
using UHack.Web.Framework.Controllers;
using UHack.Web.Framework.Security;
using UHack.Core;

namespace UHack.Web.Controllers
{
    [AppHttpsRequirement(SslRequirement.NoMatter)]
    public abstract partial class BasePublicController : BaseController
    {
        public BasePublicController()
        {
            
        }

        protected virtual ActionResult InvokeHttp404()
        {
            // Call target Controller and pass the routeData.
            //IController errorController = EngineContext.Current.Resolve<CommonController>();

            var routeData = new RouteData();
            routeData.Values.Add("controller", "Common");
            routeData.Values.Add("action", "PageNotFound");

            //errorController.Execute(new RequestContext(this.HttpContext, routeData));

            return new EmptyResult();
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Request.Cookies.AllKeys.Contains("timezoneoffset"))
            {
                Session["timezoneoffset"] = HttpContext.Request.Cookies["timezoneoffset"].Value;
            }

            ActionDescriptor actionDescriptor = filterContext.ActionDescriptor;
            string actionName = actionDescriptor.ActionName;
            string controllerName = actionDescriptor.ControllerDescriptor.ControllerName;
            GlobalValues.ActionName = actionName;
            GlobalValues.ControllerName = controllerName;

            base.OnActionExecuting(filterContext);
        }

    }
}