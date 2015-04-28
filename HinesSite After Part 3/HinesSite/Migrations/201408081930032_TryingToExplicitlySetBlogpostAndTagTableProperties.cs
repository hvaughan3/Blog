namespace HinesSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TryingToExplicitlySetBlogpostAndTagTableProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Blogpost", "TagId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Blogpost", "TagId");
        }
    }
}
