namespace HinesSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakingSomeFKsOptionalInBlogpostTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Blogpost","TagId",c => c.Int(nullable: true));
            AlterColumn("dbo.Blogpost","CommentId",c => c.Int(nullable: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Blogpost", "CommentId", c => c.Int(nullable: false));
            AlterColumn("dbo.Blogpost", "TagId", c => c.Int(nullable: false));
        }
    }
}
