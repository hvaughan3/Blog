namespace HinesSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangingBlogpostUserTableRelationshipStuff : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Blogpost", "Subtitle", c => c.String(maxLength: 150));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Blogpost", "Subtitle", c => c.String());
        }
    }
}
