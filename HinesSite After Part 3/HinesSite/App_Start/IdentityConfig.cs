﻿#region Usings

using System.Threading.Tasks;
using HinesSite.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using HinesSite.Data.Context;
using Microsoft.Owin.Security.DataProtection;

#endregion

namespace HinesSite {

    /// <summary>
    /// Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    /// </summary>
    public class UserManager : UserManager<User> {

        public UserManager(IUserStore<User> store) : base(store) { }

        public static UserManager Create(IdentityFactoryOptions<UserManager> options, IOwinContext context) {
            UserManager manager = new UserManager(new UserStore<User>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator                  = new UserValidator<User>(manager) {
                    AllowOnlyAlphanumericUserNames = false,
                    RequireUniqueEmail             = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator       = new PasswordValidator {
                    RequiredLength          = 6,
                    RequireNonLetterOrDigit = true,
                    RequireDigit            = true,
                    RequireLowercase        = true,
                    RequireUppercase        = true,
            };
            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug in here.
            manager.RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<User> {
                MessageFormat = "Your security code is: {0}"
            });
            manager.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<User> {
                Subject    = "Security Code",
                BodyFormat = "Your security code is: {0}"
            });

            manager.EmailService = new EmailService();
            manager.SmsService   = new SmsService();

            IDataProtectionProvider dataProtectionProvider = options.DataProtectionProvider;

            if(dataProtectionProvider != null) {
                manager.UserTokenProvider = new DataProtectorTokenProvider<User>(dataProtectionProvider.Create("ASP.NET Identity"));
            }

            return manager;
        }
    }

    public class EmailService : IIdentityMessageService {
        public Task SendAsync(IdentityMessage message) {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService {
        public Task SendAsync(IdentityMessage message) {
            // Plug in your sms service here to send a text message.
            return Task.FromResult(0);
        }
    }
}