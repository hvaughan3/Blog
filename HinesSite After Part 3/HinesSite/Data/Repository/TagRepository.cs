#region Usings

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HinesSite.Data.Context;
using HinesSite.Interface;
using HinesSite.Models;
#pragma warning disable 1570

#endregion

namespace HinesSite.Data.Repository {

    /// <summary>
    /// Blogpost Repo handles all Blogpost related data actions
    /// </summary>
    public class TagRepository : ITagRepository, IDisposable {

        #region Properties

        private ApplicationDbContext _dbContext;
        private ILogger              _log;
        private Stopwatch            _timespan;

        #endregion

        #region Constructor

        /// <summary>
        /// Main constructor which sets the context
        /// </summary>
        public TagRepository(IUnitOfWork unitOfWork, ILogger log) {

            if(unitOfWork == null)
                throw new ArgumentNullException("unitOfWork", "unitOfWork is null in the TagRepository");
            if(log == null)
                throw new ArgumentNullException("log", "log is null in the TagRepository");

            _dbContext = unitOfWork.DbContext;
            _log       = log;
        }

        #endregion

        #region ITagRepository

        /// <summary>
        /// Gets a list of Tags
        /// </summary>
        /// <returns>Task<List<Tag>></returns>
        public async Task<List<Tag>> GetTags(Func<IQueryable<Tag>, IQueryable<Tag>> orderBy = null) {

            // Adding .AsNoTracking() since we are not doing any write operations and this adds to performance
            IQueryable<Tag> query = _dbContext.Tags.AsNoTracking();

            if(orderBy != null) {
                return await orderBy(query).ToListAsync();
            }

            return await _dbContext.Tags.ToListAsync();
        }

        /// <summary>
        /// Get a single Blogpost by an Id
        /// </summary>
        /// <param name="tagId">The Id of the Blogpost to get details about</param>
        /// <returns>Task<Tag></returns>
        public async Task<Tag> GetTagById(int? tagId) {

            // Setting to false since we are not doing any write operations and this adds to performance
            _dbContext.Configuration.AutoDetectChangesEnabled = false;

            return await _dbContext.Tags.FindAsync(tagId);
        }

        /// <summary>
        /// Sets the new row version for concurrency checks
        /// </summary>
        /// <param name="tagToUpdate">The Tag to change</param>
        /// <param name="rowVersion">The new row version number</param>
        public async Task SetRowVersion(Tag tagToUpdate, byte [] rowVersion) {

            #region Properties

            _timespan = Stopwatch.StartNew();

            #endregion

            _dbContext.Entry(tagToUpdate).OriginalValues["RowVersion"] = rowVersion;
            await Save();

            #region Logging

            _timespan.Stop();
            _log.TraceApi("Tag Service", "TagRepository.SetRowVersion", _timespan.Elapsed, "Row Version = {0}", rowVersion);
            _timespan.Reset();

            #endregion
        }

        /// <summary>
        /// Insert a new Tag into the DB
        /// </summary>
        /// <param name="tag">The Tag to insert into the DB</param>
        public async Task InsertTag(Tag tag) {

            #region Properties

            _timespan = Stopwatch.StartNew();

            #endregion

            _dbContext.Tags.Add(tag);
            await Save();

            #region Logging

            _timespan.Stop();
            _log.TraceApi("Tag Service", "TagRepository.InsertTag", _timespan.Elapsed, "Tag Id = {0}", tag.TagId);
            _timespan.Reset();

            #endregion
        }

        /// <summary>
        /// Update a Tag in the DB
        /// </summary>
        /// <param name="tag">The Tag to update</param>
        public async Task UpdateTag(Tag tag) {

            #region Properties

            _timespan = Stopwatch.StartNew();

            #endregion

            _dbContext.Entry(tag).State = EntityState.Modified;
            await Save();

            #region Logging

            _timespan.Stop();
            _log.TraceApi("Tag Service", "TagRepository.UpdateTag", _timespan.Elapsed, "Tag Id = {0}", tag.TagId);
            _timespan.Reset();

            #endregion
        }

        /// <summary>
        /// Delete a Tag from the DB
        /// </summary>
        /// <param name="tag">The Tag to delete from the DB</param>
        public async Task DeleteTag(Tag tag) {

            #region Properties

            _timespan = Stopwatch.StartNew();

            #endregion

            //_context.Tags.Remove(tag);
            Tag tagToDelete = await (from   t in _dbContext.Tags
                                     where  t.TagId      == tag.TagId
                                         && t.RowVersion == tag.RowVersion
                                     select t).FirstOrDefaultAsync();

            #region Concurrency (null) Check

            if(tagToDelete == null) {
                throw new DbUpdateConcurrencyException();
            }

            #endregion

            await Save();

            #region Logging

            _timespan.Stop();
            _log.TraceApi("Tag Service", "TagRepository.DeleteTag", _timespan.Elapsed, "Tag Id = {0}", tag.TagId);
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
            _log.TraceApi("Tag Service", "TagRepository.Save", _timespan.Elapsed);
            _timespan.Reset();

            #endregion
        }

        #endregion

        #region IDisposable Implementation

        /// <summary>
        /// Disposes of the context
        /// </summary>
        protected void Dispose(bool disposing) {

            if(disposing) {

                if(_dbContext != null) {

                    _dbContext.Dispose();
                    _dbContext = null;
                }
            }
        }

        /// <summary>
        /// Used to dispose the context
        /// </summary>
        public void Dispose() {

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}