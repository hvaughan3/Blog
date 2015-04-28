namespace HinesSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RevertedBackAndNowTryingToGoForwardWithCommentAndUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comment", "UserId", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUsers", "CommentId", c => c.Int());
            CreateIndex("dbo.Comment", "UserId");
            AddForeignKey("dbo.Comment", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comment", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Comment", new[] { "UserId" });
            DropColumn("dbo.AspNetUsers", "CommentId");
            DropColumn("dbo.Comment", "UserId");
        }
    }
}
