using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Moravia.Timely.Models
{
    public class Period : Entity
    {
        public int activity_id { get; set; }

        public DateTime start { get; set; }
        public DateTime? end { get; set; }

        [DefaultValue(false)]
        public bool closed { get; set; }

        [ForeignKey("activity_id")]
        public Activity activity { get; set; }
    }
}