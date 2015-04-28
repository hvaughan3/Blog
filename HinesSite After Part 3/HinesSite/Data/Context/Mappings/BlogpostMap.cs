using System.Data.Entity.ModelConfiguration;
using HinesSite.Models;

namespace HinesSite.Data.Context.Mappings {

    /// <summary>
    /// Mapping used to map the Blogpost class
    /// </summary>
    public class BlogpostMap : EntityTypeConfiguration<Blogpost> {

        /// <summary>
        /// Default constructor
        /// </summary>
        public BlogpostMap() {

            HasKey(b => b.BlogpostId);
            ToTable("Blogpost");
        }
    }
}