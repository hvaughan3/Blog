#region Usings

using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using HinesSite.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using HinesSite.Data.Context;
using HinesSite.ViewModels;

#endregion
#pragma warning disable 1570

namespace HinesSite.Controllers {

    /// <summary>
    /// Controller responsible for all account related tasks
    /// </summary>
    //[Authorize]
    public class AccountController : Controller {

        #region Properties

        private UserManager          _userManager;
        private ApplicationDbContext _db = new ApplicationDbContext();

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public AccountController() { }

        /// <summary>
        /// Constructor for setting the UserManager
        /// </summary>
        /// <param name="userManager"></param>
        public AccountController(UserManager userManager) {
            UserManager = userManager;
        }

        #endregion

        #region Actions

        /// <summary>
        /// User Manager property getter and setter
        /// </summary>
        public UserManager UserManager {
            get {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<UserManager>();
            }
            private set {
                _userManager = value;
            }
        }

        //
        // GET: /Account/View
        /// <summary>
        /// The view listing all users and their blogposts
        /// </summary>
        /// <param name="id">The Id of a specific user to display results for</param>
        /// <returns>ActionResult</returns>
        [HttpGet, ActionName("UserList"), AllowAnonymous/*, Authorize( Roles = "Administrator" )*/]
        public ActionResult UserList(int? id) {

            UserIndexData viewModel = new UserIndexData {
                Users = _db.Users
                           .Include(u => u.Blogposts)
                           .Include(u => u.Comments)
                           .OrderBy(u => u.UserName)
            };

            // Gets id from the details link in order to display details of a specific user from the list
            if(id != null) {

                  ViewBag.UserId    = id.Value;
                viewModel.Blogposts = viewModel.Users.Single(u => u.BlogpostId == id.Value).Blogposts;
            }

            return View("~/Views/Account/UserList.cshtml", viewModel);
        }

        //
        // GET: /Account/Login
        /// <summary>
        /// The view for logging a user into the site
        /// </summary>
        /// <param name="returnUrl">URL to return the user to after successfully logging in</param>
        /// <returns>ActionResult</returns>
        [HttpGet, ActionName("Login"), AllowAnonymous]
        public ActionResult Login(string returnUrl) {

            ViewBag.ReturnUrl = returnUrl;
            return View("~/Views/Account/Login.cshtml");
        }

        //
        // POST: /Account/Login
        /// <summary>
        /// Logs a user into the site
        /// </summary>
        /// <param name="model">The login view model of user information</param>
        /// <param name="returnUrl">URL to return the user to after logging in successfully</param>
        /// <returns>Task<ActionResult></returns>
        [HttpPost, ActionName("Login"), ValidateAntiForgeryToken, AllowAnonymous]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl) {

            if(ModelState.IsValid) {
                User user = await UserManager.FindAsync(model.Email, model.Password);

                #region Not Found (null) Check

                if(user != null) {
                    await SignInAsync(user, model.RememberMe);
                    return RedirectToLocal(returnUrl);
                }

                #endregion

                ModelState.AddModelError("", "Invalid username or password.");
            }

            // If we got this far, something failed, redisplay form
            return View("~/Views/Account/Login.cshtml", model);
        }

