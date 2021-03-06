﻿using System;
using System.Collections.Generic;

namespace VerkiezingHelper.Helpers.Objects
{
    public class Election : BaseObject
    {
        public Election() : base(null, null)
        {
        }

        public Election(int? id, string name, int? amountOfSeats, DateTime? date) : base(id, name)
        {
            AmountOfSeats = amountOfSeats;
            Date = date;
            if (Id == null) return;

            UpdateData();
        }

        public int? AmountOfSeats { get; set; }
        public List<Coalition> Coalitions { get; private set; }
        public DateTime? Date { get; }
        public List<Party> Parties { get; set; } = new List<Party>();

        public void CreateCoalition()
        {
            throw new NotImplementedException();
        }

        public /*override*/ void Save()
        {
            Repository.Save(this);
        }

        public override void Delete()
        {
            Repository.Delete(this);
        }

        public void UpdateData()
        {
            if (Id == null) return;
            Parties = Repository.GetParties(Id.Value);
            Coalitions = Repository.GetCoalitions(Id.Value);
        }
    }
}