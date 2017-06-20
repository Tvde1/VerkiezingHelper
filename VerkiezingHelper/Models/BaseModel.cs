using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VerkiezingHelper.Helpers.DAL;

namespace VerkiezingHelper.Models
{
    public class BaseModel
    {
        protected Repository Repository { get; }
        public string Error { get; set; }
    }
}