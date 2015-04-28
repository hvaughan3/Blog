namespace HinesSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedStuffInDbContextAndCrazynessInInitializer : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.BlogpostTag", name: "Blogpost_BlogpostId", newName: "BlogpostId");
            RenameColumn(table: "dbo.BlogpostTag", name: "Tag_TagId", newName: "TagId");
            RenameIndex(table: "dbo.BlogpostTag", name: "IX_Blogpost_BlogpostId", newName: "IX_BlogpostId");
            RenameIndex(table: "dbo.BlogpostTag", name: "IX_Tag_TagId", newName: "IX_TagId");
            DropColumn("dbo.Blogpost", "TagId");
            DropColumn("dbo.Blogpost", "CommentId");
            DropColumn("dbo.Tag", "BlogpostId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tag", "BlogpostId", c => c.Int(nullable: false));
            AddColumn("dbo.Blogpost", "CommentId", c => c.Int());
            AddColumn("dbo.Blogpost", "TagId", c => c.Int(nullable: false));
            RenameIndex(table: "dbo.BlogpostTag", name: "IX_TagId", newName: "IX_Tag_TagId");
            RenameIndex(table: "dbo.BlogpostTag", name: "IX_BlogpostId", newName: "IX_Blogpost_BlogpostId");
            RenameColumn(table: "dbo.BlogpostTag", name: "TagId", newName: "Tag_TagId");
            RenameColumn(table: "dbo.BlogpostTag", name: "BlogpostId", newName: "Blogpost_BlogpostId");
        }
    }
}
