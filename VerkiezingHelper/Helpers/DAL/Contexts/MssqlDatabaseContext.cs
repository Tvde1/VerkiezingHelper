using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using VerkiezingHelper.Helpers.Objects;

namespace VerkiezingHelper.Helpers.DAL.Contexts
{
    public class MssqlDatabaseContext : IDatabaseContext
    {
        public void Save(Party party)
        {
            throw new NotImplementedException();
        }

        public void Save(Coalition coalition)
        {
            throw new NotImplementedException();
        }

        public void Save(Election election)
        {
            throw new NotImplementedException();
        }

        public void Delete(Party party)
        {
            throw new NotImplementedException();
        }

        public void Delete(Coalition coalition)
        {
            throw new NotImplementedException();
        }

        public void Delete(Election election)
        {
            throw new NotImplementedException();
        }

        public Election GetElection(string electionName)
        {
            var query = new SqlCommand("SELECT * FROM Election WHERE Name = @name");
            query.Parameters.AddWithValue("@name", electionName);
            var data = DatabaseHandler.GetData(query);
            return data.Rows.Count == 0 ? null : ObjectFactory.CreateElection(data.Rows[0]);
        }

        public List<Party> GetParties(int electionId)
        {
            var query =
                $@"
SELECT Party.PartyPk, Party.Name, Party.LeadCandidate, ElectionParty.AmountOfVotes, ElectionParty.AmountOfSeats, ElectionParty.PercentOfVotes 
FROM Party 
INNER JOIN 
ElectionParty ON Party.PartyPk = ElectionParty.PartyCk
WHERE PartyPk = {electionId}";

            var data = DatabaseHandler.GetData(query);
            return ObjectFactory.CreateList(data, ObjectFactory.CreateParty);
        }

        public List<Coalition> GetCoalitions(int electionId)
        {
            var query =
                $@"
SELECT
Coalition.CoalitionPk, Coalition.ElectionFk, Coalition.PresidentFk, Coalition.Name
FROM Coalition
INNER JOIN CoalitionParty ON Coalition.CoalitionPk = CoalitionParty.CoalitionCk
WHERE PartyPk = {electionId}";

            var data = DatabaseHandler.GetData(query);
            return ObjectFactory.CreateList(data, ObjectFactory.CreateCoalition);
        }
    }
}