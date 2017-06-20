namespace VerkiezingHelper.Helpers.Objects
{
    public class Party : BaseObject
    {
        public Party() : base(null, null)
        {
        }

        public Party(int id, string name, string leadCandidate, int amountOfVotes) : base(id, name)
        {
            LeadCandidate = leadCandidate;
            AmountOfVotes = amountOfVotes;
        }

        public string LeadCandidate { get; set; }
        public int AmountOfVotes { get; set; }
        public bool Checked { get; set; }
        public double PercentOfVotes { get; set; }
        public int AmountOfSeats { get; set; }

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