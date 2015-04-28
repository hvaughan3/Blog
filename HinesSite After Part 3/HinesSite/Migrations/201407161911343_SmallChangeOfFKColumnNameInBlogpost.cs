namespace HinesSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SmallChangeOfFKColumnNameInBlogpost : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Blogpost", new[] { "Username" });
            CreateIndex("dbo.Blogpost", "UserName");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Blogpost", new[] { "UserName" });
            CreateIndex("dbo.Blogpost", "Username");
        }
    }
}
