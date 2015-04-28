namespace HinesSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedDisplayNameOfCreatedAndModifiedOn : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Blogpost", "Title", c => c.String(nullable: false, maxLength: 75));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Blogpost", "Title", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
