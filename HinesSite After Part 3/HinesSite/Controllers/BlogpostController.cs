#region Usings

using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using HinesSite.Interface;
using HinesSite.Models;
using PagedList;
#pragma warning disable 1570

#endregion

namespace HinesSite.Controllers {

    /// <summary>
    /// Responsible for all Blogpost activity
    /// </summary>
    public class BlogpostController : Controller {

        #region Properties

        private IBlogpostRepository _blogpostRepository;
        private ITagRepository      _tagRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// Unity DI Constructor
        /// </summary>
        public BlogpostController(IBlogpostRepository blogpostRepository, ITagRepository tagRepository) {

            if(blogpostRepository == null)
                throw new ArgumentNullException("blogpostRepository", "blogpostRepository is null in the BlogpostController");
            if(tagRepository == null)
                throw new ArgumentNullException("tagRepository", "tagRepository is null in the BlogpostController");

            _blogpostRepository = blogpostRepository;
            _tagRepository      = tagRepository;
        }

        #endregion

        #region IBlogpost Implementation

        //
        // GET: Blogpost
        /// <summary>
        /// Shows the list of Blogposts
        /// </summary>
        /// <param name="sortOrder">The column sort order</param>
        /// <param name="currentFilter">The current term to filter the posts on</param>
        /// <param name="searchString">The current term to search with</param>
        /// <param name="page">The page number currently being used</param>
        /// <returns>ActionResult</returns>
        [HttpGet, ActionName("Index")]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page) {

            #region Sorting

            // This will keep the current sort order the same while paging
            ViewBag.CurrentSort = sortOrder;

            /*
             * Sorting implementation receives a sortOrder parameter from query string in URL,
             *   does descending on Date by default
             */
            ViewBag.DateSortParm  = string.IsNullOrEmpty(sortOrder) ? "Date"       : string.Empty;
            ViewBag.TitleSortParm = sortOrder ==            "Title" ? "title_desc" : "Title";

            #endregion

            #region Paging

            /*
             * The first time the page is displayed or if no page /sorting has been clicked, all will be null. If
             *   a page link is clicked, a page variable will contain the current page number to display. This also resets paging
             *   1 if the searchString changes
             */
            if (searchString != null) {
                page = 1;
            }
            else {
                searchString = currentFilter;
            }

            // Provides the view with the current filter string inbetween changing pages and gets restored to filter box when page gets re-displayed
            ViewBag.CurrentFilter = searchString;

            #endregion

            //// Uses LINQ to Entities and creates an IQueryable variable called blogposts
            ////   Could have dynamically created a LINQ statement instead o writing different ones for each sort order
            IEnumerable<Blogpost> blogposts = _blogpostRepository.GetBlogposts();

            #region Filtering

            // Adding in a conditional where clause if the searchString is not empty
            /*
             * Using if statement incase you change repositories, so if you call an IEnumerable (which is a .NET
             *   Framework implementation) you won't get all rows if the search string is empty and if you call an
             *   IQueryable object (which is a database provider implementation) you still will not get any rows when the search
             *   string is empty. The ToUpper also does the same since the .NET implementation of the Contains method is case-sensitive
             *   while the EF SQL Server providers perform case-insensitive comparisons
             */
            /* Since changing from using the context to the repository, the 'var blogpost' above goes from an
             *   IQueryable to an IEnumerable which returns all blogposts and then filters through them. This can
             *   be inefficient with large amounts of data
             */
            if (!string.IsNullOrEmpty(searchString)) {
                blogposts = blogposts.Where(b => b.Title.ToUpperInvariant().Contains(searchString.ToUpperInvariant())
                                              || b.Subtitle.ToUpperInvariant().Contains(searchString.ToUpperInvariant())
                                              || b.User.UserName.ToUpperInvariant().Contains(searchString.ToUpperInvariant()));
            }

            #endregion

            #region Sort Order Switch

            switch (sortOrder)
            {
                case "Date":
                blogposts = blogposts.OrderBy(b => b.CreatedOn);
                break;
                case "Title":
                blogposts = blogposts.OrderBy(b => b.Title);
                break;
                case "title_desc":
                blogposts = blogposts.OrderByDescending(b => b.Title);
                break;
                default:
                blogposts = blogposts.OrderByDescending(b => b.CreatedOn);
                break;
            }

            #endregion

            /*
             *  The ToPagedList extension method acts on the blogposts IQueryable object to convert the query to a single page
             *    of blogposts in a collection type that supports paging. That single page gets passed to the view. The two ?? represent
             *
             */
            const int pageSize   = 3;
                  int pageNumber = (page ?? 1);

            // No database calls are made until converting the IEnumerable object (blogposts) into a collection using ToList()
            return View("~/Views/Blogpost/Index.cshtml", blogposts.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: Blogpost/Details/5
        /// <summary>
        /// View a specific Blogpost's details
        /// </summary>
        /// <param name="id">Id of the blogpost to view</param>
        /// <returns>Task<ActionResult></returns>
        [HttpGet, ActionName("Details")]
        public async Task<ActionResult> Details(int? id) {

            Blogpost blogpost = await _blogpostRepository.GetBlogpostById(id);

            #region Not Found (null) Check

            if(blogpost == null) {

                return HttpNotFound();
            }

            #endregion

            return View("~/Views/Blogpost/Details.cshtml", blogpost);
        }

        //
        // GET: Blogpost/Create
        /// <summary>
        /// The create a new Blogpost screen
        /// </summary>
        /// <returns>Task<ActionResult></returns>
        [HttpGet, ActionName("Create")/*, Authorize*/]
        public async Task<ActionResult> Create() {

            ViewBag.CreatedOn = DateTime.Now.ToShortDateString();
            await PopulateTagsDropDownList();
            return View("~/Views/Blogpost/Create.cshtml");
        }

        //
        // POST: Blogpost/Create
        /// <summary>
        /// Action responsible for creating the actual Blogpost
        /// </summary>
        /// <param name="blogpost">The Blogpost that is being created</param>
        /// <returns>Task<ActionResult></returns>
        [HttpPost, ActionName("Create"), ValidateAntiForgeryToken/*, Authorize*/]
        public async Task<ActionResult> Create([Bind(Include = "Title,Subtitle,Body,TagId")] Blogpost blogpost, IEnumerable<HttpPostedFileBase> images) {

            try {
                if (ModelState.IsValid) {
                    await _blogpostRepository.InsertBlogpost(blogpost, images);
                    return RedirectToAction("Index");
                }
            }
            #region RetryLimitExceeded Catch

            catch (RetryLimitExceededException /* dex */) {
                // TODO: Log the error (uncomment dex variable name and add a line here to write a log
                ModelState.AddModelError("", "Unable to save changes. Try again and, if the problem persists, see your system administrator.");
            }

            #endregion

            await PopulateTagsDropDownList(blogpost.TagId);
            return View("~/Views/Blogpost/Create.cshtml", blogpost);
        }

        //
        // GET: Blogpost/Edit/5
        /// <summary>
        /// The edit a Blogpost screen
        /// </summary>
        /// <param name="id">Id of the blogpost to edit</param>
        /// <returns>Task<ActionResult></returns>
        [HttpGet, ActionName("Edit")/*, Authorize*/]
        public async Task<ActionResult> Edit(int? id) {

            #region Bad Request (null) Check

            if (id == null || id == 0) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            #endregion

            Blogpost blogpost  = await _blogpostRepository.GetBlogpostById(id);
            ViewBag.ModifiedOn = DateTime.Now.ToShortDateString();

            #region Not Found (null) Check

            if(blogpost == null) {
                return HttpNotFound();
            }

            #endregion

            await PopulateTagsDropDownList(blogpost.TagId);
            return View("~/Views/Blogpost/Edit.cshtml", blogpost);
        }

        //
        // POST: Blogpost/Edit/5
        /// <summary>
        /// Actually edit the specific Blogpost
        /// </summary>
        /// <param name="id">The Id of the Blogpost to update</param>
        /// <param name="rowVersion">Used to verify concurrency</param>
        /// <returns>Task<ActionResult></returns>
        [HttpPost, ActionName("Edit"), ValidateAntiForgeryToken/*, Authorize*/]
        public async Task<ActionResult> EditPost(int? id,  byte[] rowVersion) {

            #region Bad Request (null) Check

            if(id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            #endregion
            #region Properties

            string [] fieldsToBind        = {"Title", "Subtitle", "Body", "ModifiedOn", "RowVersion", "CommentId", "TagId", "UserId"};
            Blogpost  blogpostToUpdate    = await _blogpostRepository.GetBlogpostById(id);
            byte[]    rowVersionException = null;
            int?      tagIdException      = null;

            #endregion

            if(blogpostToUpdate == null) {
                Blogpost deletedBlogpost = new Blogpost();

                TryUpdateModel(deletedBlogpost, fieldsToBind);
                ModelState.AddModelError(string.Empty, "Unable to save changes. The Blogpost was deleted by another user.");
                return View("~/Views/Blogpost/Edit.cshtml", deletedBlogpost);
            }

            if(TryUpdateModel(blogpostToUpdate, fieldsToBind)) {
                try {
                    await _blogpostRepository.SetRowVersion(blogpostToUpdate, rowVersion);
                    return RedirectToAction("Index");
                }
                #region UpdateConcurrency Catch

                catch(DbUpdateConcurrencyException ex) {
                    // 3 lines below get both the values read from the DB and the new values entered by the user
                    DbEntityEntry entry            = ex.Entries.Single();
                    Blogpost clientValues          = (Blogpost)entry.Entity;
                    DbPropertyValues databaseEntry = entry.GetDatabaseValuesAsync().Result;
                    if(databaseEntry == null) {
                        ModelState.AddModelError(string.Empty,
                                                 "Unable to save changes. The Blogpost was deleted by another user.");
                    }
                    else {
                        Blogpost databaseValues = (Blogpost)entry.GetDatabaseValuesAsync().Result.ToObject();

                        // TODO: Make sure that the current logged on user gets inserted into the DB as the editor and the creator does not get changed

                        /*
                         * All if statements give custom error messages for each column that has DB values different from
                         *   what the user entered on the Edit page. The last if statement compares the TagId for that blogpost
                         *   and then gets the tags 'Name'
                         */
                        if(databaseValues.Title != clientValues.Title) {
                            ModelState.AddModelError("Title", "Current value: " + databaseValues.Title);
                        }
                        if(databaseValues.Subtitle != clientValues.Subtitle) {
                            ModelState.AddModelError("Subtitle", "Current value: " + databaseValues.Subtitle);
                        }
                        if(databaseValues.Body != clientValues.Body) {
                            ModelState.AddModelError("Body", "Current value: " + databaseValues.Body);
                        }
                        if(databaseValues.TagId != clientValues.TagId) {
                            // This captures the databaseValues' TagId so it can be saved below (must be done outside this catch since it needs to be awaited)
                            tagIdException = databaseValues.TagId;
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

                // Creating new catch for the connection resiliency stuff and the BlogConfiguration.cs class
                catch(RetryLimitExceededException /* dex */ ) {
                    // TODO: Log the error (uncomment dex variable name and add a line here to write a log
                    ModelState.AddModelError(string.Empty, "Unable to save changes. Try again and, if the problem persists, see the system administrator.");
                }

                #endregion
            }

            // This sets the 'TagId' of the Blogpost object to the new value retrieved from the DB, only if the rowVersionException variable was filled
            if(tagIdException != null) {
                ModelState.AddModelError("TagId", "Current value: " + (await _tagRepository.GetTagById(tagIdException)).Name);
            }

            // This sets the 'RowVersion' of the Blogpost object to the new value retrieved from the DB, only if the rowVersionException variable was filled
            if(rowVersionException != null) {
                await _blogpostRepository.SetRowVersion(blogpostToUpdate, rowVersionException);
            }

            await PopulateTagsDropDownList(blogpostToUpdate.TagId);
            return View("~/Views/Blogpost/Edit.cshtml", blogpostToUpdate);
        }

        //
        // GET: Blogpost/Delete/5
        /// <summary>
        /// The delete a Blogpost screen
        /// </summary>
        /// <param name="id">The Id of the blogpost to delete</param>
        /// <param name="concurrencyError">
        ///     Checks whether the page is being redisplayed after a concurrency error. If it = true, an error is sent to the ViewBag.
        ///     1 Error is given if the row was already deleted by another user and a different error is given if the row was changed after the
        ///     'Delete Confirmation' page is shown
        /// </param>
        /// <param name="saveChangesError">
        ///     Determines if a concurrency error has been detected already and this view is being returned to so delete can be tried again or canceled
        /// </param>
        /// <returns>Task<ActionResult></returns>
        [HttpGet, ActionName("Delete")/*, Authorize*/]
        public async Task<ActionResult> Delete(int? id, bool? concurrencyError/*, bool? saveChangesError = false*/) {

            #region Bad Request (null) Check

            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            #endregion

            Blogpost blogpost = await _blogpostRepository.GetBlogpostById(id);

            #region Not Found (null) Check & Concurrency Error Check

            if(blogpost == null) {

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

            return View("~/Views/Blogpost/Delete.cshtml", blogpost);
        }

        //
        // POST: Blogpost/Delete/5
        /// <summary>
        /// Delete a Blogpost
        /// </summary>
        /// <param name="blogpost">Blogpost to delete.</param>
        /// <returns>Task<ActionResult></returns>
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken/*, Authorize*/]
        public async Task<ActionResult> Delete(Blogpost blogpost) {

            try {
                await _blogpostRepository.DeleteBlogpost(blogpost);
                return RedirectToAction("Index");
            }
            #region UpdateConcurrency Catch

            catch (DbUpdateConcurrencyException) {

                return RedirectToAction("Delete", new { concurrencyError = true, id = blogpost.BlogpostId });
            }

            #endregion
            #region RetryLimitExceeded Catch

            catch (RetryLimitExceededException /* dex */) {

                // TODO: Log the error (uncomment dex variable name and add a line here to write a log
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again and, if the problem persists, see your system administrator.");
                return View("~/Views/Blogpost/Delete.cshtml", blogpost);
            }

            #endregion
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets a list of all tags, sorted by name, creates a SelectList collection for a drop-down list,
        /// and passes a collection to the view in a ViewBag property
        /// </summary>
        /// <param name="selectedTag">
        ///     Optional param that allows the calling code to specify the item that will be selected when the list is
        ///     rendered. The view will pass the name "TagId" to the DropDownList helper, and the helper then knows to
        ///     look in the ViewBag object for a SelectedList named "TagId"
        /// </param>
        /// <returns></returns>
        private async Task PopulateTagsDropDownList(object selectedTag = null) {

            List<Tag> tags = await (_tagRepository.GetTags(orderBy: t => t.OrderBy(x => x.Name)));
            ViewBag.TagId  = new SelectList(tags, "TagId", "Name", selectedTag);
        }

        #endregion
    }
}
