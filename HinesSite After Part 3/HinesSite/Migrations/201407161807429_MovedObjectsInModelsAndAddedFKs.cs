namespace HinesSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MovedObjectsInModelsAndAddedFKs : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Comment", name: "Blogpost_BlogpostId", newName: "BlogpostId");
            RenameIndex(table: "dbo.Comment", name: "IX_Blogpost_BlogpostId", newName: "IX_BlogpostId");
            AddColumn("dbo.Blogpost", "CommentId", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "BlogpostId", c => c.Int(nullable: false));
            AddColumn("dbo.Tag", "BlogpostId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tag", "BlogpostId");
            DropColumn("dbo.AspNetUsers", "BlogpostId");
            DropColumn("dbo.Blogpost", "CommentId");
            RenameIndex(table: "dbo.Comment", name: "IX_BlogpostId", newName: "IX_Blogpost_BlogpostId");
            RenameColumn(table: "dbo.Comment", name: "BlogpostId", newName: "Blogpost_BlogpostId");
        }
    }
}
