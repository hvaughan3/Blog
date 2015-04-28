#region Usings

using System.Data.Entity.Migrations;
using HinesSite.Data.Context;

#endregion

namespace HinesSite.Migrations {

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext> {
        public Configuration() {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context) {        //    //  This method will be called after migrating to the latest version.

            //    //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //    //  to avoid creating duplicate seed data. E.g.
            //    //
            //    //    context.People.AddOrUpdate(
            //    //      p => p.FullName,
            //    //      new Person { FullName = "Andrew Peters" },
            //    //      new Person { FullName = "Brice Lambson" },
            //    //      new Person { FullName = "Rowan Miller" }
            //    //    );
            //    //
            //    // This will run when Update-Database is used, while SiteInitializer.cs will run when the site is ran

            //    var users = new List<User>
            //    {
            //        new User
            //        {
            //            Email = "hvaughan3@gmail.com",
            //            EmailConfirmed = false,
            //            PhoneNumberConfirmed = false,
            //            UserName = "hvaughan3@gmail.com",
            //            TwoFactorEnabled = false,
            //            LockoutEnabled = false,
            //            AccessFailedCount = 0,
            //            Blogposts = new List<Blogpost>()
            //        },
            //        new User
            //        {
            //            Email = "user2@gmail.com",
            //            EmailConfirmed = false,
            //            PhoneNumberConfirmed = false,
            //            UserName = "user2@gmail.com",
            //            TwoFactorEnabled = false,
            //            LockoutEnabled = false,
            //            AccessFailedCount = 0,
            //            Blogposts = new List<Blogpost>()
            //        },
            //        new User
            //        {
            //            Email = "user3@gmail.com",
            //            EmailConfirmed = true,
            //            PhoneNumberConfirmed = false,
            //            UserName = "user3@gmail.com",
            //            TwoFactorEnabled = false,
            //            LockoutEnabled = false,
            //            AccessFailedCount = 0,
            //            Blogposts = new List<Blogpost>()
            //        }
            //    };
            //    users.ForEach(u => context.Users.AddOrUpdate(p => p.Email, u));
            //    context.SaveChanges();

            //    var tags = new List<Tag>
            //    {
            //        new Tag
            //        {
            //            //BlogpostId = 3,
            //            Name = "C#",
            //            CreatedOn = new DateTime(2014, 08, 14)
            //            //Blogposts = new List<Blogpost>()
            //        },
            //        new Tag
            //        {
            //            //BlogpostId = 2,
            //            Name = "ASP.Net",
            //            CreatedOn = new DateTime(2012, 05, 14)
            //            //Blogposts = new List<Blogpost>()
            //        },
            //        new Tag
            //        {
            //            //BlogpostId = 1,
            //            Name = "MVC",
            //            CreatedOn = new DateTime(2014, 02, 14)
            //            //Blogposts = new List<Blogpost>()
            //        },
            //        new Tag
            //        {
            //            Name = "Random",
            //            CreatedOn = new DateTime(2014, 07, 30)
            //        }
            //    };
            //    tags.ForEach(t => context.Tags.AddOrUpdate(p => p.TagId, t));
            //    context.SaveChanges();

            //    var blogposts = new List<Blogpost>
            //    {
            //        new Blogpost
            //        {
            //            Title = "Blog Post Title 1",
            //            Subtitle = "This is the blog post 1 subtitle",
            //            Body = "This is the body of blog post #1",
            //            CreatedOn = new DateTime(2008, 09, 01),
            //            //TagId = 3,
            //            Id = users.Single(u => u.UserName == "hvaughan3@gmail.com").Id,
            //            Tags = new List<Tag>(),
            //            Comments = new List<Comment>()
            //        },
            //        new Blogpost
            //        {
            //            Title = "Blog Post Title 2",
            //            Subtitle = "This is the blog post 2 subtitle",
            //            Body = "This is the body of blog post #2",
            //            CreatedOn = new DateTime(2012, 05, 31),
            //            //TagId = 2,
            //            Id = users.Single(u => u.UserName == "hvaughan3@gmail.com").Id,
            //            Tags = new List<Tag>(),
            //            Comments = new List<Comment>()
            //        },
            //        new Blogpost
            //        {
            //            Title = "Blog Post Title 3",
            //            Subtitle = "This is the blog post 3 subtitle",
            //            Body = "This is the body of blog post #3",
            //            CreatedOn = new DateTime(2014, 07, 16),
            //            //TagId = 1,
            //            Id = users.Single(u => u.UserName == "hvaughan3@gmail.com").Id,
            //            Tags = new List<Tag>(),
            //            Comments = new List<Comment>()
            //        },
            //        new Blogpost
            //        {
            //            Title = "Blog Post Title 4",
            //            Subtitle = "This is the blog post 4 subtitle",
            //            Body = "This is the body of blog post #4",
            //            CreatedOn = new DateTime(2014, 07, 30),
            //            Id = users.Single(u => u.UserName == "hvaughan3@gmail.com").Id,
            //            Tags = new List<Tag>(),
            //            Comments = new List<Comment>()
            //        },
            //        new Blogpost
            //        {
            //            Title = "Blog Post Title 5",
            //            Subtitle = "This is the blog post 5 subtitle",
            //            Body = "This is the body of blog post #5",
            //            CreatedOn = new DateTime(2014, 07, 30),
            //            Id = users.Single(u => u.UserName == "hvaughan3@gmail.com").Id,
            //            Tags = new List<Tag>(),
            //            Comments = new List<Comment>()
            //        },
            //        new Blogpost
            //        {
            //            Title = "Blog Post Title 6",
            //            Subtitle = "This is the blog post 6 subtitle",
            //            Body = "This is the body of blog post #6",
            //            CreatedOn = new DateTime(2014, 07, 30),
            //            Id = users.Single(u => u.UserName == "hvaughan3@gmail.com").Id,
            //            Tags = new List<Tag>(),
            //            Comments = new List<Comment>()
            //        }
            //    };
            //    blogposts.ForEach(b => context.Blogposts.AddOrUpdate(p => p.Title, b));
            //    context.SaveChanges();

            //    AddOrUpdateTags(context, "Blog Post Title 6", "Random");
            //    AddOrUpdateTags(context, "Blog Post Title 5", "MVC");
            //    AddOrUpdateTags(context, "Blog Post Title 4", "MVC");
            //    AddOrUpdateTags(context, "Blog Post Title 3", "C#");
            //    AddOrUpdateTags(context, "Blog Post Title 2", "ASP.Net");
            //    AddOrUpdateTags(context, "Blog Post Title 1", "MVC");
            //    context.SaveChanges();

            //    var comments = new List<Comment>
            //    {
            //        new Comment
            //        {
            //            Title = "Comment 1's Title For Blog Post #1",
            //            Name = "Nickname1",
            //            Contact = "Contact for Nickname1",
            //            CommentBody = "This is the body part of comment 1 in Blog Post #1",
            //            CreatedOn = new DateTime(2008, 09, 02),
            //            //BlogpostId = 1,
            //            BlogpostId = blogposts.Single(b => b.BlogpostId == 1).BlogpostId
            //        },
            //        new Comment
            //        {
            //            Title = "Comment 2's Title For Blog Post #1",
            //            Name = "Nickname2",
            //            Contact = "Contact for Nickname2",
            //            CommentBody = "This is the body part of comment 2 in Blog Post #1",
            //            CreatedOn = new DateTime(2008, 09, 04),
            //            //BlogpostId = 1,
            //            BlogpostId = blogposts.Single(b => b.BlogpostId == 1).BlogpostId
            //        },
            //        new Comment
            //        {
            //            Title = "Comment 1's Title For Blog Post #2",
            //            Name = "Nickname1",
            //            Contact = "Contact for Nickname1",
            //            CommentBody = "This is the body part of comment 1 in Blog Post #2",
            //            CreatedOn = new DateTime(2012, 09, 02),
            //            //BlogpostId = 2,
            //            BlogpostId = blogposts.Single(b => b.BlogpostId == 2).BlogpostId
            //        },
            //        new Comment
            //        {
            //            Title = "Comment 2's Title For Blog Post #2",
            //            Name = "Nickname2",
            //            Contact = "Contact for Nickname2",
            //            CommentBody = "This is the body part of comment 2 in Blog Post #2",
            //            CreatedOn = new DateTime(2012, 09, 07),
            //            //BlogpostId = 2,
            //            BlogpostId = blogposts.Single(b => b.BlogpostId == 2).BlogpostId
            //        },
            //        new Comment
            //        {
            //            Title = "Comment 1's Title For Blog Post #3",
            //            Name = "Nickname1",
            //            Contact = "Contact for Nickname1",
            //            CommentBody = "This is the body part of comment 1 in Blog Post #3",
            //            CreatedOn = new DateTime(2013, 09, 05),
            //            //BlogpostId = 3,
            //            BlogpostId = blogposts.Single(b => b.BlogpostId == 3).BlogpostId
            //        },
            //        new Comment
            //        {
            //            Title = "Comment 2's Title For Blog Post #3",
            //            Name = "Nickname2",
            //            Contact = "Contact for Nickname2",
            //            CommentBody = "This is the body part of comment 2 in Blog Post #3",
            //            CreatedOn = new DateTime(2013, 09, 07),
            //            //BlogpostId = 3,
            //            BlogpostId = blogposts.Single(b => b.BlogpostId == 3).BlogpostId
            //        },
            //        new Comment
            //        {
            //            Title = "Comment 1's Title For Blog Post #5",
            //            Name = "Nickname1",
            //            Contact = "Contact for Nickname1",
            //            CommentBody = "This is the body part of comment 1 in Blog Post #5",
            //            CreatedOn = new DateTime(2013, 12, 05),
            //            BlogpostId = blogposts.Single(b => b.BlogpostId == 5).BlogpostId
            //        },
            //        new Comment
            //        {
            //            Title = "Comment 2's Title For Blog Post #5",
            //            Name = "Nickname2",
            //            Contact = "Contact for Nickname2",
            //            CommentBody = "This is the body part of comment 2 in Blog Post #5",
            //            CreatedOn = new DateTime(2014, 09, 07),
            //            BlogpostId = blogposts.Single(b => b.BlogpostId == 5).BlogpostId
            //        }
            //    };
            //    comments.ForEach(c => context.Comments.AddOrUpdate(p => p.Title, c));
            //    context.SaveChanges();
            //}

            //void AddOrUpdateTags( ApplicationDbContext context, string blogpostTitle, string tagName )
            //{
            //    var blog = context.Blogposts.SingleOrDefault(b => b.Title == blogpostTitle);
            //    var tag = blog.Tags.SingleOrDefault(t => t.Name == tagName);

            //    if ( tag == null )
            //    {
            //        blog.Tags.Add(context.Tags.Single(t => t.Name == tagName));
            //    }
        }
    }
}
