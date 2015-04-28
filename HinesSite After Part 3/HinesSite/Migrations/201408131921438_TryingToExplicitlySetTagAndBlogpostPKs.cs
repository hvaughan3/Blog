namespace HinesSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TryingToExplicitlySetTagAndBlogpostPKs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tag", "BlogpostId", c => c.Int());
            AlterColumn("dbo.Blogpost", "TagId", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Blogpost", "TagId", c => c.Int(nullable: false));
            DropColumn("dbo.Tag", "BlogpostId");
        }
    }
}
