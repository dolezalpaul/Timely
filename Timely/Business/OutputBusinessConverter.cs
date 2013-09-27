using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

namespace Moravia.Timely.Business
{
    public class OutputBusinessConverter<TEntity> : BusinessComponent<TEntity>, ITypeConverter<TEntity, int>
        where TEntity : Entity
    {
        public int Convert(ResolutionContext context)
        {
            return (context.SourceValue as TEntity).id;
        }
    }
}