        //
        // GET: /Account/Register
        /// <summary>
        /// The view for registering as a new user
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet, ActionName("Register"), AllowAnonymous]
        public ActionResult Register() {
            return View("~/Views/Account/Register.cshtml");
        }

        //
        // POST: /Account/Register
        /// <summary>
        /// Registers a new user to the site
        /// </summary>
        /// <param name="model">The View Model containing User information</param>
        /// <returns>Task<ActionResult></returns>
        [HttpPost, ActionName("Register"), ValidateAntiForgeryToken, AllowAnonymous]
        public async Task<ActionResult> Register(RegisterViewModel model) {

            if(ModelState.IsValid) {

                User           user   = new User { UserName = model.Email, Email = model.Email };
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);

                if(result.Succeeded) {
                    await SignInAsync(user, false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View("~/Views/Account/Register.cshtml", model);
        }

        //
        // GET: /Account/ConfirmEmail
        /// <summary>
        /// View used to confirm a user's email address
        /// </summary>
        /// <param name="userId">The User Id for the confirmed email address</param>
        /// <param name="code">A token used to allow a user to confirm their email address</param>
        /// <returns>Task<ActionResult></returns>
        [HttpGet, ActionName("ConfirmEmail"), AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code) {

            #region Bad Request (null) Check

            if(userId == null || code == null) {
                return View("~/Views/Shared/Error.cshtml");
            }

            #endregion

            IdentityResult result = await UserManager.ConfirmEmailAsync(userId, code);

            #region Auth Check

            if(result.Succeeded) {
                return View("~/Views/Account/ConfirmEmail.cshtml");
            }

            #endregion

            AddErrors(result);
            return View("~/Views/Account/ConfirmEmail.cshtml");
        }

        //
        // GET: /Account/ForgotPassword
        /// <summary>
        /// The view used when a user forgets their password
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet, ActionName("ForgotPassword"), AllowAnonymous]
        public ActionResult ForgotPassword() {
            return View("~/Views/Account/ForgotPassword.cshtml");
        }

        //
        // POST: /Account/ForgotPassword
        /// <summary>
        /// Allows a user to reset their password
        /// </summary>
        /// <param name="model">The Reset Password View Model</param>
        /// <returns>Task<ActionResult></returns>
        [HttpPost, ActionName("ForgotPassword"), ValidateAntiForgeryToken, AllowAnonymous]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model) {

            if(ModelState.IsValid) {
                User user = await UserManager.FindByNameAsync(model.Email);

                #region Not Found (null) Check & Email Confirmed Check

                if(user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id))) {
                    ModelState.AddModelError("", "The user either does not exist or is not confirmed.");
                    return View("~/Views/Account/ForgotPassword.cshtml");
                }

                #endregion

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View("~/Views/Account/ForgotPassword.cshtml", model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        /// <summary>
        /// View used for confirmation of a forgot password action
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet, ActionName("ForgotPasswordConfirmation"), AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation() {
            return View("~/Views/Account/ForgotPasswordConfirmation.cshtml");
        }

        //
        // GET: /Account/ResetPassword
        /// <summary>
        /// View which allows a user to reset their password
        /// </summary>
        /// <param name="code">Token used to allow a user to reset their password</param>
        /// <returns>ActionResult</returns>
        [HttpGet, ActionName("ResetPassword"), AllowAnonymous]
        public ActionResult ResetPassword(string code) {

            #region Bad Request

            if(code == null) {
                return View("~/Views/Shared/Error.cshtml");
            }

            #endregion

            return View("~/Views/Account/ResetPassword.cshtml");
        }

        //
        // POST: /Account/ResetPassword
        /// <summary>
        /// Resets a given user's password
        /// </summary>
        /// <param name="model">The user's information to reset to</param>
        /// <returns>Task<ActionResult></returns>
        [HttpPost, ActionName("ResetPassword"), ValidateAntiForgeryToken, AllowAnonymous]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model) {

            if(ModelState.IsValid) {
                User user = await UserManager.FindByNameAsync(model.Email);

                #region Not Found (null) Check

                if(user == null) {
                    ModelState.AddModelError("", "No user found.");
                    return View("~/Views/Account/ResetPassword.cshtml");
                }

                #endregion

                IdentityResult result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);

                #region Auth Check

                if(result.Succeeded) {
                    return RedirectToAction("ResetPasswordConfirmation", "Account");
                }

                #endregion

                AddErrors(result);

                return View("~/Views/Account/ResetPassword.cshtml");
            }

            // If we got this far, something failed, redisplay form
            return View("~/Views/Account/ResetPassword.cshtml", model);
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        /// <summary>
        /// View used for confirming a password reset action
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet, ActionName("ResetPasswordConfirmation"), AllowAnonymous]
        public ActionResult ResetPasswordConfirmation() {
            return View("~/Views/Account/ResetPasswordConfirmation.cshtml");
        }

        //
        // POST: /Account/Disassociate
        /// <summary>
        /// Remove the link between an account and an external login provider
        /// </summary>
        /// <param name="loginProvider">The external login provider to disassociate from</param>
        /// <param name="providerKey">The external login provider's key</param>
        /// <returns>Task<ActionResult></returns>
        [HttpPost, ActionName("Disassociate"), ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey) {

            ManageMessageId? message;

            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));

            #region Auth Check

            if(result.Succeeded) {
                User user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

                await SignInAsync(user, false);

                message = ManageMessageId.RemoveLoginSuccess;
            }
            else {
                message = ManageMessageId.Error;
            }

            #endregion

            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        /// <summary>
        /// View used for managing a user's account
        /// </summary>
        /// <param name="message">Message to display to user upon an action or error</param>
        /// <returns>ActionResult</returns>
        [HttpGet, ActionName("Manage")]
        public ActionResult Manage(ManageMessageId? message) {

            ViewBag.StatusMessage = message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                                  : message == ManageMessageId.SetPasswordSuccess    ? "Your password has been set."
                                  : message == ManageMessageId.RemoveLoginSuccess    ? "The external login was removed."
                                  : message == ManageMessageId.Error                 ? "An error has occurred."
                                  : "";

            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl        = Url.Action("Manage");

            return View("~/Views/Account/ResetPasswordConfirmation.cshtml");
        }

        //
        // POST: /Account/Manage
        /// <summary>
        /// Changes a user's account information
        /// </summary>
        /// <param name="model">The User's information</param>
        /// <returns>Task<ActionResult></returns>
        [HttpPost, ActionName("Manage"),ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model) {

            #region Properties

            bool hasPassword         = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl        = Url.Action("Manage");

            #endregion

            if(hasPassword) {
                if(ModelState.IsValid) {

                    IdentityResult result = await UserManager.ChangePasswordAsync( User.Identity.GetUserId(), model.OldPassword, model.NewPassword);

                    #region Auth Check

                    if(result.Succeeded) {
                        User user = await UserManager.FindByIdAsync( User.Identity.GetUserId());

                        await SignInAsync(user, false);

                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }

                    #endregion

                    AddErrors(result);
                }
            }
            else {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];

                if(state != null) {
                    state.Errors.Clear();
                }

                if(ModelState.IsValid) {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

                    #region Auth Check

                    if(result.Succeeded) {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }

                    #endregion

                    AddErrors(result);
                }
            }

            // If we got this far, something failed, redisplay form
            return View("~/Views/Account/Manage.cshtml", model);
        }

        //
        // POST: /Account/ExternalLogin
        /// <summary>
        /// View used for authenticating a user through an external login provider
        /// </summary>
        /// <param name="provider">The external login provider used to authenticate a user to</param>
        /// <param name="returnUrl">URL to return the user to after a successfull login</param>
        /// <returns></returns>
        [HttpPost, ActionName("ExternalLogin"), ValidateAntiForgeryToken, AllowAnonymous]
        public ActionResult ExternalLogin(string provider, string returnUrl) {

            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        /// <summary>
        /// Callback used during external logins
        /// </summary>
        /// <param name="returnUrl">URL to return the user to after successfull login</param>
        /// <returns></returns>
        [HttpGet, ActionName("ExternalLoginCallback"), AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl) {

            ExternalLoginInfo loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();

            #region Not Found (null) Check

            if(loginInfo == null) {
                return RedirectToAction("Login");
            }

            #endregion

            // Sign in the user with this external login provider if the user already has a login
            User user = await UserManager.FindAsync(loginInfo.Login);

            #region Not Found (null) Check

            if(user != null) {
                 await SignInAsync(user, false);
                return RedirectToLocal(returnUrl);
            }

            #endregion

            // If the user does not have an account, then prompt the user to create an account
            ViewBag.ReturnUrl     = returnUrl;
            ViewBag.LoginProvider = loginInfo.Login.LoginProvider;

            return View("~/Views/Account/ExternalLoginConfirmation.cshtml", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
        }

        //
        // POST: /Account/LinkLogin
        /// <summary>
        /// Used to link an account to an external login provider
        /// </summary>
        /// <param name="provider">The external login provider</param>
        /// <returns>ActionResult</returns>
        [HttpPost, ActionName("LinkLogin"), ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider) {

            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        /// <summary>
        /// Callback used for linking account to external login providers
        /// </summary>
        /// <returns>Task<ActionResult></returns>
        [HttpGet, ActionName("LinkLoginCallback")]
        public async Task<ActionResult> LinkLoginCallback() {

            ExternalLoginInfo loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());

            #region Not Found (null) Check

            if(loginInfo == null) {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }

            #endregion

            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);

            #region Auth Check

            if(result.Succeeded) {
                return RedirectToAction("Manage");
            }

            #endregion

            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        /// <summary>
        /// The view which shows confirmation of your external login
        /// </summary>
        /// <param name="model">View Model for the users external login</param>
        /// <param name="returnUrl">URL which the user will be returned to once successfully logged in</param>
        /// <returns>Task<ActionResult></returns>
        [HttpPost, ActionName("ExternalLoginConfirmation"), ValidateAntiForgeryToken, AllowAnonymous]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl) {

            #region Already Authenticated Check

            if(User.Identity.IsAuthenticated) {
                return RedirectToAction("Manage");
            }

            #endregion

            if(ModelState.IsValid) {
                // Get the information about the user from the external login provider
                ExternalLoginInfo info = await AuthenticationManager.GetExternalLoginInfoAsync();

                #region Not Found (null) Check

                if(info == null) {
                    return View("~/Views/Account/ExternalLoginFailure.cshtml");
                }

                #endregion

                User user = new User { UserName = model.Email, Email = model.Email };

                IdentityResult result = await UserManager.CreateAsync(user);

                if(result.Succeeded) {

                    result = await UserManager.AddLoginAsync(user.Id, info.Login);

                    if(result.Succeeded) {
                        await SignInAsync(user, false);

                        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                        // Send an email with this link
                        // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        // string callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        // SendEmail(user.Email, callbackUrl, "Confirm your account", "Please confirm your account by clicking this link");

                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View("~/Views/Account/ExternalLoginConfirmation.cshtml", model);
        }

        //
        // POST: /Account/LogOff
        /// <summary>
        /// Logs the user out of the site
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpPost, ActionName("LogOff"), ValidateAntiForgeryToken]
        public ActionResult LogOff() {

            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        /// <summary>
        /// View which shows when user fails to login to external auth provider
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet, ActionName("ExternalLoginFailure"), AllowAnonymous]
        public ActionResult ExternalLoginFailure() {

            return View("~/Views/Account/ExternalLoginFailure.cshtml");
        }

        /// <summary>
        /// View which shows a list of account to remove
        /// </summary>
        /// <returns>ActionResult</returns>
        [ActionName("RemoveAccountList"), ChildActionOnly]
        public ActionResult RemoveAccountList() {

            IList<UserLoginInfo> linkedAccounts = UserManager.GetLogins( User.Identity.GetUserId());
                       ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;

            return PartialView("~/Views/Account/_RemoveAccountPartial.cshtml", linkedAccounts);
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Disposes of the UserManager
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing) {

            if(disposing && UserManager != null) {
                UserManager.Dispose();
                UserManager = null;
            }

            base.Dispose(disposing);
        }

        #endregion

        #region Helpers

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager {

            get { return HttpContext.GetOwinContext().Authentication; }
        }

        private async Task SignInAsync(User user, bool isPersistent) {

            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }

        private void AddErrors(IdentityResult result) {

            foreach(string error in result.Errors) {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword() {

            User user = UserManager.FindById(User.Identity.GetUserId());

            #region Not Found (null) Check

            if(user != null) {
                return user.PasswordHash != null;
            }

            #endregion

            return false;
        }

        private void SendEmail(string email, string callbackUrl, string subject, string message) {
            // For information on sending mail, please visit http://go.microsoft.com/fwlink/?LinkID=320771
        }

        /// <summary>
        /// Manages the various confirmation messages
        /// </summary>
        public enum ManageMessageId {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl) {

            if(Url.IsLocalUrl(returnUrl)) {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        private class ChallengeResult : HttpUnauthorizedResult {

            public ChallengeResult(string provider, string redirectUri, string userId = null) {

                LoginProvider = provider;
                RedirectUri   = redirectUri;
                UserId        = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri   { get; set; }
            public string UserId        { get; set; }

            public override void ExecuteResult(ControllerContext context) {

                AuthenticationProperties properties = new AuthenticationProperties { RedirectUri = RedirectUri };

                if(UserId != null) {
                    properties.Dictionary[XsrfKey] = UserId;
                }

                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        #endregion
    }
}