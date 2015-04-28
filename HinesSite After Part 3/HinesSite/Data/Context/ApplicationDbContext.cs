#region Usings

using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using HinesSite.Data.Context.Mappings;
using HinesSite.Models;
using Microsoft.AspNet.Identity.EntityFramework;

#endregion
// ReSharper disable MissingXmlDoc

namespace HinesSite.Data.Context {

    public class ApplicationDbContext : IdentityDbContext<User> {

        public ApplicationDbContext () : base("DefaultConnection", false) { }

        public static ApplicationDbContext Create() {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating (DbModelBuilder modelBuilder) {

            #region Model Mappings

            modelBuilder.Configurations.Add(new BlogpostMap());
            modelBuilder.Configurations.Add(new CommentMap());
            modelBuilder.Configurations.Add(new TagMap());
            //modelBuilder.Configurations.Add(new ImageMap());
            //modelBuilder.Configurations.Add(new BlogpostImageMap());

            #endregion

            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Blogpost>().HasMany(b => b.Tags).WithMany(t => t.Blogposts)
                .Map(m => m.MapLeftKey("BlogpostId").MapRightKey("TagId")
                    .ToTable("BlogpostTag"));

            //modelBuilder.Entity<Blogpost>().MapToStoredProcedures();
            //modelBuilder.Entity<Tag>().MapToStoredProcedures();
        }

        #region DBSets

        public DbSet<Blogpost>            Blogposts      { get; set; }
        public DbSet<Comment>             Comments       { get; set; }
        public DbSet<Tag>                 Tags           { get; set; }
        //public DbSet<Image>               Images         { get; set; }
        //public DbSet<BlogpostImageLookup> BlogpostImages { get; set; }

        #endregion
    }
}