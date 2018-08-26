using System.Web.Mvc;
using System.Threading.Tasks;

namespace UHack.Web.Controllers
{
    public partial class KeepAliveController : Controller
    {
        public async Task<ActionResult> Index()
        {
            return Content("I am alive!");
        }
    }
}