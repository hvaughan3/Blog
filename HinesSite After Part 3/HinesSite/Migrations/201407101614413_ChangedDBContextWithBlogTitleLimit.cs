namespace HinesSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedDBContextWithBlogTitleLimit : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Blogpost", "Title", c => c.String(nullable: false, maxLength: 30));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Blogpost", "Title", c => c.String(nullable: false));
        }
    }
}
