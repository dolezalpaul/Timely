using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Moravia.Timely.Business.Validators
{
    public abstract class BusinessValidator<TEntity> : BusinessComponent<TEntity>, IBusinessValidator<TEntity>
        where TEntity : Entity
    {
        public abstract void Validate(TEntity entity);
    }
}