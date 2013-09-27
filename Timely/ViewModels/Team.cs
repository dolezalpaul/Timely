using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Moravia.Timely.ViewModels
{
    public class Team : ViewModel
    {
        public string name { get; set; }
        public List<int> users { get; set; }
    }
}