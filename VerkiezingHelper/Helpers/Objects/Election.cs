using System;
using System.Collections.Generic;

namespace VerkiezingHelper.Helpers.Objects
{
    public class Election : BaseObject
    {
        private int?            _amountOfSeats;
        private List<Coalition> _coalitions;
        private DateTime?       _date;
        private List<Party>     _parties;

        public Election(int id, string name, int? amountOfSeats, DateTime? date) : base(id, name)
        {
            _amountOfSeats = amountOfSeats;
            _date = date;
            _parties = Repository.GetParties(Id);
            _coalitions = Repository.GetCoalitions(Id);
        }

        public void CreateCoalition()
        {
            throw new NotImplementedException();
        }

        public void HandleError()
        {
            throw new NotImplementedException();
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override void Delete()
        {
            throw new NotImplementedException();
        }
    }
}