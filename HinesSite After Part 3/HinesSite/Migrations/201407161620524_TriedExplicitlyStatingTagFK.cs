namespace HinesSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TriedExplicitlyStatingTagFK : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.TagBlogpost", newName: "BlogpostTag");
            DropPrimaryKey("dbo.BlogpostTag");
            AddColumn("dbo.Blogpost", "TagId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.BlogpostTag", new[] { "Blogpost_BlogpostId", "Tag_TagId" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.BlogpostTag");
            DropColumn("dbo.Blogpost", "TagId");
            AddPrimaryKey("dbo.BlogpostTag", new[] { "Tag_TagId", "Blogpost_BlogpostId" });
            RenameTable(name: "dbo.BlogpostTag", newName: "TagBlogpost");
        }
    }
}
