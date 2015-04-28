namespace HinesSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatingRowVersionColumnInBlogpostTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Blogpost", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Blogpost", "RowVersion");
        }
    }
}
