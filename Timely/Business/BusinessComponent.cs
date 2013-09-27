using System;
using System.Data.Entity;
using System.Security.Principal;

namespace Moravia.Timely.Business
{
    public class BusinessComponent<TEntity> : IBusinessComponent<TEntity>
        where TEntity : Entity
    {
        public IPrincipal Principal { get; set; }
        public DbContext Context { get; set; }
        public Service<TEntity> Service { get; set; }
        public Type EntityType { get { return typeof(TEntity); } }

        public EntityState GetEntityState(TEntity entity)
        {
            return Context.Entry(entity).State;
        }
    }
}