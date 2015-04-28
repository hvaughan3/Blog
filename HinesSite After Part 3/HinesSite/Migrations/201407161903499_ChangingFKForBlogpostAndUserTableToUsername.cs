namespace HinesSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangingFKForBlogpostAndUserTableToUsername : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Blogpost", name: "UserId", newName: "Username");
            RenameIndex(table: "dbo.Blogpost", name: "IX_UserId", newName: "IX_Username");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Blogpost", name: "IX_Username", newName: "IX_UserId");
            RenameColumn(table: "dbo.Blogpost", name: "Username", newName: "UserId");
        }
    }
}
