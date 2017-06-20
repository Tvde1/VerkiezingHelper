using VerkiezingHelper.Helpers.DAL;

namespace VerkiezingHelper.Models
{
    public class BaseModel
    {
        protected Repository Repository { get; }
        public string Error { get; set; }
    }
}