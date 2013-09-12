using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using MongoDB.Driver;

namespace Moravia.Timely.Components
{
    public class Component : IComponent
    {
        public MongoDatabase Database { get; private set; }
        public IPrincipal User { get; private set; }

        public Component(MongoDatabase database, IPrincipal principal)
        {
            Database = database;
            User = principal;
        }
    }
}