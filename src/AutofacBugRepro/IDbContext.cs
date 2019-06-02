using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using AutofacBugRepro.Models;

namespace AutofacBugRepro
{
    public interface IDbContext
    {
        ConnectionState ConnectionState { get; }
        
        DbSet<ReadyMadeProduct> ReadyMadeProducts { get; set; }

        /// <summary>
        /// Get DbSet
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>DbSet</returns>
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        /// <summary>
        /// Save changes
        /// </summary>
        /// <returns></returns>
        int SaveChanges();

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        
        void Open();
        
        Task<int> SaveChangesAsync();
    }
}