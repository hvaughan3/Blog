#region Usings

using System;
using HinesSite.Data.Context;
using HinesSite.Data.Repository;
using HinesSite.Interface;
using HinesSite.Models;

#endregion
// ReSharper disable MissingXmlDoc

namespace HinesSite.Data {

    public class UnitOfWork : IUnitOfWork {

        #region Properties

        private ApplicationDbContext _dbContext;

        public ApplicationDbContext DbContext {

            get { return _dbContext ?? (_dbContext = new ApplicationDbContext()); }
        }

        //private GenericRepository<Blogpost> _blogpostRepository;
        //private GenericRepository<Tag>      _tagRepository;

        #endregion

        // * Each repo property checks whether the repo already exists. If not, it instantiates the repo, passing
        // *   in the context instance, therefore all repos share the same context instance
        // */

        ///// <summary>
        ///// Generates a new Blogpost Repo and passes in an instance of the DB context
        ///// </summary>
        //public GenericRepository<Blogpost> BlogpostRepository {
        //    get {
        //        return _blogpostRepository ?? (_blogpostRepository = new GenericRepository<Blogpost>(_context));
        //    }
        //}

        ///// <summary>
        ///// Generates a new Tag Repo and passes in an instance of the DB context
        ///// </summary>
        //public GenericRepository<Tag> TagRepository {
        //    get {
        //        return _tagRepository ?? (_tagRepository = new GenericRepository<Tag>(_context));
        //    }
        //}

        /// <summary>
        /// Saves the DB context, saving the entity changes to the DB
        /// </summary>
        public void Save() {
            _dbContext.SaveChanges();
        }

        #region Dispose

        /// <summary>
        /// Disposes of the DB context
        /// </summary>
        public void Dispose(bool disposing) {

            if(disposing) {
                if(_dbContext != null) {

                    _dbContext.Dispose();
                    _dbContext = null;
                }
            }
        }

        /// <summary>
        /// Disposes the DB context
        /// </summary>
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}