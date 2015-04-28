using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HinesSite.Models {

    public class User : IdentityUser {

        [DisplayName("First Name"), StringLength(60, MinimumLength = 1, ErrorMessage = "Your first name must be between 1 and 35 characters long")]
        public string FirstName { get; set; }

        [DisplayName("Last Name"), StringLength(75, MinimumLength = 1, ErrorMessage = "Your last name must be between 1 and 45 characters long")]
        public string LastName { get; set; }

        private DateTime? _joinDate;
        [DisplayName("Join Date"), DataType(DataType.Date)]
        public DateTime? JoinDate {

            // The following will automatically insert today's date unless a date has already been entered for it
            get { return _joinDate ?? DateTime.Now; }
            set { _joinDate = value; }
        }

        // Made BlogpostId optional since it must be explicitly made optional for int types
        [DisplayFormat(NullDisplayText = "Has made 0 blog posts"), ForeignKey("Blogposts")]
        public int? BlogpostId { get; set; }
        public virtual ICollection<Blogpost> Blogposts { get; set; }

        [DisplayFormat(NullDisplayText = "Has not left any comments."), ForeignKey("Comments")]
        public int? CommentId { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync( UserManager<User> manager ) {

            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            ClaimsIdentity userIdentity = await manager.CreateIdentityAsync( this, DefaultAuthenticationTypes.ApplicationCookie );
            // Add custom user claims here
            return userIdentity;
        }
    }
}