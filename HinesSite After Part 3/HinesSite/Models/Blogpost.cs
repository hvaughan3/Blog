#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HinesSite.Interface;

#endregion

namespace HinesSite.Models {

    /// <summary>
    /// The main content on the site
    /// </summary>
    public class Blogpost : IAuditInfo {

        /// <summary>
        /// The unique database identifier for a Blogpost
        /// </summary>
        [Key]
        public int BlogpostId { get; set; }

        /// <summary>
        /// A Blogpost's main heading
        /// </summary>
        [Required(ErrorMessage = "The post must have a title"), StringLength(75, MinimumLength = 1, ErrorMessage = "The title must be between 1 and 75 characters")]
        public string Title { get; set; }

        /// <summary>
        /// A Blogpost's secondary heading
        /// </summary>
        [StringLength(150, MinimumLength = 1, ErrorMessage = "The subtitle must be between 1 and 150 characters"), DisplayFormat(NullDisplayText = "No subtitle")]
        public string Subtitle { get; set; }

        /// <summary>
        /// A Blogpost's main content
        /// </summary>
        [Required(ErrorMessage = "The post must have content"), StringLength(1000, MinimumLength = 5, ErrorMessage = "The post must be between 5 and 1000 characters")]
        public string Body { get; set; }

        /// <summary>
        /// The URL of an image related to a Blogpost
        /// </summary>
        [DisplayName("Images"), StringLength(200, ErrorMessage = "One or more image names might need to be shortened"), DataType(DataType.Url)]
        public string[] FileUrlArray { get; set; }

        #region IAuditInfo Implementation

        /// <summary>
        /// The date that the Blogpost was created
        /// </summary>
        [DisplayName("Created On"), DataType(DataType.Date)]
        public DateTime CreatedOn {
            // The following will automatically insert today's date unless a date has already been entered for it
            get { return _createdOn ?? DateTime.Now; }
            set { _createdOn = value; }
        }
        private DateTime? _createdOn;

        /// <summary>
        /// The date that the Blogpost was modified, was made optional
        /// </summary>
        [DisplayName("Modified On"), DataType(DataType.Date), DisplayFormat(NullDisplayText = "Not modified")]
        public DateTime? ModifiedOn { get; set; }

        #endregion

        /// <summary>
        /// The timestamp used for concurrency handling
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; }

        #region Relationships

        /// <summary>
        /// Unique database identifier for a Blogpost's comment(s)
        /// </summary>
        [DisplayName("Comments"), DisplayFormat(NullDisplayText = "No comments"), ForeignKey("Comments")]
        public int? CommentId { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

        /// <summary>
        /// Unique database identifier for a Blogpost's tag(s)
        /// </summary>
        [DisplayName("Tags"), DisplayFormat(NullDisplayText = "No tags"), ForeignKey("Tags")]
        public int? TagId { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }

        /// <summary>
        /// Unique database identifier for a Blogpost's creator
        /// </summary>
        [DisplayName("Creator"), ForeignKey("User")]
        public string UserId { get; set; }
        public virtual User User { get; set; }

        #endregion
    }
}