#region Usings

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#endregion

namespace HinesSite.Models {

    /// <summary>
    /// A single image being uploaded (usually to an Azure Blob Container)
    /// </summary>
    public class Image {

        /// <summary>
        /// The unique database identifier for the Image row
        /// </summary>
        [Key]
        public int ImageId { get; set; }

        [Required(ErrorMessage = "The image must have a title"), StringLength(25, MinimumLength = 1, ErrorMessage = "The title must be between 1 and 25 characters")]
        public string Title { get; set; }

        [StringLength(75, MinimumLength = 1, ErrorMessage = "The alternative text must be between 1 and 75 characters")]
        public string AltText { get; set; }

        [DataType(DataType.Html)]
        public string Caption { get; set; }

        [Required, DataType(DataType.ImageUrl), DisplayName("Image URL")]
        public string ImageUrl { get; set; }
    }
}
