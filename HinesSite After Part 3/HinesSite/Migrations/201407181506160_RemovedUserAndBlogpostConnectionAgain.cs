namespace HinesSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedUserAndBlogpostConnectionAgain : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Blogpost", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Blogpost", new[] { "User_Id" });
            RenameColumn(table: "dbo.Blogpost", name: "User_Id", newName: "UserId");
            AlterColumn("dbo.Blogpost", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Blogpost", "UserId");
            AddForeignKey("dbo.Blogpost", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Blogpost", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Blogpost", new[] { "UserId" });
            AlterColumn("dbo.Blogpost", "UserId", c => c.String(nullable: false, maxLength: 128));
            RenameColumn(table: "dbo.Blogpost", name: "UserId", newName: "User_Id");
            CreateIndex("dbo.Blogpost", "User_Id");
            AddForeignKey("dbo.Blogpost", "User_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
