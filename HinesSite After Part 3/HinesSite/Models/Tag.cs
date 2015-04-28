#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HinesSite.Interface;

#endregion

namespace HinesSite.Models {

    /// <summary>
    /// A single Tag (usually connected to various Blogposts)
    /// </summary>
    public class Tag : IAuditInfo {

        /// <summary>
        /// The unique database identifier for the Tag row
        /// </summary>
        [Key]
        public int TagId { get; set; }

        /// <summary>
        /// The name of the Tag
        /// </summary>
        [Required( ErrorMessage = "You must give the tag a name" ),
        StringLength( 30, MinimumLength = 1, ErrorMessage = "The tag may not be more than 30 characters long" )]
        public string Name { get; set; }

        private DateTime? _createdOn;

        /// <summary>
        /// The date that the Tag was created
        /// </summary>
        [DisplayName("Created On"), DataType( DataType.Date )]
        public DateTime CreatedOn {
            // The following will automatically insert today's date unless a date has already been entered for it
            get { return _createdOn ?? DateTime.Now; }
            set { _createdOn = value; }
        }

        private DateTime? _modifiedOn;

        /// <summary>
        /// The date that the Tag was modified, was made optional
        /// </summary>
        [DisplayName("Modified On"), DataType(DataType.Date)]
        public DateTime? ModifiedOn {
            // The following will insert todays date when the column is being set
            get { return _modifiedOn ?? DateTime.Now; }
            set { _modifiedOn = DateTime.Now; }
        }

        /// <summary>
        /// The timestamp used for concurrency handling
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; }


        public int? BlogpostId { get; set; }
        public virtual ICollection<Blogpost> Blogposts { get; set; }
    }
}