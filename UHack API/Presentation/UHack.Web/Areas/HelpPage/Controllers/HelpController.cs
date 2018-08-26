using System;
using System.Web.Http;
using System.Web.Mvc;
using UHack.Web.Areas.HelpPage.ModelDescriptions;
using UHack.Web.Areas.HelpPage.Models;
using UHack.Core;
using UHack.Core.Domain.Users;

namespace UHack.Web.Areas.HelpPage.Controllers
{
    /// <summary>
    /// The controller that will handle requests for the help page.
    /// </summary>
    public class HelpController : Controller
    {
        private const string ErrorViewName = "Error";
        private readonly IWorkContext _workContext;

        public HelpController(IWorkContext workContext)
            : this(GlobalConfiguration.Configuration)
        {
            this._workContext = workContext;
        }

        public HelpController(HttpConfiguration config)
        {
            Configuration = config;
        }

        public HttpConfiguration Configuration { get; private set; }

        public ActionResult Index()
        {
            bool isAuthenticated = System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (!isAuthenticated)
                return RedirectToRoute("Login");

            var user = _workContext.CurrentUser;
            string userRole = Enum.GetName(typeof(Role), user.RoleId);
            if (userRole != "ADMIN")
                return RedirectToRoute("HomePage");

            ViewBag.DocumentationProvider = Configuration.Services.GetDocumentationProvider();
            return View(Configuration.Services.GetApiExplorer().ApiDescriptions);
        }

        public ActionResult Api(string apiId)
        {
            if (!String.IsNullOrEmpty(apiId))
            {
                HelpPageApiModel apiModel = Configuration.GetHelpPageApiModel(apiId);
                if (apiModel != null)
                {
                    return View(apiModel);
                }
            }

            return View(ErrorViewName);
        }

        public ActionResult ResourceModel(string modelName)
        {
            if (!String.IsNullOrEmpty(modelName))
            {
                ModelDescriptionGenerator modelDescriptionGenerator = Configuration.GetModelDescriptionGenerator();
                ModelDescription modelDescription;
                if (modelDescriptionGenerator.GeneratedModels.TryGetValue(modelName, out modelDescription))
                {
                    return View(modelDescription);
                }
            }

            return View(ErrorViewName);
        }
    }
}