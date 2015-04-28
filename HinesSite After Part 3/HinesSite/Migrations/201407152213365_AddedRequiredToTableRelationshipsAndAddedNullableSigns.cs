namespace HinesSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRequiredToTableRelationshipsAndAddedNullableSigns : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comment", "Blogpost_BlogpostId", "dbo.Blogpost");
            DropIndex("dbo.Comment", new[] { "Blogpost_BlogpostId" });
            AddColumn("dbo.Comment", "Title", c => c.String(maxLength: 75));
            AlterColumn("dbo.Blogpost", "Body", c => c.String(nullable: false, maxLength: 1000));
            AlterColumn("dbo.Blogpost", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Blogpost", "ModifiedOn", c => c.DateTime());
            AlterColumn("dbo.Comment", "Name", c => c.String(maxLength: 50));
            AlterColumn("dbo.Comment", "Contact", c => c.String(maxLength: 50));
            AlterColumn("dbo.Comment", "CommentBody", c => c.String(nullable: false, maxLength: 1000));
            AlterColumn("dbo.Comment", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Comment", "ModifiedOn", c => c.DateTime());
            AlterColumn("dbo.Comment", "Blogpost_BlogpostId", c => c.Int(nullable: false));
            AlterColumn("dbo.Tag", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Tag", "ModifiedOn", c => c.DateTime());
            CreateIndex("dbo.Comment", "Blogpost_BlogpostId");
            AddForeignKey("dbo.Comment", "Blogpost_BlogpostId", "dbo.Blogpost", "BlogpostId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comment", "Blogpost_BlogpostId", "dbo.Blogpost");
            DropIndex("dbo.Comment", new[] { "Blogpost_BlogpostId" });
            AlterColumn("dbo.Tag", "ModifiedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Tag", "CreatedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Comment", "Blogpost_BlogpostId", c => c.Int());
            AlterColumn("dbo.Comment", "ModifiedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Comment", "CreatedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Comment", "CommentBody", c => c.String(nullable: false));
            AlterColumn("dbo.Comment", "Contact", c => c.String());
            AlterColumn("dbo.Comment", "Name", c => c.String());
            AlterColumn("dbo.Blogpost", "ModifiedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Blogpost", "CreatedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Blogpost", "Body", c => c.String(nullable: false));
            DropColumn("dbo.Comment", "Title");
            CreateIndex("dbo.Comment", "Blogpost_BlogpostId");
            AddForeignKey("dbo.Comment", "Blogpost_BlogpostId", "dbo.Blogpost", "BlogpostId");
        }
    }
}
