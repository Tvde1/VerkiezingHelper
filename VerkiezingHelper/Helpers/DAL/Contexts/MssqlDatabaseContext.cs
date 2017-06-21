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
        public void Save(Party party, int? electionId)
        {
            if (party?.Id == null || electionId == null) return;

            var query = new SqlCommand(
                $"UPDATE Party SET Name = @name, LeadCandidate = @lc WHERE PartyPk = {party.Id}");
            query.Parameters.AddWithValue("@name", party.Name);
            query.Parameters.AddWithValue("@lc", party.LeadCandidate);
            DatabaseHandler.ExecuteQuery(query);

            var query2 =
                $"UPDATE ElectionParty SET AmountOfVotes = {party.AmountOfVotes}, AmountOfSeats = {party.AmountOfSeats}, PercentOfVotes = {party.PercentOfVotes} WHERE PartyCk = {party.Id} AND ElectionCk = {electionId}";
            DatabaseHandler.ExecuteQuery(query2);
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
SELECT CoalitionPk, PresidentFk, Name, ElectionFk
FROM Coalition
WHERE ElectionFk = {electionId}";

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

        public Party GetParty(int partyId, int? electionId)
        {
            var query = $@"
SELECT          Party.PartyPk, Party.Name, Party.LeadCandidate, ElectionParty.AmountOfVotes, ElectionParty.AmountOfSeats, ElectionParty.PercentOfVotes, ElectionParty.ElectionCk
FROM            Party 
LEFT OUTER JOIN ElectionParty
ON              Party.PartyPk = ElectionParty.PartyCk
WHERE          (Party.PartyPk = {partyId})";
            var data = DatabaseHandler.GetData(query);

            DataRow row;
            if (data.Rows.Count > 1)
                row = data.Rows.Cast<DataRow>().FirstOrDefault(r => (int) r["ElectionCk"] == electionId) ??
                      data.Rows[0];
            else if (data.Rows.Count == 1)
                row = data.Rows[0];
            else
                return null;

            return ObjectFactory.CreateParty(row);
        }

        public List<Party> GetAllParties()
        {
            return ObjectFactory.CreateList(DatabaseHandler.GetData("SELECT * FROM Party"), ObjectFactory.CreateParty);
        }

        public Party CreateParty()
        {
            var query = "INSERT INTO Party (Name,LeadCandidate) VALUES ('',''); SELECT SCOPE_IDENTITY() as Id";
            var data = DatabaseHandler.GetData(query);
            return data.Rows.Count == 0
                ? null
                : new Party(int.Parse(((decimal) data.Rows[0]["Id"]).ToString()), "", null, 0, 0, 0);
        }

        public void AddPartyToElection(Party party, int electionId)
        {
            var query =
                $"INSERT INTO ElectionParty (PartyCk,ElectionCk,AmountOfVotes,AmountOfSeats,PercentOfVotes) VALUES ({party.Id},{electionId},{party.AmountOfVotes},{party.AmountOfSeats},{party.PercentOfVotes})";
            try
            {
                DatabaseHandler.ExecuteQuery(query);
            }
            catch
            {
                // ignored
            }
        }

        public Coalition GetCoalition(string name, int? electionId)
        {
            var query = new SqlCommand("SELECT * FROM Coalition WHERE Name = @name");
            query.Parameters.AddWithValue("@name", name);
            var data = DatabaseHandler.GetData(query);
            return data.Rows.Count == 0 ? null : ObjectFactory.CreateCoalition(data.Rows[0]);
        }

        public int? AddCoalitionToElection(Coalition coalition, int? electionId)
        {
            var query = new SqlCommand(
                $"INSERT INTO Coalition (ElectionFk,PresidentFk,Name) VALUES ({electionId},{coalition.President.Id},@name); SELECT SCOPE_IDENTITY() as Id");
            query.Parameters.AddWithValue("@name", coalition.Name);
            try
            {
                var data = DatabaseHandler.GetData(query);
                return data.Rows.Count == 0 ? (int?) null : Convert.ToInt32(((decimal) data.Rows[0]["Id"]).ToString());
            }
            catch
            {
                return null;
            }
        }

        public List<Party> GetParties(Coalition coalition)
        {
            var query = $@"
SELECT        Party.PartyPk, Party.Name, Party.LeadCandidate, ElectionParty.AmountOfSeats, ElectionParty.AmountOfVotes, ElectionParty.PercentOfVotes
FROM          CoalitionParty
INNER JOIN    Party ON CoalitionParty.PartyCk = Party.PartyPk
INNER JOIN    Coalition ON CoalitionParty.CoalitionCk = Coalition.CoalitionPk
INNER JOIN    ElectionParty ON Party.PartyPk = ElectionParty.PartyCk AND Coalition.ElectionFk = ElectionParty.ElectionCk
WHERE        (Coalition.CoalitionPk = {coalition.Id})";

            var data = DatabaseHandler.GetData(query);
            return ObjectFactory.CreateList(data, ObjectFactory.CreateParty);
        }

        public List<Party> GetParties(string[] dataParties, int electionId)
        {
            var query = new SqlCommand($@"
SELECT          Party.PartyPk, Party.Name, Party.LeadCandidate, ElectionParty.AmountOfVotes, ElectionParty.AmountOfSeats, ElectionParty.PercentOfVotes, ElectionParty.ElectionCk
FROM            Party 
LEFT OUTER JOIN ElectionParty
ON              Party.PartyPk = ElectionParty.PartyCk
WHERE          (ElectionParty.ElectionCk = {electionId}) AND (Party.Name IN ('{string.Join(",", dataParties)}'))");

            var data = DatabaseHandler.GetData(query);
            return ObjectFactory.CreateList(data, ObjectFactory.CreateParty);
        }
    }
}