using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Moravia.Timely.Components
{
    public interface IComponent
    {
        IPrincipal User { get; }
        MongoDatabase Database { get; }
    }
}
