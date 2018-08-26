using System.Web.Http;
using System.Web.Mvc;

namespace UHack.Web.Areas.HelpPage
{
    public class HelpPageAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "HelpPage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "HelpPageDefault",
                "api-help/{action}/{apiId}",
                new { controller = "Help", action = "Index", apiId = UrlParameter.Optional },
                new[] { "UHack.Web.Areas.HelpPage.Controllers" });

            HelpPageConfig.Register(GlobalConfiguration.Configuration);
        }
    }
}