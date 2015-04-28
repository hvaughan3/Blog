#region Usings

using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using HinesSite.Data;
using HinesSite.Interface;
using HinesSite.Models;
#pragma warning disable 1570

#endregion

namespace HinesSite.Controllers {

    /// <summary>
    /// Does all Blogpost Tag related functions
    /// </summary>
    public class TagController : Controller {

        #region Properties

        private ITagRepository _tagRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Unity DI Constructor
        /// </summary>
        public TagController(ITagRepository tagRepository) {

            if(tagRepository == null)
                throw new ArgumentNullException("tagRepository", "tagRepository is null in the TagController");

            _tagRepository = tagRepository;
        }

        #endregion

        #region Actions

        //
        // GET: Tag
        /// <summary>
        /// The view which displays all current Blogpost Tags
        /// </summary>
        /// <returns>Task<ActionResult></returns>
        [HttpGet, ActionName("Index")]
        public async Task<ActionResult> Index() {

            List<Tag> tags = await _tagRepository.GetTags();

            #region Not Found (null) Check

            if(tags == null || !tags.Any()) {

                return HttpNotFound();
            }

            #endregion

            return View("~/Views/Tag/Index.cshtml", tags);
        }

        //
        // GET: Tag/Details/5
        /// <summary>
        /// The view which displays a specific Tag's details
        /// </summary>
        /// <param name="id">The Id of the Tag being viewed</param>
        /// <returns>Task<ActionResult></returns>
        [HttpGet, ActionName("Details")]
        public async Task<ActionResult> Details(int? id) {

            #region Bad Request (null) Check

            if(id == null) {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            #endregion

            Tag tag = await _tagRepository.GetTagById(id);

            #region Not Found (null) Check

            if(tag == null) {

                return HttpNotFound();
            }

            #endregion

            return View("~/Views/Tag/Details.cshtml", tag);
        }

        //
        // GET: Tag/Create
        /// <summary>
        /// The view used to create a new Tag
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet, ActionName("Create")/*, Authorize*/]
        public ActionResult Create() {

            ViewBag.CreatedOn = DateTime.Now.ToShortDateString();
            return View("~/Views/Tag/Create.cshtml");
        }

        //
        // POST: Tag/Create
        /// <summary>
        /// Creates a new tag
        /// </summary>
        /// <param name="tag">The new Tag to create</param>
        /// <returns>Task<ActionResult></returns>
        [HttpPost, ActionName("Create"), ValidateAntiForgeryToken/*, Authorize*/]
        public async Task<ActionResult> Create([Bind(Include = "Name")] Tag tag) {

            try {
                if(ModelState.IsValid) {
                    await _tagRepository.InsertTag(tag);
                    return RedirectToAction("Index");
                }
            }
            #region RetryLimitExceeded Catch

            catch(RetryLimitExceededException /* dex */) {
                ModelState.AddModelError("", "Unable to save changes. Try again and, if the problem persists, see your system administrator.");
            }

            #endregion

            return View("~/Views/Tag/Create.cshtml", tag);
        }

