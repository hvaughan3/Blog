namespace HinesSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TriedAddingOptionalCommentsFKToBlogpostTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Blogpost", "CommentId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Blogpost", "CommentId");
        }
    }
}
