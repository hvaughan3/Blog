#region Usings

using System.Web.Mvc;

#endregion

namespace HinesSite.Controllers {

    /// <summary>
    /// Handles all error pages. Added the following to the release web.config transform to turn on custom errors:
    /// <customErrors mode="On" defaultRedirect="~/Error">
    ///     <error redirect="~/Error/NotFound" statusCode="404" />
    /// </customErrors>
    /// </summary>
    public class ErrorController : Controller {

        // GET: Error
        /// <summary>
        /// Default error view
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult Index() {

            return View("~/Views/Error/Index.cshtml");
        }

        // GET: Error/NotFound
        /// <summary>
        /// Error view for all 404s
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult NotFound() {

            Response.StatusCode = 404;
            return View("~/Views/Error/NotFound.cshtml");
        }
    }
}