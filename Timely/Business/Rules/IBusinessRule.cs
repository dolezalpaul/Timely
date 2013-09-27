using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moravia.Timely.Business.Rules
{
    /// <summary>
    /// This is second line of business components that defines rules to
    /// be applied when entity is created or updated. It will be executed
    /// after the entity is correctly resolved.
    /// </summary>
    /// <typeparam name="TEntity">Business Entity this rule is applicable to</typeparam>
    public interface IBusinessRule<TEntity> : IBusinessComponent<TEntity>
        where TEntity : Entity
    {
        /// <summary>
        /// Defines the priority of execution of this rule aka the sequence
        /// in which the rules for given entity will be executed. Highest
        /// number means higher priority.
        /// </summary>
        int Priority { get; }
        /// <summary>
        /// Function to be implemented in specific rule that describes the
        /// business process.
        /// </summary>
        /// <param name="entity">Entity on which to apply the rule</param>
        void Apply(TEntity entity);
    }
}
