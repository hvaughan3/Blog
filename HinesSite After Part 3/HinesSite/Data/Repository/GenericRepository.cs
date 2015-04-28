#region Usings

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HinesSite.Data.Context;
#pragma warning disable 1570

#endregion

namespace HinesSite.Data.Repository {

    /// <summary>
    /// The base repository that other inherit from
    /// </summary>
    /// <typeparam name="TEntity">Entity set that the actual repo will be created for</typeparam>
    public class GenericRepository<TEntity> where TEntity : class {

        #region Properties

        // Class variables declared for context and entity set that the repo is instantiated for
        internal ApplicationDbContext context;
        internal DbSet<TEntity>       dbSet;

        #endregion

        #region Constructor

        /// <summary>
        /// The constructor accepts a DB context instance and initializes the entity set variable
        /// </summary>
        /// <param name="context">The Db Context to use</param>
        public GenericRepository(ApplicationDbContext context) {
            this.context = context;
                 dbSet   = context.Set<TEntity>();
        }

        #endregion

        /// <summary>
        /// The Get method uses lambas to allow the calling code to specify a filter condition and a
        /// column to order the results by, and a string parameter lets the caller provide a comma-delimited
        /// list of navigation properties for eager loading
        /// </summary>
        /// <param name="filter">
        ///     The 'Expression<Func<TEntity, bool>> filter' part below means the caller will provide a lambda expression
        ///     based on the TEntity type and this expression will return a Boolean value
        ///     (caller ex: blogpost => blogpost.Title = "Blogpost Title" where "Blogpost Title" is the filter)
        /// </param>
        /// <param name="orderBy">
        ///     The 'Func<IQueryable<TEntity>, IQueryable<TEntity>> orderBy' also means the caller will provide a lambda
        ///     expression but the input to this expression is an IQueryable object for the TEntity type. The lambda
        ///     expression will return an ordered version of that IQueryable object
        ///     (Blogpost caller ex: q => q.OrderBy(b => b.Title for the orderBy parameter)
        /// </param>
        /// <param name="includeProperties"></param>
        /// <returns>Task<IEnumerable<TEntity>></returns>
        public virtual async Task<IEnumerable<TEntity>> Get(
            Expression<Func<TEntity, bool>>                 filter = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> orderBy = null,
                                          string includeProperties = ""  ) {

            // Creates an IQueryable object and then applies the filter expression if there is one
            IQueryable<TEntity> query = dbSet;

            if(filter != null) {
                query  = query.Where(filter);
            }

            // Applies eager-loading expressions after parsing the comma-delimited list
            foreach(string includeProperty in includeProperties.Split( new [] { ',' }, StringSplitOptions.RemoveEmptyEntries)) {
                query = query.Include(includeProperty);
            }

            // Applies the orderBy expression if there is one and returns the results, otherwise it returns the results from the unordered query
            if(orderBy != null) {
                return await orderBy(query).ToListAsync();
            }

            return await query.ToListAsync();
        }

        /// <summary>
        /// Gets the entity by it's Id
        /// </summary>
        /// <param name="id">The Id of the entity to find</param>
        /// <returns>Task<TEntity> (which ever kind is specified)</returns>
        public async virtual Task<TEntity> GetById(object id) {
            return await dbSet.FindAsync(id);
        }

        /// <summary>
        /// Insert the new entity into the database
        /// </summary>
        /// <param name="entity">The new entity to insert</param>
        public virtual void Insert(TEntity entity) {
            dbSet.Add(entity);
        }

        /// <summary>
        /// Updates an entity that is already in the DB
        /// </summary>
        /// <param name="entityToUpdate"></param>
        public virtual void Update(TEntity entityToUpdate) {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        /// <summary>
        /// Deletes an entity by its Id
        /// </summary>
        /// <param name="id">The Id of the entity to delete</param>
        public async virtual void Delete(object id) {
            TEntity entityToDelete = await dbSet.FindAsync(id);
            Delete(entityToDelete);
        }

        /// <summary>
        /// Deletes an entity using a reference
        /// </summary>
        /// <param name="entityToDelete">The entity to delete</param>
        public virtual void Delete(TEntity entityToDelete) {

            if(context.Entry(entityToDelete).State == EntityState.Detached) {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }
    }
}