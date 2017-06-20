using System.Collections.Generic;

namespace VerkiezingHelper.Helpers.Objects
{
    public class Coalition : BaseObject
    {
        public Coalition(int id, string name) : base(id, name)
        {
        }

        public List<Party> Parties { get; }

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