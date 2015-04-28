namespace HinesSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedNullableSignsForCreatedOnAndAddedDataTypeAndFormatStuff : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Blogpost", "CreatedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Comment", "CreatedOn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Tag", "CreatedOn", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tag", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Comment", "CreatedOn", c => c.DateTime());
            AlterColumn("dbo.Blogpost", "CreatedOn", c => c.DateTime());
        }
    }
}
