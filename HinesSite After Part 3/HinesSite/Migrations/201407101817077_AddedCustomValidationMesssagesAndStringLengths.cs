namespace HinesSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCustomValidationMesssagesAndStringLengths : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Blogpost", "Title", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.AspNetUsers", "FirstName", c => c.String(nullable: false, maxLength: 35));
            AlterColumn("dbo.AspNetUsers", "LastName", c => c.String(nullable: false, maxLength: 45));
            AlterColumn("dbo.Comment", "CommentBody", c => c.String(nullable: false));
            AlterColumn("dbo.Tag", "Name", c => c.String(nullable: false, maxLength: 30));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tag", "Name", c => c.String());
            AlterColumn("dbo.Comment", "CommentBody", c => c.String());
            AlterColumn("dbo.AspNetUsers", "LastName", c => c.String(nullable: false));
            AlterColumn("dbo.AspNetUsers", "FirstName", c => c.String(nullable: false));
            AlterColumn("dbo.Blogpost", "Title", c => c.String(nullable: false, maxLength: 30));
        }
    }
}
