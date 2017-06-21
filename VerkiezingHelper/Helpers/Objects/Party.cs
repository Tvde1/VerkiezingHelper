namespace VerkiezingHelper.Helpers.Objects
{
    public class Party : BaseObject
    {
        public Party() : base(null, null)
        {
        }

        public Party(int id, string name, string leadCandidate, int amountOfVotes, double percentOfVotes,
            int amountOfSeats) : base(id, name)
        {
            LeadCandidate = leadCandidate;
            AmountOfVotes = amountOfVotes;
            PercentOfVotes = percentOfVotes;
            AmountOfSeats = amountOfSeats;
        }

        public string LeadCandidate { get; set; }
        public int AmountOfVotes { get; set; }
        public bool Checked { get; set; }
        public double PercentOfVotes { get; set; }
        public int AmountOfSeats { get; set; }

        public void Save(int? electionId)
        {
            Repository.Save(this, electionId);
        }

        //public override void Save()
        //{
        //    throw new System.InvalidOperationException();
        //}

        public override void Delete()
        {
            Repository.Delete(this);
        }
    }
}