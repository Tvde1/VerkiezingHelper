using System.Collections;
using System.Collections.Generic;
using VerkiezingHelper.Helpers.Objects;

namespace VerkiezingHelper.Models
{
    public class IndexModel : BaseModel
    {
        public Election Election { get; set; }
        public List<Party> Parties { get; set; }
    }
}