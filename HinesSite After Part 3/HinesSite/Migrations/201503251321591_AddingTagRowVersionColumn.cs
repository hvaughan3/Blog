namespace HinesSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddingTagRowVersionColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tag", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }

        public override void Down()
        {
            DropColumn("dbo.Tag", "RowVersion");
        }
    }
}
