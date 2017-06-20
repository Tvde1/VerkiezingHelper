namespace VerkiezingHelper.Helpers.Objects
{
    public class Party : BaseObject
    {
        public Party(int id, string name, string leadCandidate, int amountOfVotes) : base(id, name)
        {
            LeadCandidate = leadCandidate;
            AmountOfVotes = amountOfVotes;
        }

        public string LeadCandidate { get; private set; }
        public int AmountOfVotes { get; private set; }

        public void UpdateInfo(string leadCandidate, int amountOfVotes)
        {
            LeadCandidate = leadCandidate;
            AmountOfVotes = amountOfVotes;
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