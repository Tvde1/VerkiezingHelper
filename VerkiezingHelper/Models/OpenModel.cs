using System.Collections.Generic;
using VerkiezingHelper.Helpers.DAL;

namespace VerkiezingHelper.Models
{
    public class OpenModel : BaseModel
    {
        private readonly Repository _repository = new Repository();

        public OpenModel()
        {
            Elections = _repository.GetAllElectionNames();
            if (Elections == null)
            {
                Warning = "Kan niet met de database verbinden.";
                Elections = new List<string>();
            }
        }

        public List<string> Elections { get; }
    }
}