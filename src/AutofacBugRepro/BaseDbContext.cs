using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutofacBugRepro.Models;

namespace AutofacBugRepro
{
    public abstract class BaseDbContext : DbContext
    {
        protected BaseDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }


        /// <summary>
        /// Attach an entity to the context or return an already attached entity (if it was already attached)
        /// </summary>
        /// <typeparam name="TEntity">TEntity</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>Attached entity</returns>
        public TEntity AttachEntityToContext<TEntity>(TEntity entity) where TEntity : class, IEntity, new()
        {
            var alreadyAttached = Set<TEntity>().Local.FirstOrDefault(x => x.Id == entity.Id);

            if (alreadyAttached != null) return alreadyAttached; //entity is already loaded

            //attach new entity
            Set<TEntity>().Attach(entity);
            return entity;
        }

        public IList<T> Query<T>(string commandText, params object[] sqlParameters)
        {
            return Database
                .SqlQuery<T>(commandText, sqlParameters).ToList();
        }
    }
}
