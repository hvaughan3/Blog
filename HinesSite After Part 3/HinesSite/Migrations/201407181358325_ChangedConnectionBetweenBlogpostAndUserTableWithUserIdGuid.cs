namespace HinesSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedConnectionBetweenBlogpostAndUserTableWithUserIdGuid : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Blogpost", name: "User_Id", newName: "UserId");
            RenameIndex(table: "dbo.Blogpost", name: "IX_User_Id", newName: "IX_UserId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Blogpost", name: "IX_UserId", newName: "IX_User_Id");
            RenameColumn(table: "dbo.Blogpost", name: "UserId", newName: "User_Id");
        }
    }
}
