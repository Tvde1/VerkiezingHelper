using System;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using VerkiezingHelper.Helpers;
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

            var parties = election.Repository.GetAllParties();

            var model = new IndexModel {Election = election, AllParties = parties};

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

            if (election.AmountOfSeats == null)
                throw new Exception("man");

            var totalVotes = model.Election.Parties.Sum(x => x.AmountOfVotes);

            foreach (var modelParty in model.Election.Parties)
            {
                var electionParty = election.Parties.FirstOrDefault(x => x.Name == modelParty.Name);
                if (electionParty == null) throw new Exception("fugg :DDDDDD");
                electionParty.AmountOfVotes = modelParty.AmountOfVotes;
                var percent = totalVotes == 0
                    ? decimal.Divide(1, model.Election.Parties.Count)
                    : decimal.Divide(modelParty.AmountOfVotes, totalVotes);
                electionParty.PercentOfVotes = (double) Math.Round(percent * 100, 2);
                electionParty.AmountOfSeats = (int) Math.Round(percent * election.AmountOfSeats.Value);
                electionParty.Save(election.Id);
            }

            election.Save();
            Session["Election"] = election; //TODO: Maybe debug if this is really needed.

            return RedirectToAction("Index");
        }


        public ActionResult PartyDetails(string id)
        {
            var election = Session["Election"] as Election;
            if (election == null)
                return RedirectToAction("Index");

            var temp = TempData["PartyModel"] as PartyModel;
            if (temp != null)
                return View(temp);

            int idInt;
            var isNew = !int.TryParse(id, out idInt);

            var model = new PartyModel();
            model.Party = election.Repository.GetParty(idInt, election.Id, isNew);
            return View(model);
        }

        [HttpPost]
        public ActionResult SaveParty(PartyModel model)
        {
            var election = Session["Election"] as Election;
            if (election == null)
                return RedirectToAction("Open");

            Party party;
            if (election.Id == null || model.Party.Id == null)
                return RedirectToAction("Index");

            try
            {
                party = election.Repository.GetParty(model.Party.Id.Value, election.Id, false);
            }
            catch
            {
                return RedirectToAction("Index");
            }

            if (string.IsNullOrEmpty(model.Party.LeadCandidate) || string.IsNullOrEmpty(model.Party.Name))
            {
                var newModel = new PartyModel
                {
                    Error = "Check uw gegevens en probeer opnieuw.",
                    Party = party
                };
                TempData["PartyModel"] = newModel;
                return RedirectToAction("PartyDetails");
            }


            election.Repository.AddPartyToElection(model.Party, election.Id);

            party.Name = model.Party.Name;
            party.LeadCandidate = model.Party.LeadCandidate;

            party.Save(election.Id);

            election.UpdateData();

            return RedirectToAction("Index");
        }

        public ActionResult NewParty()
        {
            return RedirectToAction("PartyDetails");
        }

        public ActionResult AddToParty(int? id)
        {
            var election = Session["Election"] as Election;
            if (election == null || id == null)
                return RedirectToAction("Open");

            Party party;

            try
            {
                party = election.Repository.GetParty(id.Value, election.Id, false);
            }
            catch
            {
                return RedirectToAction("Index");
            }

            election.Repository.AddPartyToElection(party, election.Id);
            election.UpdateData();
            //TempData["Election"] = election;
            return RedirectToAction("Index");
        }

        public ActionResult CoalitionDetails(string name)
        {
            var election = Session["Election"] as Election;
            if (election == null)
                return RedirectToAction("Open");

            var coalition = election.Repository.GetCoalition(name, election.Id);

            if (coalition == null)
                return RedirectToAction("Index");

            var model = new CoalitionModel {Coalition = coalition, NewName = coalition?.Name};
            return View(model);
        }

        [HttpPost]
        public ActionResult SaveCoalition(CoalitionModel model)
        {
            var election = Session["Election"] as Election;
            if (election == null)
                return RedirectToAction("Open");

            Coalition coalition;
            if (election.Id == null || model.Coalition.Id == null)
                return RedirectToAction("Index");

            try
            {
                coalition = election.Repository.GetCoalition(model.Coalition.Name, election.Id);
            }
            catch
            {
                return RedirectToAction("Index");
            }


            coalition.Name = model.NewName;

            coalition.Save();
            election.UpdateData();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult CreateCoalition(string json)
        {
            var election = Session["Election"] as Election;
            if (election == null)
                return RedirectToAction("Open");

            var data = JsonConvert.DeserializeObject<CoalitionJsonModel>(json);
            //var parties = election.Repository.GetParties(data.Parties, election.Id);

            var parties = election.Parties.Where(x => data.Parties.Contains(x.Name.RemoveWhitespace())).ToList();

            var coalition = new Coalition(data.Name, parties, election.Id);

            coalition.Id = election.Repository.AddCoalitionToElection(coalition, election.Id);
            coalition.Save();
            return RedirectToAction("CoalitionDetails", new {name = coalition.Name});
        }

        public ActionResult Export(CoalitionModel model)
        {
            var election = Session["Election"] as Election;
            if (election == null)
                return RedirectToAction("Open");

            var coalition = election.Repository.GetCoalition(model.Coalition.Name, election.Id);
            var jsonString = JsonConvert.SerializeObject(coalition, Formatting.Indented);
            return Content(jsonString, "text/html");
        }
    }
}