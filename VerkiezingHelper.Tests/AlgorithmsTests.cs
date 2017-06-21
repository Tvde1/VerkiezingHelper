using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerkiezingHelper.Helpers.Objects;

namespace VerkiezingHelper.Tests
{
    [TestClass]
    public class AlgorithmsTests
    {
        [TestMethod]
        public void TestChoosePresident()
        {
            var party1 = new Party(0, "Partij 1", "Meneer 1", 100, 0, 0);
            var party2 = new Party(1, "Partij 2", "Mevrouw 2", 120, 0, 0);
            var party3 = new Party(2, "Partij 3", "Meneer 30", 90, 0, 0);
            var coalition = new Coalition("Coalitie 1", new List<Party> {party1, party2, party3}, null);
            Assert.AreEqual(coalition.President, party2);
        }
    }
}