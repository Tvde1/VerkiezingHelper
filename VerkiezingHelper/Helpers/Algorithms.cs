using System.Collections.Generic;
using System.Linq;
using VerkiezingHelper.Helpers.Objects;

namespace VerkiezingHelper.Helpers
{
    public static class Algorithms
    {
        public static Party ChoosePresident(List<Party> parties)
        {
            if (parties.Count == 0) return null;
            return parties.OrderByDescending(x => x.AmountOfVotes).First();
        }
    }
}