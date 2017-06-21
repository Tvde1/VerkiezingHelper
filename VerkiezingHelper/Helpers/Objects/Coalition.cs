using System.Collections.Generic;

namespace VerkiezingHelper.Helpers.Objects
{
    public class Coalition : BaseObject
    {
        public Coalition() : this((int?) null, null, null)
        {
        }

        public Coalition(int? id, string name, Party president) : base(id, name)
        {
            President = president;
            UpdateParties();
        }

        public Coalition(string name, List<Party> parties, int? electionId) : base(null, name)
        {
            Parties = parties;

            President = Algorithms.ChoosePresident(Parties);
        }

        public List<Party> Parties { get; set; }
        public Party President { get; set; }

        private void UpdateParties()
        {
            Parties = Repository.GetParties(this);
        }

        public /*override*/ void Save()
        {
            Repository.Save(this);
        }

        public override void Delete()
        {
            Repository.Delete(this);
        }
    }
}