using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Moravia.Timely.Models
{
    public class User : Entity
    {
        public string name { get; set; }
        public string email { get; set; }
    }
}