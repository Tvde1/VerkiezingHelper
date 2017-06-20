using System;
using System.Collections.Generic;

namespace VerkiezingHelper.Helpers.Objects
{
    public class Election : BaseObject
    {
        public Election(int? id, string name, int? amountOfSeats, DateTime? date) : base(id, name)
        {
            AmountOfSeats = amountOfSeats;
            Date = date;
            if (Id == null) return;
            Parties = Repository.GetParties(Id.Value);
            Coalitions = Repository.GetCoalitions(Id.Value);
        }

        public int? AmountOfSeats { get; }
        public List<Coalition> Coalitions { get; }
        public DateTime? Date { get; }
        public List<Party> Parties { get; }

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
            Repository.Save(this);
        }

        public override void Delete()
        {
            Repository.Delete(this);
        }
    }
}