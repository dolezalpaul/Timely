using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;

namespace Moravia.Timely.Business.Rules
{
    public abstract class BusinessRule<TEntity> : BusinessComponent<TEntity>, IBusinessRule<TEntity>
        where TEntity : Entity
    {
        public int Priority { get; protected set; }
        public abstract void Apply(TEntity entity);
    }
}