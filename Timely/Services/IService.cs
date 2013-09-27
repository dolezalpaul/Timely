using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Principal;
using Moravia.Timely.Business;

namespace Moravia.Timely
{
    public interface IService
    {
        IPrincipal Principal { get; set; }
        DbContext Context { get; set; }
        Type EntityType { get; }
    }
}
