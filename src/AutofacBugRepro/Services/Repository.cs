using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutofacBugRepro.Models;

namespace AutofacBugRepro.Services
{
    /// <inheritdoc />
    public sealed class Repository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly IDbContext _context;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="context">Object context</param>
        public Repository(IDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        public async Task<T> GetByIdAsync(int id)
        {
            if (_context.ConnectionState == ConnectionState.Closed)
            {
                _context.Open();
            }
            return await Entities.SingleOrDefaultAsync(c => c.Id == id);
        }
        
        /// <inheritdoc />
        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public int Insert(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                Entities.Add(entity);

                return _context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = HandleErrorMessages(dbEx);

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Reloads the specified entity based on what's currently in the DB. The entity will now be in the Unchanged state.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Reload(T entity)
        {
            if (entity != null)
            {
                _context.Entry(entity).Reload();
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public void Update(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                var entry = _context.Entry(entity);

                if (entry.State == EntityState.Detached)
                {
                    _context.Set<T>().Attach(entity);
                    entry.State = EntityState.Modified;
                }

                _context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = HandleErrorMessages(dbEx);

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets a table
        /// </summary>
        public IQueryable<T> Table => Entities;

        /// <inheritdoc />
        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        public IQueryable<T> TableNoTracking => Entities.AsNoTracking();

        /// <summary>
        /// Entities
        /// </summary>
        private IDbSet<T> Entities => _context.Set<T>();

        private static string HandleErrorMessages(DbEntityValidationException dbEx)
        {
            var msg = new StringBuilder();

            foreach (var validationError in dbEx.EntityValidationErrors.SelectMany(validationErrors => validationErrors.ValidationErrors))
                msg.AppendFormat("Property: {0} Error: {1}",
                    validationError.PropertyName, validationError.ErrorMessage).AppendLine();

            return msg.ToString();
        }
    }
}