        //
        // GET: Tag/Edit/5
        /// <summary>
        /// Thew view used to edit a Tag
        /// </summary>
        /// <param name="id">The Id of the Tag being edited</param>
        /// <returns>Task<ActionResult></returns>
        [HttpGet, ActionName("Edit")/*, Authorize*/]
        public async Task<ActionResult> Edit(int? id) {

            #region Bad Request (null) Check

            if(id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            #endregion

            Tag tag            = await _tagRepository.GetTagById(id);
            ViewBag.ModifiedOn = DateTime.Now.ToShortDateString();

            #region Not Found (null) Check

            if(tag == null) {
                return HttpNotFound();
            }

            #endregion

            return View("~/Views/Tag/Edit.cshtml", tag);
        }

        //
        // POST: Tag/Edit/5
        /// <summary>
        /// Edits the information of a Tag
        /// </summary>
        /// <param name="id">The Id of the tag to update</param>
        /// <param name="rowVersion">Used to verify concurrency</param>
        /// <returns>Task<ActionResult></returns>
        [HttpPost, ActionName("Edit"), ValidateAntiForgeryToken/*, Authorize*/]
        public async Task<ActionResult> EditPost(int? id, byte[] rowVersion) {

            #region Bad Request (null) Check

            if(id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            #endregion

            #region Properties

            string[] fieldsToBind      = { "Name", "ModifiedOn", "RowVersion" };
            Tag tagToUpdate            = await _tagRepository.GetTagById(id);
            byte[] rowVersionException = null;

            #endregion

            if(tagToUpdate == null) {
                Tag deletedTag = new Tag();

                TryUpdateModel(deletedTag, fieldsToBind);
                ModelState.AddModelError(string.Empty, "Unable to save changes. The department was deleted by another user.");

                return View("~/Views/Tag/Edit.cshtml", deletedTag);
            }

            if(TryUpdateModel(tagToUpdate, fieldsToBind)) {
                try {
                    await _tagRepository.SetRowVersion(tagToUpdate, rowVersion);
                    return RedirectToAction("Index");
                }
                #region UpdateConcurrency Catch

                catch(DbUpdateConcurrencyException ex) {
                    // 3 lines below get both the values read from the DB and the new values entered by the user
                    DbEntityEntry entry            = ex.Entries.Single();
                    Tag clientValues               = (Tag)entry.Entity;
                    DbPropertyValues databaseEntry = entry.GetDatabaseValuesAsync().Result;

                    if(databaseEntry == null) {
                        ModelState.AddModelError(string.Empty, "Unable to save changes. The Tag was deleted by another user.");
                    }
                    else {
                        Tag databaseValues = (Tag)entry.GetDatabaseValuesAsync().Result.ToObject();

                        // TODO: Make sure that the current logged on user gets inserted into the DB as the editor and the creator does not get changed

                        /*
                         * All if statements give custom error messages for each column that has DB values different from
                         *   what the user entered on the Edit page. The last if statement compares the TagId for that blogpost
                         *   and then gets the tags 'Name'
                         */
                        if(databaseValues.Name != clientValues.Name) {
                            ModelState.AddModelError("Name", "Current value: " + databaseValues.Name);
                        }
                        if(databaseValues.ModifiedOn != clientValues.ModifiedOn) {
                            ModelState.AddModelError("ModifiedOn", "Current value: " + databaseValues.ModifiedOn);
                        }
                        ModelState.AddModelError(string.Empty,
                                                 "The record you attempted to edit was modified by another user "
                                                 + "after you got the original value. The edit operation was canceled and the current values in the "
                                                 + "database have been displayed. If you still want to edit this record, and overwrite the other "
                                                 + "user's changes, click the 'Save' button again. Otherwise click the 'Back to List' hyperlink.");

                        // This captures the databaseValues' RowVersion so it can be saved below (must be done outside this catch since it needs to be awaited)
                        rowVersionException = databaseValues.RowVersion;
                    }
                }

                #endregion
                #region RetryLimitExceeded Catch

                catch(RetryLimitExceededException /* dex*/) {
                    // TODO: Log the error (uncomment dex variable name and add a line here to write a log)
                    ModelState.AddModelError("", "Unable to save changes. Try again and, if the problem persists, see your system administrator.");
                }

                #endregion
            }

            // This sets the 'RowVersion' of the Blogpost object to the new value retrieved from the DB, only if the rowVersionException variable was filled
            if(rowVersionException != null) {
                await _tagRepository.SetRowVersion(tagToUpdate, rowVersionException);
            }

            return View("~/Views/Tag/Edit.cshtml", tagToUpdate);
        }

        //
        // GET: Tag/Delete/5
        /// <summary>
        /// The view used for deleting a Tag
        /// </summary>
        /// <param name="id">The Id of the tag to delete</param>
        /// <param name="saveChangesError">Determines if a saveChangesError has happened and the view is being returned to</param>
        /// <param name="concurrencyError">
        ///     Checks whether the page is being redisplayed after a concurrency error. If it = true, an error is sent to the ViewBag.
        ///     1 Error is given if the row was already deleted by another user and a different error is given if the row was changed after the
        ///     'Delete Confirmation' page is shown
        /// </param>
        /// <returns>Task<ActionResult></returns>
        [HttpGet, ActionName("Delete")/*, Authorize*/]
        public async Task<ActionResult> Delete(int? id, bool? concurrencyError/*, bool? saveChangesError*/) {

            #region Bad Request (null) Check

            if(id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            #endregion
            //#region Save Changes Error Message

            //if(saveChangesError.GetValueOrDefault()) {
            //    ViewBag.ErrorMessage = "Delete failed. Try again and, if the problem persists, contact the system administrator.";
            //}

            //#endregion

            Tag tag = await _tagRepository.GetTagById(id);

            #region Not Found (null) Check & Concurrency Error Check

            if(tag == null) {

                #region Concurrency Error Check

                if(concurrencyError.GetValueOrDefault()) {
                    return RedirectToAction("Index");
                }

                #endregion

                return HttpNotFound();
            }

            #region Concurrency Error Check

            if(concurrencyError.GetValueOrDefault()) {
                ModelState.AddModelError(string.Empty, "The record you attempted to delete was modified by"
                                                    + " another user after you got the original values. The delete operation was cancelled and the"
                                                    + " current values in the database are displayed below. If you still want to delete this record,"
                                                    + " click the delete button again. Otherwise, click the 'Back to List' hyperlink.");
            }

            #endregion

            #endregion

            return View("~/Views/Tag/Delete.cshtml", tag);
        }

        //
        // POST: Tag/Delete/5
        /// <summary>
        /// Deletes a Tag
        /// </summary>
        /// <param name="tag">The Tag to delete</param>
        /// <returns>Task<ActionResult></returns>
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken/*, Authorize*/]
        public async Task<ActionResult> Delete(Tag tag) {

            try {
                await _tagRepository.DeleteTag(tag);
                return RedirectToAction("Index");
            }
            #region UpdateConcurrency Catch

            catch(DbUpdateConcurrencyException) {

                return RedirectToAction("Delete", new { concurrencyError = true, id = tag.TagId });
            }

            #endregion
            #region RetryLimitExceeded Catch

            catch(RetryLimitExceededException /* dex */) {
                // TODO: Log the error (uncomment dex variable name and add a line here to write a log
                ModelState.AddModelError("", "Unable to save changes. Try again and, if the problem persists, contact the system administrator.");
                return View("~/Views/Tag/Delete.cshtml", tag);
            }

            #endregion
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
