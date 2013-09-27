using System;
using System.Data.Entity;
using System.Security.Principal;

namespace Moravia.Timely.Business
{
    public interface IBusinessComponent
    {
        IPrincipal Principal { get; set; }
        DbContext Context { get; set; }
        Type EntityType { get; }
    }

    public interface IBusinessComponent<TEntity> : IBusinessComponent
        where TEntity : Entity
    {
    }
}
