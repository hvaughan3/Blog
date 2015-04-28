namespace HinesSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class TurningStoredProcedureMappingForBlogAndTagModelsBackOFF : DbMigration
    {
        public override void Up()
        {
            DropStoredProcedure("dbo.Blogpost_Insert");
            DropStoredProcedure("dbo.Blogpost_Update");
            DropStoredProcedure("dbo.Blogpost_Delete");
            DropStoredProcedure("dbo.Tag_Insert");
            DropStoredProcedure("dbo.Tag_Update");
            DropStoredProcedure("dbo.Tag_Delete");
        }

        public override void Down()
        {
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
