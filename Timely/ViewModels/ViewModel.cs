using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Moravia.Timely
{
    public abstract class ViewModel
    {
        public ViewModel() : base()
        {
            this._sideloads = new HashSet<ViewModel>();
            this._links = new Dictionary<string, string>();
        }

        public int id { get; set; }

        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }

        public int version { get; set; }

        [JsonIgnore]
        public HashSet<ViewModel> _sideloads { get; set; }

        [JsonIgnore]
        public Dictionary<string, string> _links { get; set; }
    }
}