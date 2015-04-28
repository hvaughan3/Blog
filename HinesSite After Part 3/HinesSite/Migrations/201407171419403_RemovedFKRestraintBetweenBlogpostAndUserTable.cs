namespace HinesSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedFKRestraintBetweenBlogpostAndUserTable : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Blogpost", name: "UserName", newName: "User_Id");
            RenameIndex(table: "dbo.Blogpost", name: "IX_UserName", newName: "IX_User_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Blogpost", name: "IX_User_Id", newName: "IX_UserName");
            RenameColumn(table: "dbo.Blogpost", name: "User_Id", newName: "UserName");
        }
    }
}
