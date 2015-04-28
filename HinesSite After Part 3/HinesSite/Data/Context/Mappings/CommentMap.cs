#region Usings

using System.Data.Entity.ModelConfiguration;
using HinesSite.Models;

#endregion

namespace HinesSite.Data.Context.Mappings {

    /// <summary>
    /// Mapping used to map the Comment class
    /// </summary>
    public class CommentMap : EntityTypeConfiguration<Comment> {

        /// <summary>
        /// Default constructor
        /// </summary>
        public CommentMap() {

            HasKey(b => b.CommentId);
            ToTable("Comment");
        }
    }
}