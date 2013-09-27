using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Moravia.Timely
{
    public abstract class Entity
    {
        public Entity()
        {
            this.is_resolved = false;
        }

        [Key]
        public int id { get; set; }

        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }

        [ConcurrencyCheck]
        public int version { get; set; }

        [NotMapped]
        [JsonIgnore]
        public bool is_resolved { get; set; }
    }
}