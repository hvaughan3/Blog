#region Usings

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using HinesSite.Data.Context;
using HinesSite.Interface;
using HinesSite.Models;
#pragma warning disable 1570

#endregion

namespace HinesSite.Data.Repository {
    // TODO: Implement stuff into the Blogpost Repo to handles lines 246 and 247 in the blogpost controller.
    // TODO: Implement stuff into the Tag Repo (maybe?) to handle line 270 in the blogpost controller.

    /// <summary>
    /// Blogpost Repo handles all Blogpost related data actions
    /// </summary>
    public class BlogpostRepository : IBlogpostRepository, IDisposable {

        #region Properties

        private ApplicationDbContext _dbContext;
        private ILogger              _log;
        private FileRepository       _fileRepository;

        private Stopwatch            _timespan;

        #endregion

        #region Constructor

        /// <summary>
        /// Main constructor which sets the context
        /// </summary>
        public BlogpostRepository(IUnitOfWork unitOfWork, ILogger log, FileRepository fileRepository) {

            if(unitOfWork == null)
                throw new ArgumentNullException("unitOfWork", "unitOfWork is null in the BlogpostRepository");
            if(log == null)
                throw new ArgumentNullException("log", "log is null in the BlogpostRepository");
            if(fileRepository == null)
                throw new ArgumentNullException("fileRepository", "fileRepository is null in the BlogpostRepository");

            _dbContext      = unitOfWork.DbContext;
            _log            = log;
            _fileRepository = fileRepository;
        }

        #endregion

        #region IBlogpostRepository

        /// <summary>
        /// Gets a list of Blogposts
        /// </summary>
        /// <returns>IEnumerable<Blogpost></returns>
        public IEnumerable<Blogpost> GetBlogposts() {

            // Adding .AsNoTracking() since we are not doing any write operations and this adds to performance
            return _dbContext.Blogposts.AsNoTracking().AsEnumerable();
        }

        /// <summary>
        /// Get a single Blogpost by an Id
        /// </summary>
        /// <param name="blogpostId">The Id of the Blogpost to get details about</param>
        /// <returns>Task<Blogpost></returns>
        public async Task<Blogpost> GetBlogpostById(int? blogpostId) {

            // Setting to false since we are not doing any write operations and this adds to performance
            _dbContext.Configuration.AutoDetectChangesEnabled = false;

            return await _dbContext.Blogposts.FindAsync(blogpostId);
        }

        /// <summary>
        /// Sets the new row version for concurrency checks
        /// </summary>
        /// <param name="blogpostToUpdate">The Blogpost to change</param>
        /// <param name="rowVersion">The new row version number</param>
        public async Task SetRowVersion(Blogpost blogpostToUpdate, byte[] rowVersion) {

            #region Properties

            _timespan = Stopwatch.StartNew();

            #endregion

            _dbContext.Entry(blogpostToUpdate).OriginalValues["RowVersion"] = rowVersion;
            await Save();

            #region Logging

            _timespan.Stop();
            _log.TraceApi("Blogpost Service", "BlogpostRepository.SetRowVersion", _timespan.Elapsed, "Row Version = {0}", rowVersion);
            _timespan.Reset();

            #endregion
        }

        /// <summary>
        /// Insert a new Blogpost into the DB
        /// </summary>
        /// <param name="blogpost">The Blogpost to insert into the DB</param>
        /// <param name="uploadedFiles"></param>
        public async Task InsertBlogpost(Blogpost blogpost, IEnumerable<HttpPostedFileBase> images) {

            #region Properties

            string [] fileUrls = {};
                     _timespan = Stopwatch.StartNew();

            #endregion

            if(images != null) {

                HttpPostedFileBase[] imageArray = images.ToArray();

                if(imageArray.Any()) {
                    fileUrls = await _fileRepository.UploadFilesAsync(imageArray);

                    #region Not Found (null) Check

                    if(fileUrls != null) {
                        blogpost.FileUrlArray = fileUrls;
                    }
                    else {
                        #region Error Logging

                        _log.Error("Failure to save image(s) or retrieve image URL(s) in Blogpost Repository");

                        #endregion
                    }

                    #endregion
                }
            }

            _dbContext.Blogposts.Add(blogpost);
            await Save();

            #region Logging

            _timespan.Stop();
            _log.TraceApi("Blogpost Service", "BlogpostRepository.InsertBlogpost", _timespan.Elapsed, "Blogpost Id = {0}", blogpost.BlogpostId);
            _timespan.Reset();

            #endregion
        }

        /// <summary>
        /// Update a Blogpost in the DB
        /// </summary>
        /// <param name="blogpost">The Blogpost to update</param>
        public async Task UpdateBlogpost(Blogpost blogpost) {

            #region Properties

            _timespan = Stopwatch.StartNew();

            #endregion

            _dbContext.Entry(blogpost).State = EntityState.Modified;
            await Save();

            #region Logging

            _timespan.Stop();
            _log.TraceApi("Blogpost Service", "BlogpostRepository.UpdateBlogpost", _timespan.Elapsed, "Blogpost Id = {0}", blogpost.BlogpostId);
            _timespan.Reset();

            #endregion
        }

        /// <summary>
        /// Delete a blogpost from the DB
        /// </summary>
        /// <param name="blogpost">The Blogpost to delete from the DB</param>
        public async Task DeleteBlogpost(Blogpost blogpost) {

            #region Properties

            _timespan = Stopwatch.StartNew();

            #endregion

            Blogpost blogpostToDelete = await (from   b in _dbContext.Blogposts
                                               where  b.BlogpostId == blogpost.BlogpostId
                                                   && b.RowVersion == blogpost.RowVersion
                                               select b).FirstOrDefaultAsync();
            #region Concurrency (null) Check

            if(blogpostToDelete == null) {
                throw new DbUpdateConcurrencyException();
            }

            #endregion

            _dbContext.Entry(blogpostToDelete).State = EntityState.Deleted;
            await Save();

            #region Logging

            _timespan.Stop();
            _log.TraceApi("Blogpost Service", "BlogpostRepository.DeleteBlogpost", _timespan.Elapsed, "Blogpost Id = {0}", blogpost.BlogpostId);
            _timespan.Reset();

            #endregion
        }

        /// <summary>
        /// Save the changes into the DB
        /// </summary>
        /// <returns>Task</returns>
        public async Task Save() {

            #region Properties

            _timespan = Stopwatch.StartNew();

            #endregion

            await _dbContext.SaveChangesAsync();

            #region Logging

            _timespan.Stop();
            _log.TraceApi("Blogpost Service", "BlogpostRepository.Save", _timespan.Elapsed);
            _timespan.Reset();

            #endregion
        }

        #endregion

        #region IDisposable Implementation

        /// <summary>
        /// Disposes of the DB context
        /// </summary>
        protected void Dispose(bool disposing) {

            if(disposing) {

                if(_dbContext != null) {

                    _dbContext.Dispose();
                    _dbContext = null;
                }
                if(_fileRepository != null) {

                    _fileRepository.Dispose();
                    _fileRepository = null;
                }
            }
        }

        /// <summary>
        /// Used to dispose the DB context
        /// </summary>
        public void Dispose() {

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Private Methods



        #endregion
    }
}