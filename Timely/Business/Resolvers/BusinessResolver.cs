using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Moravia.Timely.Business.Resolvers
{
    public abstract class BusinessResolver<TEntity> : BusinessComponent<TEntity>, IBusinessResolver<TEntity>
        where TEntity : Entity
    {
        public abstract TEntity Resolve(TEntity entity);
    }
}