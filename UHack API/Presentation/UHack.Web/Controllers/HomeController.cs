using System;
using System.Web;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Net;

namespace UHack.Web.Controllers
{
    public partial class HomeController : BasePublicController
    {

        public HomeController()
        {
;
        }


        public ActionResult Index()
        {
            return Content("Home Page");
        }

    }
}