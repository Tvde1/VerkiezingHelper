using System.Web.Mvc;
using VerkiezingHelper.Models;

namespace VerkiezingHelper.Controllers
{
    public class DefaultController : Controller
    {
        public ActionResult Open()
        {
            var openModel = new OpenModel();
            return null;
        }

        // GET: Default
        public ActionResult Index()
        {
            return View();
        }
    }
}