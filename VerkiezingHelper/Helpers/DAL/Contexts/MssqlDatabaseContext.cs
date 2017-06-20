using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using VerkiezingHelper.Helpers.Objects;

namespace VerkiezingHelper.Helpers.DAL.Contexts
{
    public class MssqlDatabaseContext : IDatabaseContext
    {
        public void Save(Party party)
        {
            if (party?.Id == null) return;

            var query = new SqlCommand(
                $"UPDATE Party SET Name = @name, LeadCandidate = @lc WHERE PartyPk = {party.Id}");
            query.Parameters.AddWithValue("@name", party.Name);
            query.Parameters.AddWithValue("@lc", party.LeadCandidate);
            DatabaseHandler.ExecuteQuery(query);
        }

        public void Save(Coalition coalition)
        {
            if (coalition?.Id == null) return;

            var query = new SqlCommand($"UPDATE Coalition SET Name = @name WHERE CoalitionPk = {coalition.Id}");
            query.Parameters.AddWithValue("@name", coalition.Name);
            DatabaseHandler.ExecuteQuery(query);

            var currentParties =
                DatabaseHandler.GetData(new SqlCommand(
                    $"SELECT PartyCk FROM CoalitionParty WHERE CoalitionCk = {coalition.Id}"));

            foreach (DataRow dataRow in currentParties.Rows)
                if (coalition.Parties.All(x => x.Id != (int) dataRow["PartyCk"]))
                    DatabaseHandler.ExecuteQuery(
                        new SqlCommand(
                            $"DELETE FROM CoalitionParty WHERE CoalitionCk = {coalition.Id} AND PartyCk = {(int) dataRow.ItemArray[0]}"));

            foreach (var party in coalition.Parties)
            {
                if (currentParties.Rows.Cast<DataRow>()
                    .Any(currentPartiesRow => (int) currentPartiesRow["PartyCk"] == party.Id))
                    continue;

                DatabaseHandler.ExecuteQuery(new SqlCommand(
                    $"INSERT INTO CoalitionParty (PartyCk,CoalitionCk) VALUES ({party.Id},{coalition.Id})"));
            }
        }

        public void Save(Election election)
        {
            if (election?.Id == null) return;

            var query = new SqlCommand(
                $"UPDATE Election SET Name = @name, Date = @date, AmountOfSeats = @seats WHERE ElectionPk = {election.Id}");
            query.Parameters.AddWithValue("@name", election.Name);
            if (election.Date == null) query.Parameters.AddWithValue("@date", DBNull.Value);
            else query.Parameters.AddWithValue("@date", election.Date);
            if (election.AmountOfSeats == null) query.Parameters.AddWithValue("@seats", DBNull.Value);
            else query.Parameters.AddWithValue("@seats", election.AmountOfSeats);

            DatabaseHandler.ExecuteQuery(query);
        }

        public void Delete(Party party)
        {
            if (party?.Id == null) return;
            DatabaseHandler.ExecuteQuery($"DELETE FROM Party WHERE PartyPk = {party.Id}");
        }

        public void Delete(Coalition coalition)
        {
            if (coalition?.Id == null) return;
            DatabaseHandler.ExecuteQuery($"DELETE FROM Coalition WHERE CoalitionPk = {coalition.Id}");
        }

        public void Delete(Election election)
        {
            if (election?.Id == null) return;
            DatabaseHandler.ExecuteQuery($"DELETE FROM Election WHERE ElectionPk = {election.Id}");
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
WHERE ElectionParty.ElectionCk = {electionId}";

            var data = DatabaseHandler.GetData(query);
            return ObjectFactory.CreateList(data, ObjectFactory.CreateParty);
        }

        public List<Coalition> GetCoalitions(int electionId)
        {
            var query =
                $@"
SELECT Coalition.CoalitionPk, Coalition.ElectionFk, Coalition.PresidentFk, Coalition.Name
FROM Coalition
INNER JOIN CoalitionParty ON Coalition.CoalitionPk = CoalitionParty.CoalitionCk
WHERE CoalitionParty.PartyCk = {electionId}";

            var data = DatabaseHandler.GetData(query);
            return ObjectFactory.CreateList(data, ObjectFactory.CreateCoalition);
        }

        public Election SaveNewElection(string electionName)
        {
            var query = new SqlCommand(
                "INSERT INTO Election (Name,Date,AmountOfSeats) VALUES (@name,null,null); SELECT SCOPE_IDENTITY() AS Id");
            query.Parameters.AddWithValue("@name", electionName);
            try
            {
                var data = DatabaseHandler.GetData(query);
                return new Election(int.Parse(data.Rows[0]["Id"].ToString()), electionName, null, null);
            }
            catch
            {
                return new Election(null, electionName, null, null);
            }
        }

        public List<string> GetAllElectionNames()
        {
            var query = "SELECT Name FROM Election";
            try
            {
                var data = DatabaseHandler.GetData(query);
                return data.Rows.Cast<DataRow>().Select(row => (string) row["Name"]).ToList();
            }
            catch
            {
                return new List<string>();
            }
        }

        public Party GetParty(int partyId, int electionId)
        {
            var query = $@"
SELECT Party.PartyPk, Party.Name, Party.LeadCandidate, ElectionParty.AmountOfVotes, ElectionParty.AmountOfSeats, ElectionParty.PercentOfVotes 
FROM Party 
INNER JOIN 
ElectionParty ON Party.PartyPk = ElectionParty.PartyCk
WHERE ElectionParty.ElectionCk = {electionId} AND ElectionParty.PartyCk = {partyId}";
            var data = DatabaseHandler.GetData(query);

            return data.Rows.Count == 0 ? null : ObjectFactory.CreateParty(data.Rows[0]);
        }
    }
}