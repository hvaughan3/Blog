namespace HinesSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GettingBackOnTheHorse : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "FirstName", c => c.String(maxLength: 60));
            AlterColumn("dbo.AspNetUsers", "LastName", c => c.String(maxLength: 75));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "LastName", c => c.String(maxLength: 45));
            AlterColumn("dbo.AspNetUsers", "FirstName", c => c.String(maxLength: 35));
        }
    }
}
