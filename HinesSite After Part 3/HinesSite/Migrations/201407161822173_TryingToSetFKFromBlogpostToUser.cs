namespace HinesSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TryingToSetFKFromBlogpostToUser : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Blogpost", name: "Author_Id", newName: "UserId");
            RenameIndex(table: "dbo.Blogpost", name: "IX_Author_Id", newName: "IX_UserId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Blogpost", name: "IX_UserId", newName: "IX_Author_Id");
            RenameColumn(table: "dbo.Blogpost", name: "UserId", newName: "Author_Id");
        }
    }
}
