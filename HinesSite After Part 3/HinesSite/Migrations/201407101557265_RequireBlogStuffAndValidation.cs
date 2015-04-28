namespace HinesSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequireBlogStuffAndValidation : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Blogposts", newName: "Blogpost");
            RenameTable(name: "dbo.Comments", newName: "Comment");
            RenameTable(name: "dbo.Tags", newName: "Tag");
            RenameTable(name: "dbo.TagBlogposts", newName: "TagBlogpost");
            AlterColumn("dbo.Blogpost", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.Blogpost", "Body", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Blogpost", "Body", c => c.String());
            AlterColumn("dbo.Blogpost", "Title", c => c.String());
            RenameTable(name: "dbo.TagBlogpost", newName: "TagBlogposts");
            RenameTable(name: "dbo.Tag", newName: "Tags");
            RenameTable(name: "dbo.Comment", newName: "Comments");
            RenameTable(name: "dbo.Blogpost", newName: "Blogposts");
        }
    }
}
