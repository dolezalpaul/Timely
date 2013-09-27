using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moravia.Timely.Business.Validators
{
    /// <summary>
    /// This is third and last line of business components that allow you to
    /// define validation rules. If the validation fails the exception should
    /// be raised.
    /// </summary>
    /// <typeparam name="TEntity">Business Entity the validator is applicable to</typeparam>
    public interface IBusinessValidator<TEntity> : IBusinessComponent<TEntity>
        where TEntity : Entity
    {
        /// <summary>
        /// Function to be defined in specific validator that describes validation
        /// rules to be required for the given entity.
        /// </summary>
        /// <param name="entity">Entity to be validated</param>
        void Validate(TEntity entity);
    }
}
