using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VerkiezingHelper.Helpers.Objects
{
    public class Election : BaseObject
    {
        private int _amountOfSeats;
        private DateTime _date;
        private List<Party> _parties;
        private List<Coalition> _coalitions;

        public void CreateCoalition()
        {
            throw new NotImplementedException();
        }

        public void HandleError()
        {
            throw new NotImplementedException();
        }

        public Election(int id, string name) : base(id, name)
        {
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