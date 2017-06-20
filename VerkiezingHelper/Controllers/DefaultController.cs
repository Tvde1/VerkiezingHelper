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
            var election = Session["Election"] as Election;
            if (election == null)
                return RedirectToAction("Open");

            var model = new IndexModel {Election = election};

            return View(model);
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

        public ActionResult SaveElection(IndexModel model)
        {
            var election = Session["Election"] as Election;
            if (election == null)
                return RedirectToAction("Open");

            election.AmountOfSeats = model.Election.AmountOfSeats;
            election.Name = model.Election.Name;

            election.Save();
            Session["Election"] = election; //TODO: Maybe debug if this is really needed.

            return RedirectToAction("Index");
        }

        public ActionResult PartyDetails(string id)
        {
            int idInt;
            if (!int.TryParse(id, out idInt))
                return RedirectToAction("Index");

            var election = Session["Election"] as Election;
            if (election == null)
                return RedirectToAction("Index");

            var model = new PartyModel();
            return View(model);
        }

        public ActionResult SaveParty(PartyModel model)
        {
            var election = Session["Election"] as Election;
            if (election == null)
                return RedirectToAction("Open");


            Party party = null;
            if (election.Id == null || model.Party.Id == null)
                return RedirectToAction("Index");

            try
            {
                party = new Repository().GetParty(model.Party.Id.Value, election.Id.Value);
            }
            catch
            {
                return RedirectToAction("Index");
            }

            party.Name = model.Party.Name;
            party.LeadCandidate = model.Party.LeadCandidate;

            party.Save();

            return RedirectToAction("Index");
        }
    }
}