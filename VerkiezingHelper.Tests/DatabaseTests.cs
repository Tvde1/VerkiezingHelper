using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerkiezingHelper.Helpers.DAL;
using VerkiezingHelper.Helpers.Objects;

namespace VerkiezingHelper.Tests
{
    [TestClass]
    public class DatabaseTests
    {
        private readonly Repository _repository = new Repository();

        [TestMethod]
        public void TestBadPartyRequest()
        {
            Assert.IsNull(_repository.GetParty(99999999, null, false));
            Assert.IsNotNull(_repository.GetParty(99999999, null, true));
            Assert.IsNull(_repository.GetParty(99999999, 9999999, false));
            Assert.IsNotNull(_repository.GetParty(99999999, 9999999, true));
            Assert.IsNull(_repository.GetParty(-99999999, 9999999, false));
            DeleteEmptyParties();
        }

        [TestMethod]
        public void TestGetWithoutId()
        {
            var coalitie = new Coalition(null, "Naam", null);
            _repository.AddCoalitionToElection(coalitie, null);
            coalitie.Save();

            var coalitie1 = _repository.GetCoalition("Naam", null);
            Assert.IsNull(coalitie1);
        }

        public void DeleteEmptyParties()
        {
            DatabaseHandler.ExecuteQuery("DELETE FROM Party WHERE Name = '' and LeadCandidate = ''");
        }
    }
}