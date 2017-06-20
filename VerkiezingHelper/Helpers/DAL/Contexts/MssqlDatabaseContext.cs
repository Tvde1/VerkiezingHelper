using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.EnterpriseServices.Internal;
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
            if (data.Rows.Count == 0) return null;
            return ObjectFactory.CreateElection(data.Rows[0]);
        }

        public List<Party> GetParties(int id)
        {
            throw new NotImplementedException();
        }
    }
}