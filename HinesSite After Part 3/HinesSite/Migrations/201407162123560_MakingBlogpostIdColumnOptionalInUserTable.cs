namespace HinesSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakingBlogpostIdColumnOptionalInUserTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Blogpost", "TagId", c => c.Int(nullable: false));
            AlterColumn("dbo.AspNetUsers", "BlogpostId", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "BlogpostId", c => c.Int(nullable: false));
            AlterColumn("dbo.Blogpost", "TagId", c => c.Int());
        }
    }
}
