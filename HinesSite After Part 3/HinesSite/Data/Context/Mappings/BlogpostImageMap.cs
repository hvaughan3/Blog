#region Usings

using System.Data.Entity.ModelConfiguration;
using HinesSite.Models;

#endregion

namespace HinesSite.Data.Context.Mappings {

    /// <summary>
    /// Mapping used to map the BlogpostImage class
    /// </summary>
    public class BlogpostImageMap : EntityTypeConfiguration<BlogpostImageLookup> {

        /// <summary>
        /// Default constructor
        /// </summary>
        public BlogpostImageMap() {

            HasKey(b => b.BlogpostImageLookupId);
            ToTable("BlogpostImageLookup");
        }
    }
}
