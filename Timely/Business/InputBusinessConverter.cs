using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

namespace Moravia.Timely.Business
{
    public class InputBusinessConverter<TEntity> : BusinessComponent<TEntity>, ITypeConverter<int, TEntity>
        where TEntity : Entity
    {
        public TEntity Convert(ResolutionContext context)
        {
            return Service.Get((int)context.SourceValue);
        }
    }
}