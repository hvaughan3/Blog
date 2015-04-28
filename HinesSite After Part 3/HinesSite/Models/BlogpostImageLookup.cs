#region Usings

using System.ComponentModel.DataAnnotations;

#endregion

namespace HinesSite.Models {

    /// <summary>
    /// Database link between Blogposts and their attached Image(s)
    /// </summary>
    public class BlogpostImageLookup {

        /// <summary>
        /// The unique database identifier for the BlogpostImage row
        /// </summary>
        [Key]
        public int BlogpostImageLookupId { get; set; }

        /// <summary>
        /// The unique database identifier for the Blogpost row
        /// </summary>
        [Required]
        public int BlogpostId { get; set; }

        /// <summary>
        /// The unique database identifier for the Image row
        /// </summary>
        [Required]
        public int ImageId { get; set; }
    }
}
