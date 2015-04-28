#region Usings

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HinesSite.Interface;

#endregion

// ReSharper disable MissingXmlDoc

namespace HinesSite.Models {

    /// <summary>
    /// A single Comment (probably made on a specific Blogpost)
    /// </summary>
    public class Comment : IAuditInfo {

        /// <summary>
        /// The unique database identifier for the Comment row
        /// </summary>
        [Key]
        public int CommentId { get; set; }

        [StringLength( 75, MinimumLength = 1, ErrorMessage = "If you add a title, it must be between 1 and 75 characters long" )]
        public string Title { get; set; }

        [StringLength( 50, MinimumLength = 1, ErrorMessage = "If you add a nickname, it must be between 1 and 50 characters long" )]
        public string Name { get; set; }

        [StringLength( 50, MinimumLength = 1, ErrorMessage = "If you add contact information, it must be between 1 and 50 characters long" )]
        public string Contact { get; set; }

        [DisplayName( "Body" ), Required( ErrorMessage = "Your comment must have content" ),
        StringLength( 1000, MinimumLength = 1, ErrorMessage = "Your comment body must be between 1 and 1000 characters long" )]
        public string CommentBody { get; set; }

        private DateTime? _createdOn;
        [DisplayName( "Created On" ), DataType( DataType.Date )]
        public DateTime CreatedOn
        {
            // The following will automatically insert today's date unless a date has already been entered for it
            get { return _createdOn ?? DateTime.Now; }
            set { _createdOn = value; }
        }

        // Made ModifiedOn optional since it must be explicitly done for DateTime types
        [DisplayName( "Modified On" ), DisplayFormat( NullDisplayText = "Not Modified" ), DataType( DataType.Date )]
        public DateTime? ModifiedOn { get; set; }


        [ForeignKey( "Blogpost" )]
        public int BlogpostId { get; set; }
        public Blogpost Blogpost { get; set; }

        [ForeignKey( "User" )]
        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}