using System;
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
    }
}