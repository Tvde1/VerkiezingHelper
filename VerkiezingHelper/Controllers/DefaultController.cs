using System.Web.Mvc;
using VerkiezingHelper.Helpers.DAL;
using VerkiezingHelper.Models;

namespace VerkiezingHelper.Controllers
{
    public class DefaultController : Controller
    {
        public ActionResult Open()
        {
            var openModel = new OpenModel();
            throw new System.NotImplementedException();
        }

        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Load(string electionName)
        {
            var repository = new Repository();
            var eletion = repository.LoadElection(electionName);
            throw new System.NotImplementedException();
        }
    }
}