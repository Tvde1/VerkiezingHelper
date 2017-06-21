using System.Collections.Generic;
using VerkiezingHelper.Helpers.Objects;

namespace VerkiezingHelper.Helpers.DAL.Contexts
{
    internal interface IDatabaseContext
    {
        void Save(Party party, int? electionId);
        void Save(Coalition coalition);
        void Save(Election election);
        void Delete(Party party);
        void Delete(Coalition coalition);
        void Delete(Election election);
        Election GetElection(string electionName);
        List<Party> GetParties(int electionId);
        List<Coalition> GetCoalitions(int electionId);
        Election SaveNewElection(string electionName);
        List<string> GetAllElectionNames();
        Party GetParty(int partyId, int? electionId);
        List<Party> GetAllParties();
        Party CreateParty();
        void AddPartyToElection(Party party, int electionId);
        Coalition GetCoalition(string name, int? electionId);
        int? AddCoalitionToElection(Coalition coalition, int? electionId);
        List<Party> GetParties(Coalition coalition);
        List<Party> GetParties(string[] dataParties, int electionId);
    }
}