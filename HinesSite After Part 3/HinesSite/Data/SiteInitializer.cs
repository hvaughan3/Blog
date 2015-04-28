#region Usings

using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using HinesSite.Data.Context;
using HinesSite.Models;

#endregion
// ReSharper disable MissingXmlDoc

namespace HinesSite.Data {

    //   Causes a database to be dropped and rebuilt everytime there is a change, for development purposes. Also
    // allows test data to be loaded into the new database after being recreated.

    public class SiteInitializer : System.Data.Entity.DropCreateDatabaseAlways<ApplicationDbContext> {

        protected override void Seed(ApplicationDbContext context) {

            // This will run when Update-Database is used, while SiteInitializer.cs will run when the site is ran
            List<User> users = new List<User> {
                new User {
                    Email                = "hvaughan3@gmail.com",
                    EmailConfirmed       = false,
                    PhoneNumberConfirmed = false,
                    UserName             = "hvaughan3@gmail.com",
                    TwoFactorEnabled     = false,
                    LockoutEnabled       = false,
                    AccessFailedCount    = 0,
                    Blogposts            = new List<Blogpost>(),
                    Comments             = new List<Comment>()
                }, new User {
                    Email                = "user2@gmail.com",
                    EmailConfirmed       = false,
                    PhoneNumberConfirmed = false,
                    UserName             = "user2@gmail.com",
                    TwoFactorEnabled     = false,
                    LockoutEnabled       = false,
                    AccessFailedCount    = 0,
                    Blogposts            = new List<Blogpost>(),
                    Comments             = new List<Comment>()
                }, new User {
                    Email                = "user3@gmail.com",
                    EmailConfirmed       = true,
                    PhoneNumberConfirmed = false,
                    UserName             = "user3@gmail.com",
                    TwoFactorEnabled     = false,
                    LockoutEnabled       = false,
                    AccessFailedCount    = 0,
                    Blogposts            = new List<Blogpost>(),
                    Comments             = new List<Comment>()
                }
            };
            users.ForEach(u => context.Users.AddOrUpdate(u));
            context.SaveChanges();

            List<Tag> tags = new List<Tag> {
                new Tag {
                    //BlogpostId = 3,
                    Name       = "C#",
                    CreatedOn  = new DateTime(2014, 08, 14),
                    ModifiedOn = new DateTime(2015, 01, 20)
                    //Blogposts = new List<Blogpost>()
                }, new Tag {
                    //BlogpostId = 2,
                    Name       = "ASP.Net",
                    CreatedOn  = new DateTime(2012, 05, 14),
                    ModifiedOn = new DateTime(2015, 01, 10)
                    //Blogposts = new List<Blogpost>()
                }, new Tag {
                    //BlogpostId = 1,
                    Name       = "MVC",
                    CreatedOn  = new DateTime(2014, 02, 14),
                    ModifiedOn = new DateTime(2014, 12, 31)
                    //Blogposts = new List<Blogpost>()
                }, new Tag {
                    Name       = "Random",
                    CreatedOn  = new DateTime(2014, 07, 30),
                    ModifiedOn = new DateTime(2015, 02, 25)
                }
            };
            tags.ForEach(t => context.Tags.AddOrUpdate(p => p.Name, t));
            context.SaveChanges();

            List<Blogpost> blogposts = new List<Blogpost> {
                new Blogpost {
                    Title      = "Blog Post Title 1",
                    Subtitle   = "This is the blog post 1 subtitle",
                    Body       = "This is the body of blog post #1",
                    CreatedOn  = new DateTime(2008, 09, 01),
                    ModifiedOn = new DateTime(2015, 01, 02),
                    //TagId      = 3,
                    UserId     = users.SingleOrDefault(u => u.UserName == "hvaughan3@gmail.com").Id,
                    Tags       = new List<Tag>(),
                    Comments   = new List<Comment>()
                }, new Blogpost {
                    Title      = "Blog Post Title 2",
                    Subtitle   = "This is the blog post 2 subtitle",
                    Body       = "This is the body of blog post #2",
                    CreatedOn  = new DateTime(2012, 05, 31),
                    ModifiedOn = new DateTime(2015, 01, 10),
                    //TagId      = 2,
                    UserId     = users.SingleOrDefault(u => u.UserName == "hvaughan3@gmail.com").Id,
                    Tags       = new List<Tag>(),
                    Comments   = new List<Comment>()
                }, new Blogpost {
                    Title      = "Blog Post Title 3",
                    Subtitle   = "This is the blog post 3 subtitle",
                    Body       = "This is the body of blog post #3",
                    CreatedOn  = new DateTime(2014, 07, 16),
                    ModifiedOn = new DateTime(2015, 02, 21),
                    //TagId      = 1,
                    UserId     = users.SingleOrDefault(u => u.UserName == "hvaughan3@gmail.com").Id,
                    Tags       = new List<Tag>(),
                    Comments   = new List<Comment>()
                }, new Blogpost {
                    Title      = "Blog Post Title 4",
                    Subtitle   = "This is the blog post 4 subtitle",
                    Body       = "This is the body of blog post #4",
                    CreatedOn  = new DateTime(2014, 07, 30),
                    ModifiedOn = new DateTime(2015, 01, 02),
                    UserId     = users.SingleOrDefault(u => u.UserName == "hvaughan3@gmail.com").Id,
                    Tags       = new List<Tag>(),
                    Comments   = new List<Comment>()
                }, new Blogpost {
                    Title      = "Blog Post Title 5",
                    Subtitle   = "This is the blog post 5 subtitle",
                    Body       = "This is the body of blog post #5",
                    CreatedOn  = new DateTime(2014, 07, 30),
                    ModifiedOn = new DateTime(2015, 03, 05),
                    UserId     = users.SingleOrDefault(u => u.UserName == "hvaughan3@gmail.com").Id,
                    Tags       = new List<Tag>(),
                    Comments   = new List<Comment>()
                }, new Blogpost {
                    Title      = "Blog Post Title 6",
                    Subtitle   = "This is the blog post 6 subtitle",
                    Body       = "This is the body of blog post #6",
                    CreatedOn  = new DateTime(2014, 07, 30),
                    ModifiedOn = new DateTime(2015, 01, 18),
                    UserId     = users.SingleOrDefault(u => u.UserName == "hvaughan3@gmail.com").Id,
                    Tags       = new List<Tag>(),
                    Comments   = new List<Comment>()
                }
            };
            foreach(Blogpost b in blogposts) {
                Blogpost blogpostInDataBase = context.Blogposts.SingleOrDefault(u => u.User.Id == b.UserId);

                if(blogpostInDataBase == null) {
                    context.Blogposts.Add(b);
                }
            }
            context.SaveChanges();

            AddOrUpdateTags(context, "Blog Post Title 6", "Random");
            AddOrUpdateTags(context, "Blog Post Title 5", "MVC");
            AddOrUpdateTags(context, "Blog Post Title 4", "MVC");
            AddOrUpdateTags(context, "Blog Post Title 3", "C#");
            AddOrUpdateTags(context, "Blog Post Title 2", "ASP.Net");
            AddOrUpdateTags(context, "Blog Post Title 1", "MVC");
            context.SaveChanges();

            List<Comment> comments = new List<Comment> {
                new Comment {
                    Title       = "Comment 1's Title For Blog Post #1",
                    Name        = "hvaughan3@gmail.com",
                    Contact     = "Contact for Nickname1",
                    CommentBody = "This is the body part of comment 1 in Blog Post #1",
                    CreatedOn   = new DateTime(2008, 09, 02),
                    ModifiedOn  = new DateTime(2015, 01, 18),
                    //BlogpostId  = 1,
                    BlogpostId  = blogposts.SingleOrDefault(b => b.BlogpostId == 1).BlogpostId,
                    UserId      = users.SingleOrDefault(u => u.UserName == "hvaughan3@gmail.com").Id
                }, new Comment {
                    Title       = "Comment 2's Title For Blog Post #1",
                    Name        = "hvaughan3@gmail.com",
                    Contact     = "Contact for Nickname2",
                    CommentBody = "This is the body part of comment 2 in Blog Post #1",
                    CreatedOn   = new DateTime(2008, 09, 04),
                    ModifiedOn  = new DateTime(2014, 01, 18),
                    //BlogpostId  = 1,
                    BlogpostId  = blogposts.SingleOrDefault(b => b.BlogpostId == 1).BlogpostId,
                    UserId      = users.SingleOrDefault(u => u.UserName == "hvaughan3@gmail.com").Id
                }, new Comment {
                    Title       = "Comment 1's Title For Blog Post #2",
                    Name        = "hvaughan3@gmail.com",
                    Contact     = "Contact for Nickname1",
                    CommentBody = "This is the body part of comment 1 in Blog Post #2",
                    CreatedOn   = new DateTime(2012, 09, 02),
                    ModifiedOn  = new DateTime(2015, 02, 20),
                    //BlogpostId  = 2,
                    BlogpostId  = blogposts.SingleOrDefault(b => b.BlogpostId == 2).BlogpostId,
                    UserId      = users.SingleOrDefault(u => u.UserName == "hvaughan3@gmail.com").Id
                }, new Comment {
                    Title       = "Comment 2's Title For Blog Post #2",
                    Name        = "hvaughan3@gmail.com",
                    Contact     = "Contact for Nickname2",
                    CommentBody = "This is the body part of comment 2 in Blog Post #2",
                    CreatedOn   = new DateTime(2012, 09, 07),
                    ModifiedOn  = new DateTime(2015, 02, 04),
                    //BlogpostId  = 2,
                    BlogpostId  = blogposts.SingleOrDefault(b => b.BlogpostId == 2).BlogpostId,
                    UserId      = users.SingleOrDefault(u => u.UserName == "hvaughan3@gmail.com").Id
                }, new Comment {
                    Title       = "Comment 1's Title For Blog Post #3",
                    Name        = "hvaughan3@gmail.com",
                    Contact     = "Contact for Nickname1",
                    CommentBody = "This is the body part of comment 1 in Blog Post #3",
                    CreatedOn   = new DateTime(2013, 09, 05),
                    ModifiedOn  = new DateTime(2015, 03, 02),
                    //BlogpostId  = 3,
                    BlogpostId  = blogposts.SingleOrDefault(b => b.BlogpostId == 3).BlogpostId,
                    UserId      = users.SingleOrDefault(u => u.UserName == "hvaughan3@gmail.com").Id
                }, new Comment {
                    Title       = "Comment 2's Title For Blog Post #3",
                    Name        = "hvaughan3@gmail.com",
                    Contact     = "Contact for Nickname2",
                    CommentBody = "This is the body part of comment 2 in Blog Post #3",
                    CreatedOn   = new DateTime(2013, 09, 07),
                    ModifiedOn  = new DateTime(2015, 01, 23),
                    //BlogpostId  = 3,
                    BlogpostId  = blogposts.SingleOrDefault(b => b.BlogpostId == 3).BlogpostId,
                    UserId      = users.SingleOrDefault(u => u.UserName == "hvaughan3@gmail.com").Id
                }, new Comment {
                    Title       = "Comment 1's Title For Blog Post #5",
                    Name        = "hvaughan3@gmail.com",
                    Contact     = "Contact for Nickname1",
                    CommentBody = "This is the body part of comment 1 in Blog Post #5",
                    CreatedOn   = new DateTime(2013, 12, 05),
                    ModifiedOn  = new DateTime(2014, 12, 30),
                    BlogpostId  = blogposts.SingleOrDefault(b => b.BlogpostId == 5).BlogpostId,
                    UserId      = users.SingleOrDefault(u => u.UserName == "hvaughan3@gmail.com").Id
                }, new Comment {
                    Title       = "Comment 2's Title For Blog Post #5",
                    Name        = "hvaughan3@gmail.com",
                    Contact     = "Contact for Nickname2",
                    CommentBody = "This is the body part of comment 2 in Blog Post #5",
                    CreatedOn   = new DateTime(2014, 09, 07),
                    ModifiedOn  = new DateTime(2015, 03, 20),
                    BlogpostId  = blogposts.SingleOrDefault(b => b.BlogpostId == 5).BlogpostId,
                    UserId      = users.SingleOrDefault(u => u.UserName == "hvaughan3@gmail.com").Id
                }
            };
            foreach(Comment c in comments) {
                Comment commentsInDataBase     = context.Comments.FirstOrDefault(b => b.Blogpost.BlogpostId == c.BlogpostId);
                Comment userCommentsInDataBase = context.Comments.FirstOrDefault(u => u.User.Id             == c.UserId);

                if(commentsInDataBase == null || userCommentsInDataBase == null) {
                    context.Comments.Add(c);
                }
            }
            context.SaveChanges();
        }

        static void AddOrUpdateTags(ApplicationDbContext context, string blogpostTitle, string tagName) {

            Blogpost blog = context.Blogposts.SingleOrDefault(b => b.Title == blogpostTitle);
            Tag       tag = blog.Tags.SingleOrDefault(t => t.Name == tagName);

            if(tag == null) {
                blog.Tags.Add(context.Tags.Single(t => t.Name == tagName));
            }
        }
    }
}