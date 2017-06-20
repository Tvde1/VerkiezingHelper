using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using VerkiezingHelper.Helpers.Objects;

namespace VerkiezingHelper.Models
{
    public class OpenModel : BaseModel
    {
        public List<string> Elections { get; }

        public void OpenElection(string election)
        {
            
        }
    }
}