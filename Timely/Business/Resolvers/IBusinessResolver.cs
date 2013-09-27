using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moravia.Timely.Business.Resolvers
{
    /// <summary>
    /// This is first line of business components that are responsible for resolving
    /// if the user is able to access the entity, what properties can user see or 
    /// edit and what is his role in regard to the given business entity.
    /// </summary>
    /// <typeparam name="TEntity">Business Entity this resolver is applicable to</typeparam>
    public interface IBusinessResolver<TEntity> : IBusinessComponent<TEntity>
        where TEntity : Entity
    {
        /// <summary>
        /// Function to be implemented in the specific resolver.
        /// </summary>
        /// <param name="entity">Entity to be resolved</param>
        /// <returns>Resolved entity or null if user cannot access it</returns>
        TEntity Resolve(TEntity entity);
    }
}
