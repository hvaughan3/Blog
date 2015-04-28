#region

using System.Linq;
using System.Web.Mvc;
using HinesSite.ViewModels;
using HinesSite.Data;
using HinesSite.Data.Context;

#endregion

namespace HinesSite.Controllers {

    /// <summary>
    /// Homepage controller
    /// </summary>
    public class HomeController : Controller {

        #region Properies

        private ApplicationDbContext _db = new ApplicationDbContext();

        #endregion

        #region Actions

        /// <summary>
        /// The view for displaying the homepage
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult Index() {

            return View("~/Views/Home/Index.cshtml");
        }

        /// <summary>
        /// The view for the about page, displaying Blogposts
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult About() {

            /*
             * Groups the blog post entities by create date and then title, calculates # of posts, and stores results in
             *   a collection of EnrollmentDateGroup view model objects
             */
            IQueryable<BlogpostGroup> data = from  blogpost in _db.Blogposts
                                             group blogpost by new { blogpost.CreatedOn, blogpost.Title, blogpost.BlogpostId }
                                                 into dateGroup
                                                 select new BlogpostGroup {
                                                     BlogpostId = dateGroup.Key.BlogpostId,
                                                     Title      = dateGroup.Key.Title,
                                                     CreatedOn  = dateGroup.Key.CreatedOn};

            return View("~/Views/Home/About.cshtml", data.ToList());
        }

        /// <summary>
        /// The view to display the contact page
        /// </summary>
        /// <returns></returns>
        public ActionResult Contact() {

            ViewBag.Message = "Contact page.";
            return View("~/Views/Home/Contact.cshtml");
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Disposes of the DB Context
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing) {
            _db.Dispose();
            base.Dispose(disposing);
        }

        #endregion
    }
}