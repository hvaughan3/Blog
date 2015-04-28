namespace HinesSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MadeJoinDateInUserModelOptional : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "JoinDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "JoinDate", c => c.DateTime(nullable: false));
        }
    }
}
