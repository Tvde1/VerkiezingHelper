using System.Web.Mvc;
using VerkiezingHelper.Helpers.DAL;
using VerkiezingHelper.Helpers.Objects;
using VerkiezingHelper.Models;

namespace VerkiezingHelper.Controllers
{
    public class DefaultController : Controller
    {
        [HttpGet]
        public ActionResult Open()
        {
            var openModel = new OpenModel();
            return View(openModel);
        }

        // GET: Default
        public ActionResult Index()
        {
            if (!(Session["Election"] is Election election))
                return RedirectToAction("Open");

            var model = new IndexModel {Election = election};

            return View();
        }

        [HttpPost]
        public ActionResult Open(string electionName)
        {
            if (string.IsNullOrEmpty(electionName))
            {
                var model = new OpenModel {Error = "Je moet een naam opgeven."};
                return View(model);
            }

            var repository = new Repository();

            var election = repository.LoadElection(electionName);
            if (election == null)
                try
                {
                    election = repository.CreateNewElection(electionName);
                }
                catch
                {
                    election = new Election(null, electionName, null, null);
                }

            Session["Election"] = election;
            return RedirectToAction("Index");
        }
    }
}