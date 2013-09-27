using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Moravia.Timely.Models
{
    public class Activity : Entity
    {
        public DateTime date { get; set; }
        [DefaultValue(0)]
        public Decimal spent_time { get; set; }
        [DefaultValue(0)]
        public Decimal adjusted_time { get; set; }

        public int user_id { get; set; }
        public int project_id { get; set; }
        public int task_id { get; set; }

        [ForeignKey("user_id")]
        public User user { get; set; }

        [ForeignKey("project_id")]
        public Project project { get; set; }

        [ForeignKey("task_id")]
        public Task task { get; set; }

        public virtual ICollection<Period> periods { get; set; }
    }
}