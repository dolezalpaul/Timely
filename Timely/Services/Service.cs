using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using Moravia.Timely.Business;
using Moravia.Timely.Business.Resolvers;
using Moravia.Timely.Business.Rules;
using Moravia.Timely.Business.Validators;

namespace Moravia.Timely
{
    public class Service<TEntity> : IService where TEntity : Entity
    {
        public IPrincipal Principal { get; set; }
        public DbContext Context { get; set; }
        public Type EntityType { get { return typeof(TEntity); } }
        public IEnumerable<IBusinessComponent<TEntity>> Components { get; set; }
        protected DbSet<TEntity> EntitySet
        {
            get { return Context.Set<TEntity>(); }
        }

        public Service()
        {
            Components = new List<IBusinessComponent<TEntity>>();
        }

        // Apply Business Resolvers
        public virtual TEntity Resolve(TEntity entity)
        {
            if (entity != null)
            {
                if (entity.is_resolved) return entity;
                var resolvers = Components.OfType<IBusinessResolver<TEntity>>();
                foreach (var resolver in resolvers)
                {
                    entity = resolver.Resolve(entity);
                    if (entity == null) break;
                }
                entity.is_resolved = true;
            }
            return entity;
        }

        // Apply Business Rules
        protected virtual void Process(TEntity entity)
        {
            var rules = Components.OfType<IBusinessRule<TEntity>>();
            foreach (var rule in rules)
            {
                rule.Apply(entity);
            }
        }

        // Apply Business Validators
        protected virtual void Validate(TEntity entity)
        {
            var validators = Components.OfType<IBusinessValidator<TEntity>>();
            foreach (var validator in validators)
            {
                validator.Validate(entity);
            }
        }

        public virtual IEnumerable<TEntity> Get()
        {
            return EntitySet.ToList().Select(e => Resolve(e)).Where(e => e != null);
        }
        public virtual TEntity Get(int id)
        {
            return Resolve(EntitySet.Find(id));
        }
        public virtual TEntity Post(TEntity entity)
        {
            EntitySet.Add(entity);
            entity = Resolve(entity);
            if (entity != null)
            {
                Process(entity);
                Validate(entity);
                Context.SaveChanges();
            }
            return entity;
        }
        public virtual TEntity Put(int id, TEntity entity)
        {
            entity.id = id;
            EntitySet.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
            entity = Resolve(entity);
            if (entity != null)
            {
                Process(entity);
                Validate(entity);
                Context.SaveChanges();
            }
            return entity;
        }
        public virtual bool Delete(int id)
        {
            var entity = EntitySet.Find(id);
            EntitySet.Remove(entity);
            entity = Resolve(entity);
            if (entity != null)
            {
                Context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}