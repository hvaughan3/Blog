#region Usings

using System.Data.Entity.ModelConfiguration;
using HinesSite.Models;

#endregion

namespace HinesSite.Data.Context.Mappings {

    /// <summary>
    /// Mapping used to map the Blogpost class
    /// </summary>
    public class TagMap : EntityTypeConfiguration<Tag> {

        /// <summary>
        /// Default constructor
        /// </summary>
        public TagMap() {

            HasKey(t => t.TagId);
            ToTable("Tag");
        }
    }
}