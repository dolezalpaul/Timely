using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Moravia.Timely.Models
{
    public class Favorite : Entity
    {
        public int user_id { get; set; }
        public int project_id { get; set; }
        public int task_id { get; set; }

        [ForeignKey("user_id")]
        public User user { get; set; }

        [ForeignKey("project_id")]
        public Project project { get; set; }

        [ForeignKey("task_id")]
        public Task task { get; set; }
    }
}