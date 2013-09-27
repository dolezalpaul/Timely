using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Moravia.Timely.ViewModels
{
    public class Favorite : ViewModel
    {
        public int user_id { get; set; }
        public int project_id { get; set; }
        public int task_id { get; set; }
    }
}