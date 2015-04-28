#region Usings

using System.Data.Entity.ModelConfiguration;
using HinesSite.Models;

#endregion

namespace HinesSite.Data.Context.Mappings {

    /// <summary>
    /// Mapping used to map the Image class
    /// </summary>
    public class ImageMap : EntityTypeConfiguration<Image> {

        /// <summary>
        /// Default constructor
        /// </summary>
        public ImageMap() {

            HasKey(b => b.ImageId);
            ToTable("Image");
        }
    }
}
