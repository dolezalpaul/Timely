using System.Collections.Generic;

namespace Moravia.Timely.Models
{
    public class Team : Entity
    {
        public string name { get; set; }
        public virtual IList<User> users { get; set; }
    }
}