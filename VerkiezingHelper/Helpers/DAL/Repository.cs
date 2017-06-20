using System.Collections.Generic;
using VerkiezingHelper.Helpers.DAL.Contexts;
using VerkiezingHelper.Helpers.Objects;

namespace VerkiezingHelper.Helpers.DAL
{
    public class Repository
    {
        private readonly IDatabaseContext _databaseContext = new MssqlDatabaseContext();
        private readonly IExportContext _exportContext = new TxtExportContext();

        public void Save(Party party)
        {
            _databaseContext.Save(party);
        }

        public void Save(Coalition coalition)
        {
            _databaseContext.Save(coalition);
        }

        public void Save(Election election)
        {
            _databaseContext.Save(election);
        }

        public void Delete(Party party)
        {
            _databaseContext.Delete(party);
        }

        public void Delete(Coalition coalition)
        {
            _databaseContext.Delete(coalition);
        }

        public void Delete(Election election)
        {
            _databaseContext.Delete(election);
        }

        public void Export(Election election)
        {
            _exportContext.Export();
        }

        public Election LoadElection(string electionName)
        {
            if (electionName == null) return null;
            return _databaseContext.GetElection(electionName);
        }

        public List<Party> GetParties(int electionId)
        {
            return _databaseContext.GetParties(electionId);
        }

        public List<Coalition> GetCoalitions(int electionId)
        {
            return _databaseContext.GetCoalitions(electionId);
        }
    }
}