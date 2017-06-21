using System.Collections.Generic;
using VerkiezingHelper.Helpers.Objects;

namespace VerkiezingHelper.Models
{
    public class IndexModel : BaseModel
    {
        public Election Election { get; set; }
        public List<Party> AllParties { get; set; }
        public string CoalitionName { get; set; }
        public List<Party> CoalitionParties { get; set; }
    }
}